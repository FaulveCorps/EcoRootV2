using EcoWay.Helpers;
using EcoWay.ViewModels;

namespace EcoWay.Views;

public partial class MainMenuPage : ContentPage
{
    private readonly MainMenuViewModel _viewModel;
    private bool _hasShownWelcomeToast;

    public MainMenuPage(MainMenuViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.RefreshState();

        if (_hasShownWelcomeToast)
        {
            return;
        }

        _hasShownWelcomeToast = true;
        UiFeedback.TryHaptic();
        _ = UiFeedback.ShowToastAsync("Mission Hub ready. Choose your next eco mission.");
    }

    private void OnPrimaryActionClicked(object? sender, EventArgs e)
    {
        UiFeedback.TryHaptic();
        _ = UiFeedback.ShowToastAsync("Launching Story Campaign...");
    }

    private void OnOpenImpactLabClicked(object? sender, EventArgs e)
    {
        UiFeedback.TryHaptic();
        _ = UiFeedback.ShowToastAsync("Opening Impact Lab simulation...");
    }

    private void OnOpenSummaryClicked(object? sender, EventArgs e)
    {
        UiFeedback.TryHaptic();
        _ = UiFeedback.ShowToastAsync(_viewModel.CanViewSummary
            ? "Opening run summary..."
            : "Summary is locked. Make at least one decision first.");
    }
}