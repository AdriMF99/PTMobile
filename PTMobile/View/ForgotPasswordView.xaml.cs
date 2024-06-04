using Newtonsoft.Json;
using PTMobile.Models;
using System.Text;

namespace PTMobile.View;

public partial class ForgotPasswordView : ContentPage
{

    private readonly HttpClient _httpClient = new();
    public ForgotPasswordView()
    {
        InitializeComponent();
        CheckForm();
        currentUser.Text = TokenManager.currentUser;
    }


    private void OnInputTextChanged(object sender, TextChangedEventArgs e)
    {
        CheckForm();
    }

    private void CheckForm()
    {
        bool isEmailComplete = !string.IsNullOrEmpty(emailEntry.Text);

        forgotPasswordButton.IsEnabled = isEmailComplete;
        forgotPasswordButton.Opacity = isEmailComplete ? 1.0 : 0.5;
    }


    private async void ForgotPasswordButton_Clicked(object sender, EventArgs e)
    {
        string email = emailEntry.Text;
        string url = $"{DevTunnel.UrlFran}/User/forgot-password?email={email}";

        if (!string.IsNullOrEmpty(email))
        {

            try
            {
                HttpClient http = new HttpClient();
                var requestData = new { email = email };
                var json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await http.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    var isSend = await response.Content.ReadAsStringAsync();
                    await Navigation.PushAsync(new EntryCodeForgotPassword());
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


