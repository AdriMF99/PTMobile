using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using PTMobile.Interfaces;
using PTMobile.Models;
using PTMobile.Views;
using System.Text;

namespace PTMobile.ViewModels
{
    public partial class ForgotPasswordViewModel : ObservableObject
    {
        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string errorText;

        [ObservableProperty]
        private bool errorTextIsEnable = false;

        [ObservableProperty]
        private bool buttonSendIsEnabled = true;

        [ObservableProperty]
        private float buttonSendOpacity = 0.5f;


        private readonly HttpClient _httpClient = new();
        private readonly IDialogService _dialogService;


        public ForgotPasswordViewModel(HttpClient httpClient, IDialogService dialogService)
        {
            _httpClient = httpClient;
            _dialogService = dialogService;
        }



        [RelayCommand]
        public async void SendCommand()
        {
            string url = $"{DevTunnel.UrlDeborah}/User/forgot-password?email={Email}";


            if (Email == null)
            {
                ButtonSendIsEnabled = true;
                ErrorText = "Email cannot be empty.";
                ButtonSendOpacity = 0.5f;
                ErrorTextIsEnable = true;
            }


            if (!string.IsNullOrEmpty(Email))
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
                        await Shell.Current.GoToAsync(nameof(EntryCodeForgotPassword));
                    }
                    else
                    {
                        await _dialogService.DisplayAlert("Error", "Failed to send password reset instructions. Please try again later.", null, "OK");
                    }
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error" + ex.Message, "An error occurred while processing your request. Please try again later.", "OK");

                }
            }
            else
            {
                await _dialogService.DisplayAlert("Error", "Please enter your email address.", null, "OK");
            }

        }

    }
}
