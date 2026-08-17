using Microsoft.AspNetCore.Mvc;
using Mapster;
using APTRA_Gestion_de_Reservas.Models.Tickets.DTOs;
using APTRA_Gestion_de_Reservas.Models.Tickets.Enums;
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

        /// <summary>
        /// Valida un ticket, cambiando su estado de "Active" a "Used".
        /// </summary>
        /// <param name="id">Identificador del ticket a validar.</param>
        /// <returns>El ticket ya validado.</returns>
        /// <response code="200">Devuelve el ticket con su estado actualizado a "Used".</response>
        /// <response code="404">Si no existe un ticket con el ID especificado.</response>
        /// <response code="409">Si el ticket ya se encontraba en estado "Used".</response>
        [HttpPut("{id}/validar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ValidarTicket(int id)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null)
            {
                return NotFound($"No se encontró el ticket con el ID {id}.");
            }

            if (ticket.Estado == EstadoTicket.Used)
            {
                return Conflict($"El ticket con el ID {id} ya se encuentra validado (estado 'Used').");
            }

            ticket.Estado = EstadoTicket.Used;

            await _ticketRepository.UpdateAsync(ticket);

            var dto = ticket.Adapt<TicketDto>();

            return Ok(dto);
        }

        /// <summary>
        /// Elimina un ticket existente.
        /// </summary>
        /// <param name="id">Identificador del ticket a eliminar.</param>
        /// <returns>Respuesta sin contenido.</returns>
        /// <response code="204">Si la eliminación fue exitosa.</response>
        /// <response code="404">Si no existe el ticket especificado.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticketExistente = await _ticketRepository.GetByIdAsync(id);
            if (ticketExistente == null)
            {
                return NotFound($"No se encontró el ticket con el ID {id}.");
            }

            await _ticketRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
