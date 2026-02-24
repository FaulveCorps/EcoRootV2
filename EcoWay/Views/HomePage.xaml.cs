using EcoWay.ViewModels;

namespace EcoWay.Views;

public partial class HomePage : ContentPage
{
    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void OnOpenCausesClicked(object? sender, EventArgs e)
    {
        if (Shell.Current is null) return;
        await Shell.Current.GoToAsync("//CausesPage");
    }

    private async void OnOpenImpactClicked(object? sender, EventArgs e)
    {
        if (Shell.Current is null) return;
        await Shell.Current.GoToAsync("//ConsequencesPage");
    }

    private async void OnOpenSolutionsClicked(object? sender, EventArgs e)
    {
        if (Shell.Current is null) return;
        await Shell.Current.GoToAsync("//SolutionsPage");
    }

    private async void OnOpenActionClicked(object? sender, EventArgs e)
    {
        if (Shell.Current is null) return;
        await Shell.Current.GoToAsync("//ActionPlanPage");
    }
}
