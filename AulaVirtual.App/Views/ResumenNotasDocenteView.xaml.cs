using Microsoft.Maui.Controls;

namespace AulaVirtual.App.Views
{
    public partial class ResumenNotasDocenteView : ContentPage
    {
        public ResumenNotasDocenteView(ViewModels.ResumenNotasDocenteViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
