using System.ComponentModel.DataAnnotations;

namespace loadmaster_api.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string CompanyName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string ContactName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;

        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [MaxLength(50)]
        public string State { get; set; } = string.Empty;

        [MaxLength(20)]
        public string ZipCode { get; set; } = string.Empty;

        // Tenant scoping
        public int TenantId { get; set; }
        public Tenant? Tenant { get; set; }

        public virtual ICollection<Load> Loads { get; set; } = new List<Load>();
    }
}