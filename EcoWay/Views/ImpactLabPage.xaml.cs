using EcoWay.ViewModels;

namespace EcoWay.Views;

public partial class ImpactLabPage : ContentPage
{
    private CancellationTokenSource? _animationCts;

    public ImpactLabPage(ImpactLabViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
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
        _animationCts = new CancellationTokenSource();
        _ = AnimateSceneAsync(_animationCts.Token);
    }

    private void StopAmbientAnimation()
    {
        _animationCts?.Cancel();
        _animationCts?.Dispose();
        _animationCts = null;

        Sun.CancelAnimations();
    }

    private async Task AnimateSceneAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            await Sun.ScaleToAsync(1.08, 1300, Easing.CubicInOut);

            if (token.IsCancellationRequested)
            {
                break;
            }

            await Sun.ScaleToAsync(1.0, 1200, Easing.CubicInOut);
        }
    }
}
