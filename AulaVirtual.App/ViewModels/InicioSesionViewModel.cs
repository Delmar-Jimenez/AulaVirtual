using AulaVirtual.App.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AulaVirtual.App.ViewModels
{
    public partial class InicioSesionViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        public InicioSesionViewModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [ObservableProperty]
        private string correo = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasError))]
        private string errorMessage = string.Empty;

        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        [RelayCommand]
        public async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Correo) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Por favor ingrese correo y contraseña.";
                return;
            }

            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                var role = await _apiService.LoginAsync(Correo, Password);

                if (role != null)
                {
                    
                    if (role == "Administrador")
                    {
                        await Shell.Current.GoToAsync("AdminDashboard");
                    }
                    else if (role == "Docente")
                    {
                        await Shell.Current.GoToAsync("CursosDocenteView");
                    }
                    else if (role == "Estudiante")
                    {
                        await Shell.Current.GoToAsync("CursosEstudianteView");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Aviso", $"Rol desconocido: {role}.", "OK");
                    }
                }
                else
                {
                    ErrorMessage = "Credenciales incorrectas o error de conexión.";
                    await Shell.Current.DisplayAlert("Error", ErrorMessage, "OK");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Ocurrió un error al intentar iniciar sesión.";
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}



