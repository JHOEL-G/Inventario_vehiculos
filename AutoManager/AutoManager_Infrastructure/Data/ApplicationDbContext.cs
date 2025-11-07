using AutoManager.AutoManager_Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace AutoManager.AutoManager_Infrastructure.Config
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Maintenance> MaintenanceRecords { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasIndex(v => v.SerialNumber).IsUnique();

                entity.HasOne(v => v.Owner)
                        .WithMany(c => c.Vehicles)
                        .HasForeignKey(v => v.OwnerId)
                        .OnDelete(DeleteBehavior.SetNull);
                
                entity.HasMany(v => v.MaintenanceRecords)
                        .WithOne(m => m.Vehicle)
                        .HasForeignKey(m => m.VehicleId)
                        .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Maintenance>(entity =>
            {
                entity.Property(m => m.VehicleId).IsRequired();
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
