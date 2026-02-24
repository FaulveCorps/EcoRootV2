using System.Collections.ObjectModel;
using System.Windows.Input;
using EcoWay.Models;
using EcoWay.Services;

namespace EcoWay.ViewModels;

public class SolutionsViewModel : BaseViewModel
{
    private readonly HashSet<string> _activeMethodIds = new(StringComparer.OrdinalIgnoreCase);
    private string? _primaryMethodId;

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

    private string _recoverySkyColor = "#8C9A8A";
    public string RecoverySkyColor
    {
        get => _recoverySkyColor;
        set => SetProperty(ref _recoverySkyColor, value);
    }

    private string _recoveryGroundColor = "#6E6658";
    public string RecoveryGroundColor
    {
        get => _recoveryGroundColor;
        set => SetProperty(ref _recoveryGroundColor, value);
    }

    private string _recoveryWaterColor = "#6F7A7B";
    public string RecoveryWaterColor
    {
        get => _recoveryWaterColor;
        set => SetProperty(ref _recoveryWaterColor, value);
    }

    private double _recoveryWaterOpacity = 0.14;
    public double RecoveryWaterOpacity
    {
        get => _recoveryWaterOpacity;
        set => SetProperty(ref _recoveryWaterOpacity, value);
    }

    private double _recoveryHazeOpacity = 0.28;
    public double RecoveryHazeOpacity
    {
        get => _recoveryHazeOpacity;
        set => SetProperty(ref _recoveryHazeOpacity, value);
    }

    private double _recoveryDebrisOpacity = 0.52;
    public double RecoveryDebrisOpacity
    {
        get => _recoveryDebrisOpacity;
        set => SetProperty(ref _recoveryDebrisOpacity, value);
    }

    private double _recoveryBloomOpacity;
    public double RecoveryBloomOpacity
    {
        get => _recoveryBloomOpacity;
        set => SetProperty(ref _recoveryBloomOpacity, value);
    }

    private double _recoveryTreeScale = 0.7;
    public double RecoveryTreeScale
    {
        get => _recoveryTreeScale;
        set => SetProperty(ref _recoveryTreeScale, value);
    }

    private double _recoveryTreeOpacity = 0.5;
    public double RecoveryTreeOpacity
    {
        get => _recoveryTreeOpacity;
        set => SetProperty(ref _recoveryTreeOpacity, value);
    }

    private string _restorationOutcomeTitle = "No active restoration outcome";
    public string RestorationOutcomeTitle
    {
        get => _restorationOutcomeTitle;
        set => SetProperty(ref _restorationOutcomeTitle, value);
    }

    private string _restorationOutcomeDetail = "Activate methods to watch visible ecosystem recovery outcomes.";
    public string RestorationOutcomeDetail
    {
        get => _restorationOutcomeDetail;
        set => SetProperty(ref _restorationOutcomeDetail, value);
    }

    public ICommand ToggleMethodCommand { get; }
    public ICommand ResetCommand { get; }

    public SolutionsViewModel(EcoRootContentService contentService)
    {
        Methods = new ObservableCollection<SolutionItem>(contentService.Solutions);
        SelectedMethod = Methods.FirstOrDefault();

        ToggleMethodCommand = new Command<SolutionItem>(ToggleMethod);
        ResetCommand = new Command(ResetMethods);

        RecalculateVisualization();
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

            if (string.Equals(_primaryMethodId, method.Id, StringComparison.OrdinalIgnoreCase))
            {
                _primaryMethodId = _activeMethodIds.LastOrDefault();
            }
        }
        else
        {
            _activeMethodIds.Add(method.Id);
            _primaryMethodId = method.Id;
        }

        RebuildBenefits();
        RecalculateHealth();
        OnPropertyChanged(nameof(ActiveMethodText));
        RecalculateVisualization();
    }

    private void ResetMethods()
    {
        _activeMethodIds.Clear();
        _primaryMethodId = null;
        RebuildBenefits();
        RecalculateHealth();
        OnPropertyChanged(nameof(ActiveMethodText));
        RecalculateVisualization();
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

    private void RecalculateVisualization()
    {
        var healthFactor = HealthProgress;

        RecoverySkyColor = healthFactor switch
        {
            < 0.3 => "#8C9A8A",
            < 0.6 => "#93B2B7",
            < 0.85 => "#8CC9D9",
            _ => "#82D8F4"
        };

        RecoveryGroundColor = healthFactor switch
        {
            < 0.3 => "#6E6658",
            < 0.6 => "#70805D",
            < 0.85 => "#66945E",
            _ => "#5AAF62"
        };

        RecoveryWaterColor = healthFactor switch
        {
            < 0.3 => "#6F7A7B",
            < 0.6 => "#5F8FA1",
            < 0.85 => "#529EC8",
            _ => "#4AAFEA"
        };

        RecoveryWaterOpacity = Math.Clamp(0.12 + (healthFactor * 0.2), 0.12, 0.34);
        RecoveryHazeOpacity = Math.Clamp(0.36 - (healthFactor * 0.32), 0.03, 0.36);
        RecoveryDebrisOpacity = Math.Clamp(0.6 - (healthFactor * 0.52), 0.0, 0.6);
        RecoveryBloomOpacity = Math.Clamp((healthFactor - 0.25) * 0.9, 0.0, 0.8);
        RecoveryTreeScale = Math.Clamp(0.66 + (healthFactor * 0.52), 0.66, 1.18);
        RecoveryTreeOpacity = Math.Clamp(0.45 + (healthFactor * 0.55), 0.45, 1.0);

        var methodId = _primaryMethodId;
        if (!string.IsNullOrWhiteSpace(methodId) && !_activeMethodIds.Contains(methodId))
        {
            methodId = _activeMethodIds.LastOrDefault();
        }

        if (string.IsNullOrWhiteSpace(methodId))
        {
            RestorationOutcomeTitle = "No active restoration outcome";
            RestorationOutcomeDetail = "Activate methods to watch visible ecosystem recovery outcomes.";
            return;
        }

        switch (methodId)
        {
            case "phytoremediation":
                RecoveryBloomOpacity = Math.Clamp(RecoveryBloomOpacity + 0.22, 0, 0.9);
                RecoveryTreeScale = Math.Clamp(RecoveryTreeScale + 0.08, 0.66, 1.25);
                RestorationOutcomeTitle = "Phytoremediation outcome active";
                break;
            case "composting":
                RecoveryGroundColor = "#6DA35E";
                RecoveryTreeOpacity = Math.Clamp(RecoveryTreeOpacity + 0.08, 0.45, 1.0);
                RestorationOutcomeTitle = "Composting outcome active";
                break;
            case "bioremediation":
                RecoveryWaterColor = "#49B5CF";
                RecoveryHazeOpacity = Math.Clamp(RecoveryHazeOpacity - 0.06, 0.02, 0.36);
                RestorationOutcomeTitle = "Bioremediation outcome active";
                break;
            case "recycling":
                RecoveryDebrisOpacity = Math.Clamp(RecoveryDebrisOpacity - 0.18, 0, 0.6);
                RestorationOutcomeTitle = "Recycling outcome active";
                break;
            case "segregation":
                RecoveryDebrisOpacity = Math.Clamp(RecoveryDebrisOpacity - 0.14, 0, 0.6);
                RecoveryHazeOpacity = Math.Clamp(RecoveryHazeOpacity - 0.04, 0.02, 0.36);
                RestorationOutcomeTitle = "Waste segregation outcome active";
                break;
            case "covercrops":
                RecoveryBloomOpacity = Math.Clamp(RecoveryBloomOpacity + 0.18, 0, 0.9);
                RecoveryGroundColor = "#61A863";
                RecoveryTreeScale = Math.Clamp(RecoveryTreeScale + 0.1, 0.66, 1.25);
                RestorationOutcomeTitle = "Cover crops outcome active";
                break;
            default:
                RestorationOutcomeTitle = "Restoration outcome active";
                break;
        }

        var method = Methods.FirstOrDefault(m => string.Equals(m.Id, methodId, StringComparison.OrdinalIgnoreCase));
        RestorationOutcomeDetail = method is null
            ? "The visual scene reflects active restoration improvements."
            : $"{method.Icon} {method.ShortDescription} {method.Benefits.FirstOrDefault()}";
    }
}
