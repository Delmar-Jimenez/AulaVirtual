using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using AulaVirtual.App.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using AulaVirtual.App.Services;
using System;
using System.IO;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Controls;

namespace AulaVirtual.App.ViewModels
{
    [QueryProperty(nameof(Asignacion), "Asignacion")]
    public partial class EntregasTareaViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        public EntregasTareaViewModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [ObservableProperty]
        private Asignacion asignacion = new();

        [ObservableProperty]
        private ObservableCollection<Entrega> entregas = new();

        [RelayCommand]
        private async Task CargarEntregasAsync()
        {
            if (Asignacion.Id == 0) return;
            var result = await _apiService.GetAsync<List<Entrega>>($"docente/asignacion/{Asignacion.Id}/entregas");
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
            if (entrega.Nota > Asignacion.PunteoMaximo)
            {
                await Shell.Current.DisplayAlert("Error", $"La nota no puede exceder los {Asignacion.PunteoMaximo} puntos.", "OK");
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

        [RelayCommand]
        private async Task AbrirArchivoAsync(string base64String)
        {
            if (string.IsNullOrWhiteSpace(base64String))
            {
                await Shell.Current.DisplayAlert("Aviso", "Esta entrega no tiene archivo adjunto.", "OK");
                return;
            }

            try
            {
                var bytes = Convert.FromBase64String(base64String);
                var tempFile = Path.Combine(FileSystem.CacheDirectory, "entrega_adjunto.pdf");
                File.WriteAllBytes(tempFile, bytes);
                await Launcher.Default.OpenAsync(new OpenFileRequest("Abrir Entrega", new ReadOnlyFile(tempFile)));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"No se pudo abrir el archivo: {ex.Message}", "OK");
            }
        }
    }
}
