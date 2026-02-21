using System.Collections.ObjectModel;
using System.Windows.Input;
using EcoWay.Models;
using EcoWay.Services;

namespace EcoWay.ViewModels;

public class ScenarioViewModel : BaseViewModel
{
    private readonly ScenarioService _scenarioService;
    private readonly GameStateService _gameStateService;

    private Scenario? _currentScenario;
    public Scenario? CurrentScenario
    {
        get => _currentScenario;
        set => SetProperty(ref _currentScenario, value);
    }

    private int _totalScore;
    public int TotalScore
    {
        get => _totalScore;
        set => SetProperty(ref _totalScore, value);
    }

    private double _progressValue;
    public double ProgressValue
    {
        get => _progressValue;
        set => SetProperty(ref _progressValue, value);
    }

    private string _progressText = "0/0";
    public string ProgressText
    {
        get => _progressText;
        set => SetProperty(ref _progressText, value);
    }

    private string _stageBadgeText = "Morning";
    public string StageBadgeText
    {
        get => _stageBadgeText;
        set => SetProperty(ref _stageBadgeText, value);
    }

    private string _sceneMoodText = "Pick a path to shape the world.";
    public string SceneMoodText
    {
        get => _sceneMoodText;
        set => SetProperty(ref _sceneMoodText, value);
    }

    private string _noEffectsText = "Make a decision to observe environmental signals.";
    public string NoEffectsText
    {
        get => _noEffectsText;
        set => SetProperty(ref _noEffectsText, value);
    }

    public ObservableCollection<Choice> Choices { get; } = new();
    public ObservableCollection<string> ActiveEffects { get; } = new();

    public ICommand MakeChoiceCommand { get; }

    public ScenarioViewModel(ScenarioService scenarioService, GameStateService gameStateService)
    {
        _scenarioService = scenarioService;
        _gameStateService = gameStateService;

        MakeChoiceCommand = new Command<Choice>(OnMakeChoice);
    }

    public async Task InitializeAsync()
    {
        await _scenarioService.LoadDataAsync();
        LoadScenario(_gameStateService.CurrentState.CurrentScenarioId);
    }

    private void LoadScenario(string scenarioId)
    {
        if (scenarioId == "end")
        {
            // Navigate to summary
            Shell.Current.GoToAsync("SummaryPage");
            return;
        }

        CurrentScenario = _scenarioService.GetScenario(scenarioId);
        Choices.Clear();
        if (CurrentScenario != null)
        {
            foreach (var choice in CurrentScenario.Choices)
            {
                Choices.Add(choice);
            }
        }

        RefreshPresentationState();
    }

    private void OnMakeChoice(Choice choice)
    {
        if (choice == null || CurrentScenario == null)
        {
            return;
        }

        _gameStateService.RecordDecision(CurrentScenario, choice);
        AddActiveEffect(choice);
        LoadScenario(choice.TargetScenarioId);
    }

    private void RefreshPresentationState()
    {
        TotalScore = _gameStateService.CurrentState.TotalScore;

        var totalScenarios = Math.Max(1, _scenarioService.TotalScenarios);
        var visitedNodes = _gameStateService.CurrentState.History.Count + 1;
        var clampedVisited = Math.Min(visitedNodes, totalScenarios);

        StageBadgeText = clampedVisited switch
        {
            1 => "Morning",
            2 => "Midday",
            _ => "Evening"
        };

        ProgressValue = Math.Clamp((double)clampedVisited / totalScenarios, 0d, 1d);
        ProgressText = $"{clampedVisited}/{totalScenarios}";

        var visualKey = CurrentScenario?.VisualConfig?.ToLowerInvariant() ?? string.Empty;
        SceneMoodText = visualKey switch
        {
            "city_morning" => "Rush hour is here — cleaner transport choices reduce smog.",
            "office_lunch" => "Midday demand spikes — low-waste habits keep systems efficient.",
            "home_evening" => "Nightfall energy use matters — small actions compound over time.",
            _ => "Every decision bends the ecosystem in a new direction."
        };

        NoEffectsText = ActiveEffects.Count == 0
            ? "Make a decision to observe environmental signals."
            : string.Empty;
    }

    private void AddActiveEffect(Choice choice)
    {
        var effect = string.IsNullOrWhiteSpace(choice.Aftermath)
            ? "The environment subtly responds to your latest decision."
            : choice.Aftermath;

        ActiveEffects.Insert(0, effect);

        while (ActiveEffects.Count > 3)
        {
            ActiveEffects.RemoveAt(ActiveEffects.Count - 1);
        }

        NoEffectsText = string.Empty;
    }
}