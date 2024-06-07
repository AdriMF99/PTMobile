using Newtonsoft.Json;
using PTMobile.Interfaces;
using PTMobile.Models;
using PTMobile.ViewModels;
using System.Text;

namespace PTMobile.Views;

public partial class EntryCodeForgotPassword : ContentPage
{
	public EntryCodeForgotPassword(HttpClient httpClient, IDialogService dialogService)
	{
		InitializeComponent();

        BindingContext = new EntryCodeForgotPasswordViewModel(httpClient, dialogService);
        //CheckForm();
    }



    //private void OnInputTextChanged(object sender, TextChangedEventArgs e)
    //{
    //    CheckForm();
    //}

    //private void CheckForm()
    //{
    //    bool isEmailComplete = !string.IsNullOrEmpty(codeEntry.Text);


    //    ChangePasswordButton.IsEnabled = isEmailComplete;
    //    ChangePasswordButton.Opacity = isEmailComplete ? 1.0 : 0.5;
    //}


    //private async void ChangePasswordButton_Clicked(object sender, EventArgs e)
    //{
    //    string username = usernameEntry.Text;
    //    string password = passwordEntry.Text;
    //    string code = codeEntry.Text;
    //    string url = $"{DevTunnel.UrlDeborah}/User/change-password";

    //    if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(code))
    //    {

    //        try
    //        {
    //            HttpClient http = new HttpClient();
    //            var requestData = new {
    //                Username = username,
    //                newPassword = password,
    //                Code = code
    //            };

    //            var json = JsonConvert.SerializeObject(requestData);
    //            var content = new StringContent(json, Encoding.UTF8, "application/json");

    //            var response = await http.PostAsync(url, content);

    //            if (response.IsSuccessStatusCode)
    //            {
    //                var isSend = await response.Content.ReadAsStringAsync();

    //                await Navigation.PushAsync(new LoginView(http));
    //            }
    //            else 
    //            {
    //                await DisplayAlert("Error", "Failed to save the new password. Please try again later.", "OK");
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            await App.Current.MainPage.DisplayAlert("Error" + ex.Message, "An error occurred while processing your request. Please try again later.", "OK");

    //        }
    //    }
    //    else
    //    {
    //        await DisplayAlert("Error", "Please enter username, email and the code send to your mail", "OK");
    //    }
    //}
}