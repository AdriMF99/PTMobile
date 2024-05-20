using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PTMobile.Models;
using System.Text;

namespace PTMobile.View;

public partial class LoginView : ContentPage
{

    private readonly HttpClient _httpClient = new();
    public LoginView()
    {
        InitializeComponent();

        currentUser.Text = TokenManager.currentUser;

        CheckForm();
    }


    private void OnInputTextChanged(object sender, TextChangedEventArgs e)
    {
        CheckForm();
    }

    private void CheckForm()
    {
        bool isUsernameComplete = !string.IsNullOrEmpty(usernameEntry.Text);
        bool isPasswordComplete = !string.IsNullOrEmpty(passwordEntry.Text);

        bool isFormComplete = isUsernameComplete && isPasswordComplete;

        loginButton.IsEnabled = isFormComplete;
        loginButton.Opacity = isFormComplete ? 1.0 : 0.5;
    }


    private async void LoginButton_Clicked(object sender, EventArgs e)
    {
        string username = usernameEntry.Text;
        string password = passwordEntry.Text;
        string url = $"{DevTunnel.UrlFran}/User/login?username={Uri.EscapeDataString(username)}&password={Uri.EscapeDataString(password)}";

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            try { 
            var requestData = new
            {
                UserName = username,
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
                    TokenManager.currentUser = username;
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
        else
        {
            loginResultLabel.IsVisible = true;
            loginResultLabel.Text = "Error: Debes rellenar los dos campos.";
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

    private async void CreateAccountButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CreateAccountView());
    }
}