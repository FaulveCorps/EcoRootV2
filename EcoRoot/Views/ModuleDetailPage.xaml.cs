using System.Collections.Generic;
using EcoRoot.ViewModels;

namespace EcoRoot.Views;

public partial class ModuleDetailPage : ContentPage, IQueryAttributable
{
    readonly ModuleDetailViewModel viewModel;

    public ModuleDetailPage(ModuleDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        this.viewModel = viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("moduleId", out var value))
        {
            viewModel.LoadModule(value?.ToString());
        }
    }
}
