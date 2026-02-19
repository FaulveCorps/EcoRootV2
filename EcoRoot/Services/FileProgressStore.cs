using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Maui.Storage;

namespace EcoWay.Services;

public sealed class FileProgressStore : IProgressStore
{
    readonly string filePath;
    readonly object syncRoot = new();
    HashSet<string> completedLessons = new(StringComparer.OrdinalIgnoreCase);
    bool isLoaded;

    public FileProgressStore()
    {
        filePath = Path.Combine(FileSystem.AppDataDirectory, "progress.json");
    }

    public event EventHandler? ProgressChanged;

    public IReadOnlyCollection<string> GetCompletedLessonKeys()
    {
        EnsureLoaded();
        return new List<string>(completedLessons);
    }

    public bool IsLessonCompleted(string moduleId, string lessonId)
    {
        if (string.IsNullOrWhiteSpace(moduleId) || string.IsNullOrWhiteSpace(lessonId))
        {
            return false;
        }

        EnsureLoaded();
        return completedLessons.Contains(GetLessonKey(moduleId, lessonId));
    }

    public void MarkLessonCompleted(string moduleId, string lessonId)
    {
        if (string.IsNullOrWhiteSpace(moduleId) || string.IsNullOrWhiteSpace(lessonId))
        {
            return;
        }

        EnsureLoaded();

        if (completedLessons.Add(GetLessonKey(moduleId, lessonId)))
        {
            Save();
            ProgressChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    void EnsureLoaded()
    {
        if (isLoaded)
        {
            return;
        }

        lock (syncRoot)
        {
            if (isLoaded)
            {
                return;
            }

            if (!File.Exists(filePath))
            {
                isLoaded = true;
                return;
            }

            try
            {
                var json = File.ReadAllText(filePath);
                var state = JsonSerializer.Deserialize<ProgressStorageState>(json);
                completedLessons = new HashSet<string>(state?.CompletedLessons ?? new List<string>(), StringComparer.OrdinalIgnoreCase);
            }
            catch
            {
                completedLessons = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            }
            finally
            {
                isLoaded = true;
            }
        }
    }

    void Save()
    {
        try
        {
            var state = new ProgressStorageState { CompletedLessons = new List<string>(completedLessons) };
            var json = JsonSerializer.Serialize(state, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
        catch
        {
            // Ignore persistence failures for now.
        }
    }

    static string GetLessonKey(string moduleId, string lessonId) => $"{moduleId}:{lessonId}";

    sealed class ProgressStorageState
    {
        public List<string> CompletedLessons { get; set; } = new();
    }
}

