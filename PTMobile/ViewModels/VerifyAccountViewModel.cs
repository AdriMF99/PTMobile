using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using PTMobile.Interfaces;
using PTMobile.Models;
using PTMobile.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTMobile.ViewModels
{
    public partial class VerifyAccountViewModel: ObservableObject
    {
        [ObservableProperty]
        private string code;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private float buttonSendCodeOpacity = 0.5f;

        [ObservableProperty]
        private bool buttonSendCodeIsEnabled = true;

        [ObservableProperty]
        private string errorText;

        [ObservableProperty]
        private bool errorTextIsEnable = false;

        private readonly HttpClient _httpClient;
        //private readonly IDialogService _dialogService;


        public VerifyAccountViewModel()
        {
            _httpClient = new HttpClient();
            //_dialogService = dialogService;
            SendCodeCommand = new AsyncRelayCommand(SendCodeAsync);
        }
        public IAsyncRelayCommand SendCodeCommand { get; }


       // [RelayCommand]
        public async Task SendCodeAsync()
        {
            string url = $"{DevTunnel.UrlDeborah}/User/verify-account?code={Code}&email={Email}";

            if (Code == null)
            {
                ButtonSendCodeIsEnabled = true;
                ErrorText = "La contraseña no puede estar vacia";
                ButtonSendCodeOpacity = 0.5f;
                ErrorTextIsEnable = true;
            }

            if (Email == null)
                ButtonSendCodeIsEnabled = true;
                ErrorText = "El usuario no puede estar vacio";
                ButtonSendCodeOpacity = 0.5f;
                ErrorTextIsEnable = true;

            try
            {
                var requestData = new
                {
                    Code = Code,
                    Email = Email
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
                    await Shell.Current.DisplayAlert("Error", "Failed to verify your account. Please try again later.", null, "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Error: {ex.Message}", null, "OK");
            }
        }


        //     else
        //    {
        //                  await DisplayAlert("Error", "Please entry an email and the code send to your mail", "OK");
        //     
        //}

    }
}
