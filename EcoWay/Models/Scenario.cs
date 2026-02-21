namespace EcoWay.Models;

public class Scenario
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string VisualConfig { get; set; }
    public List<Choice> Choices { get; set; } = new();
}
