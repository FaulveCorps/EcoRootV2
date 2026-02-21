namespace EcoWay.Models;

public class GameState
{
    public string CurrentScenarioId { get; set; }
    public int TotalScore { get; set; }
    public List<DecisionRecord> History { get; set; } = new();
    public string EndingId { get; set; }
}
