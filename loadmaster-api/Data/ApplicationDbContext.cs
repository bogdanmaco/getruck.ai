using Microsoft.EntityFrameworkCore;
using loadmaster_api.Models;

namespace loadmaster_api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Load> Loads { get; set; }
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<Trailer> Trailers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Load>()
                .HasOne(l => l.Customer)
                .WithMany(c => c.Loads)
                .HasForeignKey(l => l.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Load>()
                .HasOne(l => l.Driver)
                .WithMany(d => d.Loads)
                .HasForeignKey(l => l.DriverId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Load>()
                .HasOne(l => l.Truck)
                .WithMany(t => t.Loads)
                .HasForeignKey(l => l.TruckId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Load>()
                .HasOne(l => l.Trailer)
                .WithMany(t => t.Loads)
                .HasForeignKey(l => l.TrailerId)
                .OnDelete(DeleteBehavior.SetNull);

            // Tenant - User relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.Tenant)
                .WithMany(t => t.Users)
                .HasForeignKey(u => u.TenantId)
                .OnDelete(DeleteBehavior.SetNull);

            // Tenant scoping for domain entities
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Tenant)
                .WithMany(t => t.Customers)
                .HasForeignKey(c => c.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Driver>()
                .HasOne(d => d.Tenant)
                .WithMany(t => t.Drivers)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Truck>()
                .HasOne(tr => tr.Tenant)
                .WithMany(t => t.Trucks)
                .HasForeignKey(tr => tr.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Trailer>()
                .HasOne(tr => tr.Tenant)
                .WithMany(t => t.Trailers)
                .HasForeignKey(tr => tr.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Load>()
                .HasOne(l => l.Tenant)
                .WithMany(t => t.Loads)
                .HasForeignKey(l => l.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}