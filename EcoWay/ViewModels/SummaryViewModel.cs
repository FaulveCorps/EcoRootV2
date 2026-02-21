using System.Windows.Input;
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
    }

    private void OnRestart()
    {
        _gameStateService.Reset();
        Shell.Current.GoToAsync("//ScenarioPage");
    }
}