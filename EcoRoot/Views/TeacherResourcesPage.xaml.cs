using System.Windows.Input;
using EcoRoot.ViewModels;

namespace EcoRoot.Views;

public partial class TeacherResourcesPage : ContentPage
{
    readonly TeacherResourcesViewModel viewModel;

    public TeacherResourcesPage(TeacherResourcesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = this.viewModel = viewModel;
    }

    public ICommand CopyResourceCommand => viewModel.CopyResourceCommand;

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
