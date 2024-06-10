using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using PTMobile.Interfaces;
using PTMobile.Models;
using PTMobile.Views;
using System.Collections.ObjectModel;

namespace PTMobile.ViewModels
{
    public partial class AllUsersViewModel : ObservableObject
    {
        [ObservableProperty]
        private string userListLabel;

        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string id;

        [ObservableProperty]
        private ObservableCollection<User> users;


        private readonly HttpClient _httpClient = new();
        private readonly IDialogService _dialogService;


        public AllUsersViewModel(HttpClient httpClient, IDialogService dialogService)
        {
            _httpClient = httpClient;
            _dialogService = dialogService;

            Users = new ObservableCollection<User>();
            AnimateLabelCommand = new AsyncRelayCommand<Label>(AnimateLabelAsync);
            LoadUsersCommand = new AsyncRelayCommand(LoadUsersAsync);
            UserTappedCommand = new AsyncRelayCommand<User>(OnUserTappedAsync);
            ImageTappedCommand = new AsyncRelayCommand<string>(OnImageTappedAsync);
            GodTappedCommand = new AsyncRelayCommand<string>(OnGodTappedAsync);
        }



        public IAsyncRelayCommand<Label> AnimateLabelCommand { get; }
        public IAsyncRelayCommand LoadUsersCommand { get; }
        public IAsyncRelayCommand<User> UserTappedCommand { get; }
        public IAsyncRelayCommand<string> ImageTappedCommand { get; }
        public IAsyncRelayCommand<string> GodTappedCommand { get; }



        private async Task AnimateLabelAsync(Label label)
        {
            if (label == null)
                return;

            // Inicializa la posición fuera de la pantalla a la izquierda
            label.TranslationX = -label.Width;

            // Anima la etiqueta para deslizarse desde la izquierda
            await label.TranslateTo(0, 0, 1000, Easing.Linear);
        }


        private async Task LoadUsersAsync()
        {
            string apiUrl = $"{DevTunnel.UrlDeborah}/User/all-users";
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
                await _dialogService.DisplayAlert("Error", "No se pudo obtener la lista de usuarios.", null, "OK");
            }
        }

        private async Task SetAdminStatus(List<User> users)
        {
            List<Task> tasks = new List<Task>();

            foreach (var user in users)
            {
                string isAdminUrl = $"{DevTunnel.UrlDeborah}/User/is-admin?username={user.UserName}";
                string isGodUrl = $"{DevTunnel.UrlDeborah}/User/is-god?username={user.UserName}";

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
                bool answer = await _dialogService.DisplayAlert("Añadir Proyectos",
                                                                $"¿Desea añadir proyectos a {user.UserName}?",
                                                                "Sí", "No");
                if (answer)
                {
                    TokenManager.selectedUserAdmin = user.UserName;
                    TokenManager.isFromAdmin = true;
                    await Shell.Current.GoToAsync(nameof(AllProjectsToAdd));
                }
            }
        }

        private async Task OnImageTappedAsync(string UserName)
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                bool answer = await _dialogService.DisplayAlert("Cambiar Rol", $"¿Deseas que '{UserName}' cambie de Rol?", "Sí", "No");

                if (answer)
                {
                    string urlSetAdmin = $"{DevTunnel.UrlDeborah}/User/set-admin?username={UserName}";
                    var response = await _httpClient.PutAsync(urlSetAdmin, null);

                    if (response.IsSuccessStatusCode)
                    {
                        var userToUpdate = Users.FirstOrDefault(u => u.UserName == UserName);
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
                        await _dialogService.DisplayAlert("Info", $"¡{UserName} tuvo problemas al cambiar de rol :(", null, "OK!");
                    }
                }
            }
        }


        private async Task OnGodTappedAsync(string userName)
        {
            if (!string.IsNullOrWhiteSpace(userName))
            {
                bool answer = await _dialogService.DisplayAlert("Cambiar Rol", $"¿Deseas que '{UserName}' cambie de Rol? (SP)", "Sí", "No");

                if (answer)
                {
                    string urlSetAdmin = $"{DevTunnel.UrlDeborah}/User/set-god?username={UserName}";
                    var response = await _httpClient.PutAsync(urlSetAdmin, null);

                    if (response.IsSuccessStatusCode)
                    {
                        var userToUpdate = Users.FirstOrDefault(u => u.UserName == UserName);
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
                        await _dialogService.DisplayAlert("Info", $"¡{UserName} tuvo problemas al cambiar de rol :( (SP)!", null,  "OK!");
                    }
                }
            }
        }


        [RelayCommand]
        public async void CreatePDFCommand()
        {
            string urlPdf = $"{DevTunnel.UrlDeborah}/reportPdfQuest";
            var response = await _httpClient.GetAsync(urlPdf);

            if (response.IsSuccessStatusCode)
            {
                byte[] pdfBytes = await response.Content.ReadAsByteArrayAsync();
                string fileName = Path.Combine(FileSystem.CacheDirectory, "report.pdf");

                File.WriteAllBytes(fileName, pdfBytes);
                await _dialogService.DisplayAlert("Error", "The PDF has been downloaded", null, "OK");

                // Abre el PDF
                await OpenPdf(fileName);
            }
            else
            {
                await _dialogService.DisplayAlert("Error", "The PDF has been downloaded", null, "OK");
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
                await _dialogService.DisplayAlert("Error", $"Error: {ex.Message}", null, "OK");
            }
        }


    }
}
