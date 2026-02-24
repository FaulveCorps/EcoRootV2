using System.ComponentModel;
using System.Collections.Specialized;
using EcoWay.Helpers;
using EcoWay.Models;
using EcoWay.ViewModels;

namespace EcoWay.Views;

public partial class ScenarioPage : ContentPage
{
    private readonly ScenarioViewModel _viewModel;
    private CancellationTokenSource? _sceneAnimationCts;
    private bool _isAppearing;
    private bool _isApplyingChoice;
    private double _vehicleSceneOffset;

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
        _viewModel.ActiveEffects.CollectionChanged += OnActiveEffectsChanged;

        await _viewModel.InitializeAsync();
        await ApplyVisualConfigAsync(animated: false);
        StartAmbientAnimations();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _isAppearing = false;
        _viewModel.PropertyChanged -= OnViewModelPropertyChanged;
        _viewModel.ActiveEffects.CollectionChanged -= OnActiveEffectsChanged;
        StopAmbientAnimations();
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if ((e.PropertyName == nameof(ScenarioViewModel.CurrentScenario)
            || e.PropertyName == nameof(ScenarioViewModel.LatestOutcomeVisual)
            || e.PropertyName == nameof(ScenarioViewModel.LatestImpactDelta))
            && _isAppearing)
        {
            MainThread.BeginInvokeOnMainThread(async () => await ApplyVisualConfigAsync(animated: true));
        }

        if (e.PropertyName == nameof(ScenarioViewModel.ProgressValue) && _isAppearing)
        {
            MainThread.BeginInvokeOnMainThread(async () => await AnimateProgressAsync());
        }
    }

    private void OnActiveEffectsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (!_isAppearing || e.Action != NotifyCollectionChangedAction.Add)
        {
            return;
        }

        MainThread.BeginInvokeOnMainThread(async () => await AnimateEffectsCardAsync());
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
        RippleRingA.CancelAnimations();
        RippleRingB.CancelAnimations();
        EffectsCard.CancelAnimations();
        OutcomeHaze.CancelAnimations();
        DebrisBand.CancelAnimations();
        BloomBand.CancelAnimations();
    }

    private async Task RunAmbientAnimationsAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.WhenAll(
                Sun.ScaleToAsync(1.08, 1600, Easing.CubicInOut),
                Sun.TranslateToAsync(0, -3, 1600, Easing.CubicInOut),
                CloudA.TranslateToAsync(12, 0, 2200, Easing.SinInOut),
                CloudB.TranslateToAsync(-10, 0, 2500, Easing.SinInOut),
                SmokePlume.TranslateToAsync(0, -6, 1400, Easing.CubicInOut),
                Vehicle.TranslateToAsync(_vehicleSceneOffset + 20, 0, 1400, Easing.SinInOut),
                PulseRippleAsync(RippleRingA, 0),
                PulseRippleAsync(RippleRingB, 180));

            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            await Task.WhenAll(
                Sun.ScaleToAsync(1.0, 1500, Easing.CubicInOut),
                Sun.TranslateToAsync(0, 0, 1500, Easing.CubicInOut),
                CloudA.TranslateToAsync(0, 0, 2200, Easing.SinInOut),
                CloudB.TranslateToAsync(0, 0, 2500, Easing.SinInOut),
                SmokePlume.TranslateToAsync(0, 0, 1400, Easing.CubicInOut),
                Vehicle.TranslateToAsync(_vehicleSceneOffset - 14, 0, 1400, Easing.SinInOut));
        }
    }

    private static async Task PulseRippleAsync(VisualElement ripple, uint delay)
    {
        if (delay > 0)
        {
            await Task.Delay((int)delay);
        }

        ripple.Scale = 0.42;
        ripple.Opacity = 0;

        await Task.WhenAll(
            ripple.FadeToAsync(0.48, 180, Easing.CubicOut),
            ripple.ScaleToAsync(1.28, 520, Easing.CubicOut));

        await ripple.FadeToAsync(0.0, 280, Easing.CubicIn);
    }

    private async Task AnimateProgressAsync()
    {
        await ScenarioProgressBar.ProgressTo(Math.Clamp(_viewModel.ProgressValue, 0.0, 1.0), 360, Easing.CubicInOut);
    }

    private async Task AnimateEffectsCardAsync()
    {
        await EffectsCard.ScaleToAsync(1.03, 120, Easing.CubicOut);
        await EffectsCard.ScaleToAsync(1.0, 160, Easing.CubicInOut);
    }

    private async void OnChoiceButtonClicked(object? sender, EventArgs e)
    {
        if (_isApplyingChoice)
        {
            return;
        }

        if (sender is not Button button)
        {
            return;
        }

        var choice = button.BindingContext as Choice
                     ?? FindAncestorBindingContext<Choice>(button.Parent as Element);

        if (choice is null)
        {
            _ = UiFeedback.ShowToastAsync("Could not resolve selected choice. Please try again.");
            return;
        }

        _isApplyingChoice = true;

        try
        {
            await button.ScaleToAsync(0.95, 90, Easing.CubicOut);
            await button.ScaleToAsync(1.0, 140, Easing.CubicInOut);

            if (_viewModel.MakeChoiceCommand.CanExecute(choice))
            {
                UiFeedback.TryHaptic();
                _ = UiFeedback.ShowToastAsync($"Choice applied: {TrimForToast(choice.Text)}");
                _viewModel.MakeChoiceCommand.Execute(choice);
            }
            else
            {
                _ = UiFeedback.ShowToastAsync("Choice is temporarily unavailable.");
            }
        }
        finally
        {
            _isApplyingChoice = false;
        }
    }

    private async Task ApplyVisualConfigAsync(bool animated)
    {
        var visualConfig = _viewModel.CurrentScenario?.VisualConfig?.ToLowerInvariant() ?? string.Empty;
        var outcomeVisual = _viewModel.LatestOutcomeVisual?.ToLowerInvariant() ?? string.Empty;

        var skyTop = Color.FromArgb("#8AD7FF");
        var skyBottom = Color.FromArgb("#F0FCFF");
        var groundColor = Color.FromArgb("#5FA35A");
        var cityColor = Color.FromArgb("#6B7D8F");
        var roadColor = Color.FromArgb("#4D5965");
        var vehicleColor = Color.FromArgb("#F4A261");
        var waterColor = Color.FromArgb("#4CA7E8");
        var treeLeftColor = Color.FromArgb("#2E8B57");
        var treeRightColor = Color.FromArgb("#3C9A5F");
        var treeOpacity = 1.0;
        var trunkOpacity = 1.0;
        var smokeOpacity = 0.2;
        var waterOpacity = 0.15;
        var hazeOpacity = 0.0;
        var debrisOpacity = 0.0;
        var bloomOpacity = 0.0;
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
                waterColor = Color.FromArgb("#4B93CD");
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
                waterColor = Color.FromArgb("#4E90C5");
                vehicleOffset = 12;
                break;
        }

        var ecoFactor = Math.Clamp((_viewModel.TotalScore + 40d) / 80d, 0.0, 1.0);
        var immediateFactor = Math.Clamp(Math.Abs(_viewModel.LatestImpactDelta) / 20d, 0.0, 1.0);
        var treeScale = 0.84 + (ecoFactor * 0.36);
        var smokeAdjusted = Math.Clamp(smokeOpacity * (1.15 - (ecoFactor * 0.6)), 0.05, 0.55);
        var waterAdjusted = Math.Clamp(waterOpacity + (ecoFactor * 0.18), 0.06, 0.45);

        if (_viewModel.LatestImpactDelta < 0)
        {
            smokeAdjusted += 0.16 * immediateFactor;
            waterAdjusted -= 0.12 * immediateFactor;
            treeScale -= 0.18 * immediateFactor;
            hazeOpacity += 0.06 * immediateFactor;
        }
        else if (_viewModel.LatestImpactDelta > 0)
        {
            smokeAdjusted -= 0.14 * immediateFactor;
            waterAdjusted += 0.12 * immediateFactor;
            treeScale += 0.16 * immediateFactor;
            bloomOpacity += 0.08 * immediateFactor;
        }

        switch (outcomeVisual)
        {
            case "deforestation":
                treeLeftColor = Color.FromArgb("#7D7F53");
                treeRightColor = Color.FromArgb("#7A7A50");
                treeOpacity = 0.42;
                trunkOpacity = 0.58;
                treeScale = Math.Clamp(treeScale - 0.26, 0.34, 1.4);
                smokeAdjusted += 0.10;
                hazeOpacity += 0.24;
                debrisOpacity += 0.82;
                waterColor = Color.FromArgb("#6B8466");
                break;

            case "clean_transport":
            case "active_transport":
                vehicleColor = Color.FromArgb("#6CCFAE");
                smokeAdjusted -= 0.08;
                waterAdjusted += 0.04;
                bloomOpacity += 0.42;
                hazeOpacity -= 0.04;
                break;

            case "emissions":
                smokeAdjusted += 0.14;
                cityColor = Color.FromArgb("#637485");
                waterAdjusted -= 0.06;
                hazeOpacity += 0.28;
                waterColor = Color.FromArgb("#6E89A1");
                break;

            case "waste":
                groundColor = Color.FromArgb("#748664");
                waterAdjusted -= 0.09;
                smokeAdjusted += 0.08;
                debrisOpacity += 0.68;
                hazeOpacity += 0.12;
                waterColor = Color.FromArgb("#6F8A5B");
                break;

            case "low_waste":
                groundColor = Color.FromArgb("#6BB067");
                waterAdjusted += 0.05;
                smokeAdjusted -= 0.06;
                bloomOpacity += 0.46;
                debrisOpacity -= 0.12;
                break;

            case "energy_overuse":
                skyTop = Color.FromArgb("#5E74A5");
                skyBottom = Color.FromArgb("#7F92B8");
                smokeAdjusted += 0.12;
                hazeOpacity += 0.22;
                break;

            case "energy_saver":
                skyTop = Color.FromArgb("#5177B5");
                skyBottom = Color.FromArgb("#9EC0E8");
                smokeAdjusted -= 0.10;
                waterAdjusted += 0.04;
                bloomOpacity += 0.38;
                hazeOpacity -= 0.04;
                waterColor = Color.FromArgb("#59B3EE");
                break;

            case "general_negative":
                hazeOpacity += 0.16;
                debrisOpacity += 0.24;
                break;

            case "general_positive":
                bloomOpacity += 0.32;
                hazeOpacity -= 0.04;
                break;
        }

        smokeAdjusted = Math.Clamp(smokeAdjusted, 0.05, 0.9);
        waterAdjusted = Math.Clamp(waterAdjusted, 0.04, 0.6);
        treeScale = Math.Clamp(treeScale, 0.34, 1.5);
        hazeOpacity = Math.Clamp(hazeOpacity, 0.0, 0.6);
        debrisOpacity = Math.Clamp(debrisOpacity, 0.0, 0.95);
        bloomOpacity = Math.Clamp(bloomOpacity, 0.0, 0.9);
        _vehicleSceneOffset = vehicleOffset;

        SkyGradientTop.Color = skyTop;
        SkyGradientBottom.Color = skyBottom;
        Ground.Color = groundColor;
        CityLine.Color = cityColor;
        Road.Color = roadColor;
        Vehicle.Color = vehicleColor;
        WaterLevel.Color = waterColor;
        TreeLeftCrown.Color = treeLeftColor;
        TreeRightCrown.Color = treeRightColor;

        if (!animated)
        {
            ScenarioProgressBar.Progress = _viewModel.ProgressValue;
            SmokePlume.Opacity = smokeAdjusted;
            WaterLevel.Opacity = waterAdjusted;
            Vehicle.TranslationX = vehicleOffset;
            TreeLeftCrown.Scale = treeScale;
            TreeRightCrown.Scale = treeScale;
            TreeLeftCrown.Opacity = treeOpacity;
            TreeRightCrown.Opacity = treeOpacity;
            TreeLeftTrunk.Opacity = trunkOpacity;
            TreeRightTrunk.Opacity = trunkOpacity;
            OutcomeHaze.Opacity = hazeOpacity;
            DebrisBand.Opacity = debrisOpacity;
            BloomBand.Opacity = bloomOpacity;
            return;
        }

        await Task.WhenAll(
            ScenarioProgressBar.ProgressTo(Math.Clamp(_viewModel.ProgressValue, 0.0, 1.0), 360, Easing.CubicInOut),
            SmokePlume.FadeToAsync(smokeAdjusted, 420, Easing.CubicInOut),
            WaterLevel.FadeToAsync(waterAdjusted, 420, Easing.CubicInOut),
            Vehicle.TranslateToAsync(vehicleOffset, 0, 420, Easing.CubicOut),
            TreeLeftCrown.ScaleToAsync(treeScale, 420, Easing.CubicOut),
            TreeRightCrown.ScaleToAsync(treeScale, 420, Easing.CubicOut),
            TreeLeftCrown.FadeToAsync(treeOpacity, 420, Easing.CubicInOut),
            TreeRightCrown.FadeToAsync(treeOpacity, 420, Easing.CubicInOut),
            TreeLeftTrunk.FadeToAsync(trunkOpacity, 420, Easing.CubicInOut),
                TreeRightTrunk.FadeToAsync(trunkOpacity, 420, Easing.CubicInOut),
                OutcomeHaze.FadeToAsync(hazeOpacity, 420, Easing.CubicInOut),
                DebrisBand.FadeToAsync(debrisOpacity, 420, Easing.CubicInOut),
                BloomBand.FadeToAsync(bloomOpacity, 420, Easing.CubicInOut));
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

    private static string TrimForToast(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return "decision saved";
        }

        const int maxLength = 42;
        var normalized = input.Replace('\n', ' ').Trim();
        return normalized.Length <= maxLength
            ? normalized
            : $"{normalized[..(maxLength - 1)]}…";
    }

    private static T? FindAncestorBindingContext<T>(Element? element) where T : class
    {
        var current = element;
        while (current is not null)
        {
            if (current.BindingContext is T typed)
            {
                return typed;
            }

            current = current.Parent;
        }

        return null;
    }
}