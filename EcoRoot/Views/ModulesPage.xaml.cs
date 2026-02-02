using EcoRoot.ViewModels;

namespace EcoRoot.Views;

public partial class ModulesPage : ContentPage
{
    public ModulesPage(ModulesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
