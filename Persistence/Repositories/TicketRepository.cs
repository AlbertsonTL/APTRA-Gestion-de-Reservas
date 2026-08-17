using Microsoft.EntityFrameworkCore;
using APTRA_Gestion_de_Reservas.Models.Tickets;

namespace APTRA_Gestion_de_Reservas.Persistence.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly AptraDbContext _context;

        public TicketRepository(AptraDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Ticket> Items, int TotalRecords)> GetPagedWithRutaAsync(int pageNumber, int pageSize)
        {
            var totalRecords = await _context.Tickets.CountAsync();
            var items = await _context.Tickets
                .Include(t => t.Ruta) // Hacemos el Join/Populate
                .OrderBy(t => t.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalRecords);
        }

        public async Task<Ticket?> GetByIdAsync(int id)
        {
            return await _context.Tickets
                .Include(t => t.Ruta)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Ticket> AddAsync(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }

        public async Task UpdateAsync(Ticket ticket)
        {
            _context.Entry(ticket).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }
        }
    }
}
