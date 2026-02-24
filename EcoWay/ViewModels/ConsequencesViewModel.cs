using System.Collections.ObjectModel;
using System.Windows.Input;
using EcoWay.Models;
using EcoWay.Services;

namespace EcoWay.ViewModels;

public class ConsequencesViewModel : BaseViewModel
{
    public ObservableCollection<ConsequenceItem> Impacts { get; }

    private ConsequenceItem? _selectedImpact;
    public ConsequenceItem? SelectedImpact
    {
        get => _selectedImpact;
        set => SetProperty(ref _selectedImpact, value, onChanged: () =>
        {
            OnPropertyChanged(nameof(SelectedTitle));
            OnPropertyChanged(nameof(SelectedSummary));
            OnPropertyChanged(nameof(SelectedKeyPoints));
            OnPropertyChanged(nameof(SelectedIcon));
        });
    }

    public string SelectedIcon => SelectedImpact?.Icon ?? "🌍";
    public string SelectedTitle => SelectedImpact?.Title ?? "Select an impact node";
    public string SelectedSummary => SelectedImpact?.Summary ?? "Tap an ecosystem node to explore consequence details.";
    public IReadOnlyList<string> SelectedKeyPoints => SelectedImpact?.KeyPoints ?? [];

    public ICommand SelectImpactCommand { get; }

    public ConsequencesViewModel(EcoRootContentService contentService)
    {
        Impacts = new ObservableCollection<ConsequenceItem>(contentService.Consequences);
        SelectedImpact = Impacts.FirstOrDefault();

        SelectImpactCommand = new Command<ConsequenceItem>(impact =>
        {
            if (impact is not null)
            {
                SelectedImpact = impact;
            }
        });
    }
}
