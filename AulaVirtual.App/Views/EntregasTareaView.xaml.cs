using Microsoft.Maui.Controls;

namespace AulaVirtual.App.Views
{
    public partial class EntregasTareaView : ContentPage
    {
        public EntregasTareaView(ViewModels.EntregasTareaViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
