using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using AulaVirtual.App.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using AulaVirtual.App.Services;

namespace AulaVirtual.App.ViewModels
{
    public partial class RevisionEntregasViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        public RevisionEntregasViewModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [ObservableProperty]
        private Asignacion currentAssignment = new();

        [ObservableProperty]
        private ObservableCollection<Entrega> entregas = new();

        [RelayCommand]
        private async Task CargarEntregasAsync()
        {
            if (CurrentAssignment.Id == 0) return;
            var result = await _apiService.GetAsync<List<Entrega>>($"docente/asignacion/{CurrentAssignment.Id}/entregas");
            if (result != null)
            {
                Entregas.Clear();
                foreach (var item in result)
                {
                    Entregas.Add(item);
                }
            }
        }

        [RelayCommand]
        private async Task SaveGradeAsync(Entrega entrega)
        {
            
            if (entrega.Nota > CurrentAssignment.PunteoMaximo)
            {
                await Shell.Current.DisplayAlert("Error", $"La nota no puede exceder los {CurrentAssignment.PunteoMaximo} puntos.", "OK");
                return;
            }
            
            var payload = new { Nota = entrega.Nota, Retroalimentacion = entrega.Retroalimentacion };
            var success = await _apiService.PostAsync($"docente/entrega/{entrega.Id}/calificar", payload);
            
            if (success)
            {
                await Shell.Current.DisplayAlert("Éxito", "Nota guardada exitosamente.", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Ocurrió un error al guardar la nota.", "OK");
            }
        }
    }
}
