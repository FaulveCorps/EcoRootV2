using System.Collections.ObjectModel;
using System.Windows.Input;
using EcoWay.Models;
using EcoWay.Services;

namespace EcoWay.ViewModels;

public class CausesViewModel : BaseViewModel
{
    private readonly HashSet<string> _activeCauseIds = new(StringComparer.OrdinalIgnoreCase);

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

    public ICommand ToggleCauseCommand { get; }
    public ICommand ResetCommand { get; }

    public CausesViewModel(EcoRootContentService contentService)
    {
        Causes = new ObservableCollection<CauseItem>(contentService.Causes);
        SelectedCause = Causes.FirstOrDefault();

        ToggleCauseCommand = new Command<CauseItem>(ToggleCause);
        ResetCommand = new Command(ResetSources);
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
        }
        else
        {
            _activeCauseIds.Add(cause.Id);
        }

        RebuildEffects();
        NotifyDerivedState();
    }

    private void ResetSources()
    {
        _activeCauseIds.Clear();
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
    }
}
