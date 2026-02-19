using System;
using System.Globalization;
using EcoWay.Resources;

namespace EcoWay.Models;

public sealed class QuizAttemptItem
{
    public QuizAttemptItem(string moduleId, string lessonId, string lessonTitle, int correctCount, int totalQuestions, DateTime attemptedAtUtc)
    {
        ModuleId = moduleId;
        LessonId = lessonId;
        LessonTitle = lessonTitle;
        CorrectCount = correctCount;
        TotalQuestions = totalQuestions;
        AttemptedAtUtc = attemptedAtUtc;
    }

    public string ModuleId { get; }

    public string LessonId { get; }

    public string LessonTitle { get; }

    public int CorrectCount { get; }

    public int TotalQuestions { get; }

    public DateTime AttemptedAtUtc { get; }

    public string ScoreText => TotalQuestions == 0
        ? Strings.QuizScoreEmpty
        : string.Format(CultureInfo.CurrentCulture, Strings.QuizScoreFormat, CorrectCount, TotalQuestions);

    public string AttemptedAtText => AttemptedAtUtc.ToLocalTime().ToString("g");
}

public sealed class QuizStatsSummary
{
    public QuizStatsSummary(int totalAttempts, int totalCorrect, int totalQuestions)
    {
        TotalAttempts = totalAttempts;
        TotalCorrect = totalCorrect;
        TotalQuestions = totalQuestions;
    }

    public int TotalAttempts { get; }

    public int TotalCorrect { get; }

    public int TotalQuestions { get; }

    public string SummaryText => TotalAttempts == 0
        ? Strings.QuizSummaryEmpty
        : string.Format(CultureInfo.CurrentCulture, Strings.QuizSummaryFormat, TotalAttempts, TotalCorrect, TotalQuestions);
}

