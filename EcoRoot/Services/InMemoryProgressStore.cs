using System;
using System.Collections.Generic;

namespace EcoRoot.Services;

public sealed class InMemoryProgressStore : IProgressStore
{
    readonly HashSet<string> completedLessons = new(StringComparer.OrdinalIgnoreCase);

    public event EventHandler? ProgressChanged;

    public IReadOnlyCollection<string> GetCompletedLessonKeys() => completedLessons;

    public bool IsLessonCompleted(string moduleId, string lessonId)
    {
        if (string.IsNullOrWhiteSpace(moduleId) || string.IsNullOrWhiteSpace(lessonId))
        {
            return false;
        }

        return completedLessons.Contains(GetLessonKey(moduleId, lessonId));
    }

    public void MarkLessonCompleted(string moduleId, string lessonId)
    {
        if (string.IsNullOrWhiteSpace(moduleId) || string.IsNullOrWhiteSpace(lessonId))
        {
            return;
        }

        if (completedLessons.Add(GetLessonKey(moduleId, lessonId)))
        {
            ProgressChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    static string GetLessonKey(string moduleId, string lessonId) => $"{moduleId}:{lessonId}";
}
