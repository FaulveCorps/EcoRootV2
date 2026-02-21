using System.Windows.Input;
using System.Collections.ObjectModel;
using EcoWay.Models;
using EcoWay.Services;

namespace EcoWay.ViewModels;

public class SummaryViewModel : BaseViewModel
{
    private readonly GameStateService _gameStateService;
    private readonly ScenarioService _scenarioService;

    private Ending _ending;
    public Ending Ending
    {
        get => _ending;
        set => SetProperty(ref _ending, value);
    }

    private int _totalScore;
    public int TotalScore
    {
        get => _totalScore;
        set => SetProperty(ref _totalScore, value);
    }

    public ObservableCollection<string> AchievementTags { get; } = [];

    public ICommand RestartCommand { get; }

    public SummaryViewModel(GameStateService gameStateService, ScenarioService scenarioService)
    {
        _gameStateService = gameStateService;
        _scenarioService = scenarioService;

        RestartCommand = new Command(OnRestart);
    }

    public void Initialize()
    {
        TotalScore = _gameStateService.CurrentState.TotalScore;
        Ending = _scenarioService.GetEnding(TotalScore);

        var decisions = _gameStateService.CurrentState.History.Count;

        AchievementTags.Clear();
        AchievementTags.Add($"Decisions: {decisions}");
        AchievementTags.Add(TotalScore >= 0 ? "Net Positive" : "Needs Recovery");
        AchievementTags.Add(TotalScore >= 20 ? "High Impact" : "Learning Run");
        AchievementTags.Add(decisions >= 3 ? "Route Explorer" : "Quick Route");
    }

    private void OnRestart()
    {
        _gameStateService.Reset();
        Shell.Current.GoToAsync("//ScenarioPage");
    }
}