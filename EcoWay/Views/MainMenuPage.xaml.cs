using EcoWay.ViewModels;

namespace EcoWay.Views;

public partial class MainMenuPage : ContentPage
{
    private readonly MainMenuViewModel _viewModel;

    public MainMenuPage(MainMenuViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.RefreshState();
    }
}