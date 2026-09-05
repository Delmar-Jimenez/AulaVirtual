using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using AulaVirtual.App.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using AulaVirtual.App.Services;

namespace AulaVirtual.App.ViewModels
{
    public partial class SemestresViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        public SemestresViewModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [ObservableProperty]
        private ObservableCollection<Semestre> semestres = new();

        [ObservableProperty]
        private string nuevoSemestreNombre = string.Empty;

        [RelayCommand]
        private async Task GuardarSemestreAsync()
        {
            if (string.IsNullOrWhiteSpace(NuevoSemestreNombre)) return;

            var nuevoSemestre = new Semestre { Nombre = NuevoSemestreNombre };
            var success = await _apiService.PostAsync("semestres", nuevoSemestre);
            
            if (success)
            {
                NuevoSemestreNombre = string.Empty;
                await CargarSemestresAsync();
                await Shell.Current.DisplayAlert("Éxito", "Semestre guardado correctamente.", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "No se pudo guardar el semestre.", "OK");
            }
        }

        [RelayCommand]
        private async Task CargarSemestresAsync()
        {
            var result = await _apiService.GetAsync<List<Semestre>>("semestres");
            if (result != null)
            {
                Semestres.Clear();
                foreach (var item in result)
                {
                    Semestres.Add(item);
                }
            }
        }
    }
}
