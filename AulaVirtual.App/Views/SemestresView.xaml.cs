namespace AulaVirtual.App.Views;

public partial class SemestresView : ContentPage
{
    public SemestresView()
    {
        InitializeComponent();
        BindingContext = MauiProgram.Services.GetService<AulaVirtual.App.ViewModels.SemestresViewModel>();
    }
}

