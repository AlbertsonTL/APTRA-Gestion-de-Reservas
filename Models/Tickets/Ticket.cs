using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using APTRA_Gestion_de_Reservas.Models.Rutas;
using APTRA_Gestion_de_Reservas.Models.Tickets.Enums;

namespace APTRA_Gestion_de_Reservas.Models.Tickets
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El código de validación es obligatorio.")]
        [StringLength(50)]
        public string CodigoValidacion { get; set; } = null!;

        [Required(ErrorMessage = "El nombre del pasajero es obligatorio.")]
        [StringLength(100)]
        public string Pasajero { get; set; } = null!;

        [Required(ErrorMessage = "El documento del pasajero es obligatorio.")]
        [StringLength(50)]
        public string Documento { get; set; } = null!;

        [Required(ErrorMessage = "La ruta es obligatoria.")]
        public int RutaId { get; set; }

        [ForeignKey("RutaId")]
        public Ruta Ruta { get; set; } = null!;

        [StringLength(200)]
        public string? Trayecto { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La fecha de viaje es obligatoria.")]
        public DateTime FechaViaje { get; set; }

        [Required(ErrorMessage = "La fecha de emisión es obligatoria.")]
        public DateTime FechaEmision { get; set; }

        [Required(ErrorMessage = "El estado del ticket es obligatorio.")]
        public EstadoTicket Estado { get; set; }
    }
}
