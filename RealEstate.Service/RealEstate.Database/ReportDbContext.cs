using Microsoft.EntityFrameworkCore;
using RealEstate.Database.Entities;

namespace RealEstate.Database
{
    public class ReportDbContext : DbContext
    {
        public ReportDbContext(DbContextOptions<ReportDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<PropertyReport> Properties { get; set; }
        public DbSet<OwnerReport> Owners { get; set; }
        public DbSet<PropertyTraceReport> PropertyTraces { get; set; }
        public DbSet<PropertyImageReport> PropertyImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relación uno a muchos entre Property y PropertyTrace
            modelBuilder.Entity<PropertyReport>()
                .HasMany(p => p.Traces)
                .WithOne(t => t.Property)
                .HasForeignKey(t => t.IdProperty)
                .OnDelete(DeleteBehavior.Cascade);  // Elimina las trazas si se elimina la propiedad

            // Relación uno a muchos entre Property y PropertyImage
            modelBuilder.Entity<PropertyReport>()
                .HasMany(p => p.Images)
                .WithOne(i => i.Property)
                .HasForeignKey(i => i.IdProperty)
                .OnDelete(DeleteBehavior.Cascade);  // Elimina las imágenes si se elimina la propiedad

            // Relación uno a muchos entre Owner y Property
            modelBuilder.Entity<OwnerReport>()
                .HasMany(o => o.Properties)
                .WithOne(p => p.Owner)
                .HasForeignKey(p => p.IdOwner)
                .OnDelete(DeleteBehavior.Restrict); // No eliminar Owner si tiene propiedades
        }
    }
}
