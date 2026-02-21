namespace EcoWay.Models;

public class DecisionRecord
{
    public string ScenarioId { get; set; }
    public string ChoiceId { get; set; }
    public int ScoreBefore { get; set; }
    public int ScoreAfter { get; set; }
    public DateTime Timestamp { get; set; }
}
