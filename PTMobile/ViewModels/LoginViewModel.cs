using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PTMobile.Interfaces;
using PTMobile.Models;
using PTMobile.Views;
using System.Runtime.InteropServices;
using System.Text;

namespace PTMobile.ViewModels
{
    public partial class LoginViewModel : ObservableObject 
    {
        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string errorText;

        [ObservableProperty]
        private bool errorTextIsEnable = false;

        [ObservableProperty]
        private bool buttonLoginIsEnabled = true;

        [ObservableProperty]
        private float buttonLoginOpacity = 0.5f;

        [ObservableProperty]
        private bool passwordIsEnabled = true;

        [ObservableProperty]
        private string errorTextLogin;

        [ObservableProperty]
        private bool errorTextLoginIsEnable = false;



        private readonly HttpClient _httpClient = new();
        private readonly IDialogService _dialogService;


        public LoginViewModel(HttpClient httpClient, IDialogService dialogService)
        {
            _httpClient = httpClient;
            _dialogService = dialogService;
        }


        [RelayCommand]
        public async void LoginFormCommand()
        {
            string url = $"{DevTunnel.UrlDeborah}/User/login?username={Uri.EscapeDataString(Username)}&password={Uri.EscapeDataString(Password)}";


            if (Password == null)
            {
                ButtonLoginIsEnabled = true;
                ErrorText = "La contraseña no puede estar vacia";
                ButtonLoginOpacity = 0.5f;
                ErrorTextIsEnable = true;
            }

            if (Username == null)
                ButtonLoginIsEnabled = true;
                ErrorText = "El usuario no puede estar vacio";
                ButtonLoginOpacity = 0.5f;
                ErrorTextIsEnable = true;

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

                    string urlAdmin = $"{DevTunnel.UrlDeborah}/User/is-admin?username={Username}";
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
                                await Shell.Current.GoToAsync(nameof(AllUsersView));
                            }
                            else
                            {
                                await Shell.Current.GoToAsync(nameof(CodeVerification));
                            }
                        }
                        else
                        {
                            string urlGod = $"{DevTunnel.UrlDeborah}/User/is-god?username={Username}";
                            var responseGod = await _httpClient.GetAsync(urlGod);
                            if (responseGod.IsSuccessStatusCode)
                            {
                                var isGod = await responseGod.Content.ReadAsStringAsync();
                                bool isGodUser = bool.Parse(isGod);
                                if (isGodUser)
                                {
                                    TokenManager.isGod = true;
                                    TokenManager.isAdmin = true;
                                   bool answer2 = await _dialogService.DisplayAlert("AdminSupremo", "Eres un Admin Supremo. ¿Qué quieres hacer?", "AdminMode", "VerCode");
                                    if (answer2)
                                    {
                                        await Shell.Current.GoToAsync(nameof(AllUsersView));
                                    }
                                    else
                                   {
                                       await Shell.Current.GoToAsync(nameof(CodeVerification));
                                    }
                                }
                                else
                                {
                                    TokenManager.isGod = false;
                                    TokenManager.isAdmin = false;
                                    await Shell.Current.GoToAsync(nameof(CodeVerification));
                                }
                            }
                        }
                    }
                    else
                    {
                        await _dialogService.DisplayAlert("Error", "No se pudo verificar si el usuario es admin.", null , "OK");
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
                await _dialogService.DisplayAlert("Error", $"Error: {ex.Message}", null, "OK");
            }
        }

 
        [RelayCommand]
        public async void ForgotPasswordCommand()
        {
            await Shell.Current.GoToAsync(nameof(ForgotPasswordView));
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
        public async void CreateAccountButton()
        {
            await Shell.Current.GoToAsync(nameof(CreateAccountView));
        }

        
    }
}