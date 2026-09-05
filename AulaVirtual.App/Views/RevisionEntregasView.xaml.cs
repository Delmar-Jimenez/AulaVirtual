namespace AulaVirtual.App.Views;

public partial class RevisionEntregasView : ContentPage
{
    public RevisionEntregasView()
    {
        InitializeComponent();
        BindingContext = MauiProgram.Services.GetService<AulaVirtual.App.ViewModels.RevisionEntregasViewModel>();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is AulaVirtual.App.ViewModels.RevisionEntregasViewModel vm)
        {
            vm.CargarEntregasCommand.Execute(null);
        }
    }
}

