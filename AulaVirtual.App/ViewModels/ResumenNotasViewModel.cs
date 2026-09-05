using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using AulaVirtual.App.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using AulaVirtual.App.Services;

namespace AulaVirtual.App.ViewModels
{
    public partial class ResumenNotasViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        public ResumenNotasViewModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [ObservableProperty]
        private Curso cursoActual = new();

        [ObservableProperty]
        private ObservableCollection<CursoEstudiante> studentGrades = new();

        [RelayCommand]
        private async Task LoadGradeSummaryAsync()
        {
            if (CursoActual.Id == 0) return;
            var result = await _apiService.GetAsync<List<CursoEstudiante>>($"docente/curso/{CursoActual.Id}/resumen-notas");
            if (result != null)
            {
                StudentGrades.Clear();
                foreach (var item in result)
                {
                    if (item.CursoId == CursoActual.Id)
                    {
                        StudentGrades.Add(item);
                    }
                }
            }
        }
    }
}
