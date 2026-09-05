using AulaVirtual.App.ViewModels;

namespace AulaVirtual.App.Views
{
    public partial class InicioSesionView : ContentPage
    {
        public InicioSesionView()
        {
            try { InitializeComponent(); } catch (System.Exception ex) { System.Diagnostics.Debug.WriteLine($"InicioSesionView Error: {ex}"); }
            BindingContext = MauiProgram.Services.GetService<AulaVirtual.App.ViewModels.InicioSesionViewModel>();
        }
    }
}
