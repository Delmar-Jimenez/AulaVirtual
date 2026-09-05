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
    public class AsignacionesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AsignacionesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("carrera-semestre")]
        public async Task<IActionResult> AssignSemesterToCareer(CarreraSemestre asignacion)
        {
            _context.CarreraSemestres.Add(asignacion);
            await _context.SaveChangesAsync();
            return Ok(asignacion);
        }

        [HttpPost("carrera-semestre-curso")]
        public async Task<IActionResult> AssignCourseToCareerSemester(CarreraSemestreCurso asignacion)
        {
            _context.CarreraSemestreCursos.Add(asignacion);
            await _context.SaveChangesAsync();
            return Ok(asignacion);
        }

        [HttpPost("curso-profesor")]
        public async Task<IActionResult> AsignarProfesorACurso(CursoProfesor asignacion)
        {
            _context.CursoProfesores.Add(asignacion);
            await _context.SaveChangesAsync();
            return Ok(asignacion);
        }

        [HttpPost("curso-estudiante")]
        public async Task<IActionResult> AsignarEstudianteACurso(CursoEstudiante asignacion)
        {
            _context.CursoEstudiantes.Add(asignacion);
            await _context.SaveChangesAsync();
            return Ok(asignacion);
        }
    }
}



