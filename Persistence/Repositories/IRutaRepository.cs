using APTRA_Gestion_de_Reservas.Models.Rutas;

namespace APTRA_Gestion_de_Reservas.Persistence.Repositories
{
    public interface IRutaRepository
    {
        Task<IEnumerable<Ruta>> GetAllAsync();
        Task<(IEnumerable<Ruta> Items, int TotalRecords)> GetPagedAsync(int pageNumber, int pageSize);
        Task<Ruta?> GetByIdAsync(int id);
        Task<Ruta> AddAsync(Ruta ruta);
        Task UpdateAsync(Ruta ruta);
        Task DeleteAsync(int id);
    }
}
