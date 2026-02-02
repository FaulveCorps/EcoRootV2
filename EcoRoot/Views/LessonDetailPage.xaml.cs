using System.Collections.Generic;
using EcoRoot.ViewModels;

namespace EcoRoot.Views;

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
}
