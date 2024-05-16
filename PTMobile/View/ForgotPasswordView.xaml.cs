using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace PTMobile.View;

public partial class ForgotPasswordView : ContentPage
{

    private readonly HttpClient _httpClient = new();
    public ForgotPasswordView()
    {

        InitializeComponent();
    }

    private async void ForgotPasswordButton_Clicked(object sender, EventArgs e)
    {
        string email = emailEntry.Text;
        string url = $"https://kfwsj1gv-5250.uks1.devtunnels.ms/User/forgot-password";

        if (!string.IsNullOrEmpty(email))
        {

            try
            {
                HttpClient http = new HttpClient();
                var requestData = new { email = email };
                var json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");


                await Navigation.PushAsync(new CodeValidationView());
                var response = await http.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var isSend = await response.Content.ReadAsStringAsync();


                }
                else
                {
                    await DisplayAlert("Error", "Failed to send password reset instructions. Please try again later.", "OK");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error" + ex.Message, "An error occurred while processing your request. Please try again later.", "OK");

            }
        }
        else
        {
            await DisplayAlert("Error", "Please enter your email address.", "OK");
        }
    }
}


