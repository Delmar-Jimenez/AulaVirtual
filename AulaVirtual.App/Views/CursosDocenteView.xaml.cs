namespace AulaVirtual.App.Views;

public partial class CursosDocenteView : ContentPage
{
    public CursosDocenteView()
    {
        InitializeComponent();
        BindingContext = MauiProgram.Services.GetService<AulaVirtual.App.ViewModels.CursosDocenteViewModel>();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is AulaVirtual.App.ViewModels.CursosDocenteViewModel vm)
        {
            vm.CargarCursosCommand.Execute(null);
        }
    }
}

