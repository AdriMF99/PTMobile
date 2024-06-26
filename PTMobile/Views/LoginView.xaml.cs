using PTMobile.Interfaces;
using PTMobile.Models;
using PTMobile.ViewModels;

namespace PTMobile.Views;

public partial class LoginView : ContentPage
{

    public LoginView()
    {
        InitializeComponent();
        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
        BindingContext = new LoginViewModel();
        currentUser.Text = TokenManager.currentUser;
    }

    protected override bool OnBackButtonPressed() => true;


    //private async void OnMoreClicked(object sender, EventArgs e)
    //{
    //    var action = await DisplayActionSheet("Options", "Cancel", null, "Option 1", "Option 2", "Option 3");
    //    // Manejar la selecci�n de las opciones aqu�
    //}


    //private void OnInputTextChanged(object sender, TextChangedEventArgs e)
    //{
    //CheckForm();
    //}

    //private void CheckForm()
    //{
    //    bool isUsernameComplete = !string.IsNullOrEmpty(usernameEntry.Text);
    //    bool isPasswordComplete = !string.IsNullOrEmpty(passwordEntry.Text);

    //    bool isFormComplete = isUsernameComplete && isPasswordComplete;

    //    loginButton.IsEnabled = isFormComplete;
    //    loginButton.Opacity = isFormComplete ? 1.0 : 0.5;
    //}


    //private async void LoginButton_Clicked(object sender, EventArgs e)
    //{
    //    string username = usernameEntry.Text;
    //    string password = passwordEntry.Text;
    //    string url = $"{DevTunnel.UrlDeborah}/User/login?username={Uri.EscapeDataString(username)}&password={Uri.EscapeDataString(password)}";

    //    if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
    //    {
    //        try
    //        {
    //            var requestData = new
    //            {
    //                UserName = username,
    //                Password = password
    //            };
    //            var json = JsonConvert.SerializeObject(requestData);
    //            var content = new StringContent(json, Encoding.UTF8, "application/json");
    //            var response = await _httpClient.PostAsync(url, content);

    //            if (response.IsSuccessStatusCode)
    //            {
    //                var isLogin = await response.Content.ReadAsStringAsync();
    //                JObject responseData = JObject.Parse(isLogin);

    //                string token = responseData?.SelectToken("Value").Value<string>();
    //                TokenManager.Token = token;
    //                TokenManager.currentUser = username;

    //                string urlAdmin = $"{DevTunnel.UrlDeborah}/User/is-admin?username={username}";
    //                var responseAdmin = await _httpClient.GetAsync(urlAdmin);
    //                if (responseAdmin.IsSuccessStatusCode)
    //                {
    //                    var isAdmin = await responseAdmin.Content.ReadAsStringAsync();
    //                    bool isAdminUser = bool.Parse(isAdmin);

    //                    if (isAdminUser)
    //                    {
    //                        TokenManager.isAdmin = true;
    //                        TokenManager.isGod = false;
    //                        bool answer = await DisplayAlert("Admin", "Eres un administrador. �Qu� quieres hacer?", "AdminMode", "VerCode");
    //                        if (answer)
    //                        {
    //                            await Navigation.PushAsync(new AllUsersView());
    //                        }
    //                        else
    //                        {
    //                            await Navigation.PushAsync(new CodeVerification());
    //                        }
    //                    }
    //                    else
    //                    {
    //                        string urlGod = $"{DevTunnel.UrlDeborah}/User/is-god?username={username}";
    //                        var responseGod = await _httpClient.GetAsync(urlGod);
    //                        if (responseGod.IsSuccessStatusCode)
    //                        {
    //                            var isGod = await responseGod.Content.ReadAsStringAsync();
    //                            bool isGodUser = bool.Parse(isGod);
    //                            if (isGodUser)
    //                            {
    //                                TokenManager.isGod = true;
    //                                TokenManager.isAdmin = true;
    //                                bool answer2 = await DisplayAlert("AdminSupremo", "Eres un Admin Supremo. �Qu� quieres hacer?", "AdminMode", "VerCode");
    //                                if (answer2)
    //                                {
    //                                    await Navigation.PushAsync(new AllUsersView());
    //                                }
    //                                else
    //                                {
    //                                    await Navigation.PushAsync(new CodeVerification());
    //                                }
    //                            }
    //                            else
    //                            {
    //                                TokenManager.isGod = false;
    //                                TokenManager.isAdmin = false;
    //                                await Navigation.PushAsync(new CodeVerification());
    //                            }
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    await DisplayAlert("Error", "No se pudo verificar si el usuario es admin.", "OK");
    //                }
    //            }

    //        }
    //        catch (Exception ex)
    //        {
    //            await DisplayAlert("Error", $"Error: {ex.Message}", "OK");
    //        }
    //    }
    //    else
    //    {
    //        loginResultLabel.IsVisible = true;
    //        loginResultLabel.Text = "Error en la autenticaci�n. Por favor, int�ntalo de nuevo.";
    //    }
    //    }



    //private async void ForgotPassword_Clicked(object sender, TappedEventArgs e)
    //{
    //    await Navigation.PushAsync(new ForgotPasswordView());
    //}


    //private void TogglePasswordVisibility(object sender, EventArgs e)
    //{
    //    if (passwordEntry != null)
    //    {
    //        passwordEntry.IsPassword = !passwordEntry.IsPassword;
    //    }
    //}

    //private async void CreateAccountButton_Clicked(object sender, EventArgs e)
    //{
    //    await Navigation.PushAsync(new CreateAccountView());
    //}
}