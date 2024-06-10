using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PTMobile.Interfaces;
using PTMobile.Models;
using PTMobile.ViewModels;
using System.Text;
namespace PTMobile.View;

public partial class SettingsUpdateFields : ContentPage
{
    //private readonly HttpClient _httpClient = new();
    public SettingsUpdateFields(HttpClient httpClient, IDialogService dialogService)
    {
        InitializeComponent();

        currentUser.Text = TokenManager.currentUser;

        BindingContext = new SettingsUpdateFieldsViewModel(httpClient, dialogService);

        // LoadDataUser();
    }

    //string id = string.Empty;

    //private async void LoadDataUser()
    //{
    //    string username = currentUser.Text;
    //    string url = $"{DevTunnel.UrlDeborah}/User/getuser?username={username}";

    //    HttpResponseMessage response = await _httpClient.GetAsync(url);
    //    //var response = await _httpClient.GetAsync(url);

    //    if (response.IsSuccessStatusCode)
    //    {

    //        var content = await response.Content
    //            .ReadAsStringAsync();
    //        User user = JsonConvert.DeserializeObject<User>(content);
    //        currentEmail.Text = user.Email;
    //        id = user.Id;
    //    }

    //    else
    //    {
    //        await DisplayAlert("Error", "No se pudo obtener los datos del usuario.", "OK");
    //    }
    //}




    //private void TogglePasswordVisibility1(object sender, EventArgs e)
    //{
    //    if (passwordEntry != null)
    //    {
    //        passwordEntry.IsPassword = !passwordEntry.IsPassword;
    //    }
    //}

    //private void TogglePasswordVisibility2(object sender, EventArgs e)
    //{
    //    if (repeatPasswordEntry != null)
    //    {
    //        repeatPasswordEntry.IsPassword = !repeatPasswordEntry.IsPassword;
    //    }
    //}


    //private async void UpdateButton_Clicked(object sender, EventArgs e)
    //{
    //    string username = currentUser.Text;
    //    string email = currentEmail.Text;
    //    string password = passwordEntry.Text;
    //    string url = $"{DevTunnel.UrlDeborah}/User/update-user?userId={id}";

    //    if (!string.IsNullOrEmpty(username) || !string.IsNullOrEmpty(password) || !string.IsNullOrEmpty(password))
    //    {
    //        try
    //        {
    //            var requestData = new
    //            {
    //                UserName = username,
    //                Email = email,
    //                Password = password
    //            };

    //            var json = JsonConvert.SerializeObject(requestData);
    //            var content = new StringContent(json, Encoding.UTF8, "application/json");
    //            var response = await _httpClient.PutAsync(url, content);

    //            if (response.IsSuccessStatusCode)
    //            {
    //                var isUpdated = await response.Content.ReadAsStringAsync();
    //                JObject responseData = JObject.Parse(isUpdated);


    //                await Navigation.PushAsync(new LoginView());
    //            }


    //            else
    //            {
    //                errortResultLabel.Text = "Error to update fields. Please, try again.";
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            errortResultLabel.Text = $"Error: {ex.Message}";
    //        }
    //   }
    //}
}


