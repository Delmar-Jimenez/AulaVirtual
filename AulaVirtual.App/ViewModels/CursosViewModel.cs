using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using AulaVirtual.App.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using AulaVirtual.App.Services;

namespace AulaVirtual.App.ViewModels
{
    public partial class CursosViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        public CursosViewModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [ObservableProperty]
        private ObservableCollection<Curso> cursos = new();

        [ObservableProperty]
        private string nuevoCursoNombre = string.Empty;

        [ObservableProperty]
        private int nuevoCursoCreditos;

        [RelayCommand]
        private async Task GuardarCursoAsync()
        {
            if (string.IsNullOrWhiteSpace(NuevoCursoNombre) || NuevoCursoCreditos <= 0) return;

            var nuevoCurso = new Curso { Nombre = NuevoCursoNombre, Creditos = NuevoCursoCreditos };
            var success = await _apiService.PostAsync("cursos", nuevoCurso);
            
            if (success)
            {
                NuevoCursoNombre = string.Empty;
                NuevoCursoCreditos = 0;
                await CargarCursosAsync();
                await Shell.Current.DisplayAlert("Éxito", "Curso guardado correctamente.", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "No se pudo guardar el curso.", "OK");
            }
        }

        [RelayCommand]
        private async Task CargarCursosAsync()
        {
            var result = await _apiService.GetAsync<List<Curso>>("cursos");
            if (result != null)
            {
                Cursos.Clear();
                foreach (var item in result)
                {
                    Cursos.Add(item);
                }
            }
        }
    }
}
