using EcoWay.ViewModels;

namespace EcoWay.Views;

public partial class ActionPlanPage : ContentPage
{
    public ActionPlanPage(ActionPlanViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
