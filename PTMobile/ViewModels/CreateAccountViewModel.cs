using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PTMobile.Models;
using PTMobile.Views;
using System.Text;

namespace PTMobile.ViewModels
{
    public partial class CreateAccountViewModel : ObservableObject
    {
        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string repeatPassword;

        [ObservableProperty]
        private string errorText;

        [ObservableProperty]
        private bool errorTextIsEnable = false;

        [ObservableProperty]
        private bool passwordIsEnabled = true;

        [ObservableProperty]
        private bool repeatPasswordIsEnabled = true;

        [ObservableProperty]
        private float buttonCreateAccountOpacity = 1;

        [ObservableProperty]
        private bool buttonCreateAccountIsEnabled = true;

        [ObservableProperty]
        private string errorTextCreateAccount;

        [ObservableProperty]
        private bool errorTextCreateAccountIsEnable = false;



        private readonly HttpClient _httpClient = new();

        public CreateAccountViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }



        private bool accountCreationRequested = false;
        [RelayCommand]
        public async void CreateAccountCommand()
        {
            string url = $"{DevTunnel.UrlDeborah}/User/create-user";

            if (Password == null)
            {
                ButtonCreateAccountIsEnabled = true;
                ErrorText = "Password cannot be empty.";
                ButtonCreateAccountOpacity = 0.5f;
                ErrorTextIsEnable = true;
            }

            if (Email == null)
            {
                ButtonCreateAccountIsEnabled = true;
                ErrorText = "Email cannot be empty.";
                ButtonCreateAccountOpacity = 0.5f;
                ErrorTextIsEnable = true;
            }

            if (Username == null)
                ButtonCreateAccountIsEnabled = true;
            ErrorText = "User cannot be empty";
            ButtonCreateAccountOpacity = 0.5f;
            ErrorTextIsEnable = true;

            bool passwordsMatch = Password == RepeatPassword;
            if (!passwordsMatch)
                ButtonCreateAccountIsEnabled = true;
            ErrorText = "Passwords do not match";
            ButtonCreateAccountOpacity = 0.5f;
            ErrorTextIsEnable = true;

            if (accountCreationRequested)
            {
                ErrorTextCreateAccountIsEnable = true;
                ErrorTextCreateAccount = "A request has already been sent.";
                return;
            }
            try
            {
                var requestData = new
                {
                    UserName = Username,
                    Email = Email,
                    Password = Password

                };
                var json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var isCreated = await response.Content.ReadAsStringAsync();
                    JObject responseData = JObject.Parse(isCreated);

                    accountCreationRequested = true;

                    await Shell.Current.GoToAsync(nameof(VerifyAccount));
                }


                else
                {
                    ErrorTextCreateAccountIsEnable = true;
                    ErrorTextCreateAccount = "Error creating account. Please try again.";
                }
            }
            catch (Exception ex)
            {
                ErrorTextCreateAccount = $"Error: {ex.Message}";
            }
        }


        [RelayCommand]
        public async void TogglePasswordVisibilityCommand()
        {
            if (Password != null)
            {
                PasswordIsEnabled = !PasswordIsEnabled;
            }
        }

        [RelayCommand]
        private async void ToggleRepeatPasswordVisibilityCommand()
        {
            if (RepeatPassword != null)
            {
                RepeatPasswordIsEnabled = !RepeatPasswordIsEnabled;
            }
        }


    }
}
