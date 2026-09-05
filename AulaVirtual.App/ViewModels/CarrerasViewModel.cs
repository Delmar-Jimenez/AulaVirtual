using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using AulaVirtual.App.Models;
using System.Threading.Tasks;
using AulaVirtual.App.Services;

namespace AulaVirtual.App.ViewModels
{
    public partial class CarrerasViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        public CarrerasViewModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [ObservableProperty]
        private ObservableCollection<Carrera> carreras = new();

        [ObservableProperty]
        private string nuevaCarreraNombre = string.Empty;

        [RelayCommand]
        private async Task GuardarCarreraAsync()
        {
            if (string.IsNullOrWhiteSpace(NuevaCarreraNombre)) return;

            var nuevaCarrera = new Carrera { Nombre = NuevaCarreraNombre };
            var success = await _apiService.PostAsync("carreras", nuevaCarrera);
            
            if (success)
            {
                NuevaCarreraNombre = string.Empty;
                await CargarCarrerasAsync();
                await Shell.Current.DisplayAlert("Éxito", "Carrera guardada correctamente.", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "No se pudo guardar la carrera.", "OK");
            }
        }

        [RelayCommand]
        private async Task CargarCarrerasAsync()
        {
            var result = await _apiService.GetAsync<List<Carrera>>("carreras");
            if (result != null)
            {
                Carreras.Clear();
                foreach (var item in result)
                {
                    Carreras.Add(item);
                }
            }
        }
    }
}
