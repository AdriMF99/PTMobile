using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PTMobile.Models;
using System.Text;
namespace PTMobile.View;

public partial class ConfigUser : ContentPage
{
    private readonly HttpClient _httpClient = new();
    public ConfigUser()
    {
        InitializeComponent();

        currentUser.Text = TokenManager.currentUser;

        LoadDataUser();
    }

    string id = string.Empty;

    private async void LoadDataUser()
    {
        string username = currentUser.Text;
        string url = $"{DevTunnel.UrlDeborah}/User/getuser?username={username}";

        HttpResponseMessage response = await _httpClient.GetAsync(url);
        //var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {

            var content = await response.Content
                .ReadAsStringAsync();
            User user = JsonConvert.DeserializeObject<User>(content);
            currentEmail.Text = user.Email;
            id = user.Id;
        }

        else
        {
            await DisplayAlert("Error", "No se pudo obtener los datos del usuario.", "OK");
        }
    }




    private void TogglePasswordVisibility1(object sender, EventArgs e)
    {
        if (passwordEntry != null)
        {
            passwordEntry.IsPassword = !passwordEntry.IsPassword;
        }
    }

    private void TogglePasswordVisibility2(object sender, EventArgs e)
    {
        if (repeatPasswordEntry != null)
        {
            repeatPasswordEntry.IsPassword = !repeatPasswordEntry.IsPassword;
        }
    }


    private async void UpdateButton_Clicked(object sender, EventArgs e)
    {
        string username = currentUser.Text;
        string email = currentEmail.Text;
        string password = passwordEntry.Text;
        string url = $"{DevTunnel.UrlDeborah}/User/update-user?userId={id}";

        if (!string.IsNullOrEmpty(username) || !string.IsNullOrEmpty(password) || !string.IsNullOrEmpty(password))
        {
            try
            {
                var requestData = new
                {
                    UserName = username,
                    Email = email,
                    Password = password
                };

                var json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var isUpdated = await response.Content.ReadAsStringAsync();
                    JObject responseData = JObject.Parse(isUpdated);


                    await Navigation.PushAsync(new LoginView());
                }


                else
                {
                    errortResultLabel.Text = "Error to update fields. Please, try again.";
                }
            }
            catch (Exception ex)
            {
                errortResultLabel.Text = $"Error: {ex.Message}";
            }
        }
    }
}




//    private async void UpdateButton_Clicked(object sender, EventArgs e)
//    {
//        string username = currentUser.Text;
//        string email = currentEmail.Text;
//        string password = passwordEntry.Text;
//        string url = $"{DevTunnel.UrlDeborah}/User/update-user?userId={id}";

//        if (!string.IsNullOrEmpty(username) || !string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(password))
//        {
//            try
//            {
//                var requestData = new
//                {
//                    UserName = username,
//                    Email = email,
//                    Password = password
//                };

//                var json = JsonConvert.SerializeObject(requestData);
//                var content = new StringContent(json, Encoding.UTF8, "application/json");
//                var response = await _httpClient.PutAsync(url, content);

//                if (response.IsSuccessStatusCode)
//                {
//                    var isUpdated = await response.Content.ReadAsStringAsync();
//                    JObject responseData = JObject.Parse(isUpdated);



//                    var requestDataLogin = new
//                    {
//                        UserName = username,
//                        Password = password
//                    };
//                    var jsonLogin = JsonConvert.SerializeObject(requestDataLogin);
//                    var contentLogin = new StringContent(json, Encoding.UTF8, "application/json");
//                    var responseLogin = await _httpClient.PostAsync(url, content);

//                    if (responseLogin.IsSuccessStatusCode)
//                    {
//                        string urlLogin = $"{DevTunnel.UrlDeborah}/User/login?username={Uri.EscapeDataString(username)}&password={Uri.EscapeDataString(password)}";
//                        var loginResponse = await _httpClient.GetAsync(urlLogin);

//                        if (loginResponse.IsSuccessStatusCode)
//                        {
//                            var loginContent = await loginResponse.Content.ReadAsStringAsync();
//                            UserResponse loginResponseObj = JsonConvert.DeserializeObject<UserResponse>(loginContent);

//                            //SecureStorage.SetAsync("accessToken", loginResponseObj.Token);

//                            await Navigation.PushAsync(new AllProjects());
//                        }
//                        else
//                        {
//                            errortResultLabel.Text = "Error al iniciar sesión automáticamente después de la actualización.";
//                        }
//                    }
//                    else
//                    {
//                        errortResultLabel.Text = "Error al actualizar campos. Por favor, inténtelo de nuevo.";
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                errortResultLabel.Text = $"Error: {ex.Message}";
//            }
//        }
//    }
//}

