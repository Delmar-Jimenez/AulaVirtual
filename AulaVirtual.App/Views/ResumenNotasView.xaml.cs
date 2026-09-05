namespace AulaVirtual.App.Views;

public partial class ResumenNotasView : ContentPage
{
    public ResumenNotasView()
    {
        InitializeComponent();
        BindingContext = MauiProgram.Services.GetService<AulaVirtual.App.ViewModels.ResumenNotasViewModel>();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is AulaVirtual.App.ViewModels.ResumenNotasViewModel vm)
        {
            vm.LoadGradeSummaryCommand.Execute(null);
        }
    }
}

