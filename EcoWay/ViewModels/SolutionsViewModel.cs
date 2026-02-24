using System.Collections.ObjectModel;
using System.Windows.Input;
using EcoWay.Models;
using EcoWay.Services;

namespace EcoWay.ViewModels;

public class SolutionsViewModel : BaseViewModel
{
    private readonly HashSet<string> _activeMethodIds = new(StringComparer.OrdinalIgnoreCase);

    public ObservableCollection<SolutionItem> Methods { get; }
    public ObservableCollection<string> ActiveBenefits { get; } = [];

    private SolutionItem? _selectedMethod;
    public SolutionItem? SelectedMethod
    {
        get => _selectedMethod;
        set => SetProperty(ref _selectedMethod, value, onChanged: () =>
        {
            OnPropertyChanged(nameof(HasSelectedMethod));
            OnPropertyChanged(nameof(SelectedMethodBenefits));
        });
    }

    public bool HasSelectedMethod => SelectedMethod is not null;

    public IReadOnlyList<string> SelectedMethodBenefits => SelectedMethod?.Benefits ?? [];

    private int _healthScore = 10;
    public int HealthScore
    {
        get => _healthScore;
        set => SetProperty(ref _healthScore, value, onChanged: () =>
        {
            OnPropertyChanged(nameof(HealthProgress));
            OnPropertyChanged(nameof(RestorationStatus));
        });
    }

    public double HealthProgress => Math.Clamp(HealthScore / 100d, 0, 1);

    public string RestorationStatus => HealthScore switch
    {
        < 30 => "☠️ Severely Polluted",
        < 60 => "⚠️ Improving",
        < 85 => "🌱 Recovering Well",
        _ => "🌿 Healthy Soil"
    };

    public string ActiveMethodText => $"Active methods: {_activeMethodIds.Count}/{Methods.Count}";

    public string EmptyBenefitsText => ActiveBenefits.Count == 0
        ? "Tap restoration methods to reveal active benefits."
        : string.Empty;

    public ICommand ToggleMethodCommand { get; }
    public ICommand ResetCommand { get; }

    public SolutionsViewModel(EcoRootContentService contentService)
    {
        Methods = new ObservableCollection<SolutionItem>(contentService.Solutions);
        SelectedMethod = Methods.FirstOrDefault();

        ToggleMethodCommand = new Command<SolutionItem>(ToggleMethod);
        ResetCommand = new Command(ResetMethods);
    }

    private void ToggleMethod(SolutionItem? method)
    {
        if (method is null)
        {
            return;
        }

        SelectedMethod = method;

        if (_activeMethodIds.Contains(method.Id))
        {
            _activeMethodIds.Remove(method.Id);
        }
        else
        {
            _activeMethodIds.Add(method.Id);
        }

        RebuildBenefits();
        RecalculateHealth();
        OnPropertyChanged(nameof(ActiveMethodText));
    }

    private void ResetMethods()
    {
        _activeMethodIds.Clear();
        RebuildBenefits();
        RecalculateHealth();
        OnPropertyChanged(nameof(ActiveMethodText));
    }

    private void RebuildBenefits()
    {
        ActiveBenefits.Clear();

        foreach (var method in Methods.Where(m => _activeMethodIds.Contains(m.Id)))
        {
            foreach (var benefit in method.Benefits)
            {
                ActiveBenefits.Add($"{method.Icon} {benefit}");
            }
        }

        OnPropertyChanged(nameof(EmptyBenefitsText));
    }

    private void RecalculateHealth()
    {
        HealthScore = Math.Clamp(10 + (_activeMethodIds.Count * 15), 10, 100);
    }
}
