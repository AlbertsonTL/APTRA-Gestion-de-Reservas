using System.ComponentModel.DataAnnotations;

namespace APTRA_Gestion_de_Reservas.Models.Rutas.DTOs
{
    /// <summary>
    /// Objeto de transferencia de datos para la actualización de una ruta.
    /// </summary>
    public class RutaUpdateDto
    {
        /// <summary>
        /// Nombre identificativo de la ruta.
        /// </summary>
        [Required(ErrorMessage = "El nombre de la ruta es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; } = null!;

        /// <summary>
        /// Ciudad o punto de origen de la ruta.
        /// </summary>
        [Required(ErrorMessage = "El origen es obligatorio.")]
        [StringLength(100)]
        public string Origen { get; set; } = null!;

        /// <summary>
        /// Ciudad o punto de destino de la ruta.
        /// </summary>
        [Required(ErrorMessage = "El destino es obligatorio.")]
        [StringLength(100)]
        public string Destino { get; set; } = null!;

        /// <summary>
        /// Precio del pasaje para esta ruta. Debe ser mayor a cero.
        /// </summary>
        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero.")]
        public decimal Precio { get; set; }

        /// <summary>
        /// Estado operativo de la ruta. True si está activa.
        /// </summary>
        [Required(ErrorMessage = "El estado de la ruta es obligatorio.")]
        public bool Estado { get; set; }
    }
}
