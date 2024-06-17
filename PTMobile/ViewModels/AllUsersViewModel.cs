using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using PTMobile.Models;
using PTMobile.Views;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace PTMobile.ViewModels
{
    public partial class AllUsersViewModel : ObservableObject
    {
        [ObservableProperty]
        private string userListLabel;

        [ObservableProperty]
        private ObservableCollection<User> users;

        private readonly HttpClient _httpClient;

        public AllUsersViewModel()
        {
            _httpClient = new HttpClient();
            Users = new ObservableCollection<User>();
            //AnimateLabelCommand = new AsyncRelayCommand<Label>(AnimateLabelAsync);
            LoadUsersCommand = new AsyncRelayCommand(LoadUsersAsync);
            UserTappedCommand = new AsyncRelayCommand<User>(OnUserTappedAsync);
            ImageTappedCommand = new AsyncRelayCommand<string>(OnImageTappedAsync);
            GodTappedCommand = new AsyncRelayCommand<string>(OnGodTappedAsync);
            CreatePDFCommand = new RelayCommand(CreatePDF);
        }

        //public IAsyncRelayCommand<Label> AnimateLabelCommand { get; }
        public IAsyncRelayCommand LoadUsersCommand { get; }
        public IAsyncRelayCommand<User> UserTappedCommand { get; }
        public IAsyncRelayCommand<string> ImageTappedCommand { get; }
        public IAsyncRelayCommand<string> GodTappedCommand { get; }
        public IRelayCommand CreatePDFCommand { get; }

        /*private async Task AnimateLabelAsync(Label label)
        {
            if (label == null)
                return;

            label.TranslationX = -label.Width;
            await label.TranslateTo(0, 0, 1000, Easing.Linear);
        }*/

        private async Task LoadUsersAsync()
        {
            string apiUrl = $"{DevTunnel.UrlAdri}/User/all-users";
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var usersList = JsonConvert.DeserializeObject<List<User>>(content);
                await SetAdminStatus(usersList);
                Users = new ObservableCollection<User>(usersList);
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "No se pudo obtener la lista de usuarios.", "OK");
            }
        }

        private async Task SetAdminStatus(List<User> users)
        {
            List<Task> tasks = new List<Task>();

            foreach (var user in users)
            {
                string isAdminUrl = $"{DevTunnel.UrlAdri}/User/is-admin?username={user.UserName}";
                string isGodUrl = $"{DevTunnel.UrlAdri}/User/is-god?username={user.UserName}";

                tasks.Add(SetUserAdminStatus(user, isAdminUrl, isGodUrl));
            }

            await Task.WhenAll(tasks);
        }

        private async Task SetUserAdminStatus(User user, string isAdminUrl, string isGodUrl)
        {
            var isAdminTask = _httpClient.GetAsync(isAdminUrl);
            var isGodTask = _httpClient.GetAsync(isGodUrl);

            await Task.WhenAll(isAdminTask, isGodTask);

            if (isAdminTask.Result.IsSuccessStatusCode && isGodTask.Result.IsSuccessStatusCode)
            {
                var isAdminContent = await isAdminTask.Result.Content.ReadAsStringAsync();
                var isGodContent = await isGodTask.Result.Content.ReadAsStringAsync();

                bool isAdmin = JsonConvert.DeserializeObject<bool>(isAdminContent);
                bool isGod = JsonConvert.DeserializeObject<bool>(isGodContent);

                user.IsAdmin = isAdmin || isGod;
                user.IsGod = isGod;
            }
            else
            {
                user.IsAdmin = false;
                user.IsGod = false;
            }
        }

        private async Task OnUserTappedAsync(User user)
        {
            if (user != null)
            {
                bool answer = await Shell.Current.DisplayAlert("Añadir Proyectos",
                                                                $"¿Desea añadir proyectos a {user.UserName}?",
                                                                "Sí", "No");
                if (answer)
                {
                    TokenManager.selectedUserAdmin = user.UserName;
                    TokenManager.isFromAdmin = true;
                    await Shell.Current.GoToAsync("//AllProjectsToAdd");
                }
            }
        }

        private async Task OnImageTappedAsync(string userName)
        {
            if (!string.IsNullOrWhiteSpace(userName))
            {
                bool answer = await Shell.Current.DisplayAlert("Cambiar Rol", $"¿Deseas que '{userName}' cambie de Rol?", "Sí", "No");

                if (answer)
                {
                    string urlSetAdmin = $"{DevTunnel.UrlAdri}/User/set-admin?username={userName}&userLoged={TokenManager.currentUser}";
                    var response = await _httpClient.PutAsync(urlSetAdmin, null);

                    if (response.IsSuccessStatusCode)
                    {
                        var userToUpdate = Users.FirstOrDefault(u => u.UserName == userName);
                        if (userToUpdate != null)
                        {
                            userToUpdate.IsAdmin = !userToUpdate.IsAdmin;
                            userToUpdate.IsGod = false;

                            OnPropertyChanged(nameof(User.IsAdmin));
                            OnPropertyChanged(nameof(User.IsGod));
                        }
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Info", $"{userName} tuvo problemas al cambiar de rol :(", "OK");
                    }
                }
            }
        }

        private async Task OnGodTappedAsync(string userName)
        {
            if (!string.IsNullOrWhiteSpace(userName))
            {
                bool answer = await Shell.Current.DisplayAlert("Cambiar Rol", $"¿Deseas que '{userName}' cambie de Rol? (SP)", "Sí", "No");

                if (answer)
                {
                    string urlSetAdmin = $"{DevTunnel.UrlAdri}/User/set-admin?username={userName}&userLoged={TokenManager.currentUser}";
                    var response = await _httpClient.PutAsync(urlSetAdmin, null);

                    if (response.IsSuccessStatusCode)
                    {
                        var userToUpdate = Users.FirstOrDefault(u => u.UserName == userName);
                        if (userToUpdate != null)
                        {
                            if (userToUpdate.IsAdmin)
                            {
                                userToUpdate.IsGod = !userToUpdate.IsGod;
                            }
                            else
                            {
                                userToUpdate.IsGod = true;
                                userToUpdate.IsAdmin = true;
                            }

                            OnPropertyChanged(nameof(User.IsGod));
                            OnPropertyChanged(nameof(User.IsAdmin));
                        }
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Info", $"{userName} tuvo problemas al cambiar de rol :( (SP)!", "OK");
                    }
                }
            }
        }

        private async void CreatePDF()
        {
            string urlPdf = $"{DevTunnel.UrlAdri}/reportPdfQuest";
            var response = await _httpClient.GetAsync(urlPdf);

            if (response.IsSuccessStatusCode)
            {
                byte[] pdfBytes = await response.Content.ReadAsByteArrayAsync();
                string fileName = Path.Combine(FileSystem.CacheDirectory, "report.pdf");

                File.WriteAllBytes(fileName, pdfBytes);
                await Shell.Current.DisplayAlert("Éxito", "El PDF se ha descargado correctamente", "OK");

                await OpenPdf(fileName);
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "No se pudo descargar el PDF", "OK");
            }
        }

        private async Task OpenPdf(string filePath)
        {
            try
            {
                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(filePath)
                });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
        }
    }
}
