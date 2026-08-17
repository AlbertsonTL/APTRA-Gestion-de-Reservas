using APTRA_Gestion_de_Reservas.Models.Rutas.DTOs;

namespace APTRA_Gestion_de_Reservas.Models.Tickets.DTOs
{
    /// <summary>
    /// Objeto de transferencia de datos para la lectura de Tickets.
    /// Contiene la información de la ruta anidada y el estado convertido a texto.
    /// </summary>
    public class TicketDto
    {
        public int Id { get; set; }
        public string CodigoValidacion { get; set; } = null!;
        public string Pasajero { get; set; } = null!;
        public string Documento { get; set; } = null!;
        public string? Trayecto { get; set; }
        public decimal Precio { get; set; }
        public DateTime FechaViaje { get; set; }
        public DateTime FechaEmision { get; set; }
        public string Estado { get; set; } = null!;

        // Objeto Ruta anidado
        public RutaDto Ruta { get; set; } = null!;
    }
}
