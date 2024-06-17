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
        private bool buttonSendCodeIsEnabled = false;

        [ObservableProperty]
        private string errorText;

        [ObservableProperty]
        private bool errorTextIsEnable = false;

        private readonly HttpClient _httpClient;
        //private readonly IDialogService _dialogService;

        public EntryCodeForgotPasswordViewModel()
        {
            _httpClient = new HttpClient();
            SendCodeCommand = new AsyncRelayCommand(SendCodeAsync, CanSendCode);
            PropertyChanged += OnPropertyChanged;
        }

        public IAsyncRelayCommand SendCodeCommand { get; }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Username) || e.PropertyName == nameof(Password) || e.PropertyName == nameof(Code))
            {
                SendCodeCommand.NotifyCanExecuteChanged();
                UpdateButtonState();
            }
        }

        private bool CanSendCode()
        {
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(Code);
        }

        private void UpdateButtonState()
        {
            ButtonSendCodeIsEnabled = CanSendCode();
            ButtonSendCodeOpacity = ButtonSendCodeIsEnabled ? 1.0f : 0.5f;
        }

        public async Task SendCodeAsync()
        {
            if (!CanSendCode())
            {
                return;
            }

            string url = $"{DevTunnel.UrlAdri}/User/change-password?username={Username}&newPassword={Password}&code={Code}";

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

                var response = await _httpClient.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    var isSend = await response.Content.ReadAsStringAsync();
                    await Shell.Current.GoToAsync("//LoginView");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Failed to save the new password. Please try again later.", null, "OK");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
