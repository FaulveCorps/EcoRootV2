using System.Collections.Generic;
using System.Windows.Input;
using EcoRoot.ViewModels;

namespace EcoRoot.Views;

public partial class ModuleDetailPage : ContentPage, IQueryAttributable
{
    readonly ModuleDetailViewModel viewModel;

    public ModuleDetailPage(ModuleDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        this.viewModel = viewModel;
    }

    public ICommand OpenLessonCommand => viewModel.OpenLessonCommand;

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        ContentLayout.Opacity = 0;
        ContentLayout.TranslationY = 20;

        await Task.WhenAll(
            ContentLayout.FadeTo(1, 600, Easing.CubicOut),
            ContentLayout.TranslateTo(0, 0, 600, Easing.CubicOut)
        );
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("moduleId", out var value))
        {
            viewModel.LoadModule(value?.ToString());
        }
    }
}
