using System.Collections.Generic;
using System.Windows.Input;
using EcoWay.Models;
using EcoWay.Resources;
using Microsoft.Maui.Controls;

namespace EcoWay.ViewModels;

public sealed class ActivityViewModel : BaseViewModel
{
    readonly List<ActivityScenario> scenarios;
    ActivityScenario? currentScenario;
    string? feedbackMessage;

    public ActivityViewModel()
    {
        Title = Strings.ActivityTitle;
        scenarios = BuildScenarios();
        SelectOptionCommand = new Command<ActivityOption>(SelectOption);
        NextScenarioCommand = new Command(NextScenario);
        LoadScenario(0);
    }

    public ActivityScenario? CurrentScenario
    {
        get => currentScenario;
        private set => SetProperty(ref currentScenario, value);
    }

    public string? FeedbackMessage
    {
        get => feedbackMessage;
        private set
        {
            if (SetProperty(ref feedbackMessage, value))
            {
                OnPropertyChanged(nameof(HasFeedback));
            }
        }
    }

    public bool HasFeedback => !string.IsNullOrWhiteSpace(FeedbackMessage);

    public ICommand SelectOptionCommand { get; }

    public ICommand NextScenarioCommand { get; }

    void SelectOption(ActivityOption? option)
    {
        if (option is null)
        {
            return;
        }

        FeedbackMessage = option.Feedback;
    }

    void NextScenario()
    {
        if (CurrentScenario is null)
        {
            return;
        }

        var index = scenarios.IndexOf(CurrentScenario);
        if (index < 0)
        {
            index = 0;
        }

        var nextIndex = (index + 1) % scenarios.Count;
        LoadScenario(nextIndex);
    }

    void LoadScenario(int index)
    {
        if (scenarios.Count == 0)
        {
            CurrentScenario = null;
            FeedbackMessage = Strings.ActivityEmpty;
            return;
        }

        CurrentScenario = scenarios[index];
        FeedbackMessage = null;
    }

    static List<ActivityScenario> BuildScenarios()
    {
        return new List<ActivityScenario>
        {
            new ActivityScenario(
                "scenario-1",
                Strings.ActivityScenarioTitle1,
                Strings.ActivityScenarioPrompt1,
                new[]
                {
                    new ActivityOption(Strings.ActivityScenarioOption1A, true, Strings.ActivityScenarioFeedback1A),
                    new ActivityOption(Strings.ActivityScenarioOption1B, false, Strings.ActivityScenarioFeedback1B),
                    new ActivityOption(Strings.ActivityScenarioOption1C, false, Strings.ActivityScenarioFeedback1C)
                }),
            new ActivityScenario(
                "scenario-2",
                Strings.ActivityScenarioTitle2,
                Strings.ActivityScenarioPrompt2,
                new[]
                {
                    new ActivityOption(Strings.ActivityScenarioOption2A, true, Strings.ActivityScenarioFeedback2A),
                    new ActivityOption(Strings.ActivityScenarioOption2B, false, Strings.ActivityScenarioFeedback2B),
                    new ActivityOption(Strings.ActivityScenarioOption2C, false, Strings.ActivityScenarioFeedback2C)
                }),
            new ActivityScenario(
                "scenario-3",
                Strings.ActivityScenarioTitle3,
                Strings.ActivityScenarioPrompt3,
                new[]
                {
                    new ActivityOption(Strings.ActivityScenarioOption3A, true, Strings.ActivityScenarioFeedback3A),
                    new ActivityOption(Strings.ActivityScenarioOption3B, false, Strings.ActivityScenarioFeedback3B),
                    new ActivityOption(Strings.ActivityScenarioOption3C, false, Strings.ActivityScenarioFeedback3C)
                })
        };
    }
}

