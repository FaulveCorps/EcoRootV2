using EcoWay.ViewModels;

namespace EcoWay.Views;

public partial class CausesPage : ContentPage
{
    public CausesPage(CausesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
