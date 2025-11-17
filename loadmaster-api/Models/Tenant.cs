using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace loadmaster_api.Models
{
    public class Tenant
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? OwnerUserId { get; set; }

        // Navigation
        public List<User>? Users { get; set; }
        public List<Customer>? Customers { get; set; }
        public List<Driver>? Drivers { get; set; }
        public List<Load>? Loads { get; set; }
        public List<Truck>? Trucks { get; set; }
        public List<Trailer>? Trailers { get; set; }
    }
}
