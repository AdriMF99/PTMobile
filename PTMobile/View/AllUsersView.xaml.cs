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

    private async void LoadUsers()
    {
        string apiUrl = $"{DevTunnel.UrlAdri}/User/all-users";
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
        foreach (var user in users)
        {
            string apiUrl = $"{DevTunnel.UrlAdri}/User/is-admin?username={user.UserName}";
            string apiUrlGod = $"{DevTunnel.UrlAdri}/User/is-god?username={user.UserName}";
            var response = await _httpClient.GetAsync(apiUrl);
            var response2 = await _httpClient.GetAsync(apiUrlGod);
            var content = await response.Content.ReadAsStringAsync();

            if (JsonConvert.DeserializeObject<bool>(content))
            {
                user.IsAdmin = true;
                user.IsGod = false;
            }
            else
            {
                if (response2.IsSuccessStatusCode)
                {
                    var content2 = await response2.Content.ReadAsStringAsync();
                    if (JsonConvert.DeserializeObject<bool>(content2))
                    {
                        user.IsGod = true;
                        user.IsAdmin = true;
                    }
                    else
                    {
                        user.IsGod = false;
                        user.IsAdmin = false;
                    }
                }
            }
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
        string urlPdf = $"{DevTunnel.UrlAdri}/reportPdfQuest";
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
        bool answer = await DisplayAlert("Cambiar Rol", $"¿Deseas que '{userName}'cambie de Rol?", "Sí", "No");

        if (answer)
        {
            string urlSetAdmin = $"{DevTunnel.UrlAdri}/User/set-admin?username={userName}";
            var response = await _httpClient.PutAsync(urlSetAdmin, null);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Info", $"¡Has cambiado el rol de '{userName}'!", "OK!");
                LoadUsers();
            }
            else
            {
                await DisplayAlert("Info", $"¡{userName} tuvo problemas al cambiar de rol :( !", "OK!");
            }
        }
    }

    private async void OnGodTapped(object sender, EventArgs e)
    {
        string userName = (string)((TappedEventArgs)e).Parameter;
        bool answer = await DisplayAlert("Cambiar Rol", $"¿Deseas que '{userName}'cambie de Rol? (SP)", "Sí", "No");

        if (answer)
        {
            string urlSetAdmin = $"{DevTunnel.UrlAdri}/User/set-god?username={userName}";
            var response = await _httpClient.PutAsync(urlSetAdmin, null);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Info", $"¡Has cambiado el rol de '{userName}' (SP)!", "OK!");
                LoadUsers();
            }
            else
            {
                await DisplayAlert("Info", $"¡{userName} tuvo problemas al cambiar de rol :( (SP)!", "OK!");
            }
        }
    }
}