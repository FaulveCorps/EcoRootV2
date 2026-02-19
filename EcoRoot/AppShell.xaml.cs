using EcoWay.Views;

namespace EcoWay;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Navigated += OnShellNavigated;
		Routing.RegisterRoute(nameof(ModuleDetailPage), typeof(ModuleDetailPage));
		Routing.RegisterRoute(nameof(LessonDetailPage), typeof(LessonDetailPage));
		Routing.RegisterRoute(nameof(QuizPage), typeof(QuizPage));
		Routing.RegisterRoute(nameof(TeacherResourcesPage), typeof(TeacherResourcesPage));
		Routing.RegisterRoute(nameof(ActivityPage), typeof(ActivityPage));
		Routing.RegisterRoute(nameof(CloudSyncPage), typeof(CloudSyncPage));
		UpdateShellTitle();
	}

	private void OnShellNavigated(object? sender, ShellNavigatedEventArgs e)
	{
		UpdateShellTitle();
	}

	private void UpdateShellTitle()
	{
		if (ShellTitleLabel is null)
		{
			return;
		}

		var title = CurrentPage?.Title;
		if (string.IsNullOrWhiteSpace(title))
		{
			title = Title;
		}

		ShellTitleLabel.Text = title;
	}
}

