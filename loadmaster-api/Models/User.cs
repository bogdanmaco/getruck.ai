using System.ComponentModel.DataAnnotations;

namespace loadmaster_api.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        
        public string Role { get; set; } = "user"; // e.g., "admin", "dispatcher", "driver"
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }
        
        // Tenant scoping
        public int? TenantId { get; set; }
        public Tenant? Tenant { get; set; }

        // Theme preferences
        public string? ThemeColors { get; set; }
        public bool IsDarkMode { get; set; } = false;
        
        // Optional comma-separated permissions (e.g. "dashboard,dispatchboard,loadmanagement")
        public string? Permissions { get; set; }
    }
}