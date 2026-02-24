namespace EcoWay.Models;

public record HeroStat(string Value, string Unit, string Label);

public record CauseItem(
    string Id,
    string Icon,
    string Title,
    string ShortDescription,
    IReadOnlyList<string> Impacts);

public record ConsequenceItem(
    string Id,
    string Icon,
    string Title,
    string Summary,
    IReadOnlyList<string> KeyPoints);

public record SolutionItem(
    string Id,
    string Icon,
    string Title,
    string ShortDescription,
    IReadOnlyList<string> Benefits);

public record FaqItem(string Icon, string Question, string Answer);

public record ActionStep(string Title, string Description, int Impact);

public record ActionPlanDefinition(
    string Title,
    string Subtitle,
    IReadOnlyList<ActionStep> ImmediateActions,
    IReadOnlyList<ActionStep> ShortTermActions,
    IReadOnlyList<ActionStep> LongTermActions);