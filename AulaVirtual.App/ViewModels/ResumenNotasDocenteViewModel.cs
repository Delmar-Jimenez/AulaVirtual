using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using AulaVirtual.App.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using AulaVirtual.App.Services;

namespace AulaVirtual.App.ViewModels
{
    [QueryProperty(nameof(CursoId), "CursoId")]
    public partial class ResumenNotasDocenteViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        public ResumenNotasDocenteViewModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [ObservableProperty]
        private int cursoId;

        [ObservableProperty]
        private ObservableCollection<ResumenEstudianteDto> resumenEstudiantes = new();

        [RelayCommand]
        private async Task CargarResumenAsync()
        {
            if (CursoId == 0) return;
            var result = await _apiService.GetAsync<List<ResumenEstudianteDto>>($"docente/curso/{CursoId}/resumen-notas");
            if (result != null)
            {
                ResumenEstudiantes.Clear();
                foreach (var item in result)
                {
                    ResumenEstudiantes.Add(item);
                }
            }
        }
    }
}
