using Mapster;
using Microsoft.Extensions.DependencyInjection;
using APTRA_Gestion_de_Reservas.Models.Rutas;
using APTRA_Gestion_de_Reservas.Models.Rutas.DTOs;
using APTRA_Gestion_de_Reservas.Models.Tickets;
using APTRA_Gestion_de_Reservas.Models.Tickets.DTOs;

namespace APTRA_Gestion_de_Reservas.Mappings
{
    public static class MapsterConfiguration
    {
        public static void RegisterMapsterConfiguration(this IServiceCollection services)
        {
            TypeAdapterConfig<RutaCreateDto, Ruta>
                .NewConfig()
                .IgnoreNullValues(true);

            // Mapeo de Entidad a DTO (Ticket -> TicketDto)
            TypeAdapterConfig<Ticket, TicketDto>
                .NewConfig()
                .Map(dest => dest.Estado, src => src.Estado.ToString());
        }
    }
}
