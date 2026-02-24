using System.Collections.ObjectModel;
using System.Windows.Input;
using EcoWay.Models;
using EcoWay.Services;

namespace EcoWay.ViewModels;

public class CausesViewModel : BaseViewModel
{
    private readonly HashSet<string> _activeCauseIds = new(StringComparer.OrdinalIgnoreCase);
    private string? _primaryOutcomeCauseId;

    public ObservableCollection<CauseItem> Causes { get; }
    public ObservableCollection<string> ActiveEffects { get; } = [];

    private CauseItem? _selectedCause;
    public CauseItem? SelectedCause
    {
        get => _selectedCause;
        set => SetProperty(ref _selectedCause, value, onChanged: () =>
        {
            OnPropertyChanged(nameof(HasSelectedCause));
            OnPropertyChanged(nameof(SelectedCauseImpacts));
        });
    }

    public bool HasSelectedCause => SelectedCause is not null;

    public IReadOnlyList<string> SelectedCauseImpacts => SelectedCause?.Impacts ?? [];

    public string EnvironmentStatus => _activeCauseIds.Count switch
    {
        0 => "🌿 Healthy Environment",
        <= 2 => "⚠️ Mild Pollution",
        <= 4 => "🔶 Moderate Pollution",
        _ => "☠️ Severe Pollution"
    };

    public string EnvironmentStatusDetail => _activeCauseIds.Count switch
    {
        0 => "No active pollution sources. Tap a source to preview impact.",
        <= 2 => "Localized stress signals detected.",
        <= 4 => "Multiple contamination pathways are active.",
        _ => "Critical ecosystem stress across connected systems."
    };

    public string ActiveSourceText => $"Active sources: {_activeCauseIds.Count}/{Causes.Count}";

    public double PollutionProgress => Causes.Count == 0
        ? 0
        : Math.Clamp((double)_activeCauseIds.Count / Causes.Count, 0, 1);

    public string EmptyEffectsText => ActiveEffects.Count == 0
        ? "Select one or more pollution sources to display active effects."
        : string.Empty;

    private string _visualSkyColor = "#8EDCFF";
    public string VisualSkyColor
    {
        get => _visualSkyColor;
        set => SetProperty(ref _visualSkyColor, value);
    }

    private string _visualGroundColor = "#67AF65";
    public string VisualGroundColor
    {
        get => _visualGroundColor;
        set => SetProperty(ref _visualGroundColor, value);
    }

    private string _visualWaterColor = "#4EA8E8";
    public string VisualWaterColor
    {
        get => _visualWaterColor;
        set => SetProperty(ref _visualWaterColor, value);
    }

    private double _visualWaterOpacity = 0.2;
    public double VisualWaterOpacity
    {
        get => _visualWaterOpacity;
        set => SetProperty(ref _visualWaterOpacity, value);
    }

    private double _visualHazeOpacity = 0.03;
    public double VisualHazeOpacity
    {
        get => _visualHazeOpacity;
        set => SetProperty(ref _visualHazeOpacity, value);
    }

    private double _visualDebrisOpacity;
    public double VisualDebrisOpacity
    {
        get => _visualDebrisOpacity;
        set => SetProperty(ref _visualDebrisOpacity, value);
    }

    private double _visualTreeScale = 1.0;
    public double VisualTreeScale
    {
        get => _visualTreeScale;
        set => SetProperty(ref _visualTreeScale, value);
    }

    private double _visualTreeOpacity = 1.0;
    public double VisualTreeOpacity
    {
        get => _visualTreeOpacity;
        set => SetProperty(ref _visualTreeOpacity, value);
    }

    private string _outcomeTitle = "No active pollution outcome";
    public string OutcomeTitle
    {
        get => _outcomeTitle;
        set => SetProperty(ref _outcomeTitle, value);
    }

    private string _outcomeDetail = "Tap a source above to project environmental outcomes visually.";
    public string OutcomeDetail
    {
        get => _outcomeDetail;
        set => SetProperty(ref _outcomeDetail, value);
    }

    public ICommand ToggleCauseCommand { get; }
    public ICommand ResetCommand { get; }

    public CausesViewModel(EcoRootContentService contentService)
    {
        Causes = new ObservableCollection<CauseItem>(contentService.Causes);
        SelectedCause = Causes.FirstOrDefault();

        ToggleCauseCommand = new Command<CauseItem>(ToggleCause);
        ResetCommand = new Command(ResetSources);

        RecalculateVisualization();
    }

    private void ToggleCause(CauseItem? cause)
    {
        if (cause is null)
        {
            return;
        }

        SelectedCause = cause;

        if (_activeCauseIds.Contains(cause.Id))
        {
            _activeCauseIds.Remove(cause.Id);

            if (string.Equals(_primaryOutcomeCauseId, cause.Id, StringComparison.OrdinalIgnoreCase))
            {
                _primaryOutcomeCauseId = _activeCauseIds.LastOrDefault();
            }
        }
        else
        {
            _activeCauseIds.Add(cause.Id);
            _primaryOutcomeCauseId = cause.Id;
        }

        RebuildEffects();
        NotifyDerivedState();
    }

    private void ResetSources()
    {
        _activeCauseIds.Clear();
        _primaryOutcomeCauseId = null;
        RebuildEffects();
        NotifyDerivedState();
    }

    private void RebuildEffects()
    {
        ActiveEffects.Clear();

        foreach (var cause in Causes.Where(c => _activeCauseIds.Contains(c.Id)))
        {
            foreach (var impact in cause.Impacts)
            {
                ActiveEffects.Add($"{cause.Icon} {impact}");
            }
        }

        OnPropertyChanged(nameof(EmptyEffectsText));
    }

    private void NotifyDerivedState()
    {
        OnPropertyChanged(nameof(EnvironmentStatus));
        OnPropertyChanged(nameof(EnvironmentStatusDetail));
        OnPropertyChanged(nameof(ActiveSourceText));
        OnPropertyChanged(nameof(PollutionProgress));
        RecalculateVisualization();
    }

    private void RecalculateVisualization()
    {
        var intensity = PollutionProgress;

        VisualSkyColor = _activeCauseIds.Count switch
        {
            0 => "#8EDCFF",
            <= 2 => "#9AC4D6",
            <= 4 => "#9B9D93",
            _ => "#8A7A72"
        };

        VisualGroundColor = _activeCauseIds.Count switch
        {
            0 => "#67AF65",
            <= 2 => "#7A9560",
            <= 4 => "#7E775D",
            _ => "#6D6457"
        };

        VisualWaterColor = _activeCauseIds.Count switch
        {
            0 => "#4EA8E8",
            <= 2 => "#628FAE",
            <= 4 => "#707F7D",
            _ => "#7F6F61"
        };

        VisualWaterOpacity = Math.Clamp(0.24 - (intensity * 0.14), 0.08, 0.25);
        VisualHazeOpacity = Math.Clamp(0.03 + (intensity * 0.34), 0.03, 0.5);
        VisualDebrisOpacity = Math.Clamp(intensity * 0.45, 0, 0.75);
        VisualTreeScale = Math.Clamp(1.0 - (intensity * 0.36), 0.5, 1.05);
        VisualTreeOpacity = Math.Clamp(1.0 - (intensity * 0.5), 0.3, 1.0);

        var outcomeId = _primaryOutcomeCauseId;
        if (!string.IsNullOrWhiteSpace(outcomeId) && !_activeCauseIds.Contains(outcomeId))
        {
            outcomeId = _activeCauseIds.LastOrDefault();
        }

        if (string.IsNullOrWhiteSpace(outcomeId))
        {
            OutcomeTitle = "No active pollution outcome";
            OutcomeDetail = "Tap a source above to project environmental outcomes visually.";
            return;
        }

        switch (outcomeId)
        {
            case "deforestation":
                VisualTreeScale = Math.Clamp(VisualTreeScale - 0.2, 0.45, 1.05);
                VisualTreeOpacity = Math.Clamp(VisualTreeOpacity - 0.22, 0.2, 1.0);
                VisualDebrisOpacity = Math.Clamp(VisualDebrisOpacity + 0.2, 0, 0.85);
                OutcomeTitle = "Deforestation outcome active";
                break;
            case "mining":
                VisualWaterColor = "#886A54";
                VisualWaterOpacity = Math.Clamp(VisualWaterOpacity + 0.07, 0.08, 0.35);
                VisualHazeOpacity = Math.Clamp(VisualHazeOpacity + 0.08, 0.03, 0.55);
                OutcomeTitle = "Mining runoff outcome active";
                break;
            case "industrial":
                VisualSkyColor = "#7E7B78";
                VisualHazeOpacity = Math.Clamp(VisualHazeOpacity + 0.14, 0.03, 0.6);
                OutcomeTitle = "Industrial waste outcome active";
                break;
            case "pesticides":
                VisualGroundColor = "#8B8B57";
                VisualTreeOpacity = Math.Clamp(VisualTreeOpacity - 0.1, 0.25, 1.0);
                OutcomeTitle = "Pesticide stress outcome active";
                break;
            case "waste":
                VisualDebrisOpacity = Math.Clamp(VisualDebrisOpacity + 0.22, 0, 0.92);
                VisualWaterColor = "#7D7F5A";
                OutcomeTitle = "Improper waste outcome active";
                break;
            case "plastic":
                VisualDebrisOpacity = Math.Clamp(VisualDebrisOpacity + 0.16, 0, 0.9);
                VisualWaterColor = "#9A729A";
                OutcomeTitle = "Plastic contamination outcome active";
                break;
            default:
                OutcomeTitle = "Pollution outcome active";
                break;
        }

        var cause = Causes.FirstOrDefault(c => string.Equals(c.Id, outcomeId, StringComparison.OrdinalIgnoreCase));
        OutcomeDetail = cause is null
            ? "Environmental stress patterns are visible in the scene."
            : $"{cause.Icon} {cause.ShortDescription} {cause.Impacts.FirstOrDefault()}";
    }
}
