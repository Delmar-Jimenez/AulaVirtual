namespace AulaVirtual.App.Views;

public partial class CargarTareaView : ContentPage
{
    public CargarTareaView()
    {
        InitializeComponent();
        BindingContext = MauiProgram.Services.GetService<AulaVirtual.App.ViewModels.CargarTareaViewModel>();
    }
}

