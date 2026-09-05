using AulaVirtual.App.ViewModels;

namespace AulaVirtual.App.Views;

public partial class NotasSemestreActualView : ContentPage
{
    public NotasSemestreActualView()
    {
        InitializeComponent();
        BindingContext = MauiProgram.Services.GetService<AulaVirtual.App.ViewModels.NotasSemestreActualViewModel>();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is AulaVirtual.App.ViewModels.NotasSemestreActualViewModel vm)
        {
            vm.CargarNotasCommand.Execute(null);
        }
    }
}
