using Microsoft.Maui.Controls;

namespace AulaVirtual.App.Views
{
    public partial class CalificacionRapidaView : ContentPage
    {
        public CalificacionRapidaView(ViewModels.CalificacionRapidaViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
