using System.ComponentModel;
using EcoWay.Helpers;
using EcoWay.Models;
using EcoWay.ViewModels;

namespace EcoWay.Views;

public partial class ImpactLabPage : ContentPage
{
    private readonly ImpactLabViewModel _viewModel;
    private CancellationTokenSource? _animationCts;
    private readonly SemaphoreSlim _actionAnimationLock = new(1, 1);

    private readonly Color _cleanWater = Color.FromArgb("#4CA7E8");
    private readonly Color _dirtyWater = Color.FromArgb("#6E7A54");
    private readonly Color _healthyGround = Color.FromArgb("#5FA35A");
    private readonly Color _stressedGround = Color.FromArgb("#7C7A54");
    private bool _isApplyingAction;

    public ImpactLabPage(ImpactLabViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        StartAmbientAnimation();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.PropertyChanged -= OnViewModelPropertyChanged;
        StopAmbientAnimation();
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ImpactLabViewModel.HealthProgress))
        {
            MainThread.BeginInvokeOnMainThread(async () => await AnimateHealthProgressAsync());
        }

        if (e.PropertyName == nameof(ImpactLabViewModel.ActionAnimationTick) && !string.IsNullOrWhiteSpace(_viewModel.LastActionId))
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await AnimateOutcomeAsync(_viewModel.LastActionId);
                await AnimateStatusPulseAsync();
            });
        }

        if (e.PropertyName == nameof(ImpactLabViewModel.ModeChipText))
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                UiFeedback.TryHaptic();
                _ = UiFeedback.ShowToastAsync($"{_viewModel.ModeChipText} mode active.");
            });
        }
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
        CloudA.CancelAnimations();
        CloudB.CancelAnimations();
        SmokePlume.CancelAnimations();
        HealthStatusCard.CancelAnimations();
    }

    private async Task AnimateSceneAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            await Task.WhenAll(
                Sun.ScaleToAsync(1.08, 1300, Easing.CubicInOut),
                Sun.TranslateToAsync(0, -2, 1300, Easing.CubicInOut),
                CloudA.TranslateToAsync(10, 0, 1800, Easing.SinInOut),
                CloudB.TranslateToAsync(-8, 0, 1900, Easing.SinInOut),
                SmokePlume.FadeToAsync(Math.Clamp(SmokePlume.Opacity + 0.05, 0.05, 0.45), 1200, Easing.CubicInOut));

            if (token.IsCancellationRequested)
            {
                break;
            }

            await Task.WhenAll(
                Sun.ScaleToAsync(1.0, 1200, Easing.CubicInOut),
                Sun.TranslateToAsync(0, 0, 1200, Easing.CubicInOut),
                CloudA.TranslateToAsync(0, 0, 1800, Easing.SinInOut),
                CloudB.TranslateToAsync(0, 0, 1900, Easing.SinInOut),
                SmokePlume.FadeToAsync(Math.Clamp(SmokePlume.Opacity - 0.04, 0.05, 0.45), 1200, Easing.CubicInOut));
        }
    }

    private async Task AnimateHealthProgressAsync()
    {
        await LabHealthBar.ProgressTo(Math.Clamp(_viewModel.HealthProgress, 0.0, 1.0), 260, Easing.CubicInOut);
    }

    private async Task AnimateStatusPulseAsync()
    {
        await HealthStatusCard.ScaleToAsync(1.025, 120, Easing.CubicOut);
        await HealthStatusCard.ScaleToAsync(1.0, 160, Easing.CubicInOut);
    }

    private async void OnImpactActionTapped(object? sender, TappedEventArgs e)
    {
        if (_isApplyingAction)
        {
            return;
        }

        if (sender is not TapGestureRecognizer tapGesture || tapGesture.Parent is not Grid grid || grid.BindingContext is not ImpactAction action)
        {
            return;
        }

        _isApplyingAction = true;

        try
        {
            if (grid.Parent is Border card)
            {
                await card.ScaleToAsync(0.975, 90, Easing.CubicOut);
                await card.ScaleToAsync(1.0, 140, Easing.CubicInOut);
            }

            if (_viewModel.ApplyActionCommand.CanExecute(action))
            {
                UiFeedback.TryHaptic();
                _ = UiFeedback.ShowToastAsync($"Applied: {TrimForToast(action.Title)}");
                _viewModel.ApplyActionCommand.Execute(action);
            }
        }
        finally
        {
            _isApplyingAction = false;
        }
    }

    private async Task AnimateOutcomeAsync(string actionId)
    {
        await _actionAnimationLock.WaitAsync();
        try
        {
            switch (actionId)
            {
                case "harm_1":
                    await AnimateRunoffAsync();
                    break;
                case "harm_2":
                    await AnimatePlasticDumpAsync();
                    break;
                case "harm_3":
                    await AnimateChemicalStressAsync();
                    break;
                case "harm_4":
                    await AnimateDeforestationAsync();
                    break;
                case "recovery_1":
                    await AnimatePhytoremediationAsync();
                    break;
                case "recovery_2":
                    await AnimateCompostingAsync();
                    break;
                case "recovery_3":
                    await AnimateRecyclingAsync();
                    break;
                case "recovery_4":
                    await AnimateCoverCropAsync();
                    break;
            }
        }
        finally
        {
            _actionAnimationLock.Release();
        }
    }

    private async Task AnimateRunoffAsync()
    {
        Water.Color = _dirtyWater;
        await Task.WhenAll(
            PollutionOverlay.FadeToAsync(Math.Clamp(PollutionOverlay.Opacity + 0.18, 0.05, 0.98), 320, Easing.CubicOut),
            SmokePlume.FadeToAsync(Math.Clamp(SmokePlume.Opacity + 0.20, 0.08, 0.95), 280, Easing.CubicOut),
            SmokePlume.ScaleToAsync(1.08, 320, Easing.CubicOut),
            Water.FadeToAsync(Math.Clamp(Water.Opacity - 0.22, 0.12, 1.0), 320, Easing.CubicOut));

        await Task.WhenAll(
            SmokePlume.FadeToAsync(Math.Clamp(SmokePlume.Opacity - 0.14, 0.08, 0.95), 260, Easing.CubicIn),
            SmokePlume.ScaleToAsync(0.92, 260, Easing.CubicIn));
    }

    private async Task AnimatePlasticDumpAsync()
    {
        TrashLeft.TranslationY = -24;
        TrashRight.TranslationY = -30;

        await Task.WhenAll(
            TrashLeft.FadeToAsync(0.88, 260, Easing.CubicOut),
            TrashRight.FadeToAsync(0.82, 280, Easing.CubicOut),
            TrashLeft.TranslateToAsync(0, 0, 260, Easing.BounceOut),
            TrashRight.TranslateToAsync(0, 0, 280, Easing.BounceOut));

        await Task.WhenAll(
            TrashLeft.RotateToAsync(-22, 220, Easing.CubicInOut),
            TrashRight.RotateToAsync(24, 220, Easing.CubicInOut));
    }

    private async Task AnimateChemicalStressAsync()
    {
        Ground.Color = _stressedGround;

        var stressedScale = Math.Max(0.45, _viewModel.TreeScale * 0.72);
        await Task.WhenAll(
            Tree1Crown.ScaleToAsync(stressedScale, 280, Easing.CubicOut),
            Tree2Crown.ScaleToAsync(stressedScale, 280, Easing.CubicOut),
            Tree1Crown.FadeToAsync(0.72, 260, Easing.CubicOut),
            Tree2Crown.FadeToAsync(0.72, 260, Easing.CubicOut));

        await Task.WhenAll(
            Tree1Crown.ScaleToAsync(Math.Max(0.58, _viewModel.TreeScale * 0.86), 240, Easing.CubicIn),
            Tree2Crown.ScaleToAsync(Math.Max(0.58, _viewModel.TreeScale * 0.86), 240, Easing.CubicIn));
    }

    private async Task AnimateDeforestationAsync()
    {
        await Task.WhenAll(
            Tree1Crown.RotateToAsync(-52, 320, Easing.CubicIn),
            Tree2Crown.RotateToAsync(48, 320, Easing.CubicIn),
            Tree1Crown.TranslateToAsync(-24, 20, 320, Easing.CubicIn),
            Tree2Crown.TranslateToAsync(24, 20, 320, Easing.CubicIn));

        await Task.WhenAll(
            Tree1Crown.FadeToAsync(0.04, 220, Easing.CubicOut),
            Tree2Crown.FadeToAsync(0.04, 220, Easing.CubicOut),
            Tree1Trunk.FadeToAsync(0.34, 220, Easing.CubicOut),
            Tree2Trunk.FadeToAsync(0.34, 220, Easing.CubicOut),
            Tree1Stump.FadeToAsync(0.95, 200, Easing.CubicOut),
            Tree2Stump.FadeToAsync(0.95, 200, Easing.CubicOut));
    }

    private async Task AnimatePhytoremediationAsync()
    {
        Water.Color = _cleanWater;
        Ground.Color = _healthyGround;

        Tree1Stump.Opacity = 0.3;
        Tree2Stump.Opacity = 0.3;
        Tree1Trunk.Opacity = 1;
        Tree2Trunk.Opacity = 1;

        Tree1Crown.Opacity = Math.Max(Tree1Crown.Opacity, 0.25);
        Tree2Crown.Opacity = Math.Max(Tree2Crown.Opacity, 0.25);

        await Task.WhenAll(
            Tree1Crown.FadeToAsync(0.98, 280, Easing.CubicOut),
            Tree2Crown.FadeToAsync(0.98, 280, Easing.CubicOut),
            Tree1Crown.RotateToAsync(0, 260, Easing.CubicOut),
            Tree2Crown.RotateToAsync(0, 260, Easing.CubicOut),
            Tree1Crown.TranslateToAsync(0, 0, 260, Easing.CubicOut),
            Tree2Crown.TranslateToAsync(0, 0, 260, Easing.CubicOut),
            Tree1Crown.ScaleToAsync(Math.Max(0.72, _viewModel.TreeScale), 300, Easing.BounceOut),
            Tree2Crown.ScaleToAsync(Math.Max(0.72, _viewModel.TreeScale), 300, Easing.BounceOut),
            SproutLeft.FadeToAsync(0.92, 220, Easing.CubicOut),
            SproutRight.FadeToAsync(0.92, 220, Easing.CubicOut));

        await Task.WhenAll(
            SproutLeft.ScaleToAsync(1.0, 300, Easing.CubicOut),
            SproutRight.ScaleToAsync(1.0, 300, Easing.CubicOut),
            Tree1Stump.FadeToAsync(0.08, 260, Easing.CubicIn),
            Tree2Stump.FadeToAsync(0.08, 260, Easing.CubicIn));
    }

    private async Task AnimateCompostingAsync()
    {
        Ground.Color = Color.FromArgb("#6CAF63");

        await Task.WhenAll(
            SproutLeft.FadeToAsync(1.0, 220, Easing.CubicOut),
            SproutRight.FadeToAsync(1.0, 220, Easing.CubicOut),
            SproutLeft.ScaleToAsync(1.18, 240, Easing.CubicOut),
            SproutRight.ScaleToAsync(1.18, 240, Easing.CubicOut),
            Water.FadeToAsync(Math.Clamp(Water.Opacity + 0.12, 0.2, 1.0), 240, Easing.CubicOut));

        await Task.WhenAll(
            SproutLeft.ScaleToAsync(1.0, 220, Easing.CubicInOut),
            SproutRight.ScaleToAsync(1.0, 220, Easing.CubicInOut));
    }

    private async Task AnimateRecyclingAsync()
    {
        await Task.WhenAll(
            TrashLeft.TranslateToAsync(-12, -6, 220, Easing.CubicIn),
            TrashRight.TranslateToAsync(12, -8, 220, Easing.CubicIn),
            TrashLeft.FadeToAsync(0.0, 220, Easing.CubicIn),
            TrashRight.FadeToAsync(0.0, 220, Easing.CubicIn),
            PollutionOverlay.FadeToAsync(Math.Clamp(PollutionOverlay.Opacity - 0.18, 0.05, 0.98), 260, Easing.CubicOut));

        TrashLeft.TranslationX = 0;
        TrashLeft.TranslationY = 0;
        TrashRight.TranslationX = 0;
        TrashRight.TranslationY = 0;
    }

    private async Task AnimateCoverCropAsync()
    {
        CoverCropBand.Opacity = Math.Max(CoverCropBand.Opacity, 0.2);

        await Task.WhenAll(
            CoverCropBand.FadeToAsync(0.86, 240, Easing.CubicOut),
            AnimateScaleXAsync(CoverCropBand, 1.0, 260, Easing.CubicOut),
            Tree1Crown.ScaleToAsync(Math.Max(0.8, _viewModel.TreeScale * 1.05), 260, Easing.CubicOut),
            Tree2Crown.ScaleToAsync(Math.Max(0.8, _viewModel.TreeScale * 1.05), 260, Easing.CubicOut));

        await Task.WhenAll(
            Tree1Crown.ScaleToAsync(Math.Max(0.76, _viewModel.TreeScale), 220, Easing.CubicInOut),
            Tree2Crown.ScaleToAsync(Math.Max(0.76, _viewModel.TreeScale), 220, Easing.CubicInOut));
    }

    private static Task AnimateScaleXAsync(VisualElement element, double to, uint length, Easing easing)
    {
        var tcs = new TaskCompletionSource();
        var from = element.ScaleX;

        var animation = new Animation(v => element.ScaleX = v, from, to, easing);
        animation.Commit(
            owner: element,
            name: $"ScaleX_{Guid.NewGuid()}",
            length: length,
            finished: (_, _) => tcs.TrySetResult());

        return tcs.Task;
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

    private async void OnGoStoryClicked(object? sender, EventArgs e)
    {
        if (Shell.Current is null)
        {
            return;
        }

        UiFeedback.TryHaptic();
        _ = UiFeedback.ShowToastAsync("Switching to Story Mode...");
        await Shell.Current.GoToAsync("//ScenarioPage");
    }

    private static string TrimForToast(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return "impact action";
        }

        const int maxLength = 42;
        var normalized = input.Replace('\n', ' ').Trim();
        return normalized.Length <= maxLength
            ? normalized
            : $"{normalized[..(maxLength - 1)]}…";
    }
}
