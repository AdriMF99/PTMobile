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
        string url = $"{DevTunnel.UrlAdri}/User/login?username={Uri.EscapeDataString(username)}&password={Uri.EscapeDataString(password)}";

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            try
            {
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

                    string token = responseData?.SelectToken("Value").Value<string>();
                    TokenManager.Token = token;
                    TokenManager.currentUser = username;

                    string urlAdmin = $"{DevTunnel.UrlAdri}/User/is-admin?username={username}";
                    var responseAdmin = await _httpClient.GetAsync(urlAdmin);
                    if (responseAdmin.IsSuccessStatusCode)
                    {
                        var isAdmin = await responseAdmin.Content.ReadAsStringAsync();
                        bool isAdminUser = bool.Parse(isAdmin);

                        if (isAdminUser)
                        {
                            TokenManager.isAdmin = true;
                            TokenManager.isGod = false;
                            bool answer = await DisplayAlert("Admin", "Eres un administrador. ¿Qué quieres hacer?", "AdminMode", "VerCode");
                            if (answer)
                            {
                                await Navigation.PushAsync(new AllUsersView());
                            }
                            else
                            {
                                await Navigation.PushAsync(new CodeVerification());
                            }
                        } 
                        else
                        {
                            string urlGod = $"{DevTunnel.UrlAdri}/User/is-god?username={username}";
                            var responseGod = await _httpClient.GetAsync(urlGod);
                            if (responseGod.IsSuccessStatusCode)
                            {
                                var isGod = await responseGod.Content.ReadAsStringAsync();
                                bool isGodUser = bool.Parse(isGod);
                                if (isGodUser)
                                {
                                    TokenManager.isGod = true;
                                    TokenManager.isAdmin = true;
                                    bool answer2 = await DisplayAlert("AdminSupremo", "Eres un Admin Supremo. ¿Qué quieres hacer?", "AdminMode", "VerCode");
                                    if (answer2)
                                    {
                                        await Navigation.PushAsync(new AllUsersView());
                                    }
                                    else
                                    {
                                        await Navigation.PushAsync(new CodeVerification());
                                    }
                                }
                                else
                                {
                                    TokenManager.isGod = false;
                                    TokenManager.isAdmin = false;
                                    await Navigation.PushAsync(new CodeVerification());
                                }
                            }
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo verificar si el usuario es admin.", "OK");
                    }
                }
                else
                {
                    loginResultLabel.IsVisible = true;
                    loginResultLabel.Text = "Error en la autenticación. Por favor, inténtalo de nuevo.";
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
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