using Microsoft.AspNetCore.Mvc;
using Mapster;
using APTRA_Gestion_de_Reservas.Models.Rutas;
using APTRA_Gestion_de_Reservas.Models.Rutas.DTOs;
using APTRA_Gestion_de_Reservas.Persistence.Repositories;
using APTRA_Gestion_de_Reservas.Modules.Common.DTOs;

namespace APTRA_Gestion_de_Reservas.Modules.Controllers.Api
{
    /// <summary>
    /// Controlador para la gestión de Rutas de transporte.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RutasController : ControllerBase
    {
        private readonly IRutaRepository _rutaRepository;

        /// <summary>
        /// Constructor del controlador de rutas.
        /// </summary>
        /// <param name="rutaRepository">Repositorio de rutas inyectado.</param>
        public RutasController(IRutaRepository rutaRepository)
        {
            _rutaRepository = rutaRepository;
        }

        /// <summary>
        /// Crea una nueva ruta de transporte.
        /// </summary>
        /// <param name="dto">Datos de la nueva ruta.</param>
        /// <returns>La ruta recién creada.</returns>
        /// <response code="201">Devuelve la ruta recién creada.</response>
        /// <response code="400">Si el modelo enviado no es válido.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateRuta([FromBody] RutaCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var nuevaRuta = dto.Adapt<Ruta>();

            await _rutaRepository.AddAsync(nuevaRuta);

            return CreatedAtAction(nameof(CreateRuta), new { id = nuevaRuta.Id }, nuevaRuta);
        }

        /// <summary>
        /// Obtiene todas las rutas de forma paginada.
        /// </summary>
        /// <param name="pageNumber">Número de página (por defecto 1).</param>
        /// <param name="pageSize">Tamaño de la página (por defecto 10).</param>
        /// <returns>Lista paginada de rutas.</returns>
        /// <response code="200">Devuelve la lista paginada de rutas.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResponse<Ruta>>> GetRutas([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var (items, totalRecords) = await _rutaRepository.GetPagedAsync(pageNumber, pageSize);
            var response = new PagedResponse<Ruta>(items, totalRecords, pageNumber, pageSize);
            return Ok(response);
        }

        /// <summary>
        /// Actualiza una ruta existente.
        /// </summary>
        /// <param name="id">Identificador de la ruta a editar.</param>
        /// <param name="dto">Nuevos datos de la ruta.</param>
        /// <returns>Respuesta sin contenido.</returns>
        /// <response code="204">Si la actualización fue exitosa.</response>
        /// <response code="400">Si los datos enviados no son válidos.</response>
        /// <response code="404">Si no existe la ruta especificada.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateRuta(int id, [FromBody] RutaUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rutaExistente = await _rutaRepository.GetByIdAsync(id);
            if (rutaExistente == null)
            {
                return NotFound($"No se encontró la ruta con el ID {id}.");
            }

            dto.Adapt(rutaExistente);

            await _rutaRepository.UpdateAsync(rutaExistente);

            return NoContent();
        }

        /// <summary>
        /// Elimina una ruta existente.
        /// </summary>
        /// <param name="id">Identificador de la ruta a eliminar.</param>
        /// <returns>Respuesta sin contenido.</returns>
        /// <response code="204">Si la eliminación fue exitosa.</response>
        /// <response code="404">Si no existe la ruta especificada.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRuta(int id)
        {
            var rutaExistente = await _rutaRepository.GetByIdAsync(id);
            if (rutaExistente == null)
            {
                return NotFound($"No se encontró la ruta con el ID {id}.");
            }

            await _rutaRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
