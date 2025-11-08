using AutoManager.AutoManager_Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
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

            // Configuración global para convertir DateTime a UTC
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v.Kind == DateTimeKind.Unspecified
                    ? DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    : v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );

            var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
                v => v.HasValue
                    ? (v.Value.Kind == DateTimeKind.Unspecified
                        ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc)
                        : v.Value.ToUniversalTime())
                    : v,
                v => v.HasValue
                    ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc)
                    : v
            );

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(dateTimeConverter);
                    }
                    else if (property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(nullableDateTimeConverter);
                    }
                }
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}