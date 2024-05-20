using Newtonsoft.Json;
using PTMobile.Models;

namespace PTMobile;

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
            string apiUrl = $"{DevTunnel.UrlFran}/api/Project/getProjects";

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
            bool answer = await DisplayAlert("Add", $"¿Quieres añadir '{project.ProjectName}' a tus proyectos?", "SÍ", "NO");
            if (answer)
            {
                using (var httpClient = new HttpClient())
                {
                    // Acción si se pulsa "SÍ"
                    string urlAdd = $"{DevTunnel.UrlFran}/Project/add-project-user?projectName={project.ProjectName}&userName={TokenManager.currentUser}";
                    HttpResponseMessage response = await httpClient.PutAsync(urlAdd, null);
                    if (response.IsSuccessStatusCode)
                    {
                        await DisplayAlert("Info", $"¡{project.ProjectName}' añadido!", "OK!");
                    }
                    else
                    {
                        await DisplayAlert("Info", $"¡{project.ProjectName}' tuvo problemas al añadirse!", "OK!");
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

    public async void OnUpdateButtonTapped(object sender, EventArgs e)
    {
        var button = sender as SwipeItem;
        var project = button?.BindingContext as Project;

        if (project != null)
        {
            bool answer = await DisplayAlert("Delete", $"¿Quieres borrar '{project.ProjectName}'?", "SÍ", "NO");

            if (answer)
            {
                using (var httpClient = new HttpClient())
                {
                    string urlDelete = $"{DevTunnel.UrlFran}/api/Project/deleteProject?projectId={project.Id}";
                    HttpResponseMessage response = await httpClient.DeleteAsync(urlDelete);
                    if (response.IsSuccessStatusCode)
                    {
                        await DisplayAlert("Info", $"¡{project.ProjectName}' está borrado!", "OK!");
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

    private async void OnAddProjectButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PasswordAddProject());
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