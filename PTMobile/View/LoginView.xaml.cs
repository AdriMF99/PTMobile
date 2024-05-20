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
    }


    //private async void OnLoginClicked(object sender, EventArgs e)
    //{
    //    string username = usernameEntry.Text;
    //    string password = passwordEntry.Text;

    //    try
    //    {
    //        var formData = new Dictionary<string, string>
    //    {
    //        { "UserName", "AAA" },
    //        { "Password", "12345678Aa" }
    //    };

    //        var jsonContent = new StringContent(JsonSerializer.Serialize(formData), Encoding.UTF8, "application/json");

    //       var response = await _httpClient.PostAsync($"{BaseAddress}/User/login", jsonContent);
    //        //var response = await _httpClient.GetAsync($"{BaseAddress}/User/all-users");

    //        if (response.IsSuccessStatusCode)
    //        {
    //            var responseBody = await response.Content.ReadAsStringAsync();
    //            Console.WriteLine(responseBody);
    //        }
    //        else
    //        {

    //            Console.WriteLine($"Error: {response.StatusCode}");
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //        Console.WriteLine($"Error: {ex.Message}");
    //    }

    // var result = await _httpClient.PostAsync($"{BaseAddress}/User/login");
    //var result = await _httpClient.GetStringAsync(" https://bgm8xsbd-5250.uks1.devtunnels.ms/User/all-users");

    //Console.WriteLine( result );





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
                    loginResultLabel.Text = "Error en la autenticaci�n. Por favor, int�ntalo de nuevo.";
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
}