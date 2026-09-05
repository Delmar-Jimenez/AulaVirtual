using AulaVirtual.Api.Data;
using AulaVirtual.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AulaVirtual.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador")] 
    public class CarrerasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CarrerasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Carrera>>> ObtenerCarreras()
        {
            return await _context.Carreras.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Carrera>> ObtenerCarrera(int id)
        {
            var carrera = await _context.Carreras.FindAsync(id);
            if (carrera == null) return NotFound();
            return carrera;
        }

        [HttpPost]
        public async Task<ActionResult<Carrera>> PostCareer(Carrera carrera)
        {
            _context.Carreras.Add(carrera);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObtenerCarrera), new { id = carrera.Id }, carrera);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCareer(int id, Carrera carrera)
        {
            if (id != carrera.Id) return BadRequest();

            _context.Entry(carrera).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCarrera(int id)
        {
            var carrera = await _context.Carreras.FindAsync(id);
            if (carrera == null) return NotFound();

            _context.Carreras.Remove(carrera);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}



