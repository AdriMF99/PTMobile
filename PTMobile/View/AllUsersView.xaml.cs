using Newtonsoft.Json;
using PTMobile.Models;

namespace PTMobile.View;

public partial class AllUsersView : ContentPage
{
    private HttpClient _httpClient = new HttpClient();

    public AllUsersView()
    {
        InitializeComponent();
        currentUser.Text = TokenManager.currentUser;
        LoadUsers();

    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Inicializa la posición fuera de la pantalla a la izquierda
        usersListLabel.TranslationX = -this.Width;

        // Anima la etiqueta para deslizarse desde la izquierda
        await usersListLabel.TranslateTo(0, 0, 1000, Easing.Linear);
    }



    private async void LoadUsers()
    {
        string apiUrl = $"{DevTunnel.UrlDeborah}/User/all-users";
        var response = await _httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<User>>(content);
            await SetAdminStatus(users);
            usersList.ItemsSource = users;
        }
        else
        {
            await DisplayAlert("Error", "No se pudo obtener la lista de usuarios.", "OK");
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


    private async void OnUserTapped(object sender, EventArgs e)
    {
        var label = sender as Label;
        var user = label?.BindingContext as User;

        if (user != null)
        {
            bool answer = await DisplayAlert("Añadir Proyectos",
                                             $"¿Desea añadir proyectos a {user.UserName}?",
                                             "Sí", "No");
            if (answer)
            {
                TokenManager.selectedUserAdmin = user.UserName;
                TokenManager.isFromAdmin = true;
                await Navigation.PushAsync(new AllProjectsToAdd());
            }
        }
    }

    private async void PdfClicked(object sender, EventArgs e)
    {
        string urlPdf = $"{DevTunnel.UrlDeborah}/reportPdfQuest";
        var response = await _httpClient.GetAsync(urlPdf);

        if (response.IsSuccessStatusCode)
        {
            byte[] pdfBytes = await response.Content.ReadAsByteArrayAsync();
            string fileName = Path.Combine(FileSystem.CacheDirectory, "report.pdf");

            File.WriteAllBytes(fileName, pdfBytes);

            await DisplayAlert("Éxito", "El PDF ha sido descargado.", "OK");

            // Abre el PDF
            await OpenPdf(fileName);
        }
        else
        {
            await DisplayAlert("Error", "No se pudo descargar el PDF.", "OK");
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
            await DisplayAlert("Error", $"No se pudo abrir el PDF: {ex.Message}", "OK");
        }
    }

    private async void OnImageTapped(object sender, EventArgs e)
    {
        string userName = (string)((TappedEventArgs)e).Parameter;
        bool answer = await DisplayAlert("Cambiar Rol", $"¿Deseas que '{userName}' cambie de Rol?", "Sí", "No");

        if (answer)
        {
            string urlSetAdmin = $"{DevTunnel.UrlDeborah}/User/set-admin?username={userName}";
            var response = await _httpClient.PutAsync(urlSetAdmin, null);

            if (response.IsSuccessStatusCode)
            {
                var userToUpdate = usersList.ItemsSource.Cast<User>().FirstOrDefault(u => u.UserName == userName);
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
                await DisplayAlert("Info", $"¡{userName} tuvo problemas al cambiar de rol :(", "OK!");
            }
        }
    }


    private async void OnGodTapped(object sender, EventArgs e)
    {
        string userName = (string)((TappedEventArgs)e).Parameter;
        bool answer = await DisplayAlert("Cambiar Rol", $"¿Deseas que '{userName}' cambie de Rol? (SP)", "Sí", "No");

        if (answer)
        {
            string urlSetAdmin = $"{DevTunnel.UrlDeborah}/User/set-god?username={userName}";
            var response = await _httpClient.PutAsync(urlSetAdmin, null);

            if (response.IsSuccessStatusCode)
            {
                var userToUpdate = usersList.ItemsSource.Cast<User>().FirstOrDefault(u => u.UserName == userName);
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
                await DisplayAlert("Info", $"¡{userName} tuvo problemas al cambiar de rol :( (SP)!", "OK!");
            }
        }
    }
}