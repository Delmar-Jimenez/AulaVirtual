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
    [QueryProperty(nameof(Curso), "Curso")]
    public partial class DetalleCursoEstudianteViewModel : ObservableObject, IRecipient<RefreshTasksMessage>
    {
        private readonly ApiService _apiService;

        public DetalleCursoEstudianteViewModel(ApiService apiService)
        {
            _apiService = apiService;
            WeakReferenceMessenger.Default.Register(this);
        }

        public void Receive(RefreshTasksMessage message)
        {
            _ = LoadDetailsAsync();
        }

        [ObservableProperty]
        private Curso curso = new();

        [ObservableProperty]
        private ObservableCollection<Asignacion> tasksAndResources = new();

        [RelayCommand]
        private async Task LoadDetailsAsync()
        {
            if (Curso.Id == 0) return;
            try
            {
                var result = await _apiService.GetAsync<List<Asignacion>>($"estudiante/curso/{Curso.Id}/asignaciones");
                if (result != null)
                {
                    TasksAndResources.Clear();
                    foreach (var item in result)
                    {
                        if (item.EsVisible)
                        {
                            TasksAndResources.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Error al cargar tareas: {ex.Message}", "OK");
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
