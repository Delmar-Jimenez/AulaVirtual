using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using AulaVirtual.App.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using AulaVirtual.App.Services;

namespace AulaVirtual.App.ViewModels
{
    public partial class CursosEstudianteViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        public CursosEstudianteViewModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [ObservableProperty]
        private ObservableCollection<Curso> assignedCourses = new();

        [RelayCommand]
        private async Task CargarCursosAsync()
        {
            var result = await _apiService.GetAsync<List<CursoEstudiante>>("estudiante/cursos");
            if (result != null)
            {
                AssignedCourses.Clear();
                foreach (var item in result)
                {
                    if (item.Curso != null)
                    {
                        AssignedCourses.Add(item.Curso);
                    }
                }
            }
        }

        [RelayCommand]
        private async Task CursoSeleccionadoAsync(Curso curso)
        {
            if (curso == null) return;
            var navParam = new Dictionary<string, object>
            {
                { "Curso", curso }
            };
            await Shell.Current.GoToAsync("DetalleCursoEstudianteView", navParam);
        }

        [RelayCommand]
        private async Task IrANotasAsync()
        {
            await Shell.Current.GoToAsync("NotasSemestreActualView");
        }

        [RelayCommand]
        private async Task IrAHistorialAsync()
        {
            await Shell.Current.GoToAsync("HistorialAcademicoView");
        }
    }
}
