using Newtonsoft.Json;
using PTMobile.Models;

namespace PTMobile;

public partial class AllProjects : ContentPage
{
    //public ObservableCollection<Project> Projects { get; } = new ObservableCollection<Project>();

    public AllProjects()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        List<Project> projects = await GetProjectsAsync();
        if (projects != null)
        {
            projectsList.ItemsSource = projects;
        }
    }


    private async Task<List<Project>> GetProjectsAsync()
    {
        using (var httpClient = new HttpClient())
        {
            string apiUrl = "https://cmg6rb8b-5250.uks1.devtunnels.ms/api/Project/getProjects";

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
                    string url = $"https://cmg6rb8b-5250.uks1.devtunnels.ms/api/Project/select-project?projectData={project.ProjectName}";
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


}