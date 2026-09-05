namespace AulaVirtual.App.Views;

public partial class CarrerasView : ContentPage
{
    public CarrerasView()
    {
        InitializeComponent();
        BindingContext = MauiProgram.Services.GetService<AulaVirtual.App.ViewModels.CarrerasViewModel>();
    }
}

