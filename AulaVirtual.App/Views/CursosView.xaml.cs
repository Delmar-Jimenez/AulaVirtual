namespace AulaVirtual.App.Views;

public partial class CursosView : ContentPage
{
    public CursosView()
    {
        InitializeComponent();
        BindingContext = MauiProgram.Services.GetService<AulaVirtual.App.ViewModels.CursosViewModel>();
    }
}

