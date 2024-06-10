using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using PTMobile.Models;
using PTMobile.View;
using PTMobile.Views;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace PTMobile.ViewModels
{
    public partial class AllProjectsViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient;
        public ObservableCollection<Project> Projects { get; } = new ObservableCollection<Project>();

        [ObservableProperty]
        private string currentUser;

        [ObservableProperty]
        private bool isAdmin;

        public AllProjectsViewModel()
        {
            _httpClient = new HttpClient();
            LoadProjectsCommand = new AsyncRelayCommand(LoadProjectsAsync);
            SearchProjectsCommand = new AsyncRelayCommand<string>(SearchProjectsAsync);
            ChangeViewCommand = new RelayCommand(ChangeView);
            ShowConfirmationCommand = new AsyncRelayCommand<Project>(ShowConfirmationAsync);
            UpdateProjectCommand = new AsyncRelayCommand<Project>(UpdateProjectAsync);
        }

        public IAsyncRelayCommand LoadProjectsCommand { get; }
        public IAsyncRelayCommand<string> SearchProjectsCommand { get; }
        public IRelayCommand ChangeViewCommand { get; }
        public IAsyncRelayCommand<Project> ShowConfirmationCommand { get; }
        public IAsyncRelayCommand<Project> UpdateProjectCommand { get; }

        private async Task LoadProjectsAsync()
        {
            CurrentUser = TokenManager.currentUser;
            IsAdmin = TokenManager.isAdmin;
            var projects = await GetProjectsAsync(CurrentUser);
            if (projects != null)
            {
                Projects.Clear();
                foreach (var project in projects)
                {
                    Projects.Add(project);
                }
            }
        }

        private async Task<List<Project>> GetProjectsAsync(string username)
        {
            string apiUrl = $"{DevTunnel.UrlFran}/api/Project/get-user-projects?userName={username}";
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Project>>(content);
            }
            else
            {
                return null;
            }
        }

        private async Task SearchProjectsAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                await LoadProjectsAsync();
            }
            else
            {
                var filteredProjects = Projects
                    .Where(p => p.ProjectName.ToLower().Contains(searchTerm.ToLower()))
                    .ToList();
                Projects.Clear();
                foreach (var project in filteredProjects)
                {
                    Projects.Add(project);
                }
            }
        }

        private async void ChangeView()
        {
            await Shell.Current.GoToAsync(nameof(LoginView));
        }

        private async Task ShowConfirmationAsync(Project project)
        {
            if (project != null)
            {
                bool answer = await Shell.Current.DisplayAlert("Cast", $"¿Quieres conectar '{project.ProjectName}'?", "SÍ", "NO");
                if (answer)
                {
                    string url = $"{DevTunnel.UrlFran}/api/Project/select-project?projectData={project.ProjectName}&tvCode={TokenManager.TvCode}";
                    HttpResponseMessage response = await _httpClient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        await Shell.Current.DisplayAlert("Info", $"¡{project.ProjectName}' está conectado!", "OK!");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Info", $"¡{project.ProjectName}' tuvo problemas al conectar!", "OK!");
                    }
                }
            }
        }

        private async Task UpdateProjectAsync(Project project)
        {
            if (project != null)
            {
                bool answer = await Shell.Current.DisplayAlert("Delete", $"¿Quieres borrar '{project.ProjectName}'?", "SÍ", "NO");
                if (answer)
                {
                    string urlDelete = $"{DevTunnel.UrlFran}/api/Project/deleteProject?projectId={Uri.EscapeDataString(project.Id.ToString())}";
                    HttpResponseMessage response = await _httpClient.DeleteAsync(urlDelete);
                    if (response.IsSuccessStatusCode)
                    {
                        Projects.Remove(project);
                        await Shell.Current.DisplayAlert("Info", $"¡{project.ProjectName}' está borrado!", "OK!");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Info", $"¡{project.ProjectName}' tuvo problemas al borrar!", "OK!");
                    }
                }
            }
        }
    }
}
