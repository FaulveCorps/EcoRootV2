using EcoWay.Models;

namespace EcoWay.Services;

public class GameStateService
{
    public GameState CurrentState { get; private set; }

    public GameStateService()
    {
        Reset();
    }

    public void Reset()
    {
        CurrentState = new GameState
        {
            CurrentScenarioId = "start",
            TotalScore = 0,
            History = new List<DecisionRecord>()
        };
    }

    public void RecordDecision(Scenario scenario, Choice choice)
    {
        var record = new DecisionRecord
        {
            ScenarioId = scenario.Id,
            ChoiceId = choice.Id,
            ScoreBefore = CurrentState.TotalScore,
            ScoreAfter = CurrentState.TotalScore + choice.ImpactDelta,
            Timestamp = DateTime.Now
        };

        CurrentState.TotalScore += choice.ImpactDelta;
        CurrentState.History.Add(record);
        CurrentState.CurrentScenarioId = choice.TargetScenarioId;
    }
}