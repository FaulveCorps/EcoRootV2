using System.ComponentModel;
using EcoWay.ViewModels;

namespace EcoWay.Views;

public partial class ScenarioPage : ContentPage
{
    private readonly ScenarioViewModel _viewModel;
    private CancellationTokenSource? _sceneAnimationCts;
    private bool _isAppearing;

    public ScenarioPage(ScenarioViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _isAppearing = true;
        _viewModel.PropertyChanged += OnViewModelPropertyChanged;

        await _viewModel.InitializeAsync();
        await ApplyVisualConfigAsync(animated: false);
        StartAmbientAnimations();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _isAppearing = false;
        _viewModel.PropertyChanged -= OnViewModelPropertyChanged;
        StopAmbientAnimations();
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ScenarioViewModel.CurrentScenario) && _isAppearing)
        {
            MainThread.BeginInvokeOnMainThread(async () => await ApplyVisualConfigAsync(animated: true));
        }
    }

    private void StartAmbientAnimations()
    {
        StopAmbientAnimations();
        _sceneAnimationCts = new CancellationTokenSource();
        _ = RunAmbientAnimationsAsync(_sceneAnimationCts.Token);
    }

    private void StopAmbientAnimations()
    {
        _sceneAnimationCts?.Cancel();
        _sceneAnimationCts?.Dispose();
        _sceneAnimationCts = null;

        Sun.CancelAnimations();
        CloudA.CancelAnimations();
        CloudB.CancelAnimations();
        SmokePlume.CancelAnimations();
        Vehicle.CancelAnimations();
    }

    private async Task RunAmbientAnimationsAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.WhenAll(
                Sun.ScaleToAsync(1.08, 1600, Easing.CubicInOut),
                CloudA.TranslateToAsync(12, 0, 2200, Easing.SinInOut),
                CloudB.TranslateToAsync(-10, 0, 2500, Easing.SinInOut),
                SmokePlume.TranslateToAsync(0, -6, 1400, Easing.CubicInOut));

            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            await Task.WhenAll(
                Sun.ScaleToAsync(1.0, 1500, Easing.CubicInOut),
                CloudA.TranslateToAsync(0, 0, 2200, Easing.SinInOut),
                CloudB.TranslateToAsync(0, 0, 2500, Easing.SinInOut),
                SmokePlume.TranslateToAsync(0, 0, 1400, Easing.CubicInOut));
        }
    }

    private async Task ApplyVisualConfigAsync(bool animated)
    {
        var visualConfig = _viewModel.CurrentScenario?.VisualConfig?.ToLowerInvariant() ?? string.Empty;

        var skyTop = Color.FromArgb("#8AD7FF");
        var skyBottom = Color.FromArgb("#F0FCFF");
        var groundColor = Color.FromArgb("#5FA35A");
        var cityColor = Color.FromArgb("#6B7D8F");
        var roadColor = Color.FromArgb("#4D5965");
        var vehicleColor = Color.FromArgb("#F4A261");
        var smokeOpacity = 0.2;
        var waterOpacity = 0.15;
        var vehicleOffset = 0d;

        switch (visualConfig)
        {
            case "city_morning":
                skyTop = Color.FromArgb("#8DD8FF");
                skyBottom = Color.FromArgb("#F7FDFF");
                groundColor = Color.FromArgb("#65AF61");
                cityColor = Color.FromArgb("#6C8092");
                roadColor = Color.FromArgb("#4D5965");
                smokeOpacity = 0.18;
                waterOpacity = 0.08;
                vehicleOffset = -10;
                break;

            case "office_lunch":
                skyTop = Color.FromArgb("#79CAFF");
                skyBottom = Color.FromArgb("#DDF4FF");
                groundColor = Color.FromArgb("#78B86A");
                cityColor = Color.FromArgb("#5F7285");
                roadColor = Color.FromArgb("#465360");
                vehicleColor = Color.FromArgb("#E98153");
                smokeOpacity = 0.35;
                waterOpacity = 0.12;
                vehicleOffset = 6;
                break;

            case "home_evening":
                skyTop = Color.FromArgb("#4766A7");
                skyBottom = Color.FromArgb("#86A6D7");
                groundColor = Color.FromArgb("#4D8E56");
                cityColor = Color.FromArgb("#4D5F73");
                roadColor = Color.FromArgb("#3C4754");
                vehicleColor = Color.FromArgb("#7FC9D8");
                smokeOpacity = 0.14;
                waterOpacity = 0.25;
                vehicleOffset = 12;
                break;
        }

        var ecoFactor = Math.Clamp((_viewModel.TotalScore + 40d) / 80d, 0.0, 1.0);
        var treeScale = 0.84 + (ecoFactor * 0.36);
        var smokeAdjusted = Math.Clamp(smokeOpacity * (1.15 - (ecoFactor * 0.6)), 0.05, 0.55);
        var waterAdjusted = Math.Clamp(waterOpacity + (ecoFactor * 0.18), 0.06, 0.45);

        SkyGradientTop.Color = skyTop;
        SkyGradientBottom.Color = skyBottom;
        Ground.Color = groundColor;
        CityLine.Color = cityColor;
        Road.Color = roadColor;
        Vehicle.Color = vehicleColor;

        if (!animated)
        {
            SmokePlume.Opacity = smokeAdjusted;
            WaterLevel.Opacity = waterAdjusted;
            Vehicle.TranslationX = vehicleOffset;
            TreeLeftCrown.Scale = treeScale;
            TreeRightCrown.Scale = treeScale;
            return;
        }

        await Task.WhenAll(
            SmokePlume.FadeToAsync(smokeAdjusted, 420, Easing.CubicInOut),
            WaterLevel.FadeToAsync(waterAdjusted, 420, Easing.CubicInOut),
            Vehicle.TranslateToAsync(vehicleOffset, 0, 420, Easing.CubicOut),
            TreeLeftCrown.ScaleToAsync(treeScale, 420, Easing.CubicOut),
            TreeRightCrown.ScaleToAsync(treeScale, 420, Easing.CubicOut));
    }
}