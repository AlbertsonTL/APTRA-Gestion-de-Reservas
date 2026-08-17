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
            // Configuración global de Mapster (opcional, ya que Mapster mapea automáticamente propiedades con el mismo nombre)
            // Aquí podemos agregar reglas personalizadas en el futuro si los DTOs y Entidades difieren.
            
            TypeAdapterConfig<RutaCreateDto, Ruta>
                .NewConfig()
                .IgnoreNullValues(true);
        }
    }
}
