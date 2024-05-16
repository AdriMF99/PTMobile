using System.Net.Http;
using System.Text.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using Microsoft.Maui.ApplicationModel.Communication;
using Newtonsoft.Json;
using PTMobile.Models;

namespace PTMobile.View;

public partial class LoginView : ContentPage
{

    private readonly HttpClient _httpClient = new();
    private const string BaseAddress = "https://7z9w9942-5250-inspect.uks1.devtunnels.ms";


    public LoginView()
	{
        InitializeComponent();
	}

    private async void LoginButton_Clicked(object sender, EventArgs e)
    {
        string username = usernameEntry.Text;
        string password = passwordEntry.Text;
        string url = $"{DevTunnel.UrlAdri}/User/login?username={Uri.EscapeDataString(username)}&password={Uri.EscapeDataString(password)}";

        try
        {
            var requestData = new { UserName = username,
                Password = password
            };
            var json = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var isLogin = await response.Content.ReadAsStringAsync();
                JObject responseData = JObject.Parse(isLogin);
                //loginResultLabel.IsVisible = true;

                string token = responseData?.SelectToken("Value").Value<string>();
                TokenManager.Token = token;

                //loginResultLabel.Text = token;
                await Navigation.PushAsync(new CodeVerification());
            }
            else
            {
                loginResultLabel.IsVisible = true;
                loginResultLabel.Text = "Error en la autenticación. Por favor, inténtalo de nuevo.";
            }
        }
        catch (Exception ex)
        {
            loginResultLabel.IsVisible = true;
            loginResultLabel.Text = $"Error: {ex.Message}";
        }

    }

    private async void ForgotPassword_Clicked(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new ForgotPasswordView());
    }

    private void TogglePasswordVisibility(object sender, EventArgs e)
    {
        if (passwordEntry != null)
        {
            passwordEntry.IsPassword = !passwordEntry.IsPassword;
        }
    }
}