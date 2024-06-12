using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PTMobile.Interfaces;
using PTMobile.Models;
using PTMobile.Views;
using System.Text;

namespace PTMobile.ViewModels
{ 
    public partial class SettingsUpdateFieldsViewModel: ObservableObject
    {
        [ObservableProperty]
        private string currentUser;

        [ObservableProperty]
        private string currentEmail;

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
        private float buttonUpdateFieldsOpacity = 0.5F;

        [ObservableProperty]
        private bool buttonUpdateFieldsIsEnabled = true;

        [ObservableProperty]
        private string errorTextUpdateFields;

        [ObservableProperty]
        private bool errorTextUpdateFieldsIsEnable = false;



        private readonly HttpClient _httpClient = new();
        //private readonly IDialogService _dialogService;

        public SettingsUpdateFieldsViewModel()
        {
            _httpClient = new HttpClient();
            //_dialogService = dialogService;
        }



        string id = string.Empty;

        private async void LoadDataUser()
        {
            string url = $"{DevTunnel.UrlFran}/User/getuser?username={CurrentUser}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            //var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {

                var content = await response.Content
                    .ReadAsStringAsync();
                User user = JsonConvert.DeserializeObject<User>(content);
                CurrentEmail = user.Email;
                id = user.Id;
            }

            else
            {
                await Shell.Current.DisplayAlert("Error", "Unavailable user data.", null, "OK");
            }
        }



        [RelayCommand]
        public async void UpdateFieldsCommand()
        {
            string url = $"{DevTunnel.UrlFran}/User/getuser?username={CurrentUser}";

            bool passwordsMatch = Password == RepeatPassword;
            if (!passwordsMatch)
                ButtonUpdateFieldsIsEnabled = true;
                ErrorText = "Passwords do not match";
                ButtonUpdateFieldsOpacity = 0.5f;
                ErrorTextIsEnable = true;


            try
            {
                var requestData = new
                {
                    UserName = CurrentUser,
                    Email = CurrentEmail,
                    Password = Password

                };
                var json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var isUpdated = await response.Content.ReadAsStringAsync();
                    JObject responseData = JObject.Parse(isUpdated);

                    await Shell.Current.GoToAsync(nameof(LoginView));
                }


                else
                {
                    ErrorTextUpdateFieldsIsEnable = true;
                    ErrorTextUpdateFields = "Error creating account. Please try again.";
                }
            }
            catch (Exception ex)
            {
                ErrorTextUpdateFields = $"Error: {ex.Message}";
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
