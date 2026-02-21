using System.Windows.Input;
using EcoWay.Services;

namespace EcoWay.ViewModels;

public class MainMenuViewModel : BaseViewModel
{
    private readonly GameStateService _gameStateService;
    private readonly ScenarioService _scenarioService;
    private readonly Command _viewSummaryCommand;

    private bool _hasActiveRun;
    private bool _canViewSummary;
    private string _playerRankText = "Rookie Ranger";
    private string _runStateText = "No Active Run";
    private string _totalScoreText = "0";
    private string _decisionsCountText = "0";
    private string _lastCheckpointText = "none";
    private string _primaryActionText = "▶ Start New Story Run";
    private string _primaryActionHint = "Begin from the first scenario and shape your ecosystem arc.";
    private string _summaryActionText = "Summary Locked";
    private string _summaryHintText = "Finish at least one decision to unlock run results.";
    private string _dailyQuestTitle = "Daily Quest: Eco Sprint";
    private string _dailyQuestProgressText = "0/5 milestones";
    private string _dailyQuestHintText = "Complete five decisions to max your daily eco streak.";
    private double _dailyQuestProgress;

    public ICommand PrimaryActionCommand { get; }
    public ICommand OpenImpactLabCommand { get; }
    public ICommand ViewSummaryCommand => _viewSummaryCommand;
    public ICommand ResetRunCommand { get; }

    public string PlayerRankText
    {
        get => _playerRankText;
        set => SetProperty(ref _playerRankText, value);
    }

    public string RunStateText
    {
        get => _runStateText;
        set => SetProperty(ref _runStateText, value);
    }

    public string TotalScoreText
    {
        get => _totalScoreText;
        set => SetProperty(ref _totalScoreText, value);
    }

    public string DecisionsCountText
    {
        get => _decisionsCountText;
        set => SetProperty(ref _decisionsCountText, value);
    }

    public string LastCheckpointText
    {
        get => _lastCheckpointText;
        set => SetProperty(ref _lastCheckpointText, value);
    }

    public string PrimaryActionText
    {
        get => _primaryActionText;
        set => SetProperty(ref _primaryActionText, value);
    }

    public string PrimaryActionHint
    {
        get => _primaryActionHint;
        set => SetProperty(ref _primaryActionHint, value);
    }

    public string SummaryActionText
    {
        get => _summaryActionText;
        set => SetProperty(ref _summaryActionText, value);
    }

    public string SummaryHintText
    {
        get => _summaryHintText;
        set => SetProperty(ref _summaryHintText, value);
    }

    public string DailyQuestTitle
    {
        get => _dailyQuestTitle;
        set => SetProperty(ref _dailyQuestTitle, value);
    }

    public string DailyQuestProgressText
    {
        get => _dailyQuestProgressText;
        set => SetProperty(ref _dailyQuestProgressText, value);
    }

    public string DailyQuestHintText
    {
        get => _dailyQuestHintText;
        set => SetProperty(ref _dailyQuestHintText, value);
    }

    public double DailyQuestProgress
    {
        get => _dailyQuestProgress;
        set => SetProperty(ref _dailyQuestProgress, value);
    }

    public bool CanViewSummary
    {
        get => _canViewSummary;
        set
        {
            if (SetProperty(ref _canViewSummary, value))
            {
                _viewSummaryCommand.ChangeCanExecute();
            }
        }
    }

    public MainMenuViewModel(GameStateService gameStateService, ScenarioService scenarioService)
    {
        _gameStateService = gameStateService;
        _scenarioService = scenarioService;

        PrimaryActionCommand = new Command(async () => await StartOrContinueAsync());
        OpenImpactLabCommand = new Command(async () => await GoToAsync("//ImpactLabPage"));
        _viewSummaryCommand = new Command(async () => await OpenSummaryAsync(), () => CanViewSummary);
        ResetRunCommand = new Command(async () => await ResetRunAsync());

        RefreshState();
    }

    public void RefreshState()
    {
        var state = _gameStateService.CurrentState;
        var decisions = state.History.Count;
        var isStart = string.Equals(state.CurrentScenarioId, "start", StringComparison.OrdinalIgnoreCase);
        var isEnd = string.Equals(state.CurrentScenarioId, "end", StringComparison.OrdinalIgnoreCase);

        _hasActiveRun = decisions > 0 && !isStart && !isEnd;
        CanViewSummary = decisions > 0;

        TotalScoreText = state.TotalScore.ToString("+#;-#;0");
        DecisionsCountText = decisions.ToString();
        LastCheckpointText = decisions == 0 ? "none" : state.CurrentScenarioId;

        RunStateText = isEnd
            ? "Run Complete"
            : _hasActiveRun
                ? "Run In Progress"
                : "No Active Run";

        PrimaryActionText = _hasActiveRun
            ? "▶ Continue Story Run"
            : "▶ Start New Story Run";

        PrimaryActionHint = _hasActiveRun
            ? $"Resume at checkpoint '{state.CurrentScenarioId}'."
            : "Launch a fresh campaign run from the opening scenario.";

        SummaryActionText = CanViewSummary ? "View Run Summary" : "Summary Locked";
        SummaryHintText = CanViewSummary
            ? "Inspect your current ending and score breakdown."
            : "Finish at least one decision to unlock run results.";

        const int questGoal = 5;
        var questProgressCount = Math.Min(decisions, questGoal);
        DailyQuestProgress = Math.Clamp((double)questProgressCount / questGoal, 0d, 1d);
        DailyQuestProgressText = $"{questProgressCount}/{questGoal} milestones";
        DailyQuestHintText = questProgressCount >= questGoal
            ? "Daily quest complete — your ecosystem instincts are on fire."
            : "Complete five decisions to max your daily eco streak.";

        PlayerRankText = GetPlayerRank(state.TotalScore, decisions);
    }

    private async Task StartOrContinueAsync()
    {
        if (!_hasActiveRun)
        {
            _gameStateService.Reset();
        }

        await GoToAsync("//ScenarioPage");
    }

    private async Task OpenSummaryAsync()
    {
        await _scenarioService.LoadDataAsync();
        await GoToAsync("SummaryPage");
    }

    private Task ResetRunAsync()
    {
        _gameStateService.Reset();
        RefreshState();
        return Task.CompletedTask;
    }

    private static string GetPlayerRank(int score, int decisions)
    {
        if (decisions == 0)
        {
            return "Rank: Rookie Ranger";
        }

        if (score >= 25)
        {
            return "Rank: Ecosystem Guardian";
        }

        if (score >= 10)
        {
            return "Rank: Green Strategist";
        }

        if (score >= 0)
        {
            return "Rank: Field Explorer";
        }

        return "Rank: Recovery Trainee";
    }

    private static async Task GoToAsync(string route)
    {
        if (Shell.Current is null)
        {
            return;
        }

        await Shell.Current.GoToAsync(route);
    }
}