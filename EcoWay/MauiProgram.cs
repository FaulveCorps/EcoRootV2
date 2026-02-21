using Microsoft.Extensions.Logging;
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
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<ScenarioService>();
		builder.Services.AddSingleton<GameStateService>();

		builder.Services.AddTransient<ScenarioViewModel>();
		builder.Services.AddTransient<ScenarioPage>();

		builder.Services.AddTransient<SummaryViewModel>();
		builder.Services.AddTransient<SummaryPage>();

		builder.Services.AddTransient<ImpactLabViewModel>();
		builder.Services.AddTransient<ImpactLabPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
