using Microsoft.EntityFrameworkCore;
using APTRA_Gestion_de_Reservas.Models.Rutas;

namespace APTRA_Gestion_de_Reservas.Persistence.Repositories
{
    public class RutaRepository : IRutaRepository
    {
        private readonly AptraDbContext _context;

        public RutaRepository(AptraDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ruta>> GetAllAsync()
        {
            return await _context.Rutas.ToListAsync();
        }

        public async Task<Ruta?> GetByIdAsync(int id)
        {
            return await _context.Rutas.FindAsync(id);
        }

        public async Task<Ruta> AddAsync(Ruta ruta)
        {
            _context.Rutas.Add(ruta);
            await _context.SaveChangesAsync();
            return ruta;
        }

        public async Task UpdateAsync(Ruta ruta)
        {
            _context.Entry(ruta).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ruta = await _context.Rutas.FindAsync(id);
            if (ruta != null)
            {
                _context.Rutas.Remove(ruta);
                await _context.SaveChangesAsync();
            }
        }
    }
}
