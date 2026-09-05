using AulaVirtual.Api.Data;
using AulaVirtual.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AulaVirtual.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Docente")]
    public class DocenteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DocenteController(AppDbContext context)
        {
            _context = context;
        }

        private int GetUserId()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdString, out var userId) ? userId : 0;
        }

        [HttpGet("cursos")]
        public async Task<ActionResult<IEnumerable<CursoProfesor>>> GetMisCursos()
        {
            var userId = GetUserId();
            var cursos = await _context.CursoProfesores
                .Include(cp => cp.Curso)
                .Where(cp => cp.ProfesorId == userId)
                .ToListAsync();

            return Ok(cursos);
        }

        [HttpGet("curso/{cursoId}/asignaciones")]
        public async Task<ActionResult<IEnumerable<Asignacion>>> GetAsignaciones(int cursoId)
        {
            var userId = GetUserId();
            var isAssigned = await _context.CursoProfesores
                .AnyAsync(cp => cp.ProfesorId == userId && cp.CursoId == cursoId);

            if (!isAssigned) return Forbid("No estás asignado a este curso.");

            var asignaciones = await _context.Asignaciones
                .Where(a => a.CursoId == cursoId)
                .ToListAsync();

            return Ok(asignaciones);
        }

        public class ResumenEstudianteDto
        {
            public int EstudianteId { get; set; }
            public string Nombre { get; set; } = string.Empty;
            public decimal NotaAcumulada { get; set; }
        }

        [HttpGet("curso/{cursoId}/resumen-notas")]
        public async Task<ActionResult<IEnumerable<ResumenEstudianteDto>>> GetResumenNotas(int cursoId)
        {
            var userId = GetUserId();
            var isAssigned = await _context.CursoProfesores
                .AnyAsync(cp => cp.ProfesorId == userId && cp.CursoId == cursoId);

            if (!isAssigned) return Forbid("No estás asignado a este curso.");

            var estudiantes = await _context.CursoEstudiantes
                .Include(ce => ce.Estudiante)
                .Where(ce => ce.CursoId == cursoId)
                .ToListAsync();

            var entregasCurso = await _context.Entregas
                .Include(e => e.Asignacion)
                .Where(e => e.Asignacion!.CursoId == cursoId)
                .ToListAsync();

            var resumen = estudiantes.Select(ce => new ResumenEstudianteDto
            {
                EstudianteId = ce.EstudianteId,
                Nombre = ce.Estudiante?.NombreCompleto ?? "Desconocido",
                NotaAcumulada = entregasCurso
                    .Where(e => e.EstudianteId == ce.EstudianteId)
                    .Sum(e => e.Nota ?? 0)
            }).ToList();

            return Ok(resumen);
        }

        [HttpPost("asignacion")]
        public async Task<IActionResult> CrearAsignacion(Asignacion asignacion)
        {
            var userId = GetUserId();
            
            
            var isAssigned = await _context.CursoProfesores
                .AnyAsync(cp => cp.ProfesorId == userId && cp.CursoId == asignacion.CursoId);

            if (!isAssigned)
                return Forbid("No estás asignado a este curso.");

            
            var puntosActuales = await _context.Asignaciones
                .Where(a => a.CursoId == asignacion.CursoId)
                .SumAsync(a => a.PunteoMaximo);

            if (puntosActuales + asignacion.PunteoMaximo > 100)
            {
                return BadRequest($"No se puede crear la asignación. El curso ya tiene {puntosActuales} puntos y el límite es 100.");
            }

            if (asignacion.FechaVencimiento.HasValue)
            {
                asignacion.FechaVencimiento = asignacion.FechaVencimiento.Value.ToUniversalTime();
            }

            _context.Asignaciones.Add(asignacion);
            await _context.SaveChangesAsync();
            return Ok(asignacion);
        }

        [HttpGet("asignacion/{asignacionId}/entregas")]
        public async Task<ActionResult<IEnumerable<Entrega>>> GetEntregas(int asignacionId)
        {
            var userId = GetUserId();
            
            
            var asignacion = await _context.Asignaciones
                .Include(a => a.Entregas)
                .ThenInclude(e => e.Estudiante)
                .FirstOrDefaultAsync(a => a.Id == asignacionId);

            if (asignacion == null) return NotFound();

            var isAssigned = await _context.CursoProfesores
                .AnyAsync(cp => cp.ProfesorId == userId && cp.CursoId == asignacion.CursoId);

            if (!isAssigned) return Forbid();

            return Ok(asignacion.Entregas);
        }

        public class CalificacionDto
        {
            public decimal Nota { get; set; }
            public string Retroalimentacion { get; set; } = string.Empty;
        }

        [HttpPost("entrega/{entregaId}/calificar")]
        public async Task<IActionResult> CalificarEntrega(int entregaId, [FromBody] CalificacionDto calificacion)
        {
            var entrega = await _context.Entregas
                .Include(e => e.Asignacion)
                .FirstOrDefaultAsync(e => e.Id == entregaId);

            if (entrega == null) return NotFound();

            if (calificacion.Nota > entrega.Asignacion!.PunteoMaximo)
                return BadRequest("La nota no puede exceder el punteo máximo.");

            entrega.Nota = calificacion.Nota;
            entrega.Retroalimentacion = calificacion.Retroalimentacion;
            
            await _context.SaveChangesAsync();
            return Ok(entrega);
        }

        public class CalificacionRapidaDto
        {
            public int EstudianteId { get; set; }
            public TipoAsignacion Tipo { get; set; }
            public string Titulo { get; set; } = string.Empty;
            public decimal Nota { get; set; }
        }

        [HttpPost("curso/{cursoId}/calificacion-rapida")]
        public async Task<IActionResult> CalificacionRapida(int cursoId, [FromBody] CalificacionRapidaDto dto)
        {
            var userId = GetUserId();
            var isAssigned = await _context.CursoProfesores
                .AnyAsync(cp => cp.ProfesorId == userId && cp.CursoId == cursoId);

            if (!isAssigned) return Forbid();

            var asignacion = await _context.Asignaciones
                .FirstOrDefaultAsync(a => a.CursoId == cursoId && a.Tipo == dto.Tipo && a.Titulo == dto.Titulo);

            if (asignacion == null)
            {
                decimal punteoMax = dto.Tipo == TipoAsignacion.ExamenFinal ? 30 : 15;
                
                var puntosActuales = await _context.Asignaciones
                    .Where(a => a.CursoId == cursoId)
                    .SumAsync(a => a.PunteoMaximo);

                if (puntosActuales + punteoMax > 100)
                {
                    return BadRequest("Se excede el punteo máximo del curso (100) al crear esta asignación automáticamente.");
                }

                asignacion = new Asignacion
                {
                    CursoId = cursoId,
                    Titulo = dto.Titulo,
                    Descripcion = $"Calificación directa de {dto.Titulo}",
                    PunteoMaximo = punteoMax,
                    EsVisible = true,
                    Tipo = dto.Tipo,
                    FechaVencimiento = DateTime.UtcNow
                };
                _context.Asignaciones.Add(asignacion);
                await _context.SaveChangesAsync();
            }

            if (dto.Nota > asignacion.PunteoMaximo)
                return BadRequest($"La nota excede el máximo permitido ({asignacion.PunteoMaximo}) para {dto.Tipo}.");

            var entrega = await _context.Entregas
                .FirstOrDefaultAsync(e => e.AsignacionId == asignacion.Id && e.EstudianteId == dto.EstudianteId);

            if (entrega == null)
            {
                entrega = new Entrega
                {
                    AsignacionId = asignacion.Id,
                    EstudianteId = dto.EstudianteId,
                    FechaEntrega = DateTime.UtcNow,
                    Nota = dto.Nota,
                    Retroalimentacion = "Calificado directamente"
                };
                _context.Entregas.Add(entrega);
            }
            else
            {
                entrega.Nota = dto.Nota;
            }

            await _context.SaveChangesAsync();
            return Ok(new { Asignacion = asignacion, Entrega = entrega });
        }

        [HttpDelete("tarea/{id}")]
        public async Task<IActionResult> EliminarTarea(int id)
        {
            var userId = GetUserId();
            
            var asignacion = await _context.Asignaciones
                .Include(a => a.Entregas)
                .FirstOrDefaultAsync(a => a.Id == id);
                
            if (asignacion == null) return NotFound();

            var isAssigned = await _context.CursoProfesores
                .AnyAsync(cp => cp.ProfesorId == userId && cp.CursoId == asignacion.CursoId);

            if (!isAssigned) return Forbid("No estás asignado a este curso.");

            _context.Entregas.RemoveRange(asignacion.Entregas);
            _context.Asignaciones.Remove(asignacion);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Tarea eliminada exitosamente" });
        }
    }
}
