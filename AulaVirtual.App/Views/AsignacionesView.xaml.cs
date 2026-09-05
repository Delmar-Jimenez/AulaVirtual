namespace AulaVirtual.App.Views;

public partial class AsignacionesView : ContentPage
{
    public AsignacionesView()
    {
        InitializeComponent();
        BindingContext = MauiProgram.Services.GetService<AulaVirtual.App.ViewModels.AsignacionesViewModel>();
    }
}

