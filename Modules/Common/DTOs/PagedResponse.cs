namespace APTRA_Gestion_de_Reservas.Modules.Common.DTOs
{
    /// <summary>
    /// Envoltorio genérico para respuestas paginadas.
    /// </summary>
    /// <typeparam name="T">Tipo de dato de los elementos paginados.</typeparam>
    public class PagedResponse<T>
    {
        public IEnumerable<T> Data { get; set; } = new List<T>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalRecords / (double)PageSize);
        public bool HasNextPage => PageNumber < TotalPages;
        public bool HasPreviousPage => PageNumber > 1;

        public PagedResponse() { }

        public PagedResponse(IEnumerable<T> data, int totalRecords, int pageNumber, int pageSize)
        {
            Data = data;
            TotalRecords = totalRecords;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
