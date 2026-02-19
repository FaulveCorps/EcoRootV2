using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using EcoWay.Models;
using EcoWay.Resources;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using Microsoft.Maui.Controls;

namespace EcoWay.ViewModels;

public sealed class TeacherResourcesViewModel : BaseViewModel
{
    string statusMessage = string.Empty;

    public TeacherResourcesViewModel()
    {
        Title = Strings.TeacherResourcesTitle;
        CopyResourceCommand = new Command<TeacherResourceItem>(async item => await CopyResourceAsync(item));
    }

    public string IntroText => Strings.TeacherResourcesIntro;

    public IReadOnlyList<TeacherResourceItem> Resources { get; } = new[]
    {
        new TeacherResourceItem(
            "Discussion Prompts",
            "Ask: How does soil pollution affect food safety in your community? What local practices could improve soil health?"),
        new TeacherResourceItem(
            "Quick Check (5 min)",
            "Students list 3 pollution sources and 2 prevention actions in pairs."),
        new TeacherResourceItem(
            "Mini Activity",
            "Map local waste disposal points and discuss possible soil risks and mitigation steps."),
        new TeacherResourceItem(
            "Local Case Study",
            "Research a nearby cleanup initiative and summarize its impact on the community."),
        new TeacherResourceItem(
            "Exit Ticket",
            "Write one action you can take this week to reduce soil pollution."),
        new TeacherResourceItem(
            "Assessment Idea",
            "Create a one‑page infographic on soil pollution causes and solutions.")
    };

    public ICommand CopyResourceCommand { get; }

    public string StatusMessage
    {
        get => statusMessage;
        private set
        {
            if (SetProperty(ref statusMessage, value))
            {
                OnPropertyChanged(nameof(HasStatusMessage));
            }
        }
    }

    public bool HasStatusMessage => !string.IsNullOrWhiteSpace(StatusMessage);

    async Task CopyResourceAsync(TeacherResourceItem? item)
    {
        if (item is null)
        {
            return;
        }

        try
        {
            var text = $"{item.Title}{Environment.NewLine}{item.Description}";
            await Clipboard.Default.SetTextAsync(text);
            StatusMessage = Strings.TeacherResourcesCopied;
        }
        catch
        {
            StatusMessage = Strings.TeacherResourcesCopyFailed;
        }
    }
}

