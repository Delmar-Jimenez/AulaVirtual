namespace AulaVirtual.App.Views;

public partial class TareasPendientesView : ContentPage
{
    public TareasPendientesView()
    {
        InitializeComponent();
        BindingContext = MauiProgram.Services.GetService<AulaVirtual.App.ViewModels.TareasPendientesViewModel>();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (BindingContext is AulaVirtual.App.ViewModels.TareasPendientesViewModel vm)
        {
            vm.CargarTareasPendientesCommand.Execute(null);
        }
    }
}

