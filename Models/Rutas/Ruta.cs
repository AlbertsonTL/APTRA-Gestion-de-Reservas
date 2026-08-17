using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APTRA_Gestion_de_Reservas.Models.Rutas
{
    public class Ruta
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la ruta es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El origen es obligatorio.")]
        [StringLength(100)]
        public string Origen { get; set; } = null!;

        [Required(ErrorMessage = "El destino es obligatorio.")]
        [StringLength(100)]
        public string Destino { get; set; } = null!;

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El estado de la ruta es obligatorio.")]
        public bool Estado { get; set; }
    }
}
