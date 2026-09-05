using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using AulaVirtual.App.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using AulaVirtual.App.Services;

namespace AulaVirtual.App.ViewModels
{
    public partial class CursosDocenteViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        public CursosDocenteViewModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [ObservableProperty]
        private ObservableCollection<Curso> activeCourses = new();

        [ObservableProperty]
        private ObservableCollection<Curso> inactiveCourses = new();

        [RelayCommand]
        private async Task CargarCursosAsync()
        {
            var result = await _apiService.GetAsync<List<CursoProfesor>>("docente/cursos");
            if (result != null)
            {
                ActiveCourses.Clear();
                InactiveCourses.Clear();
                foreach (var item in result)
                {
                    if (item.Curso != null)
                    {
                        ActiveCourses.Add(item.Curso);
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
            await Shell.Current.GoToAsync("DetalleCursoDocenteView", navParam);
        }
    }
}
