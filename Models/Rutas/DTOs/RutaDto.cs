namespace APTRA_Gestion_de_Reservas.Models.Rutas.DTOs
{
    /// <summary>
    /// Objeto de transferencia de datos genérico para la lectura de Rutas.
    /// </summary>
    public class RutaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Origen { get; set; } = null!;
        public string Destino { get; set; } = null!;
        public decimal Precio { get; set; }
        public bool Estado { get; set; }
    }
}
