using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AulaVirtual.Api.Models
{
    public class Rol
    {
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Nombre { get; set; } = string.Empty; 
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }

    public class Usuario
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string NombreCompleto { get; set; } = string.Empty;
        [Required, MaxLength(100)]
        public string Correo { get; set; } = string.Empty;
        [Required]
        public string ClaveHash { get; set; } = string.Empty;
        public int RolId { get; set; }
        public Rol? Rol { get; set; }

        public ICollection<CursoProfesor> CursosProfesor { get; set; } = new List<CursoProfesor>();
        public ICollection<CursoEstudiante> CursosEstudiante { get; set; } = new List<CursoEstudiante>();
    }

    public class Carrera
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;
        public ICollection<CarreraSemestre> Semestres { get; set; } = new List<CarreraSemestre>();
    }

    public class Semestre
    {
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Nombre { get; set; } = string.Empty;
        public ICollection<CarreraSemestre> Carreras { get; set; } = new List<CarreraSemestre>();
    }

    public class CarreraSemestre
    {
        public int Id { get; set; }
        public int CarreraId { get; set; }
        public Carrera? Carrera { get; set; }
        public int SemestreId { get; set; }
        public Semestre? Semestre { get; set; }

        public ICollection<CarreraSemestreCurso> Cursos { get; set; } = new List<CarreraSemestreCurso>();
    }

    public class Curso
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;
        public int Creditos { get; set; }

        public ICollection<Asignacion> Asignaciones { get; set; } = new List<Asignacion>();
    }

    public class CarreraSemestreCurso
    {
        public int Id { get; set; }
        public int CarreraSemestreId { get; set; }
        public CarreraSemestre? CarreraSemestre { get; set; }
        public int CursoId { get; set; }
        public Curso? Curso { get; set; }
    }

    public class CursoProfesor
    {
        public int Id { get; set; }
        public int CursoId { get; set; }
        public Curso? Curso { get; set; }
        public int ProfesorId { get; set; }
        public Usuario? Profesor { get; set; }
    }

    public class CursoEstudiante
    {
        public int Id { get; set; }
        public int CursoId { get; set; }
        public Curso? Curso { get; set; }
        public int EstudianteId { get; set; }
        public Usuario? Estudiante { get; set; }

        public decimal NotaFinal { get; set; }
        [MaxLength(20)]
        public string Estado { get; set; } = "Pendiente"; 

        public ICollection<Entrega> Entregas { get; set; } = new List<Entrega>();
    }

    public enum TipoAsignacion
    {
        Tarea,
        Recurso,
        Parcial,
        ExamenFinal
    }

    public class Asignacion
    {
        public int Id { get; set; }
        public int CursoId { get; set; }
        public Curso? Curso { get; set; }

        [Required, MaxLength(200)]
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string AdjuntoUrl { get; set; } = string.Empty;

        public DateTime? FechaVencimiento { get; set; }
        public decimal PunteoMaximo { get; set; }
        public bool EsVisible { get; set; }
        public TipoAsignacion Tipo { get; set; } = TipoAsignacion.Tarea;

        public ICollection<Entrega> Entregas { get; set; } = new List<Entrega>();

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public bool YaEntregada { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public decimal NotaObtenida { get; set; }
    }

    public class Entrega
    {
        public int Id { get; set; }
        public int AsignacionId { get; set; }
        public Asignacion? Asignacion { get; set; }

        public int EstudianteId { get; set; }
        public Usuario? Estudiante { get; set; }

        public string AdjuntoUrl { get; set; } = string.Empty;
        public DateTime FechaEntrega { get; set; } = DateTime.UtcNow;
        public decimal? Nota { get; set; }
        public string Retroalimentacion { get; set; } = string.Empty;
    }
}
