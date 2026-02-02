using EcoRoot.ViewModels;

namespace EcoRoot.Views;

public partial class CloudSyncPage : ContentPage
{
    public CloudSyncPage(CloudSyncViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
