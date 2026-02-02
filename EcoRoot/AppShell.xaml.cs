using EcoRoot.Views;

namespace EcoRoot;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(ModuleDetailPage), typeof(ModuleDetailPage));
		Routing.RegisterRoute(nameof(LessonDetailPage), typeof(LessonDetailPage));
		Routing.RegisterRoute(nameof(QuizPage), typeof(QuizPage));
		Routing.RegisterRoute(nameof(TeacherResourcesPage), typeof(TeacherResourcesPage));
		Routing.RegisterRoute(nameof(ActivityPage), typeof(ActivityPage));
		Routing.RegisterRoute(nameof(CloudSyncPage), typeof(CloudSyncPage));
	}
}
