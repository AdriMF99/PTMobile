using PTMobile.Interfaces;
using PTMobile.Models;
using PTMobile.ViewModels;

namespace PTMobile.Views;

public partial class ForgotPasswordView : ContentPage
{

    //private readonly HttpClient _httpClient = new();
    public ForgotPasswordView()
    {
        InitializeComponent();
        currentUser.Text = TokenManager.currentUser;

        BindingContext = new ForgotPasswordViewModel();
    }

    //private async void ForgotPasswordButton_Clicked(object sender, EventArgs e)
    //{
    //    string email = emailEntry.Text;
    //    string url = $"{DevTunnel.UrlDeborah}/User/forgot-password?email={email}";

    //    if (!string.IsNullOrEmpty(email))
    //    {

    //        try
    //        {
    //            HttpClient http = new HttpClient();
    //            var requestData = new { email = email };
    //            var json = JsonConvert.SerializeObject(requestData);
    //            var content = new StringContent(json, Encoding.UTF8, "application/json");

    //            var response = await http.PostAsync(url, null);

    //            if (response.IsSuccessStatusCode)
    //            {
    //                var isSend = await response.Content.ReadAsStringAsync();
    //                await Navigation.PushAsync(new EntryCodeForgotPassword());
    //            }
    //            else
    //            {
    //                await DisplayAlert("Error", "Failed to send password reset instructions. Please try again later.", "OK");
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            await App.Current.MainPage.DisplayAlert("Error" + ex.Message, "An error occurred while processing your request. Please try again later.", "OK");

    //        }
    //    }
    //    else
    //    {
    //        await DisplayAlert("Error", "Please enter your email address.", "OK");
    //    }
    // }
}


