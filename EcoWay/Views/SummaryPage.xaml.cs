using EcoWay.ViewModels;

namespace EcoWay.Views;

public partial class SummaryPage : ContentPage
{
    private readonly SummaryViewModel _viewModel;

    public SummaryPage(SummaryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.Initialize();
    }

    private async void OnGoHomeClicked(object? sender, EventArgs e)
    {
        if (Shell.Current is null)
        {
            return;
        }

        await Shell.Current.GoToAsync("//MainMenuPage");
    }

    private async void OnGoImpactLabClicked(object? sender, EventArgs e)
    {
        if (Shell.Current is null)
        {
            return;
        }

        await Shell.Current.GoToAsync("//ImpactLabPage");
    }
}