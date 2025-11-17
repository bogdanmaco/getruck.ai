using System.ComponentModel.DataAnnotations;

namespace loadmaster_api.Models
{
    public class Driver
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string LicenseNumber { get; set; } = string.Empty;

        public DateTime LicenseExpiryDate { get; set; }
        
        public string Status { get; set; } = "available"; // available, on_duty, off_duty
        
        // Tenant scoping
        public int TenantId { get; set; }
        public Tenant? Tenant { get; set; }

        public virtual ICollection<Load> Loads { get; set; } = new List<Load>();
    }
}