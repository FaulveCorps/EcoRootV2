using EcoWay.Views;

namespace EcoWay;

public partial class AppShell : Shell
{
	private bool _hasShownStartupSplash;

	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute("SplashPage", typeof(SplashPage));
		Routing.RegisterRoute("HomePage", typeof(HomePage));
		Routing.RegisterRoute("CausesPage", typeof(CausesPage));
		Routing.RegisterRoute("ConsequencesPage", typeof(ConsequencesPage));
		Routing.RegisterRoute("SolutionsPage", typeof(SolutionsPage));
		Routing.RegisterRoute("ActionPlanPage", typeof(ActionPlanPage));
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

		if (_hasShownStartupSplash)
		{
			return;
		}

		_hasShownStartupSplash = true;

		MainThread.BeginInvokeOnMainThread(async () =>
		{
			try
			{
				await GoToAsync("SplashPage", false);
			}
			catch
			{
				// If shell navigation is not yet ready, app can continue to Home.
			}
		});
	}
}
