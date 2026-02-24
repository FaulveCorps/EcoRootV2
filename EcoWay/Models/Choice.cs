namespace EcoWay.Models;

public class Choice
{
    public string Id { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public string TargetScenarioId { get; set; } = string.Empty;
    public int ImpactDelta { get; set; }

    public string Icon { get; set; } = "🌱";
    public string Teaser { get; set; } = string.Empty;
    public string Aftermath { get; set; } = string.Empty;
    public string OutcomeVisual { get; set; } = string.Empty;

    public bool HasTeaser => !string.IsNullOrWhiteSpace(Teaser);
}
