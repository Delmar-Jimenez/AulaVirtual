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
    [Authorize(Roles = "Estudiante")]
    public class EstudianteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EstudianteController(AppDbContext context)
        {
            _context = context;
        }

        private int GetUserId()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdString, out var userId) ? userId : 0;
        }

        [HttpGet("cursos")]
        public async Task<ActionResult<IEnumerable<CursoEstudiante>>> GetMisCursos()
        {
            var userId = GetUserId();
            var cursos = await _context.CursoEstudiantes
                .Include(ce => ce.Curso)
                .Where(ce => ce.EstudianteId == userId)
                .ToListAsync();

            return Ok(cursos);
        }

        [HttpGet("asignaciones")]
        public async Task<ActionResult<IEnumerable<Asignacion>>> GetTodasAsignaciones()
        {
            var userId = GetUserId();
            
            var misCursosIds = await _context.CursoEstudiantes
                .Where(ce => ce.EstudianteId == userId)
                .Select(ce => ce.CursoId)
                .ToListAsync();

            var query = await _context.Asignaciones
                .Where(a => misCursosIds.Contains(a.CursoId) && a.EsVisible)
                .Select(a => new 
                {
                    Asignacion = a,
                    Curso = a.Curso,
                    Entrega = _context.Entregas.FirstOrDefault(e => e.AsignacionId == a.Id && e.EstudianteId == userId)
                })
                .ToListAsync();

            var asignaciones = query.Select(x => 
            {
                var a = x.Asignacion;
                a.Curso = x.Curso;
                a.YaEntregada = x.Entrega != null;
                a.NotaObtenida = x.Entrega?.Nota ?? 0;
                a.Entregas = new List<Entrega>();
                return a;
            }).ToList();

            return Ok(asignaciones);
        }

        [HttpGet("curso/{cursoId}/asignaciones")]
        public async Task<ActionResult<IEnumerable<Asignacion>>> GetAsignaciones(int cursoId)
        {
            var userId = GetUserId();
            
            var isEnrolled = await _context.CursoEstudiantes
                .AnyAsync(ce => ce.EstudianteId == userId && ce.CursoId == cursoId);

            if (!isEnrolled) return Forbid("No estás inscrito en este curso.");

            var query = await _context.Asignaciones
                .Where(a => a.CursoId == cursoId && a.EsVisible)
                .Select(a => new 
                {
                    Asignacion = a,
                    Entrega = _context.Entregas.FirstOrDefault(e => e.AsignacionId == a.Id && e.EstudianteId == userId)
                })
                .ToListAsync();

            var asignaciones = query.Select(x => 
            {
                var a = x.Asignacion;
                a.YaEntregada = x.Entrega != null;
                a.NotaObtenida = x.Entrega?.Nota ?? 0;
                a.Entregas = new List<Entrega>();
                return a;
            }).ToList();

            return Ok(asignaciones);
        }

        public class EntregaDto
        {
            public string AdjuntoUrl { get; set; } = string.Empty;
        }

        [HttpPost("asignacion/{asignacionId}/entregar")]
        public async Task<IActionResult> EntregarTarea(int asignacionId, [FromBody] EntregaDto entregaDto)
        {
            var userId = GetUserId();
            
            var asignacion = await _context.Asignaciones.FindAsync(asignacionId);
            if (asignacion == null) return NotFound();

            var entrega = new Entrega
            {
                AsignacionId = asignacionId,
                EstudianteId = userId,
                AdjuntoUrl = entregaDto.AdjuntoUrl,
                FechaEntrega = DateTime.UtcNow
            };

            _context.Entregas.Add(entrega);
            await _context.SaveChangesAsync();
            return Ok(entrega);
        }

        [HttpGet("notas")]
        public async Task<ActionResult<IEnumerable<Entrega>>> GetMisNotas()
        {
            var userId = GetUserId();
            
            var notas = await _context.Entregas
                .Include(e => e.Asignacion)
                .ThenInclude(a => a.Curso)
                .Where(e => e.EstudianteId == userId && e.Nota.HasValue)
                .ToListAsync();

            return Ok(notas);
        }

        public class CursoNotaDto
        {
            public int CursoId { get; set; }
            public string NombreCurso { get; set; } = string.Empty;
            public decimal NotaAcumulada { get; set; }
        }

        [HttpGet("{estudianteId}/notas-actuales")]
        public async Task<ActionResult<IEnumerable<CursoNotaDto>>> GetNotasActuales(int estudianteId)
        {
            var userId = GetUserId();
            if (estudianteId == 0) estudianteId = userId;
            if (estudianteId != userId) return Forbid("No tienes acceso a estas notas.");

            var cursosActivos = await _context.CursoEstudiantes
                .Include(ce => ce.Curso)
                .Where(ce => ce.EstudianteId == estudianteId && ce.Estado == "Pendiente")
                .ToListAsync();

            var entregas = await _context.Entregas
                .Include(e => e.Asignacion)
                .Where(e => e.EstudianteId == estudianteId)
                .ToListAsync();

            var notas = cursosActivos.Select(ce => new CursoNotaDto
            {
                CursoId = ce.CursoId,
                NombreCurso = ce.Curso?.Nombre ?? "Desconocido",
                NotaAcumulada = entregas.Where(e => e.Asignacion?.CursoId == ce.CursoId && e.Nota.HasValue).Sum(e => e.Nota.Value)
            }).ToList();

            return Ok(notas);
        }

        public class CursoHistorialDto
        {
            public int CursoId { get; set; }
            public string NombreCurso { get; set; } = string.Empty;
            public decimal NotaFinal { get; set; }
            public string Estado { get; set; } = string.Empty;
        }

        public class HistorialDto
        {
            public List<CursoHistorialDto> Aprobados { get; set; } = new();
            public List<CursoHistorialDto> Reprobados { get; set; } = new();
            public List<CursoHistorialDto> Pendientes { get; set; } = new();
        }

        [HttpGet("{estudianteId}/historial")]
        public async Task<ActionResult<HistorialDto>> GetHistorial(int estudianteId)
        {
            var userId = GetUserId();
            if (estudianteId == 0) estudianteId = userId;
            if (estudianteId != userId) return Forbid("No tienes acceso a este historial.");

            var historial = new HistorialDto();

            var cursosTomados = await _context.CursoEstudiantes
                .Include(ce => ce.Curso)
                .Where(ce => ce.EstudianteId == estudianteId)
                .ToListAsync();

            historial.Aprobados = cursosTomados
                .Where(ce => ce.NotaFinal >= 61 && ce.Estado != "Pendiente")
                .Select(ce => new CursoHistorialDto
                {
                    CursoId = ce.CursoId,
                    NombreCurso = ce.Curso?.Nombre ?? "Desconocido",
                    NotaFinal = ce.NotaFinal,
                    Estado = "Aprobado"
                }).ToList();

            historial.Reprobados = cursosTomados
                .Where(ce => ce.NotaFinal < 61 && ce.Estado != "Pendiente")
                .Select(ce => new CursoHistorialDto
                {
                    CursoId = ce.CursoId,
                    NombreCurso = ce.Curso?.Nombre ?? "Desconocido",
                    NotaFinal = ce.NotaFinal,
                    Estado = "Reprobado"
                }).ToList();

            historial.Pendientes = cursosTomados
                .Where(ce => ce.Estado == "Pendiente")
                .Select(ce => new CursoHistorialDto
                {
                    CursoId = ce.CursoId,
                    NombreCurso = ce.Curso?.Nombre ?? "Desconocido",
                    NotaFinal = ce.NotaFinal,
                    Estado = "Pendiente"
                }).ToList();

            return Ok(historial);
        }
    }
}
