using AulaVirtual.Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AulaVirtual.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("cerrar-semestre")]
        public async Task<IActionResult> CerrarSemestre()
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var inscripciones = await _context.CursoEstudiantes
                    .Where(ce => ce.Estado == "Pendiente")
                    .ToListAsync();

                foreach (var inscripcion in inscripciones)
                {
                    var notaFinal = await _context.Entregas
                        .Where(e => e.EstudianteId == inscripcion.EstudianteId && e.Asignacion!.CursoId == inscripcion.CursoId)
                        .SumAsync(e => (decimal?)e.Nota) ?? 0;
                        
                    inscripcion.NotaFinal = notaFinal;

                    if (notaFinal >= 61)
                    {
                        inscripcion.Estado = "Aprobado";
                    }
                    else
                    {
                        inscripcion.Estado = "Reprobado";
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { message = "Semestre cerrado exitosamente." });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { message = "Error al cerrar semestre: " + ex.Message });
            }
        }
    }
}
