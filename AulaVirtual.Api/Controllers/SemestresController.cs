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
    public class SemestresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SemestresController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Semestre>>> ObtenerSemestres()
        {
            return await _context.Semestres.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Semestre>> ObtenerSemestre(int id)
        {
            var semestre = await _context.Semestres.FindAsync(id);
            if (semestre == null) return NotFound();
            return semestre;
        }

        [HttpPost]
        public async Task<ActionResult<Semestre>> PostSemester(Semestre semestre)
        {
            _context.Semestres.Add(semestre);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObtenerSemestre), new { id = semestre.Id }, semestre);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSemester(int id, Semestre semestre)
        {
            if (id != semestre.Id) return BadRequest();
            _context.Entry(semestre).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarSemestre(int id)
        {
            var semestre = await _context.Semestres.FindAsync(id);
            if (semestre == null) return NotFound();
            _context.Semestres.Remove(semestre);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}



