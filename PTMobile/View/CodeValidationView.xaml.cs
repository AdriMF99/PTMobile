using PTMobile.Models;

namespace PTMobile.View;

public partial class CodeValidationView : ContentPage
{
    private readonly HttpClient _httpClient = new();

    public CodeValidationView()
    {
        InitializeComponent();
        currentUser.Text = TokenManager.currentUser;
    }

    private async void OnChangeClicked(object sender, EventArgs e)
    {
        string code = codigoEntry.Text;
        string username = usernameEntry.Text;
        string newpass = newPasswordEntry.Text;
        string url = $"{DevTunnel.UrlFran}/User/change-password?username={username}&newPassword={newpass}&code={code}";

        if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(newpass))
        {
            var response = await _httpClient.PostAsync(url, null);
            if (response.IsSuccessStatusCode)
            {
                await Navigation.PushAsync(new LoginView());
            }
        }
    }

    private void ojoPulsado(object sender, EventArgs e)
    {
        if (newPasswordEntry != null)
        {
            newPasswordEntry.IsPassword = !newPasswordEntry.IsPassword;
        }
    }
}