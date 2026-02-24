using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using EcoWay.Models;
using EcoWay.Services;

namespace EcoWay.ViewModels;

public class ActionPlanViewModel : BaseViewModel
{
    private readonly EcoRootContentService _contentService;

    public ObservableCollection<PickerOption> RoleOptions { get; }
    public ObservableCollection<PickerOption> LocationOptions { get; }

    private PickerOption? _selectedRole;
    public PickerOption? SelectedRole
    {
        get => _selectedRole;
        set => SetProperty(ref _selectedRole, value, onChanged: OnSelectionChanged);
    }

    private PickerOption? _selectedLocation;
    public PickerOption? SelectedLocation
    {
        get => _selectedLocation;
        set => SetProperty(ref _selectedLocation, value, onChanged: OnSelectionChanged);
    }

    private string _planTitle = "Your Personalized Action Plan";
    public string PlanTitle
    {
        get => _planTitle;
        set => SetProperty(ref _planTitle, value);
    }

    private string _planSubtitle = "Choose your role and location to generate focused recommendations.";
    public string PlanSubtitle
    {
        get => _planSubtitle;
        set => SetProperty(ref _planSubtitle, value);
    }

    private bool _isPlanVisible;
    public bool IsPlanVisible
    {
        get => _isPlanVisible;
        set => SetProperty(ref _isPlanVisible, value);
    }

    public ObservableCollection<ActionChecklistItem> ImmediateActions { get; } = [];
    public ObservableCollection<ActionChecklistItem> ShortTermActions { get; } = [];
    public ObservableCollection<ActionChecklistItem> LongTermActions { get; } = [];

    private int _completedCount;
    public int CompletedCount
    {
        get => _completedCount;
        set => SetProperty(ref _completedCount, value);
    }

    private int _totalCount;
    public int TotalCount
    {
        get => _totalCount;
        set => SetProperty(ref _totalCount, value);
    }

    private int _impactScore;
    public int ImpactScore
    {
        get => _impactScore;
        set => SetProperty(ref _impactScore, value);
    }

    private double _progressRatio;
    public double ProgressRatio
    {
        get => _progressRatio;
        set => SetProperty(ref _progressRatio, value, onChanged: () =>
        {
            OnPropertyChanged(nameof(ProgressText));
        });
    }

    public string ProgressText => $"{Math.Round(ProgressRatio * 100)}% Complete";

    public string CommunityPeople => "15,847";
    public string CommunityHectares => "2,341";
    public string CommunityCountries => "89";

    public IReadOnlyList<FaqItem> FaqItems { get; }

    public ICommand GeneratePlanCommand { get; }
    public ICommand ResetPlanCommand { get; }

    public ActionPlanViewModel(EcoRootContentService contentService)
    {
        _contentService = contentService;

        RoleOptions = new ObservableCollection<PickerOption>(
            contentService.Roles.Select(r => new PickerOption(r, contentService.GetRoleLabel(r))));

        LocationOptions = new ObservableCollection<PickerOption>(
            contentService.Locations.Select(l => new PickerOption(l, contentService.GetLocationLabel(l))));

        FaqItems = contentService.FaqItems;

        GeneratePlanCommand = new Command(GeneratePlan, () => CanGeneratePlan());
        ResetPlanCommand = new Command(ResetPlan);
    }

    private void OnSelectionChanged()
    {
        if (GeneratePlanCommand is Command command)
        {
            command.ChangeCanExecute();
        }
    }

    private bool CanGeneratePlan()
    {
        return SelectedRole is not null && SelectedLocation is not null;
    }

    private void GeneratePlan()
    {
        if (SelectedRole is null || SelectedLocation is null)
        {
            return;
        }

        var plan = _contentService.GetActionPlan(SelectedRole.Key, SelectedLocation.Key);
        if (plan is null)
        {
            return;
        }

        PlanTitle = plan.Title;
        PlanSubtitle = plan.Subtitle;

        ReplaceItems(ImmediateActions, plan.ImmediateActions);
        ReplaceItems(ShortTermActions, plan.ShortTermActions);
        ReplaceItems(LongTermActions, plan.LongTermActions);

        IsPlanVisible = true;
        RecalculateProgress();
    }

    private void ResetPlan()
    {
        SelectedRole = null;
        SelectedLocation = null;

        ClearItems(ImmediateActions);
        ClearItems(ShortTermActions);
        ClearItems(LongTermActions);

        PlanTitle = "Your Personalized Action Plan";
        PlanSubtitle = "Choose your role and location to generate focused recommendations.";
        IsPlanVisible = false;

        CompletedCount = 0;
        TotalCount = 0;
        ImpactScore = 0;
        ProgressRatio = 0;
    }

    private void ReplaceItems(ObservableCollection<ActionChecklistItem> target, IEnumerable<ActionStep> source)
    {
        ClearItems(target);

        foreach (var step in source)
        {
            var item = new ActionChecklistItem(step.Title, step.Description, step.Impact);
            item.PropertyChanged += OnChecklistItemPropertyChanged;
            target.Add(item);
        }
    }

    private void ClearItems(ObservableCollection<ActionChecklistItem> target)
    {
        foreach (var item in target)
        {
            item.PropertyChanged -= OnChecklistItemPropertyChanged;
        }

        target.Clear();
    }

    private void OnChecklistItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ActionChecklistItem.IsCompleted))
        {
            RecalculateProgress();
        }
    }

    private void RecalculateProgress()
    {
        var allItems = ImmediateActions
            .Concat(ShortTermActions)
            .Concat(LongTermActions)
            .ToList();

        TotalCount = allItems.Count;
        CompletedCount = allItems.Count(i => i.IsCompleted);
        ImpactScore = allItems.Where(i => i.IsCompleted).Sum(i => i.Impact);

        ProgressRatio = TotalCount == 0
            ? 0
            : Math.Clamp((double)CompletedCount / TotalCount, 0, 1);
    }
}

public class PickerOption
{
    public string Key { get; }
    public string Label { get; }

    public PickerOption(string key, string label)
    {
        Key = key;
        Label = label;
    }
}

public class ActionChecklistItem : BaseViewModel
{
    private bool _isCompleted;

    public string Title { get; }
    public string Description { get; }
    public int Impact { get; }
    public string ImpactText => $"+{Impact}";

    public bool IsCompleted
    {
        get => _isCompleted;
        set => SetProperty(ref _isCompleted, value);
    }

    public ActionChecklistItem(string title, string description, int impact)
    {
        Title = title;
        Description = description;
        Impact = impact;
    }
}
