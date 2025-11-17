using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using loadmaster_api.Data;
using loadmaster_api.Models;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;

namespace loadmaster_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public class RegisterRequest
        {
            public string Username { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public class LoginRequest
        {
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public class CreateDispatcherRequest
        {
            public string Username { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            // comma separated permissions (e.g. "dashboard,dispatchboard,loadmanagement,customer,fleet")
            public string? Permissions { get; set; }
        }

        public class UpdateThemeRequest
        {
            public string? ThemeColors { get; set; }
            public bool IsDarkMode { get; set; }
        }

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            try
            {
                if (await _context.Users.AnyAsync(u => u.Username == request.Username))
                {
                    return BadRequest(new { message = "Username is already taken" });
                }

                if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                {
                    return BadRequest(new { message = "Email is already registered" });
                }

                // Create tenant workspace for this user
                using (var tx = await _context.Database.BeginTransactionAsync())
                {
                    var tenant = new Tenant
                    {
                        Name = $"{request.Username}'s TMS",
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.Tenants.Add(tenant);
                    await _context.SaveChangesAsync();

                    var user = new User
                    {
                        Username = request.Username,
                        Email = request.Email,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                        Role = "owner",
                        TenantId = tenant.Id
                    };

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    tenant.OwnerUserId = user.Id;
                    _context.Tenants.Update(tenant);
                    await _context.SaveChangesAsync();

                    await tx.CommitAsync();
                }

                return Ok(new { message = "User and workspace created successfully" });
            }
            catch (Exception ex)
            {
                // Return exception details to help debugging locally
                return StatusCode(500, new { message = "Failed to register user", error = ex.Message, stack = ex.StackTrace });
            }
        }

        // Add a new endpoint to update theme preferences
        [Authorize]
        [HttpPost("update-theme")]
        public async Task<IActionResult> UpdateTheme([FromBody] UpdateThemeRequest request)
        {
            try
            {
                var userIdClaim = User.FindFirst("sub") ?? User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized(new { message = "No user ID claim found" });
                }

                if (!int.TryParse(userIdClaim.Value, out var userId))
                {
                    return BadRequest(new { message = "Invalid user id in token" });
                }

                var user = await _context.Users.FindAsync(userId);

                if (user == null) return NotFound(new { message = "User not found" });

                user.ThemeColors = request.ThemeColors;
                user.IsDarkMode = request.IsDarkMode;
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Theme preferences updated",
                    theme = new
                    {
                        colors = user.ThemeColors,
                        isDark = user.IsDarkMode
                    }
                });
            }
            catch (Exception ex)
            {
                // Return exception message to aid debugging in development
                return StatusCode(500, new { message = "Failed to update theme preferences", error = ex.Message });
            }
        }

        // Create a dispatcher account under the caller's tenant. Only callers with 'admin' or 'owner' role may create dispatchers.
        [Authorize]
        [HttpPost("create-dispatcher")]
        public async Task<IActionResult> CreateDispatcher([FromBody] CreateDispatcherRequest request)
        {
            try
            {
                var callerRole = User.FindFirst("role")?.Value ?? string.Empty;

                // allow if caller is admin or owner role
                var callerIdClaim = User.FindFirst("sub") ?? User.FindFirst(ClaimTypes.NameIdentifier);
                int? callerId = null;
                if (callerIdClaim != null && int.TryParse(callerIdClaim.Value, out var parsed)) callerId = parsed;

                // get caller's tenant (if any) so dispatcher is created in same tenant
                var tenantClaim = User.FindFirst("tenant");
                int? tenantId = null;
                if (tenantClaim != null) tenantId = int.Parse(tenantClaim.Value);

                var isOwnerByTenant = false;
                if (tenantId != null && callerId != null)
                {
                    var tenant = await _context.Tenants.FindAsync(tenantId.Value);
                    if (tenant != null && tenant.OwnerUserId == callerId.Value) isOwnerByTenant = true;
                }

                if (callerRole != "admin" && callerRole != "owner" && !isOwnerByTenant)
                {
                    return StatusCode(403, new { message = "Insufficient role: only admin or owner can create dispatchers" });
                }

                // check uniqueness
                if (await _context.Users.AnyAsync(u => u.Username == request.Username))
                {
                    return BadRequest(new { message = "Username already taken" });
                }
                if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                {
                    return BadRequest(new { message = "Email already registered" });
                }

                var dispatcher = new User
                {
                    Username = request.Username,
                    Email = request.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    Role = "dispatcher",
                    TenantId = tenantId,
                    Permissions = request.Permissions
                };

                _context.Users.Add(dispatcher);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Dispatcher account created", id = dispatcher.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to create dispatcher", error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null) return BadRequest(new { message = "Invalid username or password" });
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)) return BadRequest(new { message = "Invalid username or password" });

            var token = GenerateJwtToken(user);

            user.LastLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var response = new
            {
                token = token,
                user = new
                {
                    id = user.Id,
                    username = user.Username,
                    email = user.Email,
                    role = user.Role,
                    tenantId = user.TenantId,
                    themeColors = user.ThemeColors,
                    isDarkMode = user.IsDarkMode,
                    permissions = user.Permissions
                }
            };

            return Ok(response);
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key is not configured")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("sub", user.Id.ToString()),
                new Claim("name", user.Username),
                new Claim("email", user.Email),
                new Claim("role", user.Role)
            };

            if (user.TenantId != null)
            {
                claims.Add(new Claim("tenant", user.TenantId.Value.ToString()));
            }
            if (!string.IsNullOrEmpty(user.Permissions))
            {
                claims.Add(new Claim("permissions", user.Permissions));
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}