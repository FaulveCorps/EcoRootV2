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

    public ICommand PrimaryActionCommand { get; }
    public ICommand OpenImpactLabCommand { get; }
    public ICommand ViewSummaryCommand => _viewSummaryCommand;

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