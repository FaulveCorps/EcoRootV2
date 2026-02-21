namespace EcoWay.Models;

public class ImpactAction
{
    public string Id { get; set; } = string.Empty;
    public string Icon { get; set; } = "🌱";
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string EffectText { get; set; } = string.Empty;
    public int Delta { get; set; }
}
