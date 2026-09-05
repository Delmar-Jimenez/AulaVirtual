namespace AulaVirtual.App.Views;

public partial class UsuariosView : ContentPage
{
    public UsuariosView()
    {
        InitializeComponent();
        BindingContext = MauiProgram.Services.GetService<AulaVirtual.App.ViewModels.UsuariosViewModel>();
    }
}

