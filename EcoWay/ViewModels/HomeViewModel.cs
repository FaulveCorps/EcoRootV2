using EcoWay.Models;
using EcoWay.Services;

namespace EcoWay.ViewModels;

public class HomeViewModel : BaseViewModel
{
    public string HeadlineLine1 => "The Ground";
    public string HeadlineLine2 => "Beneath Our Feet";
    public string HeadlineLine3 => "Needs Help";
    public string Subtitle => "A mobile learning journey on soil pollution, impact, and restoration.";

    public IReadOnlyList<HeroStat> Stats { get; }

    public string CommunityPeople => "15,847";
    public string CommunityHectares => "2,341";
    public string CommunityCountries => "89";

    public HomeViewModel(EcoRootContentService contentService)
    {
        Stats = contentService.HeroStats;
    }
}
