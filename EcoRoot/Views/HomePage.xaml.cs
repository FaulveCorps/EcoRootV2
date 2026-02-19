using EcoWay.ViewModels;

namespace EcoWay.Views;

public partial class HomePage : ContentPage
{
    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        // Reset state for animation
        MainContent.Opacity = 0;
        MainContent.TranslationY = 50;

        // Animate
        await Task.WhenAll(
            MainContent.FadeTo(1, 800, Easing.CubicOut),
            MainContent.TranslateTo(0, 0, 800, Easing.CubicOut)
        );
    }
}

