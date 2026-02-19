using System.Collections.Generic;
using EcoWay.ViewModels;

namespace EcoWay.Views;

public partial class LessonDetailPage : ContentPage, IQueryAttributable
{
    readonly LessonDetailViewModel viewModel;

    public LessonDetailPage(LessonDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        this.viewModel = viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        query.TryGetValue("moduleId", out var moduleId);
        query.TryGetValue("lessonId", out var lessonId);
        viewModel.LoadLesson(moduleId?.ToString(), lessonId?.ToString());
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        ContentLayout.Opacity = 0;
        ContentLayout.TranslationY = 30;

        await Task.WhenAll(
            ContentLayout.FadeTo(1, 800, Easing.CubicOut),
            ContentLayout.TranslateTo(0, 0, 800, Easing.CubicOut)
        );
    }
}

