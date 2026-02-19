using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using EcoWay.Models;
using EcoWay.Resources;
using EcoWay.Services;
using EcoWay.Views;
using Microsoft.Maui.Controls;

namespace EcoWay.ViewModels;

public sealed class ModulesViewModel : BaseViewModel
{
    readonly IContentService contentService;
    readonly IProgressStore progressStore;

    public ModulesViewModel(IContentService contentService, IProgressStore progressStore)
    {
        this.contentService = contentService;
        this.progressStore = progressStore;
        Title = Strings.ModulesTitle;
        Modules = Array.Empty<ModuleListItem>();
        OpenModuleCommand = new Command<ModuleListItem>(async module => await OpenModuleAsync(module));
        LoadModules();
        progressStore.ProgressChanged += (_, _) => LoadModules();
    }

    public IReadOnlyList<ModuleListItem> Modules { get; private set; }

    public bool HasModules { get; private set; }

    public bool IsModulesEmpty => !HasModules;

    public ICommand OpenModuleCommand { get; }

    async Task OpenModuleAsync(ModuleListItem? module)
    {
        if (module is null)
        {
            return;
        }

        await Shell.Current.GoToAsync($"{nameof(ModuleDetailPage)}?moduleId={module.ModuleId}");
    }

    void LoadModules()
    {
        var items = new List<ModuleListItem>();
        foreach (var module in contentService.GetModules())
        {
            var completed = 0;
            foreach (var lesson in module.Lessons)
            {
                if (progressStore.IsLessonCompleted(module.Id, lesson.Id))
                {
                    completed++;
                }
            }

            items.Add(new ModuleListItem(module.Id, module.Title, module.Summary, completed, module.Lessons.Count));
        }

        Modules = items;
        HasModules = items.Count > 0;
        OnPropertyChanged(nameof(Modules));
        OnPropertyChanged(nameof(HasModules));
        OnPropertyChanged(nameof(IsModulesEmpty));
    }
}

