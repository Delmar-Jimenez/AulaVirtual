namespace AulaVirtual.App.Views;

public partial class DetalleCursoEstudianteView : ContentPage
{
    public DetalleCursoEstudianteView()
    {
        InitializeComponent();
        BindingContext = MauiProgram.Services.GetService<AulaVirtual.App.ViewModels.DetalleCursoEstudianteViewModel>();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is AulaVirtual.App.ViewModels.DetalleCursoEstudianteViewModel vm)
        {
            vm.LoadDetailsCommand.Execute(null);
        }
    }
}

