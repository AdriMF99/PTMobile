using PTMobile.Models;

namespace PTMobile.Views;

public partial class CodeValidationView : ContentPage
{
    private readonly HttpClient _httpClient = new();

    public CodeValidationView()
    {
        InitializeComponent();
        currentUser.Text = TokenManager.currentUser;
    }


    //private async void OnMoreClicked(object sender, EventArgs e)
    //{
    //    var action = await DisplayActionSheet("Options", "Cancel", null, "Option 1", "Option 2", "Option 3");
    //    // Manejar la selección de las opciones aquí
    //}


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
                //await Navigation.PushAsync(new LoginView(_httpClient));
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