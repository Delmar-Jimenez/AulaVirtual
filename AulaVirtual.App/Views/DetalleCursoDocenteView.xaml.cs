namespace AulaVirtual.App.Views;

public partial class DetalleCursoDocenteView : ContentPage
{
    public DetalleCursoDocenteView()
    {
        InitializeComponent();
        BindingContext = MauiProgram.Services.GetService<AulaVirtual.App.ViewModels.DetalleCursoDocenteViewModel>();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (BindingContext is AulaVirtual.App.ViewModels.DetalleCursoDocenteViewModel vm)
        {
            vm.CargarAsignacionesCommand.Execute(null);
        }
    }
}

