using Microsoft.EntityFrameworkCore;
using APTRA_Gestion_de_Reservas.Models.Rutas;

namespace APTRA_Gestion_de_Reservas.Persistence
{
    public class AptraDbContext : DbContext
    {
        public AptraDbContext(DbContextOptions<AptraDbContext> options)
            : base(options)
        {
        }

        public DbSet<Ruta> Rutas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraciones adicionales con Fluent API
            modelBuilder.Entity<Ruta>(entity =>
            {
                entity.ToTable("Rutas");
                
                // Aseguramos a nivel de base de datos que el precio debe ser positivo (SQL Server CHECK constraint)
                entity.ToTable(t => t.HasCheckConstraint("CK_Ruta_PrecioPositivo", "[Precio] > 0"));
            });
        }
    }
}
