using AulaVirtual.App.ViewModels;

namespace AulaVirtual.App.Views
{
    public partial class PanelAdminView : ContentPage
    {
        public PanelAdminView(PanelAdminViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
