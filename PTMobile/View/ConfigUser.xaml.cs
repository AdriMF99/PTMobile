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
        currentEmail.Text = TokenManager.currentEmail;
        
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
        string url = $"{DevTunnel.UrlDeborah}/User/change-usermane";

      
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
            var response = await _httpClient.PostAsync(url, content);


            if (response.IsSuccessStatusCode)
            {
                var isCreated = await response.Content.ReadAsStringAsync();
                JObject responseData = JObject.Parse(isCreated);

               
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