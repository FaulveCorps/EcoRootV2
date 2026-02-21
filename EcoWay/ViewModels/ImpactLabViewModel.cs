using System.Collections.ObjectModel;
using System.Windows.Input;
using EcoWay.Models;

namespace EcoWay.ViewModels;

public class ImpactLabViewModel : BaseViewModel
{
    private readonly List<ImpactAction> _harmActions =
    [
        new ImpactAction
        {
            Id = "harm_1",
            Icon = "🏭",
            Title = "Release untreated factory runoff",
            Description = "Short-term output increases while toxins enter nearby soil.",
            EffectText = "Heavy chemical residues spread through topsoil and drainage channels.",
            Delta = -18
        },
        new ImpactAction
        {
            Id = "harm_2",
            Icon = "🛢️",
            Title = "Dump mixed plastic waste on open land",
            Description = "Disposal is fast, but decomposition takes centuries.",
            EffectText = "Microplastics accumulate and block healthy soil respiration.",
            Delta = -14
        },
        new ImpactAction
        {
            Id = "harm_3",
            Icon = "🧪",
            Title = "Overuse high-strength chemical treatment",
            Description = "Pest pressure drops quickly but beneficial soil organisms decline.",
            EffectText = "Soil biology weakens and fertility starts to collapse.",
            Delta = -16
        },
        new ImpactAction
        {
            Id = "harm_4",
            Icon = "🌲",
            Title = "Clear vegetation near the watershed",
            Description = "Land opens for development, yet erosion risk spikes.",
            EffectText = "Loose sediment and pollutants flow into local water systems.",
            Delta = -20
        }
    ];

    private readonly List<ImpactAction> _recoveryActions =
    [
        new ImpactAction
        {
            Id = "recovery_1",
            Icon = "🌻",
            Title = "Plant phytoremediation species",
            Description = "Use deep-rooted plants to absorb contaminants over time.",
            EffectText = "Toxin concentrations decline as remediation plants stabilize the area.",
            Delta = 16
        },
        new ImpactAction
        {
            Id = "recovery_2",
            Icon = "🍂",
            Title = "Apply compost and organic matter",
            Description = "Restore microbial activity and improve nutrient retention.",
            EffectText = "Soil structure improves and moisture retention begins to recover.",
            Delta = 14
        },
        new ImpactAction
        {
            Id = "recovery_3",
            Icon = "♻️",
            Title = "Enforce segregation and recycling stream",
            Description = "Prevent mixed waste leakage into vulnerable ground.",
            EffectText = "Pollution load drops as hazardous and recyclable waste are separated.",
            Delta = 12
        },
        new ImpactAction
        {
            Id = "recovery_4",
            Icon = "🌿",
            Title = "Establish cover crops and buffer strips",
            Description = "Protect exposed soil and slow runoff movement.",
            EffectText = "Erosion slows down and biodiversity begins returning to the site.",
            Delta = 18
        }
    ];

    private bool _isHarmMode = true;
    public bool IsHarmMode
    {
        get => _isHarmMode;
        set => SetProperty(ref _isHarmMode, value);
    }

    private string _modeTitle = "Harm Simulation";
    public string ModeTitle
    {
        get => _modeTitle;
        set => SetProperty(ref _modeTitle, value);
    }

    private string _modeSubtitle = "Apply damaging actions and observe environmental decline.";
    public string ModeSubtitle
    {
        get => _modeSubtitle;
        set => SetProperty(ref _modeSubtitle, value);
    }

    private double _environmentHealth;
    public double EnvironmentHealth
    {
        get => _environmentHealth;
        set => SetProperty(ref _environmentHealth, value);
    }

    private string _environmentStatus = "Stable";
    public string EnvironmentStatus
    {
        get => _environmentStatus;
        set => SetProperty(ref _environmentStatus, value);
    }

    private string _statusHint = "Select an action to simulate a visible outcome.";
    public string StatusHint
    {
        get => _statusHint;
        set => SetProperty(ref _statusHint, value);
    }

    private double _pollutionOpacity;
    public double PollutionOpacity
    {
        get => _pollutionOpacity;
        set => SetProperty(ref _pollutionOpacity, value);
    }

    private double _waterClarityOpacity;
    public double WaterClarityOpacity
    {
        get => _waterClarityOpacity;
        set => SetProperty(ref _waterClarityOpacity, value);
    }

    private double _treeScale = 1.0;
    public double TreeScale
    {
        get => _treeScale;
        set => SetProperty(ref _treeScale, value);
    }

    private string _healthText = "0%";
    public string HealthText
    {
        get => _healthText;
        set => SetProperty(ref _healthText, value);
    }

    private double _healthProgress;
    public double HealthProgress
    {
        get => _healthProgress;
        set => SetProperty(ref _healthProgress, value);
    }

    private string _modeChipText = "Harm";
    public string ModeChipText
    {
        get => _modeChipText;
        set => SetProperty(ref _modeChipText, value);
    }

    public Color HarmChipBackground => IsHarmMode
        ? Color.FromArgb("#C84B4B")
        : Color.FromArgb("#4A6268");

    public Color RecoveryChipBackground => IsHarmMode
        ? Color.FromArgb("#4A6268")
        : Color.FromArgb("#2E8B57");

    public ObservableCollection<ImpactAction> CurrentActions { get; } = [];
    public ObservableCollection<string> ActiveEffects { get; } = [];

    public ICommand SetModeCommand { get; }
    public ICommand ApplyActionCommand { get; }
    public ICommand ResetCommand { get; }

    public ImpactLabViewModel()
    {
        SetModeCommand = new Command<string>(SetMode);
        ApplyActionCommand = new Command<ImpactAction>(ApplyAction);
        ResetCommand = new Command(ResetCurrentMode);

        SetMode("harm");
    }

    private void SetMode(string? mode)
    {
        IsHarmMode = !string.Equals(mode, "recovery", StringComparison.OrdinalIgnoreCase);

        if (IsHarmMode)
        {
            ModeChipText = "Harm";
            ModeTitle = "Harm Simulation";
            ModeSubtitle = "Apply damaging actions and observe environmental decline.";
            SetActions(_harmActions);
            EnvironmentHealth = 100;
            StatusHint = "Every action below contributes to environmental harm.";
        }
        else
        {
            ModeChipText = "Recovery";
            ModeTitle = "Recovery Simulation";
            ModeSubtitle = "Apply restorative actions and watch the ecosystem heal.";
            SetActions(_recoveryActions);
            EnvironmentHealth = 18;
            StatusHint = "Each action below contributes to environmental recovery.";
        }

        ActiveEffects.Clear();
        RefreshVisualState();

        OnPropertyChanged(nameof(HarmChipBackground));
        OnPropertyChanged(nameof(RecoveryChipBackground));
    }

    private void SetActions(IEnumerable<ImpactAction> actions)
    {
        CurrentActions.Clear();
        foreach (var action in actions)
        {
            CurrentActions.Add(action);
        }
    }

    private void ResetCurrentMode()
    {
        SetMode(IsHarmMode ? "harm" : "recovery");
    }

    private void ApplyAction(ImpactAction? action)
    {
        if (action is null)
        {
            return;
        }

        EnvironmentHealth = Math.Clamp(EnvironmentHealth + action.Delta, 0, 100);

        ActiveEffects.Insert(0, $"{action.Icon} {action.EffectText}");
        while (ActiveEffects.Count > 4)
        {
            ActiveEffects.RemoveAt(ActiveEffects.Count - 1);
        }

        RefreshVisualState();
    }

    private void RefreshVisualState()
    {
        HealthText = $"{Math.Round(EnvironmentHealth)}%";
        HealthProgress = Math.Clamp(EnvironmentHealth / 100.0, 0.0, 1.0);

        EnvironmentStatus = EnvironmentHealth switch
        {
            >= 80 => "Thriving Environment",
            >= 60 => "Recovering",
            >= 35 => "Vulnerable",
            _ => "Critical Condition"
        };

        PollutionOpacity = Math.Clamp(1.0 - (EnvironmentHealth / 100.0), 0.05, 0.95);
        WaterClarityOpacity = Math.Clamp(0.2 + (EnvironmentHealth / 125.0), 0.2, 1.0);
        TreeScale = 0.72 + (EnvironmentHealth / 200.0);
    }
}
