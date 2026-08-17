using Mapster;
using Microsoft.Extensions.DependencyInjection;
using APTRA_Gestion_de_Reservas.Models.Rutas;
using APTRA_Gestion_de_Reservas.Models.Rutas.DTOs;

namespace APTRA_Gestion_de_Reservas.Mappings
{
    public static class MapsterConfiguration
    {
        public static void RegisterMapsterConfiguration(this IServiceCollection services)
        {
            TypeAdapterConfig<RutaCreateDto, Ruta>
                .NewConfig()
                .IgnoreNullValues(true);
        }
    }
}
