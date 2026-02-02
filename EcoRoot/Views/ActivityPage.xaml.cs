using EcoRoot.ViewModels;

namespace EcoRoot.Views;

public partial class ActivityPage : ContentPage
{
    public ActivityPage(ActivityViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
