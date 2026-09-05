using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AulaVirtual.App.Services;
using System.Threading.Tasks;

namespace AulaVirtual.App.ViewModels
{
    public partial class PanelAdminViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        public PanelAdminViewModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [RelayCommand]
        private async Task CerrarSemestreAsync()
        {
            bool answer = await Shell.Current.DisplayAlert("Cerrar Semestre", "¿Estás seguro de cerrar el semestre? Esta acción calculará los aprobados/reprobados y archivará los cursos. Es irreversible.", "Sí, Cerrar", "Cancelar");
            if (answer)
            {
                var response = await _apiService.PostAsync<object>("admin/cerrar-semestre", new { });
                if (response != null)
                {
                    await Shell.Current.DisplayAlert("Éxito", "Semestre cerrado exitosamente.", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Aviso", "La solicitud fue enviada pero no se pudo validar la respuesta. (Es posible que se haya procesado).", "OK");
                }
            }
        }

        [RelayCommand]
        private async Task IrACarrerasAsync() => await Shell.Current.GoToAsync("CarrerasView");

        [RelayCommand]
        private async Task IrASemestresAsync() => await Shell.Current.GoToAsync("SemestresView");

        [RelayCommand]
        private async Task IrACursosAsync() => await Shell.Current.GoToAsync("CursosView");

        [RelayCommand]
        private async Task IrAUsuariosAsync() => await Shell.Current.GoToAsync("UsuariosView");

        [RelayCommand]
        private async Task IrAAsignacionesAsync() => await Shell.Current.GoToAsync("AsignacionesView");
    }
}
