using EcoWay.ViewModels;

namespace EcoWay.Views;

public partial class MainMenuPage : ContentPage
{
    private readonly MainMenuViewModel _viewModel;
    private CancellationTokenSource? _menuAnimationCts;

    public MainMenuPage(MainMenuViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.RefreshState();
        StartAmbientAnimation();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        StopAmbientAnimation();
    }

    private void StartAmbientAnimation()
    {
        StopAmbientAnimation();
        _menuAnimationCts = new CancellationTokenSource();
        _ = RunAmbientAnimationAsync(_menuAnimationCts.Token);
    }

    private void StopAmbientAnimation()
    {
        _menuAnimationCts?.Cancel();
        _menuAnimationCts?.Dispose();
        _menuAnimationCts = null;

        SunOrb.CancelAnimations();
        CloudLeft.CancelAnimations();
        CloudRight.CancelAnimations();
        LeafIcon.CancelAnimations();
        PrimaryActionButton.CancelAnimations();
        CampaignPanel.CancelAnimations();
    }

    private async Task RunAmbientAnimationAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            await Task.WhenAll(
                SunOrb.ScaleToAsync(1.08, 1500, Easing.CubicInOut),
                SunOrb.TranslateToAsync(0, -3, 1500, Easing.CubicInOut),
                CloudLeft.TranslateToAsync(12, 0, 2200, Easing.SinInOut),
                CloudRight.TranslateToAsync(-10, 0, 2200, Easing.SinInOut),
                LeafIcon.RotateToAsync(10, 1300, Easing.CubicInOut),
                PrimaryActionButton.ScaleToAsync(1.025, 850, Easing.CubicOut),
                CampaignPanel.ScaleToAsync(1.01, 900, Easing.CubicOut));

            if (token.IsCancellationRequested)
            {
                break;
            }

            await Task.WhenAll(
                SunOrb.ScaleToAsync(1.0, 1400, Easing.CubicInOut),
                SunOrb.TranslateToAsync(0, 0, 1400, Easing.CubicInOut),
                CloudLeft.TranslateToAsync(0, 0, 2200, Easing.SinInOut),
                CloudRight.TranslateToAsync(0, 0, 2200, Easing.SinInOut),
                LeafIcon.RotateToAsync(-8, 1300, Easing.CubicInOut),
                PrimaryActionButton.ScaleToAsync(1.0, 900, Easing.CubicInOut),
                CampaignPanel.ScaleToAsync(1.0, 900, Easing.CubicInOut));
        }
    }
}