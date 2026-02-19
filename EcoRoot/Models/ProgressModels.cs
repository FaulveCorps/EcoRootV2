using System.Globalization;
using EcoWay.Resources;

namespace EcoWay.Models;

public sealed class ModuleProgressItem
{
    public ModuleProgressItem(string moduleId, string title, int completedLessons, int totalLessons)
    {
        ModuleId = moduleId;
        Title = title;
        CompletedLessons = completedLessons;
        TotalLessons = totalLessons;
    }

    public string ModuleId { get; }

    public string Title { get; }

    public int CompletedLessons { get; }

    public int TotalLessons { get; }

    public double CompletionRatio => TotalLessons == 0 ? 0 : (double)CompletedLessons / TotalLessons;

    public string Summary => string.Format(CultureInfo.CurrentCulture, Strings.ModuleProgressSummaryFormat, CompletedLessons, TotalLessons);
}

public sealed class LessonProgressItem
{
    public LessonProgressItem(string moduleId, string lessonId, string title, string summary, string? timeEstimate, bool isCompleted)
    {
        ModuleId = moduleId;
        LessonId = lessonId;
        Title = title;
        Summary = summary;
        TimeEstimate = timeEstimate;
        IsCompleted = isCompleted;
    }

    public string ModuleId { get; }

    public string LessonId { get; }

    public string Title { get; }

    public string Summary { get; }

    public string? TimeEstimate { get; }

    public bool IsCompleted { get; }

    public string StatusText => IsCompleted ? Strings.Completed : Strings.NotCompleted;
}

public sealed class ModuleListItem
{
    public ModuleListItem(string moduleId, string title, string summary, int completedLessons, int totalLessons)
    {
        ModuleId = moduleId;
        Title = title;
        Summary = summary;
        CompletedLessons = completedLessons;
        TotalLessons = totalLessons;
    }

    public string ModuleId { get; }

    public string Title { get; }

    public string Summary { get; }

    public int CompletedLessons { get; }

    public int TotalLessons { get; }

    public double CompletionRatio => TotalLessons == 0 ? 0 : (double)CompletedLessons / TotalLessons;

    public string ProgressSummary => string.Format(CultureInfo.CurrentCulture, Strings.ModuleProgressSummaryFormat, CompletedLessons, TotalLessons);
}

public sealed class BadgeItem
{
    public BadgeItem(string title, string description, bool isAchieved)
    {
        Title = title;
        Description = description;
        IsAchieved = isAchieved;
    }

    public string Title { get; }

    public string Description { get; }

    public bool IsAchieved { get; }

    public string StatusText => IsAchieved ? Strings.BadgeUnlocked : Strings.BadgeLocked;
}

