using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Input;
using EcoRoot.Resources;
using EcoRoot.Services;
using EcoRoot.Views;
using Microsoft.Maui.Controls;

namespace EcoRoot.ViewModels;

public sealed class HomeViewModel : BaseViewModel
{
    readonly IContentService contentService;
    readonly IProgressStore progressStore;
    string progressSummary = string.Empty;
    double progressRatio;

    public HomeViewModel(IContentService contentService, IProgressStore progressStore)
    {
        this.contentService = contentService;
        this.progressStore = progressStore;
        Title = Strings.HomeTitle;
        PrimaryActionCommand = new Command(async () => await Shell.Current.GoToAsync($"{nameof(ModuleDetailPage)}?moduleId=foundations"));
        SecondaryActionCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(GlossaryPage)));
        TeacherResourcesCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(TeacherResourcesPage)));
        ActivityCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(ActivityPage)));
        UpdateProgressSummary();
        progressStore.ProgressChanged += (_, _) => UpdateProgressSummary();
    }

    public string Headline => Strings.HomeHeadline;

    public string Subtitle => Strings.HomeSubtitle;

    public string FocusDescription => Strings.HomeFocusDescription;

    public string PrimaryActionText => Strings.HomePrimaryAction;

    public string SecondaryActionText => Strings.HomeSecondaryAction;

    public string TeacherResourcesText => Strings.HomeTeacherResourcesAction;

    public string ActivityText => Strings.ActivityAction;

    public string ProgressSummary
    {
        get => progressSummary;
        private set => SetProperty(ref progressSummary, value);
    }

    public double ProgressRatio
    {
        get => progressRatio;
        private set => SetProperty(ref progressRatio, value);
    }

    public ICommand PrimaryActionCommand { get; }

    public ICommand SecondaryActionCommand { get; }

    public ICommand TeacherResourcesCommand { get; }

    public ICommand ActivityCommand { get; }

    public IReadOnlyList<string> LearningPillars { get; } = new[]
    {
        "Foundations",
        "Pollution Sources",
        "Impacts",
        "Solutions",
        "Local Context"
    };

    void UpdateProgressSummary()
    {
        var total = 0;
        var completed = 0;

        foreach (var module in contentService.GetModules())
        {
            total += module.Lessons.Count;
            foreach (var lesson in module.Lessons)
            {
                if (progressStore.IsLessonCompleted(module.Id, lesson.Id))
                {
                    completed++;
                }
            }
        }

        ProgressSummary = total == 0
            ? Strings.HomeProgressEmpty
            : string.Format(CultureInfo.CurrentCulture, Strings.HomeProgressFormat, completed, total);
        ProgressRatio = total == 0 ? 0 : (double)completed / total;
    }
}
