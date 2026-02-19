using System;
using System.Collections.Generic;
using EcoWay.Models;

namespace EcoWay.Services;

public interface IQuizStatsStore
{
    event EventHandler? StatsChanged;

    void RecordAttempt(string moduleId, string lessonId, string? lessonTitle, int correctCount, int totalQuestions);

    IReadOnlyList<QuizAttemptItem> GetRecentAttempts(int take);

    QuizStatsSummary GetSummary();
}

