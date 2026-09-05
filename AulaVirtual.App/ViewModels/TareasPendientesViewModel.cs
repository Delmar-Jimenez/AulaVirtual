using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using AulaVirtual.App.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using AulaVirtual.App.Services;
using CommunityToolkit.Mvvm.Messaging;

namespace AulaVirtual.App.ViewModels
{
    public partial class TareasPendientesViewModel : ObservableObject, IRecipient<RefreshTasksMessage>
    {
        private readonly ApiService _apiService;

        public TareasPendientesViewModel(ApiService apiService)
        {
            _apiService = apiService;
            WeakReferenceMessenger.Default.Register(this);
        }

        public void Receive(RefreshTasksMessage message)
        {
            _ = CargarTareasPendientesAsync();
        }

        [ObservableProperty]
        private ObservableCollection<Asignacion> tareasPendientes = new();

        [RelayCommand]
        private async Task CargarTareasPendientesAsync()
        {
            try
            {
                var result = await _apiService.GetAsync<List<Asignacion>>("estudiante/asignaciones"); 
                if (result != null)
                {
                    TareasPendientes.Clear();
                    foreach (var item in result)
                    {
                        if (item.Tipo == TipoAsignacion.Tarea && !item.YaEntregada && item.FechaVencimiento > System.DateTime.UtcNow)
                        {
                            TareasPendientes.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Error al cargar tareas pendientes: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task IrAEntregaAsync(Asignacion asignacion)
        {
            if (asignacion == null) return;
            var navParam = new Dictionary<string, object>
            {
                { "Asignacion", asignacion }
            };
            await Shell.Current.GoToAsync("CargarTareaView", navParam);
        }
    }
}
