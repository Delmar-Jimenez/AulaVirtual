using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using AulaVirtual.App.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using AulaVirtual.App.Services;

namespace AulaVirtual.App.ViewModels
{
    [QueryProperty(nameof(Curso), "Curso")]
    public partial class DetalleCursoDocenteViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        public DetalleCursoDocenteViewModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [ObservableProperty]
        private Curso curso = new();

        [ObservableProperty]
        private ObservableCollection<Asignacion> asignacionesLista = new();

        [ObservableProperty]
        private string nuevaAsignacionTitulo = string.Empty;

        [ObservableProperty]
        private string nuevaAsignacionDescripcion = string.Empty;

        [ObservableProperty]
        private decimal nuevaAsignacionPuntos;

        [ObservableProperty]
        private DateTime nuevaAsignacionVencimiento = DateTime.Today;

        [ObservableProperty]
        private bool nuevaAsignacionVisible = true;

        [ObservableProperty]
        private string? archivoBase64;

        [ObservableProperty]
        private string nombreArchivo = string.Empty;

        [RelayCommand]
        private async Task SeleccionarArchivoAsync()
        {
            try
            {
                var result = await FilePicker.Default.PickAsync();
                if (result != null)
                {
                    NombreArchivo = result.FileName;
                    using var stream = await result.OpenReadAsync();
                    using var memoryStream = new System.IO.MemoryStream();
                    await stream.CopyToAsync(memoryStream);
                    ArchivoBase64 = Convert.ToBase64String(memoryStream.ToArray());
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"No se pudo seleccionar el archivo: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task CargarAsignacionesAsync()
        {
            if (Curso.Id == 0) return;
            var result = await _apiService.GetAsync<List<Asignacion>>($"docente/curso/{Curso.Id}/asignaciones");
            if (result != null)
            {
                AsignacionesLista.Clear();
                foreach (var item in result)
                {
                    AsignacionesLista.Add(item);
                }
            }
        }

        [RelayCommand]
        private async Task CrearAsignacionAsync()
        {
            if (string.IsNullOrWhiteSpace(NuevaAsignacionTitulo)) return;

            var nuevaAsig = new Asignacion
            {
                CursoId = Curso.Id,
                Titulo = NuevaAsignacionTitulo,
                Descripcion = NuevaAsignacionDescripcion,
                PunteoMaximo = NuevaAsignacionPuntos,
                FechaVencimiento = NuevaAsignacionVencimiento,
                EsVisible = NuevaAsignacionVisible,
                Tipo = TipoAsignacion.Tarea,
                AdjuntoUrl = ArchivoBase64 ?? string.Empty
            };

            try
            {
                var success = await _apiService.PostAsync("docente/asignacion", nuevaAsig);
                if (success)
                {
                    NuevaAsignacionTitulo = string.Empty;
                    NuevaAsignacionDescripcion = string.Empty;
                    NuevaAsignacionPuntos = 0;
                    ArchivoBase64 = null;
                    NombreArchivo = string.Empty;
                    NuevaAsignacionVisible = true;
                    await CargarAsignacionesAsync();
                    await Shell.Current.DisplayAlert("Éxito", "Asignación creada correctamente.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        private async Task AsignacionSeleccionadaAsync(Asignacion asignacion)
        {
            if (asignacion == null) return;
            var navParam = new Dictionary<string, object>
            {
                { "Asignacion", asignacion }
            };
            await Shell.Current.GoToAsync("EntregasTareaView", navParam);
        }

        [RelayCommand]
        private async Task CalificacionRapidaAsync(string titulo)
        {
            var tipoEnum = titulo == "Examen Final" ? TipoAsignacion.ExamenFinal : TipoAsignacion.Parcial;
            var navParam = new Dictionary<string, object>
            {
                { "CursoId", Curso.Id },
                { "Tipo", tipoEnum },
                { "Titulo", titulo }
            };
            await Shell.Current.GoToAsync("CalificacionRapidaView", navParam);
        }

        [RelayCommand]
        private async Task VerResumenNotasAsync()
        {
            var navParam = new Dictionary<string, object>
            {
                { "CursoId", Curso.Id }
            };
            await Shell.Current.GoToAsync("ResumenNotasDocenteView", navParam);
        }

        [RelayCommand]
        private async Task EliminarAsignacionAsync(Asignacion asignacion)
        {
            if (asignacion == null) return;
            
            bool answer = await Shell.Current.DisplayAlert("Eliminar Tarea", "¿Estás seguro de eliminar esta tarea?", "Sí", "Cancelar");
            if (answer)
            {
                var response = await _apiService.DeleteAsync($"docente/tarea/{asignacion.Id}");
                if (response)
                {
                    AsignacionesLista.Remove(asignacion);
                }
            }
        }
    }
}
