using System.ComponentModel.DataAnnotations;

namespace loadmaster_api.Models
{
    public class Trailer
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string TrailerNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Type { get; set; } = string.Empty; // flatbed, dry_van, reefer, etc.

        [Required]
        [MaxLength(17)]
        public string VIN { get; set; } = string.Empty;

        public decimal Length { get; set; }
        public decimal Capacity { get; set; } // in pounds

        [Required]
        public string Status { get; set; } = "available"; // available, in_use, maintenance, out_of_service

        public DateTime LastMaintenanceDate { get; set; }

        // Tenant scoping
        public int TenantId { get; set; }
        public Tenant? Tenant { get; set; }

        public virtual ICollection<Load> Loads { get; set; } = new List<Load>();
    }
}