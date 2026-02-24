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

    private bool _hasLatestOutcome;
    public bool HasLatestOutcome
    {
        get => _hasLatestOutcome;
        set => SetProperty(ref _hasLatestOutcome, value);
    }

    private string _latestOutcomeTitle = "No recent outcome yet";
    public string LatestOutcomeTitle
    {
        get => _latestOutcomeTitle;
        set => SetProperty(ref _latestOutcomeTitle, value);
    }

    private string _latestOutcomeDetail = "Pick a choice to preview immediate ecosystem impact.";
    public string LatestOutcomeDetail
    {
        get => _latestOutcomeDetail;
        set => SetProperty(ref _latestOutcomeDetail, value);
    }

    private string _latestOutcomeIcon = "🧭";
    public string LatestOutcomeIcon
    {
        get => _latestOutcomeIcon;
        set => SetProperty(ref _latestOutcomeIcon, value);
    }

    private string _latestImpactText = "±0 eco pts";
    public string LatestImpactText
    {
        get => _latestImpactText;
        set => SetProperty(ref _latestImpactText, value);
    }

    private string _latestOutcomeVisual = string.Empty;
    public string LatestOutcomeVisual
    {
        get => _latestOutcomeVisual;
        set => SetProperty(ref _latestOutcomeVisual, value);
    }

    private int _latestImpactDelta;
    public int LatestImpactDelta
    {
        get => _latestImpactDelta;
        set => SetProperty(ref _latestImpactDelta, value);
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
            if (Shell.Current is not null)
            {
                _ = Shell.Current.GoToAsync("SummaryPage");
            }
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

        UpdateLatestOutcome(choice);
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
        var baseMood = visualKey switch
        {
            "city_morning" => "Rush hour is here — cleaner transport choices reduce smog.",
            "office_lunch" => "Midday demand spikes — low-waste habits keep systems efficient.",
            "home_evening" => "Nightfall energy use matters — small actions compound over time.",
            _ => "Every decision bends the ecosystem in a new direction."
        };

        SceneMoodText = HasLatestOutcome
            ? $"{baseMood} Latest shift: {LatestOutcomeTitle.ToLowerInvariant()}."
            : baseMood;

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

    private void UpdateLatestOutcome(Choice choice)
    {
        LatestImpactDelta = choice.ImpactDelta;
        LatestImpactText = $"{choice.ImpactDelta:+#;-#;0} eco pts";
        LatestOutcomeIcon = string.IsNullOrWhiteSpace(choice.Icon)
            ? (choice.ImpactDelta >= 0 ? "🌱" : "⚠️")
            : choice.Icon;

        var resolvedVisual = ResolveOutcomeVisual(choice);
        LatestOutcomeVisual = resolvedVisual;
        LatestOutcomeTitle = BuildOutcomeTitle(resolvedVisual, choice.ImpactDelta);
        LatestOutcomeDetail = BuildOutcomeDetail(choice.Aftermath);
        HasLatestOutcome = true;
    }

    private static string ResolveOutcomeVisual(Choice choice)
    {
        if (!string.IsNullOrWhiteSpace(choice.OutcomeVisual))
        {
            return choice.OutcomeVisual.Trim().ToLowerInvariant();
        }

        var text = $"{choice.Text} {choice.Aftermath}".ToLowerInvariant();

        if (text.Contains("deforest") || text.Contains("clear vegetation") || text.Contains("tree loss"))
        {
            return "deforestation";
        }

        if (text.Contains("waste") || text.Contains("plastic") || text.Contains("packaging"))
        {
            return "waste";
        }

        if (text.Contains("traffic") || text.Contains("haze") || text.Contains("smog") || text.Contains("emission"))
        {
            return "emissions";
        }

        if (text.Contains("energy") || text.Contains("power") || text.Contains("load"))
        {
            return "energy";
        }

        return choice.ImpactDelta >= 0 ? "general_positive" : "general_negative";
    }

    private static string BuildOutcomeTitle(string outcomeVisual, int impactDelta)
    {
        return outcomeVisual switch
        {
            "deforestation" when impactDelta < 0 => "Deforestation Pressure Increased",
            "deforestation" => "Forest Recovery Accelerated",
            "emissions" when impactDelta < 0 => "Emissions Spiked",
            "emissions" => "Air Quality Improved",
            "clean_transport" => "Cleaner Mobility Outcome",
            "active_transport" => "Low-Emission Mobility Boost",
            "waste" when impactDelta < 0 => "Waste Burden Increased",
            "waste" => "Waste Load Reduced",
            "low_waste" => "Low-Waste Momentum",
            "energy_overuse" => "Energy Demand Surge",
            "energy_saver" => "Energy Efficiency Gain",
            "energy" when impactDelta < 0 => "Energy Strain Increased",
            "energy" => "Energy Use Optimized",
            "general_positive" => "Ecosystem Resilience Improved",
            "general_negative" => "Ecosystem Stress Increased",
            _ when impactDelta >= 0 => "Positive Environmental Shift",
            _ => "Negative Environmental Shift"
        };
    }

    private static string BuildOutcomeDetail(string aftermath)
    {
        if (string.IsNullOrWhiteSpace(aftermath))
        {
            return "The simulation updated to reflect the selected outcome.";
        }

        const int maxLength = 120;
        var normalized = aftermath.Replace('\n', ' ').Trim();
        return normalized.Length <= maxLength
            ? normalized
            : $"{normalized[..(maxLength - 1)]}…";
    }
}