using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AulaVirtual.App.Models
{
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    public class Usuario
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string ClaveHash { get; set; } = string.Empty;
        public int RolId { get; set; }
        public Rol? Rol { get; set; }
    }

    public class Carrera
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    public class Semestre
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    public class Curso
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Creditos { get; set; }
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
        public string Estado { get; set; } = "Pendiente";
    }

    public enum TipoAsignacion
    {
        Tarea,
        Recurso,
        Parcial,
        ExamenFinal
    }

    public partial class Asignacion : ObservableObject
    {
        public int Id { get; set; }
        public int CursoId { get; set; }
        public Curso? Curso { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string AdjuntoUrl { get; set; } = string.Empty;
        public DateTime? FechaVencimiento { get; set; }
        public decimal PunteoMaximo { get; set; }
        public bool EsVisible { get; set; }
        public TipoAsignacion Tipo { get; set; } = TipoAsignacion.Tarea;
        
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(NotaDisplay))]
        private bool yaEntregada;
        
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(NotaDisplay))]
        private decimal notaObtenida;
        
        public string NotaDisplay
        {
            get
            {
                if (NotaObtenida == 0 && !YaEntregada)
                {
                    return $"Estado: Pendiente (Valor: {PunteoMaximo} pts)";
                }
                return $"Nota asignada: {NotaObtenida}";
            }
        }
    }

    public partial class Entrega : ObservableObject
    {
        public int Id { get; set; }
        public int AsignacionId { get; set; }
        public Asignacion? Asignacion { get; set; }
        public int EstudianteId { get; set; }
        public Usuario? Estudiante { get; set; }
        public string AdjuntoUrl { get; set; } = string.Empty;
        public DateTime FechaEntrega { get; set; }
        
        [ObservableProperty]
        private decimal? nota;
        
        [ObservableProperty]
        private string retroalimentacion = string.Empty;
    }

    public class CursoNotaDto
    {
        public int CursoId { get; set; }
        public string NombreCurso { get; set; } = string.Empty;
        public decimal NotaAcumulada { get; set; }
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

    public class ResumenEstudianteDto
    {
        public int EstudianteId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal NotaAcumulada { get; set; }
    }
}
