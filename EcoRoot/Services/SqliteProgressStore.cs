using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Maui.Storage;
using SQLite;

namespace EcoRoot.Services;

public sealed class SqliteProgressStore : IProgressStore
{
    readonly string dbPath;
    readonly string legacyJsonPath;
    readonly object syncRoot = new();
    SQLiteConnection? connection;
    bool isInitialized;

    public SqliteProgressStore()
    {
        dbPath = Path.Combine(FileSystem.AppDataDirectory, "ecoroot.db3");
        legacyJsonPath = Path.Combine(FileSystem.AppDataDirectory, "progress.json");
        Initialize();
    }

    public event EventHandler? ProgressChanged;

    public IReadOnlyCollection<string> GetCompletedLessonKeys()
    {
        lock (syncRoot)
        {
            EnsureInitialized();
            var items = connection!.Table<LessonProgressEntity>().ToList();
            return items.Select(item => GetLessonKey(item.ModuleId, item.LessonId)).ToList();
        }
    }

    public bool IsLessonCompleted(string moduleId, string lessonId)
    {
        if (string.IsNullOrWhiteSpace(moduleId) || string.IsNullOrWhiteSpace(lessonId))
        {
            return false;
        }

        lock (syncRoot)
        {
            EnsureInitialized();
            return connection!.Table<LessonProgressEntity>()
                .Any(item => item.ModuleId == moduleId && item.LessonId == lessonId);
        }
    }

    public void MarkLessonCompleted(string moduleId, string lessonId)
    {
        if (string.IsNullOrWhiteSpace(moduleId) || string.IsNullOrWhiteSpace(lessonId))
        {
            return;
        }

        lock (syncRoot)
        {
            EnsureInitialized();
            var exists = connection!.Table<LessonProgressEntity>()
                .Any(item => item.ModuleId == moduleId && item.LessonId == lessonId);
            if (exists)
            {
                return;
            }

            connection.Insert(new LessonProgressEntity
            {
                ModuleId = moduleId,
                LessonId = lessonId,
                CompletedAtUtc = DateTime.UtcNow
            });
        }

        ProgressChanged?.Invoke(this, EventArgs.Empty);
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
            connection.CreateTable<LessonProgressEntity>();
            TryMigrateFromJson();
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

    void TryMigrateFromJson()
    {
        if (!File.Exists(legacyJsonPath))
        {
            return;
        }

        var existing = connection!.Table<LessonProgressEntity>().Any();
        if (existing)
        {
            return;
        }

        try
        {
            var json = File.ReadAllText(legacyJsonPath);
            var state = JsonSerializer.Deserialize<ProgressStorageState>(json);
            if (state?.CompletedLessons is null)
            {
                return;
            }

            foreach (var key in state.CompletedLessons)
            {
                if (TryParseKey(key, out var moduleId, out var lessonId))
                {
                    connection.Insert(new LessonProgressEntity
                    {
                        ModuleId = moduleId,
                        LessonId = lessonId,
                        CompletedAtUtc = DateTime.UtcNow
                    });
                }
            }
        }
        catch
        {
            // Ignore migration failures.
        }
    }

    static bool TryParseKey(string? key, out string moduleId, out string lessonId)
    {
        moduleId = string.Empty;
        lessonId = string.Empty;

        if (string.IsNullOrWhiteSpace(key))
        {
            return false;
        }

        var index = key.IndexOf(':');
        if (index <= 0 || index >= key.Length - 1)
        {
            return false;
        }

        moduleId = key[..index];
        lessonId = key[(index + 1)..];
        return true;
    }

    static string GetLessonKey(string moduleId, string lessonId) => $"{moduleId}:{lessonId}";

    sealed class ProgressStorageState
    {
        public List<string> CompletedLessons { get; set; } = new();
    }

    sealed class LessonProgressEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        [Indexed(Name = "IX_Lesson", Unique = true)]
        public string ModuleId { get; set; } = string.Empty;

        [Indexed(Name = "IX_Lesson", Unique = true)]
        public string LessonId { get; set; } = string.Empty;

        public DateTime CompletedAtUtc { get; set; }
    }
}
