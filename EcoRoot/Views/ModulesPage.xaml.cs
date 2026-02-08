using System.Windows.Input;
using EcoRoot.ViewModels;

namespace EcoRoot.Views;

public partial class ModulesPage : ContentPage
{
    readonly ModulesViewModel viewModel;

    public ModulesPage(ModulesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = this.viewModel = viewModel;
    }

    public ICommand OpenModuleCommand => viewModel.OpenModuleCommand;

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        MainContent.Opacity = 0;
        MainContent.TranslationY = 30;

        await Task.WhenAll(
            MainContent.FadeTo(1, 600, Easing.CubicOut),
            MainContent.TranslateTo(0, 0, 600, Easing.CubicOut)
        );
    }
}
