using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using EcoRoot.Models;
using EcoRoot.Services;
using EcoRoot.Views;
using Microsoft.Maui.Controls;

namespace EcoRoot.ViewModels;

public sealed class ModuleDetailViewModel : BaseViewModel
{
    readonly IContentService contentService;
    readonly IProgressStore progressStore;
    Module? module;
    IReadOnlyList<LessonProgressItem> lessons = Array.Empty<LessonProgressItem>();
    string? moduleId;

    public ModuleDetailViewModel(IContentService contentService, IProgressStore progressStore)
    {
        this.contentService = contentService;
        this.progressStore = progressStore;
        Title = "Module";
        OpenLessonCommand = new Command<LessonProgressItem>(async lesson => await OpenLessonAsync(lesson));
        progressStore.ProgressChanged += (_, _) => BuildLessonItems();
    }

    public Module? Module
    {
        get => module;
        private set
        {
            if (SetProperty(ref module, value))
            {
                Title = value?.Title ?? "Module";
                BuildLessonItems();
            }
        }
    }

    public IReadOnlyList<LessonProgressItem> Lessons
    {
        get => lessons;
        private set => SetProperty(ref lessons, value);
    }

    public bool HasLessons { get; private set; }

    public bool IsLessonsEmpty => !HasLessons;

    public ICommand OpenLessonCommand { get; }

    public void LoadModule(string? moduleId)
    {
        if (string.IsNullOrWhiteSpace(moduleId))
        {
            Module = null;
            return;
        }

        this.moduleId = moduleId;
        Module = contentService.GetModule(moduleId);
    }

    async Task OpenLessonAsync(LessonProgressItem? lesson)
    {
        if (lesson is null || string.IsNullOrWhiteSpace(moduleId))
        {
            return;
        }

        await Shell.Current.GoToAsync($"{nameof(LessonDetailPage)}?moduleId={moduleId}&lessonId={lesson.LessonId}");
    }

    void BuildLessonItems()
    {
        if (Module is null)
        {
            Lessons = Array.Empty<LessonProgressItem>();
            return;
        }

        var items = new List<LessonProgressItem>();
        foreach (var lesson in Module.Lessons)
        {
            var completed = progressStore.IsLessonCompleted(Module.Id, lesson.Id);
            items.Add(new LessonProgressItem(Module.Id, lesson.Id, lesson.Title, lesson.Summary, lesson.TimeEstimate, completed));
        }

        Lessons = items;
        HasLessons = items.Count > 0;
        OnPropertyChanged(nameof(HasLessons));
        OnPropertyChanged(nameof(IsLessonsEmpty));
    }
}
