using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EcoRoot.Models;
using Microsoft.Maui.Storage;
using SQLite;

namespace EcoRoot.Services;

public sealed class SqliteQuizStatsStore : IQuizStatsStore
{
    readonly string dbPath;
    readonly object syncRoot = new();
    SQLiteConnection? connection;
    bool isInitialized;

    public SqliteQuizStatsStore()
    {
        dbPath = Path.Combine(FileSystem.AppDataDirectory, "ecoroot.db3");
        Initialize();
    }

    public event EventHandler? StatsChanged;

    public void RecordAttempt(string moduleId, string lessonId, string? lessonTitle, int correctCount, int totalQuestions)
    {
        if (string.IsNullOrWhiteSpace(moduleId) || string.IsNullOrWhiteSpace(lessonId))
        {
            return;
        }

        lock (syncRoot)
        {
            EnsureInitialized();
            connection!.Insert(new QuizAttemptEntity
            {
                ModuleId = moduleId,
                LessonId = lessonId,
                LessonTitle = lessonTitle,
                CorrectCount = correctCount,
                TotalQuestions = totalQuestions,
                AttemptedAtUtc = DateTime.UtcNow
            });
        }

        StatsChanged?.Invoke(this, EventArgs.Empty);
    }

    public IReadOnlyList<QuizAttemptItem> GetRecentAttempts(int take)
    {
        lock (syncRoot)
        {
            EnsureInitialized();
            var attempts = connection!.Table<QuizAttemptEntity>()
                .OrderByDescending(attempt => attempt.AttemptedAtUtc)
                .Take(Math.Max(take, 0))
                .ToList();

            return attempts
                .Select(attempt => new QuizAttemptItem(
                    attempt.ModuleId,
                    attempt.LessonId,
                    attempt.LessonTitle ?? string.Empty,
                    attempt.CorrectCount,
                    attempt.TotalQuestions,
                    attempt.AttemptedAtUtc))
                .ToList();
        }
    }

    public QuizStatsSummary GetSummary()
    {
        lock (syncRoot)
        {
            EnsureInitialized();
            var attempts = connection!.Table<QuizAttemptEntity>().ToList();
            var totalAttempts = attempts.Count;
            var totalCorrect = attempts.Sum(attempt => attempt.CorrectCount);
            var totalQuestions = attempts.Sum(attempt => attempt.TotalQuestions);
            return new QuizStatsSummary(totalAttempts, totalCorrect, totalQuestions);
        }
    }

    void Initialize()
    {
        lock (syncRoot)
        {
            if (isInitialized)
            {
                return;
            }

            connection = new SQLiteConnection(dbPath);
            connection.CreateTable<QuizAttemptEntity>();
            isInitialized = true;
        }
    }

    void EnsureInitialized()
    {
        if (!isInitialized)
        {
            Initialize();
        }
    }

    sealed class QuizAttemptEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public string ModuleId { get; set; } = string.Empty;

        [Indexed]
        public string LessonId { get; set; } = string.Empty;

        public string? LessonTitle { get; set; }

        public int CorrectCount { get; set; }

        public int TotalQuestions { get; set; }

        public DateTime AttemptedAtUtc { get; set; }
    }
}
