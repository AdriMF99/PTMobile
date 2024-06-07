using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using PTMobile.Interfaces;
using PTMobile.Models;
using PTMobile.Views;
using System.Text;

namespace PTMobile.ViewModels
{
    public partial class EntryCodeForgotPasswordViewModel : ObservableObject
    {
        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string repeatPassword;

        [ObservableProperty]
        private string code;

        [ObservableProperty]
        private float buttonSendCodeOpacity = 0.5f;

        [ObservableProperty]
        private bool buttonSendCodeIsEnabled = true;

        [ObservableProperty]
        private string errorText;

        [ObservableProperty]
        private bool errorTextIsEnable = false;

        private readonly HttpClient _httpClient = new();
        private readonly IDialogService _dialogService;


        public EntryCodeForgotPasswordViewModel(HttpClient httpClient, IDialogService dialogService)
        {
            _httpClient = httpClient;
            _dialogService = dialogService;
        }




        [RelayCommand]
        public async void SendCodeCommand()
        {
            string url = $"{DevTunnel.UrlDeborah}/User/change-password";

            if (Username == null)
                ButtonSendCodeIsEnabled = true;
            ErrorText = "User cannot be empty.";
            ButtonSendCodeOpacity = 0.5f;
            ErrorTextIsEnable = true;

            if (Password == null)
                ButtonSendCodeIsEnabled = true;
            ErrorText = "Password cannot be empty.";
            ButtonSendCodeOpacity = 0.5f;
            ErrorTextIsEnable = true;

            if (Code == null)
                ButtonSendCodeIsEnabled = true;
            ErrorText = "Code cannot be empty.";
            ButtonSendCodeOpacity = 0.5f;
            ErrorTextIsEnable = true;


            try
            {
                var requestData = new
                {
                    username = Username,
                    newPassword = Password,
                    code = Code
                };

                var json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var isSend = await response.Content.ReadAsStringAsync();

                    await Shell.Current.GoToAsync(nameof(LoginView));
                }
                else
                {
                    await _dialogService.DisplayAlert("Error", "Failed to save the new password. Please try again later.", null, "OK");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error" + ex.Message, "An error occurred while processing your request. Please try again later.", "OK");

            }

        }
        // else
        //{
        //                await _dialogService.DisplayAlert("Error", "Please enter username, email and the code send to your mail.", null, "OK");

        // }


    }

}
