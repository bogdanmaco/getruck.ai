using System.ComponentModel.DataAnnotations;

namespace loadmaster_api.Models
{
    public class Truck
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string TruckNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Make { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Model { get; set; } = string.Empty;

        public int Year { get; set; }

        [Required]
        [MaxLength(17)]
        public string VIN { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = "available"; // available, in_use, maintenance, out_of_service

        public DateTime LastMaintenanceDate { get; set; }
        public decimal Mileage { get; set; }

        // Tenant scoping
        public int TenantId { get; set; }
        public Tenant? Tenant { get; set; }

        public virtual ICollection<Load> Loads { get; set; } = new List<Load>();
    }
}