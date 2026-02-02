using System;
using System.Collections.Generic;
using EcoRoot.Models;

namespace EcoRoot.Services;

public interface IQuizStatsStore
{
    event EventHandler? StatsChanged;

    void RecordAttempt(string moduleId, string lessonId, string? lessonTitle, int correctCount, int totalQuestions);

    IReadOnlyList<QuizAttemptItem> GetRecentAttempts(int take);

    QuizStatsSummary GetSummary();
}
