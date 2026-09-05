using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using AulaVirtual.App.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using AulaVirtual.App.Services;

namespace AulaVirtual.App.ViewModels
{
    public partial class UsuariosViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        public UsuariosViewModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [ObservableProperty]
        private ObservableCollection<Usuario> usuarios = new();

        [ObservableProperty]
        private string nuevoUsuarioNombreCompleto = string.Empty;

        [ObservableProperty]
        private string nuevoUsuarioCorreo = string.Empty;

        [ObservableProperty]
        private string nuevoUsuarioContrasena = string.Empty;

        [ObservableProperty]
        private ObservableCollection<string> rolesDisponibles = new(new[] { "Administrador", "Docente", "Estudiante" });

        [ObservableProperty]
        private string rolSeleccionado = string.Empty;

        [RelayCommand]
        private async Task GuardarUsuarioAsync()
        {
            if (string.IsNullOrWhiteSpace(NuevoUsuarioNombreCompleto) || string.IsNullOrWhiteSpace(NuevoUsuarioCorreo) || string.IsNullOrWhiteSpace(RolSeleccionado) || string.IsNullOrWhiteSpace(NuevoUsuarioContrasena)) return;

            int rolId = RolSeleccionado switch
            {
                "Administrador" => 1,
                "Docente" => 2,
                "Estudiante" => 3,
                _ => 3
            };

            var nuevoUsuario = new { NombreCompleto = NuevoUsuarioNombreCompleto, Correo = NuevoUsuarioCorreo, ClaveHash = NuevoUsuarioContrasena, RolId = rolId };
            var success = await _apiService.PostAsync("usuarios", nuevoUsuario);
            
            if (success)
            {
                NuevoUsuarioNombreCompleto = string.Empty;
                NuevoUsuarioCorreo = string.Empty;
                NuevoUsuarioContrasena = string.Empty;
                RolSeleccionado = string.Empty;
                await CargarUsuariosAsync();
                await Shell.Current.DisplayAlert("Éxito", "Usuario guardado correctamente.", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "No se pudo guardar el usuario.", "OK");
            }
        }

        [RelayCommand]
        private async Task CargarUsuariosAsync()
        {
            var result = await _apiService.GetAsync<List<Usuario>>("usuarios");
            if (result != null)
            {
                Usuarios.Clear();
                foreach (var item in result)
                {
                    Usuarios.Add(item);
                }
            }
        }
    }
}
