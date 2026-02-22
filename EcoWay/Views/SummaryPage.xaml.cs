using EcoWay.Helpers;
using EcoWay.ViewModels;

namespace EcoWay.Views;

public partial class SummaryPage : ContentPage
{
    private readonly SummaryViewModel _viewModel;
    private bool _hasShownScoreToast;

    public SummaryPage(SummaryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.Initialize();

        if (_hasShownScoreToast)
        {
            return;
        }

        _hasShownScoreToast = true;
        UiFeedback.TryHaptic();
        _ = UiFeedback.ShowToastAsync($"Run complete. Final score: {_viewModel.TotalScore} pts");
    }

    private async void OnGoHomeClicked(object? sender, EventArgs e)
    {
        if (Shell.Current is null)
        {
            return;
        }

        UiFeedback.TryHaptic();
        _ = UiFeedback.ShowToastAsync("Returning to Mission Hub...");
        await Shell.Current.GoToAsync("//MainMenuPage");
    }

    private async void OnGoImpactLabClicked(object? sender, EventArgs e)
    {
        if (Shell.Current is null)
        {
            return;
        }

        UiFeedback.TryHaptic();
        _ = UiFeedback.ShowToastAsync("Switching to Impact Lab...");
        await Shell.Current.GoToAsync("//ImpactLabPage");
    }
}