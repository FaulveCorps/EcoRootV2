using EcoWay.Views;

namespace EcoWay;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute("MainMenuPage", typeof(MainMenuPage));
		Routing.RegisterRoute("ScenarioPage", typeof(ScenarioPage));
		Routing.RegisterRoute("SummaryPage", typeof(SummaryPage));
		Routing.RegisterRoute("ImpactLabPage", typeof(ImpactLabPage));
	}
}
