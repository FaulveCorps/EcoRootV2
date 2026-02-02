using EcoRoot.ViewModels;

namespace EcoRoot.Views;

public partial class TeacherResourcesPage : ContentPage
{
    public TeacherResourcesPage(TeacherResourcesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
