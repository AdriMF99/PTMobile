using Newtonsoft.Json;
using PTMobile.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace PTMobile;

public partial class AllProjects : ContentPage
{
    public List<Project> projectitos;

    public AllProjects()
    {
        InitializeComponent();
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
    }


    private async Task<List<Project>> GetProjectsAsync()
    {
        using (var httpClient = new HttpClient())
        {
            string apiUrl = "https://4c1kzvwr-5250.uks1.devtunnels.ms/api/Project/getProjects";

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
        var button = sender as Image;
        var project = button?.BindingContext as Project;

        if (project != null)
        {
            bool answer = await DisplayAlert("Cast", $"¿Quieres conectar '{project.ProjectName}'?", "SÍ", "NO");
            if (answer)
            {
                using (var httpClient = new HttpClient())
                {
                    // Acción si se pulsa "SÍ"
                    string url = $"https://4c1kzvwr-5250.uks1.devtunnels.ms/api/Project/select-project?projectData={project.ProjectName}&tvCode={TokenManager.TvCode}";
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        await DisplayAlert("Info", $"¡{project.ProjectName}' está conectado!", "OK!");
                    }
                    else
                    {
                        await DisplayAlert("Info", $"¡{project.ProjectName}' tuvo problemas al conectar!", "OK!");
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
        var button = sender as Image;
        var project = button?.BindingContext as Project;

        if (project != null)
        {
            bool answer = await DisplayAlert("Delete", $"¿Quieres borrar '{project.ProjectName}'?", "SÍ", "NO");

            if (answer)
            {
                using(var httpClient = new HttpClient())
                {
                    string urlDelete = $"https://4c1kzvwr-5250.uks1.devtunnels.ms/api/Project/deleteProject?projectId={project.Id}";
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
        var searchTerm = e.NewTextValue?.ToLower().ToString();

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
}