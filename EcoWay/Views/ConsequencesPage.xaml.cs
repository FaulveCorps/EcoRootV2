using EcoWay.ViewModels;

namespace EcoWay.Views;

public partial class ConsequencesPage : ContentPage
{
    public ConsequencesPage(ConsequencesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
