using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using EcoRoot.Models;
using EcoRoot.Services;
using EcoRoot.Views;
using Microsoft.Maui.Controls;

namespace EcoRoot.ViewModels;

public sealed class LessonDetailViewModel : BaseViewModel
{
    readonly IContentService contentService;
    readonly IProgressStore progressStore;
    Lesson? lesson;
    IReadOnlyList<LessonSection> sections = Array.Empty<LessonSection>();
    string? moduleId;
    string? lessonId;
    string? moduleTitle;
    bool isCompleted;

    public LessonDetailViewModel(IContentService contentService, IProgressStore progressStore)
    {
        this.contentService = contentService;
        this.progressStore = progressStore;
        Title = "Lesson";
        StartQuizCommand = new Command(async () => await StartQuizAsync());
        progressStore.ProgressChanged += (_, _) => UpdateCompletionStatus();
    }

    public Lesson? Lesson
    {
        get => lesson;
        private set
        {
            if (SetProperty(ref lesson, value))
            {
                Title = value?.Title ?? "Lesson";
            }
        }
    }

    public string? ModuleTitle
    {
        get => moduleTitle;
        private set => SetProperty(ref moduleTitle, value);
    }

    public IReadOnlyList<LessonSection> Sections
    {
        get => sections;
        private set => SetProperty(ref sections, value);
    }

    public string? TimeEstimate => Lesson?.TimeEstimate;

    public bool HasQuiz => Lesson?.Quiz.Count > 0;

    public bool IsCompleted
    {
        get => isCompleted;
        private set
        {
            if (SetProperty(ref isCompleted, value))
            {
                OnPropertyChanged(nameof(CompletionStatus));
            }
        }
    }

    public string CompletionStatus => IsCompleted ? "Completed" : "Not completed yet";

    public ICommand StartQuizCommand { get; }

    public void LoadLesson(string? moduleId, string? lessonId)
    {
        if (string.IsNullOrWhiteSpace(moduleId) || string.IsNullOrWhiteSpace(lessonId))
        {
            Lesson = null;
            Sections = Array.Empty<LessonSection>();
            ModuleTitle = null;
            this.moduleId = null;
            this.lessonId = null;
            OnPropertyChanged(nameof(HasQuiz));
            return;
        }

        this.moduleId = moduleId;
        this.lessonId = lessonId;
        var module = contentService.GetModule(moduleId);
        ModuleTitle = module?.Title;
        Lesson = contentService.GetLesson(moduleId, lessonId);
        Sections = Lesson?.Sections ?? Array.Empty<LessonSection>();
        UpdateCompletionStatus();
        OnPropertyChanged(nameof(HasQuiz));
        OnPropertyChanged(nameof(TimeEstimate));
    }

    async Task StartQuizAsync()
    {
        if (!HasQuiz || string.IsNullOrWhiteSpace(moduleId) || string.IsNullOrWhiteSpace(lessonId))
        {
            return;
        }

        await Shell.Current.GoToAsync($"{nameof(QuizPage)}?moduleId={moduleId}&lessonId={lessonId}");
    }

    void UpdateCompletionStatus()
    {
        if (string.IsNullOrWhiteSpace(moduleId) || string.IsNullOrWhiteSpace(lessonId))
        {
            IsCompleted = false;
            return;
        }

        IsCompleted = progressStore.IsLessonCompleted(moduleId, lessonId);
    }
}
