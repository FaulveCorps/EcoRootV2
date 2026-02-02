using EcoRoot.ViewModels;
using EcoRoot.Views;
using EcoRoot.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EcoRoot;

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

		builder.Services.AddSingleton<SampleContentService>();
		builder.Services.AddSingleton<IContentService, JsonContentService>();
		builder.Services.AddSingleton<IProgressStore, SqliteProgressStore>();
		builder.Services.AddSingleton<IQuizStatsStore, SqliteQuizStatsStore>();

		builder.Services.AddSingleton<HomeViewModel>();
		builder.Services.AddSingleton<HomePage>();
		builder.Services.AddSingleton<ModulesViewModel>();
		builder.Services.AddSingleton<ModulesPage>();
		builder.Services.AddSingleton<GlossaryViewModel>();
		builder.Services.AddSingleton<GlossaryPage>();
		builder.Services.AddSingleton<ProgressViewModel>();
		builder.Services.AddSingleton<ProgressPage>();
		builder.Services.AddSingleton<TeacherResourcesViewModel>();
		builder.Services.AddSingleton<TeacherResourcesPage>();
		builder.Services.AddSingleton<ActivityViewModel>();
		builder.Services.AddSingleton<ActivityPage>();
		builder.Services.AddSingleton<CloudSyncViewModel>();
		builder.Services.AddSingleton<CloudSyncPage>();

		builder.Services.AddTransient<ModuleDetailViewModel>();
		builder.Services.AddTransient<ModuleDetailPage>();
		builder.Services.AddTransient<LessonDetailViewModel>();
		builder.Services.AddTransient<LessonDetailPage>();
		builder.Services.AddTransient<QuizViewModel>();
		builder.Services.AddTransient<QuizPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
