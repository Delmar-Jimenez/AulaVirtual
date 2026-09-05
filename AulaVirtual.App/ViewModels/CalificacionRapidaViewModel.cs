using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using AulaVirtual.App.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using AulaVirtual.App.Services;

namespace AulaVirtual.App.ViewModels
{
    public partial class EstudianteCalificacion : ObservableObject
    {
        [ObservableProperty]
        private int estudianteId;
        
        [ObservableProperty]
        private string nombre = string.Empty;
        
        [ObservableProperty]
        private decimal nota;
    }

    [QueryProperty(nameof(CursoId), "CursoId")]
    [QueryProperty(nameof(Tipo), "Tipo")]
    [QueryProperty(nameof(Titulo), "Titulo")]
    public partial class CalificacionRapidaViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        public CalificacionRapidaViewModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [ObservableProperty]
        private int cursoId;

        [ObservableProperty]
        private TipoAsignacion tipo;

        [ObservableProperty]
        private string titulo = string.Empty;

        [ObservableProperty]
        private ObservableCollection<EstudianteCalificacion> estudiantes = new();

        [RelayCommand]
        private async Task CargarEstudiantesAsync()
        {
            if (CursoId == 0) return;
            var result = await _apiService.GetAsync<List<ResumenEstudianteDto>>($"docente/curso/{CursoId}/resumen-notas");
            if (result != null)
            {
                Estudiantes.Clear();
                foreach (var item in result)
                {
                    Estudiantes.Add(new EstudianteCalificacion
                    {
                        EstudianteId = item.EstudianteId,
                        Nombre = item.Nombre,
                        Nota = 0 
                    });
                }
            }
        }

        [RelayCommand]
        private async Task CalificarAsync(EstudianteCalificacion est)
        {
            var payload = new
            {
                EstudianteId = est.EstudianteId,
                Tipo = Tipo,
                Titulo = Titulo,
                Nota = est.Nota
            };

            try
            {
                var success = await _apiService.PostAsync($"docente/curso/{CursoId}/calificacion-rapida", payload);
                if (success)
                {
                    await Shell.Current.DisplayAlert("Éxito", $"Nota asignada a {est.Nombre}", "OK");
                }
            }
            catch (System.Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
