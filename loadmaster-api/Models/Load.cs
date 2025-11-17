using System.ComponentModel.DataAnnotations;

namespace loadmaster_api.Models
{
    public class Load
    {
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; } = null!;

        public int? DriverId { get; set; }
        public virtual Driver? Driver { get; set; }

        public int? TruckId { get; set; }
        public virtual Truck? Truck { get; set; }

        public int? TrailerId { get; set; }
        public virtual Trailer? Trailer { get; set; }

    // Tenant scoping
    public int TenantId { get; set; }
    public Tenant? Tenant { get; set; }

        [Required]
        [MaxLength(200)]
        public string PickupLocation { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string DeliveryLocation { get; set; } = string.Empty;

        public DateTime PickupDate { get; set; }
        public DateTime DeliveryDate { get; set; }

        [Required]
        public string Status { get; set; } = "pending"; // pending, in_transit, delivered, cancelled

        public decimal Revenue { get; set; }
        public decimal Distance { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}