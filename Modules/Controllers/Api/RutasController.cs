using Microsoft.AspNetCore.Mvc;
using Mapster;
using APTRA_Gestion_de_Reservas.Models.Rutas;
using APTRA_Gestion_de_Reservas.Models.Rutas.DTOs;
using APTRA_Gestion_de_Reservas.Persistence.Repositories;

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
    }
}
