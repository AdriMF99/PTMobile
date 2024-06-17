using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using PTMobile.Models;
using PTMobile.Views;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PTMobile.ViewModels
{
    public partial class AllProjectsToAddViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Project> _projects;

        [ObservableProperty]
        private int _columns = 1;

        [ObservableProperty]
        private string _currentUser;

        public ICommand ChangeViewCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }

        public AllProjectsToAddViewModel()
        {
            CurrentUser = TokenManager.currentUser;
            ChangeViewCommand = new RelayCommand(ChangeView);
            BackCommand = new RelayCommand(async () => await Back());
            AddCommand = new RelayCommand<Project>(async (project) => await AddProject(project));
            DeleteCommand = new RelayCommand<Project>(async (project) => await DeleteProject(project));
            LoadProjects();
        }

        private async void LoadProjects()
        {
            var projects = await GetProjectsAsync();
            TokenManager.Allprojects = projects;
            if (projects != null)
            {
                Projects = new ObservableCollection<Project>(projects);
            }
        }

        private async Task<List<Project>> GetProjectsAsync()
        {
            using (var httpClient = new HttpClient())
            {
                string apiUrl = $"{DevTunnel.UrlAdri}/api/Project/getProjects";
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

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
        }

        private void ChangeView()
        {
            Columns = (Columns == 1) ? 2 : 1;
        }

        private async Task Back()
        {
            await Shell.Current.GoToAsync("//AllProjects");
        }


        private async Task AddProject(Project project)
        {
            if (project != null)
            {
                bool answer = await Application.Current.MainPage.DisplayAlert("Add", $"¿Quieres añadir '{project.ProjectName}' a los proyectos?", "SÍ", "NO");
                if (answer)
                {
                    using (var httpClient = new HttpClient())
                    {
                        string projectName = project.ProjectName;
                        string usuarcillo = TokenManager.currentUser;
                        string userAdmin = TokenManager.selectedUserAdmin;
                        string urlAdd = $"{DevTunnel.UrlAdri}/api/Project/add-project-user?projectName={projectName}&userName={usuarcillo}";
                        string urlAddAdmin = $"{DevTunnel.UrlAdri}/api/Project/add-project-user?projectName={projectName}&userName={TokenManager.selectedUserAdmin}";

                        HttpResponseMessage response = TokenManager.isFromAdmin ? await httpClient.PutAsync(urlAddAdmin, null) : await httpClient.PutAsync(urlAdd, null);
                        if (response.IsSuccessStatusCode)
                        {
                            await Application.Current.MainPage.DisplayAlert("Info", $"¡{project.ProjectName}' añadido!", "OK");
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Info", $"¡{project.ProjectName}' tuvo problemas al añadirse!", "OK");
                        }
                    }
                }
            }
        }

        private async Task DeleteProject(Project project)
        {
            if (project != null)
            {
                bool answer = await Application.Current.MainPage.DisplayAlert("Delete", $"¿Quieres borrar '{project.ProjectName}' de los proyectos?", "SÍ", "NO");
                if (answer)
                {
                    using (var httpClient = new HttpClient())
                    {
                        string urlDelete = $"{DevTunnel.UrlAdri}/api/Project/delete-project-user?projectName={project.ProjectName}&userName={TokenManager.currentUser}";
                        string urlDeleteAdmin = $"{DevTunnel.UrlAdri}/api/Project/delete-project-user?projectName={project.ProjectName}&userName={TokenManager.selectedUserAdmin}";

                        HttpResponseMessage response = TokenManager.isFromAdmin ? await httpClient.DeleteAsync(urlDeleteAdmin) : await httpClient.DeleteAsync(urlDelete);
                        if (response.IsSuccessStatusCode)
                        {
                            await Application.Current.MainPage.DisplayAlert("Info", $"¡{project.ProjectName}' se ha borrado de los proyectos!", "OK");
                            LoadProjects();
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Info", $"¡{project.ProjectName}' tuvo problemas al borrarse!", "OK");
                        }
                    }
                }
            }
        }

        public async void SearchProjects(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                var projects = await GetProjectsAsync();
                Projects = new ObservableCollection<Project>(projects);
            }
            else
            {
                var allProjects = TokenManager.Allprojects;
                if (allProjects != null)
                {
                    var filteredProjects = allProjects.Where(p => p.ProjectName.ToLower().Contains(searchTerm.ToLower())).ToList();
                    Projects = new ObservableCollection<Project>(filteredProjects);
                }
            }
        }
    }
}