using Newtonsoft.Json;
using PTMobile.Models;
using System.Text;

namespace PTMobile.Views;

public partial class AllProjectsToAdd : ContentPage
{
    public List<Project> projectitos;
    private readonly GridItemsLayout _oneColumnLayout = new GridItemsLayout(ItemsLayoutOrientation.Vertical) { Span = 1 };
    private readonly GridItemsLayout _twoColumnLayout = new GridItemsLayout(ItemsLayoutOrientation.Vertical) { Span = 2 };
    private bool _isOneColumn = true;

    public AllProjectsToAdd()
    {
        InitializeComponent();
        currentUser.Text = TokenManager.currentUser;
        projectsList.ItemsLayout = _oneColumnLayout;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        List<Project> projects = await GetProjectsAsync();
        TokenManager.Allprojects = await GetProjectsAsync();
        if (projects != null)
        {
            projectsList.ItemsSource = projects;
        }

        ShowSwipeAnimation();
    }


    private async Task<List<Project>> GetProjectsAsync()
    {
        using (var httpClient = new HttpClient())
        {
            string apiUrl = $"{DevTunnel.UrlDeborah}/api/Project/getProjects";

            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Project>>(content);
            }
            else
            {
                // Handle API call failure (e.g., display error message)
                return null;
            }
        }
    }

    public async void OnShowConfirmation(object sender, EventArgs e)
    {
        var button = sender as SwipeItem;
        var project = button?.BindingContext as Project;

        if (project != null)
        {
            bool answer = await DisplayAlert("Add", $"¿Quieres añadir '{project.ProjectName}' a los proyectos?", "SÍ", "NO");
            if (answer)
            {
                using (var httpClient = new HttpClient())
                {
                    // Acción si se pulsa "SÍ"
                    string projectName = project.ProjectName.ToString();
                    string usuarcillo = TokenManager.currentUser.ToString();
                    string userAdmin = TokenManager.selectedUserAdmin.ToString();
                    string urlAdd = $"{DevTunnel.UrlDeborah}/api/Project/add-project-user?projectName={Uri.EscapeDataString(projectName)}&userName={Uri.EscapeDataString(usuarcillo)}";
                    string urlAddAdmin = $"{DevTunnel.UrlDeborah}/api/Project/add-project-user?projectName={Uri.EscapeDataString(projectName)}&userName={Uri.EscapeDataString(userAdmin)}";

                    if (TokenManager.isFromAdmin == true)
                    {
                        HttpResponseMessage response = await httpClient.PutAsync(urlAddAdmin, null);
                        if (response.IsSuccessStatusCode)
                        {
                            await DisplayAlert("Info", $"¡{project.ProjectName}' añadido a '{userAdmin}'!", "OK!");
                        }
                        else
                        {
                            await DisplayAlert("Info", $"¡{project.ProjectName}' tuvo problemas al añadirse!", "OK!");
                        }
                    }
                    else
                    {
                        HttpResponseMessage response = await httpClient.PutAsync(urlAdd, null);
                        if (response.IsSuccessStatusCode)
                        {
                            await DisplayAlert("Info", $"¡{project.ProjectName}' añadido a '{usuarcillo}'!", "OK!");
                        }
                        else
                        {
                            await DisplayAlert("Info", $"¡{project.ProjectName}' tuvo problemas al añadirse!", "OK!");
                        }
                    }
                }
            }
            else
            {
                // Acción si se pulsa "NO"
            }
        }
        else
        {
            await DisplayAlert("Error", "No se pudo obtener la información del proyecto", "OK");
        }
    }

    public async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AllProjects());
    }

    public async void OnUpdateButtonTapped(object sender, EventArgs e)
    {
        var button = sender as SwipeItem;
        var project = button?.BindingContext as Project;

        if (project != null)
        {
            bool answer = await DisplayAlert("Delete", $"¿Quieres borrar '{project.ProjectName}' de los proyectos?", "SÍ", "NO");

            if (answer)
            {
                using (var httpClient = new HttpClient())
                {
                    string urlDelete = $"{DevTunnel.UrlDeborah}/api/Project/delete-project-user?projectName={project.ProjectName}&userName={TokenManager.currentUser}";
                    string urlDeleteAdmin = $"{DevTunnel.UrlDeborah}/api/Project/delete-project-user?projectName={project.ProjectName}&userName={TokenManager.selectedUserAdmin}";

                    if (TokenManager.isFromAdmin == true)
                    {
                        HttpResponseMessage response = await httpClient.DeleteAsync(urlDeleteAdmin);
                        if (response.IsSuccessStatusCode)
                        {
                            await DisplayAlert("Info", $"¡{project.ProjectName}' se ha borrado de los proyectos de '{TokenManager.selectedUserAdmin}'!", "OK!");
                            List<Project> projects = await GetProjectsAsync();
                            TokenManager.Allprojects = projects;
                            if (projects != null)
                            {
                                projectsList.ItemsSource = projects;
                            }
                        }
                        else
                        {
                            await DisplayAlert("Info", $"¡{project.ProjectName}' tuvo problemas al borrar!", "OK!");
                        }
                    }
                    else
                    {
                        HttpResponseMessage response = await httpClient.DeleteAsync(urlDelete);
                        if (response.IsSuccessStatusCode)
                        {
                            await DisplayAlert("Info", $"¡{project.ProjectName}' se ha borrado de los proyectos de '{TokenManager.currentUser}'!", "OK!");
                            List<Project> projects = await GetProjectsAsync();
                            TokenManager.Allprojects = projects;
                            if (projects != null)
                            {
                                projectsList.ItemsSource = projects;
                            }
                        }
                        else
                        {
                            await DisplayAlert("Info", $"¡{project.ProjectName}' tuvo problemas al borrar!", "OK!");
                        }
                    }

                    
                }
            }
            else
            {
                // Acción si se pulsa "NO"
            }
        }
        else
        {
            await DisplayAlert("Error", "No se pudo obtener la información del proyecto", "OK");
        }
    }

    private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var searchTerm = e.NewTextValue?.ToLower();

        if (string.IsNullOrEmpty(searchTerm))
        {
            List<Project> projects = await GetProjectsAsync();
            if (projects != null)
            {
                projectsList.ItemsSource = projects;
            }
        }
        else
        {
            var allProjects = TokenManager.Allprojects;
            if (allProjects != null)
            {
                var filteredProjects = allProjects.Where(p => p.ProjectName.ToLower().Contains(searchTerm)).ToList();
                projectsList.ItemsSource = filteredProjects;
            }
        }
    }

    private void OnChangeViewButtonClicked(object sender, EventArgs e)
    {
        if (_isOneColumn)
        {
            projectsList.ItemsLayout = _twoColumnLayout;
        }
        else
        {
            projectsList.ItemsLayout = _oneColumnLayout;
        }

        _isOneColumn = !_isOneColumn;
    }

    private async void ShowSwipeAnimation()
    {
        var firstProject = projectsList?.ItemsSource?.Cast<Project>().FirstOrDefault();

        if (firstProject != null)
        {
            await Task.Delay(1000);

            await projectsList.TranslateTo(-50, 0, 250, Easing.SinInOut);
            await projectsList.TranslateTo(0, 0, 250, Easing.SinInOut);
            await Task.Delay(500);

            await projectsList.TranslateTo(50, 0, 250, Easing.SinInOut);
            await projectsList.TranslateTo(0, 0, 250, Easing.SinInOut);
        }
    }
}