using PTMobile.Interfaces;
using PTMobile.ViewModels;

namespace PTMobile.Views;

public partial class VerifyAccount : ContentPage
{
    public VerifyAccount(HttpClient httpClient, IDialogService dialogService)
    {
        InitializeComponent();
        BindingContext = new VerifyAccountViewModel(httpClient, dialogService);
        //CheckForm();
    }


    //private void OnInputTextChanged(object sender, TextChangedEventArgs e)
    //{
    //    CheckForm();
    //}

    //private void CheckForm()
    //{
    //    bool isCodeComplete = !string.IsNullOrEmpty(codeEntry.Text);
    //    bool isEmailComplete = !string.IsNullOrEmpty(emailEntry.Text);

    //    bool isFormComplete = isCodeComplete && isEmailComplete;

    //    verifyAccountButton.IsEnabled = isFormComplete;
    //    verifyAccountButton.Opacity = isFormComplete ? 1.0 : 0.5;
    //}



    //private async void VerifyAccountButton_Clicked(object sender, EventArgs e)
    //{
    //    string code = codeEntry.Text;
    //    string email = emailEntry.Text;
    //    string url = $"{DevTunnel.UrlDeborah}/User/verify-account?code={code}&email={email}";

    //    if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(email))
    //    { 

    //        try
    //        {
    //            HttpClient http = new HttpClient();
    //            var requestData = new
    //            {
    //                Code = code,
    //                Email = email
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
    //                await DisplayAlert("Error", "Failed to verify your account. Please try again later.", "OK");
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            await App.Current.MainPage.DisplayAlert("Error" + ex.Message, "An error occurred while processing your request. Please try again later.", "OK");

    //        }
    //    }
    //    else
    //    {
    //        await DisplayAlert("Error", "Please entry an email and the code send to your mail", "OK");
    //    }
//}
}