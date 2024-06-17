using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PTMobile.Models;
using PTMobile.Views;
using System.Text;

namespace PTMobile.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string errorText;

        [ObservableProperty]
        private bool errorTextIsEnable = false;

        [ObservableProperty]
        private bool passwordIsEnabled = false;

        [ObservableProperty]
        private bool buttonLoginIsEnabled = false;

        [ObservableProperty]
        private float buttonLoginOpacity = 0.5f;



        [ObservableProperty]
        private string errorTextLogin;

        [ObservableProperty]
        private bool errorTextLoginIsEnable = false;

        public LoginViewModel()
        {
            _httpClient = new HttpClient();
            LoginFormCommand = new AsyncRelayCommand(LoginFormAsync);
            ForgotPasswordCommand = new AsyncRelayCommand(ForgotPasswordAsync);
            TogglePasswordVisibilityCommand = new RelayCommand(TogglePasswordVisibility);
            CreateAccountCommand = new AsyncRelayCommand(CreateAccountAsync);

            ValidateInputs();
        }

        public IAsyncRelayCommand LoginFormCommand { get; }
        public IAsyncRelayCommand ForgotPasswordCommand { get; }
        public IRelayCommand TogglePasswordVisibilityCommand { get; }
        public IAsyncRelayCommand CreateAccountCommand { get; }



        private void ValidateInputs()
        {
            ErrorText = string.Empty;
            ErrorTextIsEnable = false;
            ButtonLoginIsEnabled = true;
            ButtonLoginOpacity = 1.0f;

            if (string.IsNullOrEmpty(Username))
            {
                ErrorText = "Username cannot be empty.";
                ErrorTextIsEnable = true;
                ButtonLoginIsEnabled = false;
                ButtonLoginOpacity = 0.5f;
            }

            if (string.IsNullOrEmpty(Password))
            {
                ErrorText = "Password cannot be empty.";
                ErrorTextIsEnable = true;
                ButtonLoginIsEnabled = false;
                ButtonLoginOpacity = 0.5f;
            }
        }

        partial void OnUsernameChanged(string value) => ValidateInputs();
        partial void OnPasswordChanged(string value) => ValidateInputs();



        private async Task LoginFormAsync()
        {
            //if (string.IsNullOrEmpty(Username))
            //{
            //    ErrorText = "User cannot be empty.";
            //    ErrorTextIsEnable = true;
            //    ButtonLoginOpacity = 0.5f;
            //    return;
            //}

            //if (string.IsNullOrEmpty(Password))
            //{
            //    ErrorText = "La contraseña no puede estar vacía";
            //    ErrorTextIsEnable = true;
            //    ButtonLoginOpacity = 0.5f;
            //    return;
            //}


            ValidateInputs();

            if (ErrorTextIsEnable)
            {
                return;
            }

            string url = $"{DevTunnel.UrlFran}/User/login?username={Username}&password={Password}";

            try
            {
                var requestData = new
                {
                    UserName = Username,
                    Password = Password
                };
                var json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var isLogin = await response.Content.ReadAsStringAsync();
                    JObject responseData = JObject.Parse(isLogin);

                    string token = responseData?.SelectToken("Value").Value<string>();
                    TokenManager.Token = token;
                    TokenManager.currentUser = Username;

                    string urlDeleted = $"{DevTunnel.UrlFran}/User/is-deleted?username={Username}";
                    var responseDelete = await _httpClient.GetAsync(urlDeleted);

                    if (responseDelete.IsSuccessStatusCode)
                    {
                        var isDeleted = await responseDelete.Content.ReadAsStringAsync();
                        bool isDeletedUser = bool.Parse(isDeleted);

                        if (isDeletedUser)
                        {
                            await Shell.Current.DisplayAlert("Deleted", "This user is deleted!", "OK");
                        }
                        else
                        {
                            string urlAdmin = $"{DevTunnel.UrlFran}/User/is-admin?username={Username}";
                            var responseAdmin = await _httpClient.GetAsync(urlAdmin);
                            if (responseAdmin.IsSuccessStatusCode)
                            {
                                var isAdmin = await responseAdmin.Content.ReadAsStringAsync();
                                bool isAdminUser = bool.Parse(isAdmin);

                                if (isAdminUser)
                                {
                                    TokenManager.isAdmin = true;
                                    TokenManager.isGod = false;
                                    bool answer = await Shell.Current.DisplayAlert("Admin", "Eres un administrador. ¿Qué quieres hacer?", "AdminMode", "VerCode");
                                    if (answer)
                                    {
                                        (Shell.Current as AppShell)?.ShowAdminFlyoutItems();
                                        await Shell.Current.GoToAsync("//AllUsersView");
                                    }
                                    else
                                    {
                                        (Shell.Current as AppShell)?.ShowAdminFlyoutItems();
                                        await Shell.Current.GoToAsync("//CodeVerification");
                                    }
                                }
                                else
                                {
                                    string urlGod = $"{DevTunnel.UrlFran}/User/is-god?username={Username}";
                                    var responseGod = await _httpClient.GetAsync(urlGod);
                                    if (responseGod.IsSuccessStatusCode)
                                    {
                                        var isGod = await responseGod.Content.ReadAsStringAsync();
                                        bool isGodUser = bool.Parse(isGod);
                                        if (isGodUser)
                                        {
                                            TokenManager.isGod = true;
                                            TokenManager.isAdmin = true;
                                            bool answer2 = await Shell.Current.DisplayAlert("AdminSupremo", "Eres un Admin Supremo. ¿Qué quieres hacer?", "AdminMode", "VerCode");
                                            if (answer2)
                                            {
                                                (Shell.Current as AppShell)?.ShowAdminFlyoutItems();
                                                await Shell.Current.GoToAsync("//AllUsersView");
                                            }
                                            else
                                            {
                                                (Shell.Current as AppShell)?.ShowAdminFlyoutItems();
                                                await Shell.Current.GoToAsync("//CodeVerification");
                                            }
                                        }
                                        else
                                        {
                                            TokenManager.isGod = false;
                                            TokenManager.isAdmin = false;
                                            await Shell.Current.GoToAsync("//CodeVerification");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                await Shell.Current.DisplayAlert("Error", "No se pudo verificar si el usuario es admin.", "OK");
                            }
                        }
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Deleted", "ERROR EN GET DELETED", "OK");
                    }
                }
                else
                {
                    ErrorTextLoginIsEnable = true;
                    ErrorTextLogin = "Authentication failed. Please try again";
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
        }

        private async Task ForgotPasswordAsync()
        {
            await Shell.Current.GoToAsync(nameof(ForgotPasswordView));
        }

        private void TogglePasswordVisibility()
        {
            PasswordIsEnabled = !PasswordIsEnabled;
        }

        private async Task CreateAccountAsync()
        {
            await Shell.Current.GoToAsync(nameof(CreateAccountView));
        }
    }
}
