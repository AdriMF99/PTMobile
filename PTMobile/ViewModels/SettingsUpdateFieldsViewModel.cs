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
    public partial class SettingsUpdateFieldsViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient;

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

        public SettingsUpdateFieldsViewModel()
        {
            _httpClient = new HttpClient();
            UpdateFieldsCommand = new AsyncRelayCommand(UpdateFields);
            TogglePasswordVisibilityCommand = new RelayCommand(TogglePasswordVisibility);
            ToggleRepeatPasswordVisibilityCommand = new RelayCommand(ToggleRepeatPasswordVisibility);
            LoadDataUser();
        }

        public IAsyncRelayCommand UpdateFieldsCommand { get; }
        public IRelayCommand TogglePasswordVisibilityCommand { get; }
        public IRelayCommand ToggleRepeatPasswordVisibilityCommand { get; }

        private string id = string.Empty;

        private async void LoadDataUser()
        {
            string url = $"{DevTunnel.UrlFran}/User/getuser?username={TokenManager.currentUser}";
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                User user = JsonConvert.DeserializeObject<User>(content);
                CurrentUser = user.UserName;
                CurrentEmail = user.Email;
                id = user.Id;
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Unavailable user data.", "OK");
            }
        }

        private async Task UpdateFields()
        {
            if (Password != RepeatPassword)
            {
                ErrorText = "Passwords do not match";
                ErrorTextIsEnable = true;
                ButtonUpdateFieldsOpacity = 0.5f;
                ButtonUpdateFieldsIsEnabled = false;
                return;
            }

            string url = $"{DevTunnel.UrlFran}/User/update-user?userId={id}";
            var requestData = new
            {
                UserName = CurrentUser,
                Email = CurrentEmail,
                Password = Password
            };

            try
            {
                var json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    await Shell.Current.GoToAsync(nameof(LoginView));
                }
                else
                {
                    ErrorTextUpdateFieldsIsEnable = true;
                    ErrorTextUpdateFields = "Error updating account. Please try again.";
                }
            }
            catch (Exception ex)
            {
                ErrorTextUpdateFieldsIsEnable = true;
                ErrorTextUpdateFields = $"Error: {ex.Message}";
            }
        }

        private void TogglePasswordVisibility()
        {
            PasswordIsEnabled = !PasswordIsEnabled;
        }

        private void ToggleRepeatPasswordVisibility()
        {
            RepeatPasswordIsEnabled = !RepeatPasswordIsEnabled;
        }
    }
}
