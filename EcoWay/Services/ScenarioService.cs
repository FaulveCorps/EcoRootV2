using System.Text.Json;
using EcoWay.Models;

namespace EcoWay.Services;

public class ScenarioData
{
    public List<Scenario> Scenarios { get; set; } = new();
    public List<Ending> Endings { get; set; } = new();
}

public class ScenarioService
{
    private ScenarioData _data = new();
    private bool _isLoaded;

    public int TotalScenarios => _data.Scenarios.Count;

    public async Task LoadDataAsync()
    {
        if (_isLoaded)
        {
            return;
        }

        using var stream = await FileSystem.OpenAppPackageFileAsync("scenarios.json");
        using var reader = new StreamReader(stream);
        var json = await reader.ReadToEndAsync();

        _data = JsonSerializer.Deserialize<ScenarioData>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new ScenarioData();

        _isLoaded = true;
    }

    public Scenario? GetScenario(string id)
    {
        return _data.Scenarios.FirstOrDefault(s => s.Id == id);
    }

    public Ending? GetEnding(int score)
    {
        return _data.Endings
            .OrderByDescending(e => e.MinimumScore)
            .FirstOrDefault(e => score >= e.MinimumScore);
    }
}