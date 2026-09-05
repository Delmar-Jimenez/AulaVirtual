using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using AulaVirtual.App.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using AulaVirtual.App.Services;

namespace AulaVirtual.App.ViewModels
{
    public partial class HistorialAcademicoViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        public HistorialAcademicoViewModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [ObservableProperty]
        private ObservableCollection<CursoGrupo> historialAgrupado = new();

        [ObservableProperty]
        private bool estaVacio;

        [RelayCommand]
        private async Task CargarHistorialAsync()
        {
            try
            {
                var result = await _apiService.GetAsync<HistorialDto>("estudiante/0/historial");
                if (result != null)
                {
                    HistorialAgrupado.Clear();
                    
                    if (result.Aprobados != null && result.Aprobados.Count > 0)
                        HistorialAgrupado.Add(new CursoGrupo("Cursos Aprobados", result.Aprobados));
                    
                    if (result.Reprobados != null && result.Reprobados.Count > 0)
                        HistorialAgrupado.Add(new CursoGrupo("Cursos Reprobados", result.Reprobados));
                        
                    if (result.Pendientes != null && result.Pendientes.Count > 0)
                        HistorialAgrupado.Add(new CursoGrupo("Cursos Pendientes", result.Pendientes));
                        
                    EstaVacio = HistorialAgrupado.Count == 0;
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"Error al cargar el historial: {ex.Message}", "OK");
            }
        }
    }
}
