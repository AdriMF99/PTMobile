using Newtonsoft.Json;
using PTMobile.Models;
using PTMobile.ViewModels;
using PTMobile.Views;

namespace PTMobile.Views;

public partial class AllProjects : ContentPage
{
    //public List<Project> projectitos;
    //private readonly GridItemsLayout _oneColumnLayout = new GridItemsLayout(ItemsLayoutOrientation.Vertical) { Span = 1 };
    //private readonly GridItemsLayout _twoColumnLayout = new GridItemsLayout(ItemsLayoutOrientation.Vertical) { Span = 2 };
    //private bool _isOneColumn = true;

    public AllProjects()
    {
        InitializeComponent();
        BindingContext = new AllProjectsViewModel();
    }

    protected override bool OnBackButtonPressed() => true;

    //private async void OnMoreClicked(object sender, EventArgs e)
    //{
    //    var action = await DisplayActionSheet("Options", "Cancel", null, "Option 1", "Option 2", "Option 3");
    //    // Manejar la selección de las opciones aquí
    //}


    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var viewModel = BindingContext as AllProjectsViewModel;
        if (viewModel != null)
        {
            await viewModel.LoadProjectsCommand.ExecuteAsync(null);
        }
    }

    private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var viewModel = BindingContext as AllProjectsViewModel;
        if (viewModel != null)
        {
            await viewModel.SearchProjectsCommand.ExecuteAsync(e.NewTextValue);
        }
    }

    //protected override async void OnAppearing()
    //{
    //    base.OnAppearing();

    //    if (TokenManager.isAdmin == true)
    //    {
    //        adminbutton.IsVisible = true;
    //    }
    //    else
    //    {
    //        adminbutton.IsVisible = false;
    //    }

    //    List<Project> projects = await GetProjectsAsync(TokenManager.currentUser);
    //    TokenManager.Allprojects = await GetProjectsAsync(TokenManager.currentUser);
    //    if (projects != null)
    //    {
    //        projectsList.ItemsSource = projects;
    //    }

    //    ShowSwipeAnimation();
    //}


    //private async Task<List<Project>> GetProjectsAsync(string username)
    //{
    //    using (var httpClient = new HttpClient())
    //    {
    //        string apiUrl = $"{DevTunnel.UrlDeborah}/api/Project/get-user-projects?userName={username}";

    //        HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

    //        if (response.IsSuccessStatusCode)
    //        {
    //            string content = await response.Content.ReadAsStringAsync();
    //            return JsonConvert.DeserializeObject<List<Project>>(content);
    //        }
    //        else
    //        {
    //            // Handle API call failure (e.g., display error message)
    //            return null;
    //        }
    //    }
    //}


    //private async void OnNavigateToSharedPageClicked(object sender, EventArgs e)
    //{
    //    await Shell.Current.GoToAsync(nameof(UpdateFields));
    //}


    //public async void OnShowConfirmation(object sender, EventArgs e)
    //{
    //    var button = sender as SwipeItem;
    //    var project = button?.BindingContext as Project;

    //    if (project != null)
    //    {
    //        bool answer = await DisplayAlert("Cast", $"¿Quieres conectar '{project.ProjectName}'?", "SÍ", "NO");
    //        if (answer)
    //        {
    //            using (var httpClient = new HttpClient())
    //            {
    //                // Acción si se pulsa "SÍ"
    //                string url = $"{DevTunnel.UrlDeborah}/api/Project/select-project?projectData={project.ProjectName}&tvCode={TokenManager.TvCode}";
    //                HttpResponseMessage response = await httpClient.GetAsync(url);
    //                if (response.IsSuccessStatusCode)
    //                {
    //                    await DisplayAlert("Info", $"¡{project.ProjectName}' está conectado!", "OK!");
    //                }
    //                else
    //                {
    //                    await DisplayAlert("Info", $"¡{project.ProjectName}' tuvo problemas al conectar!", "OK!");
    //                }
    //            }
    //        }
    //        else
    //        {
    //            // Acción si se pulsa "NO"
    //        }
    //    }
    //    else
    //    {
    //        await DisplayAlert("Error", "No se pudo obtener la información del proyecto", "OK");
    //    }
    //}

    //public async void OnUpdateButtonTapped(object sender, EventArgs e)
    //{
    //    var button = sender as SwipeItem;
    //    var project = button?.BindingContext as Project;

    //    if (project != null)
    //    {
    //        bool answer = await DisplayAlert("Delete", $"¿Quieres borrar '{project.ProjectName}'?", "SÍ", "NO");

    //        if (answer)
    //        {
    //            using (var httpClient = new HttpClient())
    //            {
    //                string urlDelete = $"{DevTunnel.UrlDeborah}/api/Project/deleteProject?projectId={project.Id}";
    //                HttpResponseMessage response = await httpClient.DeleteAsync(urlDelete);
    //                if (response.IsSuccessStatusCode)
    //                {
    //                    await DisplayAlert("Info", $"¡{project.ProjectName}' está borrado!", "OK!");
    //                    List<Project> projects = await GetProjectsAsync(TokenManager.currentUser);
    //                    TokenManager.Allprojects = projects;
    //                    if (projects != null)
    //                    {
    //                        projectsList.ItemsSource = projects;
    //                    }
    //                }
    //                else
    //                {
    //                    await DisplayAlert("Info", $"¡{project.ProjectName}' tuvo problemas al borrar!", "OK!");
    //                }
    //            }
    //        }
    //        else
    //        {
    //            // Acción si se pulsa "NO"
    //        }
    //    }
    //    else
    //    {
    //        await DisplayAlert("Error", "No se pudo obtener la información del proyecto", "OK");
    //    }
    //}

    //private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    //{
    //    var searchTerm = e.NewTextValue?.ToLower();

    //    if (string.IsNullOrEmpty(searchTerm))
    //    {
    //        List<Project> projects = await GetProjectsAsync(TokenManager.currentUser);
    //        if (projects != null)
    //        {
    //            projectsList.ItemsSource = projects;
    //        }
    //    }
    //    else
    //    {
    //        var allProjects = TokenManager.Allprojects;
    //        if (allProjects != null)
    //        {
    //            var filteredProjects = allProjects.Where(p => p.ProjectName.ToLower().Contains(searchTerm)).ToList();
    //            projectsList.ItemsSource = filteredProjects;
    //        }
    //    }
    //}

    //private void OnChangeViewButtonClicked(object sender, EventArgs e)
    //{
    //    if (_isOneColumn)
    //    {
    //        projectsList.ItemsLayout = _twoColumnLayout;
    //    }
    //    else
    //    {
    //        projectsList.ItemsLayout = _oneColumnLayout;
    //    }

    //    _isOneColumn = !_isOneColumn;
    //}

    //private async void OnAddProjectButtonClicked(object sender, EventArgs e)
    //{
    //    TokenManager.isFromAdmin = false;
    //    await Navigation.PushAsync(new PasswordAddProject());
    //}

    //private async void OnAdminClicked(object sender, EventArgs e)
    //{
    //    TokenManager.isFromAdmin = true;
    //    await Navigation.PushAsync(new AllUsersView());
    //}

    //private async void ShowSwipeAnimation()
    //{
    //    var firstProject = projectsList?.ItemsSource?.Cast<Project>().FirstOrDefault();

    //    if (firstProject != null)
    //    {
    //        await Task.Delay(1000);

    //        await projectsList.TranslateTo(-50, 0, 250, Easing.SinInOut);
    //        await projectsList.TranslateTo(0, 0, 250, Easing.SinInOut);
    //        await Task.Delay(500);

    //        await projectsList.TranslateTo(50, 0, 250, Easing.SinInOut);
    //        await projectsList.TranslateTo(0, 0, 250, Easing.SinInOut);
    //    }
    //}
}