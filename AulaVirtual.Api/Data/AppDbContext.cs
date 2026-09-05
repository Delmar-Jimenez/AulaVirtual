using AulaVirtual.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AulaVirtual.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Carrera> Carreras { get; set; }
        public DbSet<Semestre> Semestres { get; set; }
        public DbSet<CarreraSemestre> CarreraSemestres { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<CarreraSemestreCurso> CarreraSemestreCursos { get; set; }
        public DbSet<CursoProfesor> CursoProfesores { get; set; }
        public DbSet<CursoEstudiante> CursoEstudiantes { get; set; }
        public DbSet<Asignacion> Asignaciones { get; set; }
        public DbSet<Entrega> Entregas { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Rol>().HasData(
                new Rol { Id = 1, Nombre = "Administrador" },
                new Rol { Id = 2, Nombre = "Docente" },
                new Rol { Id = 3, Nombre = "Estudiante" }
            );

            
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario { Id = 1, NombreCompleto = "Admin Default", Correo = "admin@mesoamericana.edu", ClaveHash = "admin123", RolId = 1 }
            );
        }
    }
}
