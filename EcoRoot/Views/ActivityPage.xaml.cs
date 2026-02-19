using System.Windows.Input;
using EcoWay.ViewModels;
using Microsoft.Maui.Controls.Shapes;

namespace EcoWay.Views;

public partial class ActivityPage : ContentPage
{
    readonly ActivityViewModel viewModel;

    public ActivityPage(ActivityViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = this.viewModel = viewModel;
        this.viewModel.PropertyChanged += ViewModel_PropertyChanged;
    }

    public ICommand SelectOptionCommand => viewModel.SelectOptionCommand;

    private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ActivityViewModel.CurrentScenario))
        {
            AnimateScenarioChange();
        }
        else if (e.PropertyName == nameof(ActivityViewModel.HasFeedback) && viewModel.HasFeedback)
        {
            AnimateFeedback();
        }
    }

    private async void AnimateScenarioChange()
    {
        // 1. Reset Interaction Elements
        FeedbackVisual.Opacity = 0;
        
        // 2. Animate Visualization (Sun Rise)
        SunVisual.TranslationY = 50;
        SunVisual.Opacity = 0;
        ElementsVisual.ScaleY = 0;

        // 3. Animate Content Entrance
        ScenarioCard.Opacity = 0;
        ScenarioCard.TranslationX = -20;
        OptionsList.Opacity = 0;
        OptionsList.TranslationY = 20;

        await Task.WhenAll(
            SunVisual.TranslateTo(0, 0, 1000, Easing.SpringOut),
            SunVisual.FadeTo(1, 1000),
            ElementsVisual.ScaleYTo(1, 800, Easing.SpringOut),
            ScenarioCard.FadeTo(1, 500),
            ScenarioCard.TranslateTo(0, 0, 500, Easing.CubicOut),
            OptionsList.FadeTo(1, 600, Easing.CubicIn),
            OptionsList.TranslateTo(0, 0, 600, Easing.CubicOut)
        );
    }

    private async void AnimateFeedback()
    {
        FeedbackVisual.Opacity = 0;
        FeedbackVisual.Scale = 0.8;
        
        await Task.WhenAll(
            FeedbackVisual.FadeTo(1, 400),
            FeedbackVisual.ScaleTo(1, 500, Easing.BounceOut)
        );
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        AnimateScenarioChange(); // Trigger initial animation
    }
}

