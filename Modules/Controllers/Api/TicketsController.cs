using Microsoft.AspNetCore.Mvc;
using Mapster;
using APTRA_Gestion_de_Reservas.Models.Tickets.DTOs;
using APTRA_Gestion_de_Reservas.Persistence.Repositories;
using APTRA_Gestion_de_Reservas.Modules.Common.DTOs;

namespace APTRA_Gestion_de_Reservas.Modules.Controllers.Api
{
    /// <summary>
    /// Controlador para la gestión de Tickets de los pasajeros.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;

        /// <summary>
        /// Constructor del controlador de tickets.
        /// </summary>
        /// <param name="ticketRepository">Repositorio de tickets inyectado.</param>
        public TicketsController(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        /// <summary>
        /// Obtiene todos los tickets de forma paginada incluyendo la información de su ruta asociada.
        /// </summary>
        /// <param name="pageNumber">Número de página (por defecto 1).</param>
        /// <param name="pageSize">Tamaño de la página (por defecto 10).</param>
        /// <returns>Lista paginada de tickets.</returns>
        /// <response code="200">Devuelve la lista paginada de tickets.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResponse<TicketDto>>> GetTickets([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var (items, totalRecords) = await _ticketRepository.GetPagedWithRutaAsync(pageNumber, pageSize);
            
            // Mapster convierte Automáticamente Ticket a TicketDto, e internamente mapea Ticket.Ruta a TicketDto.RutaDto
            var dtos = items.Adapt<IEnumerable<TicketDto>>();

            var response = new PagedResponse<TicketDto>(dtos, totalRecords, pageNumber, pageSize);
            return Ok(response);
        }
    }
}
