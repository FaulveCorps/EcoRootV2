using EcoWay.ViewModels;

namespace EcoWay.Views;

public partial class SolutionsPage : ContentPage
{
    public SolutionsPage(SolutionsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
