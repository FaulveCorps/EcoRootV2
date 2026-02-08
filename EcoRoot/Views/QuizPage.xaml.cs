using System.Collections.Generic;
using System.Windows.Input;
using EcoRoot.ViewModels;

namespace EcoRoot.Views;

public partial class QuizPage : ContentPage, IQueryAttributable
{
    readonly QuizViewModel viewModel;

    public QuizPage(QuizViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        this.viewModel = viewModel;
    }

    public ICommand SelectOptionCommand => viewModel.SelectOptionCommand;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        query.TryGetValue("moduleId", out var moduleId);
        query.TryGetValue("lessonId", out var lessonId);
        viewModel.LoadQuiz(moduleId?.ToString(), lessonId?.ToString());
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
