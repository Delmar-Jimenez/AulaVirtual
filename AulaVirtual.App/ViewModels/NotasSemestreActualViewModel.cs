using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using AulaVirtual.App.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using AulaVirtual.App.Services;

namespace AulaVirtual.App.ViewModels
{
    public partial class NotasSemestreActualViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        public NotasSemestreActualViewModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [ObservableProperty]
        private ObservableCollection<CursoNotaDto> misNotas = new();

        [ObservableProperty]
        private bool estaVacio;

        [RelayCommand]
        private async Task CargarNotasAsync()
        {
            try
            {
                var result = await _apiService.GetAsync<List<CursoNotaDto>>("estudiante/0/notas-actuales");
                if (result != null)
                {
                    MisNotas.Clear();
                    foreach (var item in result)
                    {
                        MisNotas.Add(item);
                    }
                }
                EstaVacio = MisNotas.Count == 0;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"Error al cargar notas: {ex.Message}", "OK");
            }
        }
    }
}
