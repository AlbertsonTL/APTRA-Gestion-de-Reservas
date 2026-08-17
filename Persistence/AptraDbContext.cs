using Microsoft.EntityFrameworkCore;
using APTRA_Gestion_de_Reservas.Models.Rutas;
using APTRA_Gestion_de_Reservas.Models.Tickets;
using APTRA_Gestion_de_Reservas.Models.Tickets.Enums;

namespace APTRA_Gestion_de_Reservas.Persistence
{
    public class AptraDbContext : DbContext
    {
        public AptraDbContext(DbContextOptions<AptraDbContext> options)
            : base(options)
        {
        }

        public DbSet<Ruta> Rutas { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ruta>(entity =>
            {
                entity.ToTable("Rutas");
                
                entity.ToTable(t => t.HasCheckConstraint("CK_Ruta_PrecioPositivo", "[Precio] > 0"));
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable("Tickets");
                
                // Índice único para el Código de Validación
                entity.HasIndex(t => t.CodigoValidacion).IsUnique();

                // Conversión de Enum a String
                entity.Property(t => t.Estado)
                    .HasConversion(
                        v => v.ToString(),
                        v => (EstadoTicket)Enum.Parse(typeof(EstadoTicket), v)
                    );
            });
        }
    }
}
