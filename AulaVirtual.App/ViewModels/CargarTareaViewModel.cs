using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AulaVirtual.App.Models;
using System.Threading.Tasks;
using AulaVirtual.App.Services;
using System.IO;
using System;
using CommunityToolkit.Mvvm.Messaging;

namespace AulaVirtual.App.ViewModels
{
    [QueryProperty(nameof(TareaActual), "Asignacion")]
    public partial class CargarTareaViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        public CargarTareaViewModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [ObservableProperty]
        private Asignacion tareaActual = new();

        [ObservableProperty]
        private string rutaAdjunto = string.Empty;

        [ObservableProperty]
        private string nombreArchivo = "Ningún archivo seleccionado";

        [RelayCommand]
        private async Task SeleccionarArchivoAsync()
        {
            try
            {
                var result = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = "Selecciona un archivo"
                });

                if (result != null)
                {
                    NombreArchivo = result.FileName;
                    using var stream = await result.OpenReadAsync();
                    using var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);
                    var bytes = memoryStream.ToArray();
                    RutaAdjunto = Convert.ToBase64String(bytes);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Error al seleccionar el archivo: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task EnviarTareaAsync()
        {
            if (string.IsNullOrWhiteSpace(RutaAdjunto) || TareaActual.Id == 0) return;

            var payload = new { AdjuntoUrl = RutaAdjunto };
            try
            {
                var success = await _apiService.PostAsync($"estudiante/asignacion/{TareaActual.Id}/entregar", payload);
                if (success)
                {
                    await Shell.Current.DisplayAlert("Éxito", "Tarea enviada correctamente.", "OK");
                    RutaAdjunto = string.Empty;
                    NombreArchivo = "Ningún archivo seleccionado";
                    WeakReferenceMessenger.Default.Send(new RefreshTasksMessage());
                    await Shell.Current.GoToAsync("..");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
