using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using EcoWay.Services;
using EcoWay.ViewModels;
using EcoWay.Views;

namespace EcoWay;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<EcoRootContentService>();

		builder.Services.AddTransient<HomeViewModel>();
		builder.Services.AddTransient<HomePage>();

		builder.Services.AddTransient<CausesViewModel>();
		builder.Services.AddTransient<CausesPage>();

		builder.Services.AddTransient<ConsequencesViewModel>();
		builder.Services.AddTransient<ConsequencesPage>();

		builder.Services.AddTransient<SolutionsViewModel>();
		builder.Services.AddTransient<SolutionsPage>();

		builder.Services.AddTransient<ActionPlanViewModel>();
		builder.Services.AddTransient<ActionPlanPage>();

		builder.Services.AddTransient<SplashPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
