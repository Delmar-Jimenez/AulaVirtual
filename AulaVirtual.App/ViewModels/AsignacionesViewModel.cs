using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using AulaVirtual.App.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using AulaVirtual.App.Services;

namespace AulaVirtual.App.ViewModels
{
    public partial class AsignacionesViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        public AsignacionesViewModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [ObservableProperty]
        private ObservableCollection<CursoEstudiante> asignacionesEstudiante = new();

        [ObservableProperty]
        private ObservableCollection<CursoProfesor> asignacionesProfesor = new();

        [ObservableProperty]
        private ObservableCollection<Usuario> estudiantesDisponibles = new();

        [ObservableProperty]
        private ObservableCollection<Curso> cursosDisponibles = new();

        [ObservableProperty]
        private Usuario? estudianteSeleccionado;

        [ObservableProperty]
        private Curso? cursoSeleccionado;

        [RelayCommand]
        private async Task GuardarAsignacionAsync()
        {
            if (EstudianteSeleccionado == null || CursoSeleccionado == null) return;

            try
            {
                var nuevaAsignacion = new { CursoId = CursoSeleccionado.Id, EstudianteId = EstudianteSeleccionado.Id };
                var success = await _apiService.PostAsync("asignaciones/curso-estudiante", nuevaAsignacion);
                
                if (success)
                {
                    EstudianteSeleccionado = null;
                    CursoSeleccionado = null;
                    await CargarAsignacionesAsync();
                    await Shell.Current.DisplayAlert("Éxito", "Asignación guardada correctamente.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [ObservableProperty]
        private ObservableCollection<Usuario> docentesDisponibles = new();

        [ObservableProperty]
        private Usuario? docenteSeleccionado;

        [ObservableProperty]
        private Curso? cursoDocenteSeleccionado;

        [RelayCommand]
        private async Task AsignarDocenteAsync()
        {
            if (DocenteSeleccionado == null || CursoDocenteSeleccionado == null) return;

            try
            {
                var nuevaAsignacion = new { CursoId = CursoDocenteSeleccionado.Id, ProfesorId = DocenteSeleccionado.Id };
                var success = await _apiService.PostAsync("asignaciones/curso-profesor", nuevaAsignacion);
                
                if (success)
                {
                    DocenteSeleccionado = null;
                    CursoDocenteSeleccionado = null;
                    await CargarAsignacionesAsync();
                    await Shell.Current.DisplayAlert("Éxito", "Docente asignado correctamente al curso.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        private async Task CargarAsignacionesAsync()
        {
            var estudiantesResult = await _apiService.GetAsync<List<CursoEstudiante>>("asignaciones/curso-estudiante");
            if (estudiantesResult != null)
            {
                AsignacionesEstudiante.Clear();
                foreach (var item in estudiantesResult)
                {
                    AsignacionesEstudiante.Add(item);
                }
            }

            var usersResult = await _apiService.GetAsync<List<Usuario>>("usuarios");
            if (usersResult != null)
            {
                EstudiantesDisponibles.Clear();
                DocentesDisponibles.Clear();
                foreach (var u in usersResult)
                {
                    if (u.RolId == 3 || u.Rol?.Nombre == "Estudiante")
                    {
                        EstudiantesDisponibles.Add(u);
                    }
                    else if (u.RolId == 2 || u.Rol?.Nombre == "Docente")
                    {
                        DocentesDisponibles.Add(u);
                    }
                }
            }

            var cursosResult = await _apiService.GetAsync<List<Curso>>("cursos");
            if (cursosResult != null)
            {
                CursosDisponibles.Clear();
                foreach (var c in cursosResult)
                {
                    CursosDisponibles.Add(c);
                }
            }
        }
    }
}
