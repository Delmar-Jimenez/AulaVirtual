namespace AulaVirtual.App.Views;

public partial class HistorialAcademicoView : ContentPage
{
    public HistorialAcademicoView()
    {
        InitializeComponent();
        BindingContext = MauiProgram.Services.GetService<AulaVirtual.App.ViewModels.HistorialAcademicoViewModel>();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is AulaVirtual.App.ViewModels.HistorialAcademicoViewModel vm)
        {
            vm.CargarHistorialCommand.Execute(null);
        }
    }
}

