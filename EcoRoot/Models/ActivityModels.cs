using System.Collections.Generic;

namespace EcoWay.Models;

public sealed class ActivityScenario
{
    public ActivityScenario(string id, string title, string prompt, IReadOnlyList<ActivityOption> options)
    {
        Id = id;
        Title = title;
        Prompt = prompt;
        Options = options;
    }

    public string Id { get; }

    public string Title { get; }

    public string Prompt { get; }

    public IReadOnlyList<ActivityOption> Options { get; }
}

public sealed class ActivityOption
{
    public ActivityOption(string text, bool isCorrect, string feedback)
    {
        Text = text;
        IsCorrect = isCorrect;
        Feedback = feedback;
    }

    public string Text { get; }

    public bool IsCorrect { get; }

    public string Feedback { get; }
}

