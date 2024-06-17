using Newtonsoft.Json;
using PTMobile.Models;
using PTMobile.ViewModels;
using System.Text;

namespace PTMobile.Views;

public partial class AllProjectsToAdd : ContentPage
{

    public AllProjectsToAdd()
    {
        InitializeComponent();
        BindingContext = new AllProjectsToAddViewModel();
    }

    protected override bool OnBackButtonPressed() => true;

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var viewModel = BindingContext as AllProjectsToAddViewModel;
        viewModel?.SearchProjects(e.NewTextValue);
    }
}