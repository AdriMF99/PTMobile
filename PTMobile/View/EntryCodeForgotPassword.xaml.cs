using Newtonsoft.Json;
using PTMobile.Models;
using System.Text;

namespace PTMobile.View;

public partial class EntryCodeForgotPassword : ContentPage
{
	public EntryCodeForgotPassword()
	{
		InitializeComponent();
        CheckForm();
    }


    private void TogglePasswordVisibility(object sender, EventArgs e)
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


    private void OnInputTextChanged(object sender, TextChangedEventArgs e)
    {
        CheckForm();
    }

    private void CheckForm()
    {
        bool isUserComplete = !string.IsNullOrEmpty(usernameEntry.Text);
        bool passwordsMatch = passwordEntry.Text == repeatPasswordEntry.Text;
        bool isCodeComplete = !string.IsNullOrEmpty(codeEntry.Text);

        bool isFormComplete = isUserComplete && passwordsMatch && isCodeComplete;

        ChangePasswordButton.IsEnabled = isFormComplete;
        ChangePasswordButton.Opacity = isFormComplete ? 1.0 : 0.5;
    }


    private async void ChangePasswordButton_Clicked(object sender, EventArgs e)
    {
        string username = usernameEntry.Text;
        string password = passwordEntry.Text;
        string code = codeEntry.Text;
        string url = $"{DevTunnel.UrlDeborah}/User/change-password";

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(code))
        {

            try
            {
                HttpClient http = new HttpClient();
                var requestData = new {
                    Username = username,
                    newPassword = password,
                    Code = code
                };

                var json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await http.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var isSend = await response.Content.ReadAsStringAsync();

                    await Navigation.PushAsync(new LoginView());
                }
                else 
                {
                    await DisplayAlert("Error", "Failed to save the new password. Please try again later.", "OK");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error" + ex.Message, "An error occurred while processing your request. Please try again later.", "OK");

            }
        }
        else
        {
            await DisplayAlert("Error", "Please enter username, email and the code send to your mail", "OK");
        }
    }
}