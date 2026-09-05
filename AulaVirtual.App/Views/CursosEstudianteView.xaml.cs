namespace AulaVirtual.App.Views;

public partial class CursosEstudianteView : ContentPage
{
    public CursosEstudianteView()
    {
        InitializeComponent();
        BindingContext = MauiProgram.Services.GetService<AulaVirtual.App.ViewModels.CursosEstudianteViewModel>();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is AulaVirtual.App.ViewModels.CursosEstudianteViewModel vm)
        {
            vm.CargarCursosCommand.Execute(null);
        }
    }
}

