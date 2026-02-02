using EcoRoot.ViewModels;

namespace EcoRoot.Views;

public partial class GlossaryPage : ContentPage
{
    public GlossaryPage(GlossaryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
