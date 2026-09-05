
using System;
using AulaVirtual.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AulaVirtual.Api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AulaVirtual.Api.Models.Asignacion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AdjuntoUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CursoId")
                        .HasColumnType("integer");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("EsVisible")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("FechaVencimiento")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("PunteoMaximo")
                        .HasColumnType("numeric");

                    b.Property<int>("Tipo")
                        .HasColumnType("integer");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("Id");

                    b.HasIndex("CursoId");

                    b.ToTable("Asignaciones");
                });

            modelBuilder.Entity("AulaVirtual.Api.Models.Carrera", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Carreras");
                });

            modelBuilder.Entity("AulaVirtual.Api.Models.CarreraSemestre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CarreraId")
                        .HasColumnType("integer");

                    b.Property<int>("SemestreId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CarreraId");

                    b.HasIndex("SemestreId");

                    b.ToTable("CarreraSemestres");
                });

            modelBuilder.Entity("AulaVirtual.Api.Models.CarreraSemestreCurso", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CarreraSemestreId")
                        .HasColumnType("integer");

                    b.Property<int>("CursoId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CarreraSemestreId");

                    b.HasIndex("CursoId");

                    b.ToTable("CarreraSemestreCursos");
                });

            modelBuilder.Entity("AulaVirtual.Api.Models.Curso", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Creditos")
                        .HasColumnType("integer");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Cursos");
                });

            modelBuilder.Entity("AulaVirtual.Api.Models.CursoEstudiante", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CursoId")
                        .HasColumnType("integer");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<int>("EstudianteId")
                        .HasColumnType("integer");

                    b.Property<decimal>("NotaFinal")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("CursoId");

                    b.HasIndex("EstudianteId");

                    b.ToTable("CursoEstudiantes");
                });

            modelBuilder.Entity("AulaVirtual.Api.Models.CursoProfesor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CursoId")
                        .HasColumnType("integer");

                    b.Property<int>("ProfesorId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CursoId");

                    b.HasIndex("ProfesorId");

                    b.ToTable("CursoProfesores");
                });

            modelBuilder.Entity("AulaVirtual.Api.Models.Entrega", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AdjuntoUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("AsignacionId")
                        .HasColumnType("integer");

                    b.Property<int?>("CursoEstudianteId")
                        .HasColumnType("integer");

                    b.Property<int>("EstudianteId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("FechaEntrega")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal?>("Nota")
                        .HasColumnType("numeric");

                    b.Property<string>("Retroalimentacion")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AsignacionId");

                    b.HasIndex("CursoEstudianteId");

                    b.HasIndex("EstudianteId");

                    b.ToTable("Entregas");
                });

            modelBuilder.Entity("AulaVirtual.Api.Models.Rol", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Nombre = "Administrador"
                        },
                        new
                        {
                            Id = 2,
                            Nombre = "Docente"
                        },
                        new
                        {
                            Id = 3,
                            Nombre = "Estudiante"
                        });
                });

            modelBuilder.Entity("AulaVirtual.Api.Models.Semestre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("Semestres");
                });

            modelBuilder.Entity("AulaVirtual.Api.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaveHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("NombreCompleto")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("RolId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RolId");

                    b.ToTable("Usuarios");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ClaveHash = "admin123",
                            Correo = "admin@mesoamericana.edu",
                            NombreCompleto = "Admin Default",
                            RolId = 1
                        });
                });

            modelBuilder.Entity("AulaVirtual.Api.Models.Asignacion", b =>
                {
                    b.HasOne("AulaVirtual.Api.Models.Curso", "Curso")
                        .WithMany("Asignaciones")
                        .HasForeignKey("CursoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Curso");
                });

            modelBuilder.Entity("AulaVirtual.Api.Models.CarreraSemestre", b =>
                {
                    b.HasOne("AulaVirtual.Api.Models.Carrera", "Carrera")
                        .WithMany("Semestres")
                        .HasForeignKey("CarreraId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AulaVirtual.Api.Models.Semestre", "Semestre")
                        .WithMany("Carreras")
                        .HasForeignKey("SemestreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Carrera");

                    b.Navigation("Semestre");
                });

            modelBuilder.Entity("AulaVirtual.Api.Models.CarreraSemestreCurso", b =>
                {
                    b.HasOne("AulaVirtual.Api.Models.CarreraSemestre", "CarreraSemestre")
                        .WithMany("Cursos")
                        .HasForeignKey("CarreraSemestreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AulaVirtual.Api.Models.Curso", "Curso")
                        .WithMany()
                        .HasForeignKey("CursoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CarreraSemestre");

                    b.Navigation("Curso");
                });

            modelBuilder.Entity("AulaVirtual.Api.Models.CursoEstudiante", b =>
                {
                    b.HasOne("AulaVirtual.Api.Models.Curso", "Curso")
                        .WithMany()
                        .HasForeignKey("CursoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AulaVirtual.Api.Models.Usuario", "Estudiante")
                        .WithMany("CursosEstudiante")
                        .HasForeignKey("EstudianteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Curso");

                    b.Navigation("Estudiante");
                });

            modelBuilder.Entity("AulaVirtual.Api.Models.CursoProfesor", b =>
                {
                    b.HasOne("AulaVirtual.Api.Models.Curso", "Curso")
                        .WithMany()
                        .HasForeignKey("CursoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AulaVirtual.Api.Models.Usuario", "Profesor")
                        .WithMany("CursosProfesor")
                        .HasForeignKey("ProfesorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Curso");

                    b.Navigation("Profesor");
                });

            modelBuilder.Entity("AulaVirtual.Api.Models.Entrega", b =>
                {
                    b.HasOne("AulaVirtual.Api.Models.Asignacion", "Asignacion")
                        .WithMany("Entregas")
                        .HasForeignKey("AsignacionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AulaVirtual.Api.Models.CursoEstudiante", null)
                        .WithMany("Entregas")
                        .HasForeignKey("CursoEstudianteId");

                    b.HasOne("AulaVirtual.Api.Models.Usuario", "Estudiante")
                        .WithMany()
                        .HasForeignKey("EstudianteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Asignacion");

                    b.Navigation("Estudiante");
                });

            modelBuilder.Entity("AulaVirtual.Api.Models.Usuario", b =>
                {
                    b.HasOne("AulaVirtual.Api.Models.Rol", "Rol")
                        .WithMany("Usuarios")
                        .HasForeignKey("RolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Rol");
                });

            modelBuilder.Entity("AulaVirtual.Api.Models.Asignacion", b =>
                {
                    b.Navigation("Entregas");
                });

            modelBuilder.Entity("AulaVirtual.Api.Models.Carrera", b =>
                {
                    b.Navigation("Semestres");
                });

            modelBuilder.Entity("AulaVirtual.Api.Models.CarreraSemestre", b =>
                {
                    b.Navigation("Cursos");
                });

            modelBuilder.Entity("AulaVirtual.Api.Models.Curso", b =>
                {
                    b.Navigation("Asignaciones");
                });

            modelBuilder.Entity("AulaVirtual.Api.Models.CursoEstudiante", b =>
                {
                    b.Navigation("Entregas");
                });

            modelBuilder.Entity("AulaVirtual.Api.Models.Rol", b =>
                {
                    b.Navigation("Usuarios");
                });

            modelBuilder.Entity("AulaVirtual.Api.Models.Semestre", b =>
                {
                    b.Navigation("Carreras");
                });

            modelBuilder.Entity("AulaVirtual.Api.Models.Usuario", b =>
                {
                    b.Navigation("CursosEstudiante");

                    b.Navigation("CursosProfesor");
                });
#pragma warning restore 612, 618
        }
    }
}
