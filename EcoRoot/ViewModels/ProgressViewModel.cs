using System.Collections.ObjectModel;
using System.Windows.Input;
using EcoRoot.Resources;
using EcoRoot.Models;
using EcoRoot.Services;
using EcoRoot.Views;
using Microsoft.Maui.Controls;

namespace EcoRoot.ViewModels;

public sealed class ProgressViewModel : BaseViewModel
{
    readonly IContentService contentService;
    readonly IProgressStore progressStore;
    readonly IQuizStatsStore quizStatsStore;

    public ProgressViewModel(IContentService contentService, IProgressStore progressStore, IQuizStatsStore quizStatsStore)
    {
        this.contentService = contentService;
        this.progressStore = progressStore;
        this.quizStatsStore = quizStatsStore;
        Title = Strings.ProgressTitle;
        Headline = Strings.ProgressHeadline;
        Body = Strings.ProgressBody;
        ModuleProgress = new ObservableCollection<ModuleProgressItem>();
        RecentQuizAttempts = new ObservableCollection<QuizAttemptItem>();
        Badges = new ObservableCollection<BadgeItem>();
        OpenCloudSyncCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(CloudSyncPage)));
        LoadProgress();
        LoadQuizStats();
        BuildBadges();
        progressStore.ProgressChanged += (_, _) =>
        {
            LoadProgress();
            BuildBadges();
        };
        quizStatsStore.StatsChanged += (_, _) =>
        {
            LoadQuizStats();
            BuildBadges();
        };
    }

    public string Headline { get; }

    public string Body { get; }

    public ObservableCollection<ModuleProgressItem> ModuleProgress { get; }

    public ObservableCollection<QuizAttemptItem> RecentQuizAttempts { get; }

    public ObservableCollection<BadgeItem> Badges { get; }

    public string QuizSummaryText { get; private set; } = string.Empty;

    public bool HasQuizAttempts { get; private set; }

    public bool IsQuizAttemptsEmpty => !HasQuizAttempts;

    public ICommand OpenCloudSyncCommand { get; }

    void LoadProgress()
    {
        ModuleProgress.Clear();

        foreach (var module in contentService.GetModules())
        {
            var completed = 0;
            foreach (var lesson in module.Lessons)
            {
                if (progressStore.IsLessonCompleted(module.Id, lesson.Id))
                {
                    completed++;
                }
            }

            ModuleProgress.Add(new ModuleProgressItem(module.Id, module.Title, completed, module.Lessons.Count));
        }
    }

    void LoadQuizStats()
    {
        var summary = quizStatsStore.GetSummary();
        QuizSummaryText = summary.SummaryText;
        OnPropertyChanged(nameof(QuizSummaryText));

        RecentQuizAttempts.Clear();
        foreach (var attempt in quizStatsStore.GetRecentAttempts(5))
        {
            RecentQuizAttempts.Add(attempt);
        }

        HasQuizAttempts = RecentQuizAttempts.Count > 0;
        OnPropertyChanged(nameof(HasQuizAttempts));
        OnPropertyChanged(nameof(IsQuizAttemptsEmpty));
    }

    void BuildBadges()
    {
        var totalLessons = 0;
        var completedLessons = 0;
        var completedModules = 0;

        foreach (var module in contentService.GetModules())
        {
            totalLessons += module.Lessons.Count;
            var moduleCompleted = 0;
            foreach (var lesson in module.Lessons)
            {
                if (progressStore.IsLessonCompleted(module.Id, lesson.Id))
                {
                    moduleCompleted++;
                }
            }

            completedLessons += moduleCompleted;
            if (module.Lessons.Count > 0 && moduleCompleted == module.Lessons.Count)
            {
                completedModules++;
            }
        }

        var quizSummary = quizStatsStore.GetSummary();
        var anyLesson = completedLessons > 0;
        var anyModule = completedModules > 0;
        var allLessons = totalLessons > 0 && completedLessons == totalLessons;
        var anyQuiz = quizSummary.TotalAttempts > 0;
        var quizExplorer = quizSummary.TotalAttempts >= 3;

        Badges.Clear();
        Badges.Add(new BadgeItem(Strings.BadgeFirstLessonTitle, Strings.BadgeFirstLessonDesc, anyLesson));
        Badges.Add(new BadgeItem(Strings.BadgeQuizStarterTitle, Strings.BadgeQuizStarterDesc, anyQuiz));
        Badges.Add(new BadgeItem(Strings.BadgeModuleFinisherTitle, Strings.BadgeModuleFinisherDesc, anyModule));
        Badges.Add(new BadgeItem(Strings.BadgeQuizExplorerTitle, Strings.BadgeQuizExplorerDesc, quizExplorer));
        Badges.Add(new BadgeItem(Strings.BadgeSoilChampionTitle, Strings.BadgeSoilChampionDesc, allLessons));
    }
}
