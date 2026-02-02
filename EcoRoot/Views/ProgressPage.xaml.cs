using EcoRoot.ViewModels;

namespace EcoRoot.Views;

public partial class ProgressPage : ContentPage
{
    public ProgressPage(ProgressViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
