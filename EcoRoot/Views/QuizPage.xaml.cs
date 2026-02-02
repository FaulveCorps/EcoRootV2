using System.Collections.Generic;
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

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        query.TryGetValue("moduleId", out var moduleId);
        query.TryGetValue("lessonId", out var lessonId);
        viewModel.LoadQuiz(moduleId?.ToString(), lessonId?.ToString());
    }
}
