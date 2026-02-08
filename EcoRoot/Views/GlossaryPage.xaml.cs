using EcoRoot.ViewModels;

namespace EcoRoot.Views;

public partial class GlossaryPage : ContentPage
{
    public GlossaryPage(GlossaryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        MainContent.Opacity = 0;
        MainContent.TranslationY = 20;
        await Task.WhenAll(
            MainContent.FadeTo(1, 400, Easing.SinOut),
            MainContent.TranslateTo(0, 0, 400, Easing.SinOut)
        );
    }
}
