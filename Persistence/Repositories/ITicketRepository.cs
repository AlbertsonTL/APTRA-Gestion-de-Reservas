using APTRA_Gestion_de_Reservas.Models.Tickets;

namespace APTRA_Gestion_de_Reservas.Persistence.Repositories
{
    public interface ITicketRepository
    {
        Task<(IEnumerable<Ticket> Items, int TotalRecords)> GetPagedWithRutaAsync(int pageNumber, int pageSize);
        Task<Ticket?> GetByIdAsync(int id);
        Task<Ticket> AddAsync(Ticket ticket);
        Task UpdateAsync(Ticket ticket);
        Task DeleteAsync(int id);
    }
}
