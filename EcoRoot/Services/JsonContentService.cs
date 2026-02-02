using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using EcoRoot.Models;
using Microsoft.Maui.Storage;

namespace EcoRoot.Services;

public sealed class JsonContentService : IContentService
{
    readonly SampleContentService fallback;
    readonly object syncRoot = new();
    readonly string cachePath;
    IReadOnlyList<Module>? cachedModules;
    bool isLoaded;

    public JsonContentService(SampleContentService fallback)
    {
        this.fallback = fallback;
        cachePath = Path.Combine(FileSystem.AppDataDirectory, GetCacheFileName());
    }

    public IReadOnlyList<Module> GetModules()
    {
        EnsureLoaded();
        return cachedModules ?? fallback.GetModules();
    }

    public Module? GetModule(string id)
    {
        return GetModules().FirstOrDefault(module => module.Id == id);
    }

    public Lesson? GetLesson(string moduleId, string lessonId)
    {
        return GetModule(moduleId)?.Lessons.FirstOrDefault(lesson => lesson.Id == lessonId);
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

            try
            {
                cachedModules = LoadFromCache() ?? LoadFromPackage();
            }
            catch
            {
                cachedModules = null;
            }
            finally
            {
                isLoaded = true;
            }
        }
    }

    IReadOnlyList<Module>? LoadFromCache()
    {
        if (!File.Exists(cachePath))
        {
            return null;
        }

        try
        {
            var json = File.ReadAllText(cachePath);
            return ParseModules(json);
        }
        catch
        {
            return null;
        }
    }

    IReadOnlyList<Module>? LoadFromPackage()
    {
        using var stream = FileSystem.OpenAppPackageFileAsync(GetSeedFileName()).GetAwaiter().GetResult();
        using var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();
        var modules = ParseModules(json);
        if (modules is not null)
        {
            TryWriteCache(json);
        }

        return modules;
    }

    IReadOnlyList<Module>? ParseModules(string json)
    {
        var seed = JsonSerializer.Deserialize<ContentSeedDto>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (seed?.Modules is null || seed.Modules.Count == 0)
        {
            return null;
        }

        var modules = new List<Module>();
        foreach (var module in seed.Modules)
        {
            var lessons = new List<Lesson>();
            if (module.Lessons is not null)
            {
                foreach (var lesson in module.Lessons)
                {
                    var sections = new List<LessonSection>();
                    if (lesson.Sections is not null)
                    {
                        foreach (var section in lesson.Sections)
                        {
                            sections.Add(new LessonSection(section.Heading ?? string.Empty, section.Body ?? string.Empty));
                        }
                    }

                    var quiz = new List<QuizQuestion>();
                    if (lesson.Quiz is not null)
                    {
                        foreach (var question in lesson.Quiz)
                        {
                            quiz.Add(new QuizQuestion(
                                question.Prompt ?? string.Empty,
                                question.Options ?? new List<string>(),
                                question.CorrectIndex,
                                question.Explanation ?? string.Empty));
                        }
                    }

                    lessons.Add(new Lesson(
                        lesson.Id ?? string.Empty,
                        lesson.Title ?? string.Empty,
                        lesson.Summary ?? string.Empty,
                        sections,
                        quiz));
                }
            }

            modules.Add(new Module(
                module.Id ?? string.Empty,
                module.Title ?? string.Empty,
                module.Summary ?? string.Empty,
                lessons));
        }

        return modules;
    }

    void TryWriteCache(string json)
    {
        try
        {
            File.WriteAllText(cachePath, json);
        }
        catch
        {
            // Ignore cache write failures.
        }
    }

    static string GetSeedFileName()
    {
        var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        return culture == "es" ? "content_seed.es.json" : "content_seed.json";
    }

    static string GetCacheFileName()
    {
        var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        return culture == "es" ? "content_seed.es.json" : "content_seed.json";
    }

    sealed class ContentSeedDto
    {
        public List<ModuleDto>? Modules { get; set; }
    }

    sealed class ModuleDto
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public List<LessonDto>? Lessons { get; set; }
    }

    sealed class LessonDto
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public List<LessonSectionDto>? Sections { get; set; }
        public List<QuizQuestionDto>? Quiz { get; set; }
    }

    sealed class LessonSectionDto
    {
        public string? Heading { get; set; }
        public string? Body { get; set; }
    }

    sealed class QuizQuestionDto
    {
        public string? Prompt { get; set; }
        public List<string>? Options { get; set; }
        public int CorrectIndex { get; set; }
        public string? Explanation { get; set; }
    }
}
