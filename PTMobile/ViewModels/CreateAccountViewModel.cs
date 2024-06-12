//using CommunityToolkit.Mvvm.ComponentModel;
//using CommunityToolkit.Mvvm.Input;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using PTMobile.Models;
//using PTMobile.Views;
//using System.ComponentModel;
//using System.Text;

//namespace PTMobile.ViewModels
//{
//    public partial class CreateAccountViewModel : ObservableObject
//    {
//        [ObservableProperty]
//        private string email;

//        [ObservableProperty]
//        private string username;

//        [ObservableProperty]
//        private string password;

//        [ObservableProperty]
//        private string repeatPassword;

//        [ObservableProperty]
//        private string errorText;

//        [ObservableProperty]
//        private bool errorTextIsEnable = false;

//        [ObservableProperty]
//        private bool passwordIsEnabled = true;

//        [ObservableProperty]
//        private bool repeatPasswordIsEnabled = true;

//        [ObservableProperty]
//        private float buttonCreateAccountOpacity = 0.5f;

//        [ObservableProperty]
//        private bool buttonCreateAccountIsEnabled = false;

//        [ObservableProperty]
//        private string errorTextCreateAccount;

//        [ObservableProperty]
//        private bool errorTextCreateAccountIsEnable = false;



//        private readonly HttpClient _httpClient;
//        private bool accountCreationRequested = false;


//        public CreateAccountViewModel()
//        {
//            _httpClient = new HttpClient();
//            CreateAccountCommand = new AsyncRelayCommand(CreateAccountAsync);
//            TogglePasswordVisibilityCommand = new RelayCommand(TogglePasswordVisibility);
//            ToggleRepeatPasswordVisibilityCommand = new RelayCommand(ToggleRepeatPasswordVisibility);
//            //CreateAccountCommand = new AsyncRelayCommand(CreateAccountAsync);

//            ValidateInputs();
//        }
//        public IAsyncRelayCommand CreateAccountCommand { get; }
//        public IRelayCommand TogglePasswordVisibilityCommand { get; }
//        public IRelayCommand ToggleRepeatPasswordVisibilityCommand { get; }
//        //public IAsyncRelayCommand CreateAccountCommand { get; }



//        private void ValidateInputs()
//        {
//            ErrorText = string.Empty;
//            ErrorTextIsEnable = false;
//            ButtonCreateAccountIsEnabled = true;
//            ButtonCreateAccountOpacity = 1.0f;

//            if (string.IsNullOrEmpty(Password))
//            {
//                ErrorText = "Password cannot be empty.";
//                ErrorTextIsEnable = true;
//                ButtonCreateAccountIsEnabled = false;
//                ButtonCreateAccountOpacity = 0.5f;
//            }

//            if (string.IsNullOrEmpty(Email))
//            {
//                ErrorText = "Email cannot be empty.";
//                ErrorTextIsEnable = true;
//                ButtonCreateAccountIsEnabled = false;
//                ButtonCreateAccountOpacity = 0.5f;
//            }

//            if (string.IsNullOrEmpty(Username))
//            {
//                ErrorText = "User cannot be empty.";
//                ErrorTextIsEnable = true;
//                ButtonCreateAccountIsEnabled = false;
//                ButtonCreateAccountOpacity = 0.5f;
//            }

//            bool passwordsMatch = Password == RepeatPassword;
//            if (!passwordsMatch)
//            {
//                ErrorText = "Passwords do not match.";
//                ErrorTextIsEnable = true;
//                ButtonCreateAccountIsEnabled = false;
//                ButtonCreateAccountOpacity = 0.5f;
//            }
//        }


//        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
//        {
//            // Valida las entradas cada vez que cambia una propiedad observada
//            ValidateInputs();
//        }

//        partial void OnEmailChanged(string value) => ValidateInputs();
//        partial void OnUsernameChanged(string value) => ValidateInputs();
//        partial void OnPasswordChanged(string value) => ValidateInputs();
//        partial void OnRepeatPasswordChanged(string value) => ValidateInputs();



//        //[RelayCommand]
//        public async Task CreateAccountAsync()
//        {
//            ValidateInputs();

//            string url = $"{DevTunnel.UrlDeborah}/User/create-user";

//            //if (string.IsNullOrEmpty(Password))
//            //{
//            //    ErrorText = "Password cannot be empty.";
//            //    ErrorTextIsEnable = true;
//            //    ButtonCreateAccountIsEnabled = false;
//            //    ButtonCreateAccountOpacity = 0.5f;
//            //}

//            //if (string.IsNullOrEmpty(Email))
//            //{
//            //    ErrorText = "Email cannot be empty.";
//            //    ErrorTextIsEnable = true;
//            //    ButtonCreateAccountIsEnabled = false;
//            //    ButtonCreateAccountOpacity = 0.5f;
//            //}

//            //if (string.IsNullOrEmpty(Username))
//            //{
//            //    ErrorText = "User cannot be empty.";
//            //    ErrorTextIsEnable = true;
//            //    ButtonCreateAccountIsEnabled = false;
//            //    ButtonCreateAccountOpacity = 0.5f;
//            //}


//            //bool passwordsMatch = Password == RepeatPassword;
//            //if (!passwordsMatch)
//            //{
//            //    ErrorText = "Passwords do not match.";
//            //    ErrorTextIsEnable = true;
//            //    ButtonCreateAccountIsEnabled = false;
//            //    ButtonCreateAccountOpacity = 0.5f;
//            //}

//            //if (ErrorTextIsEnable)
//            //{
//            //    return;
//            //}

//            //if (accountCreationRequested)
//            //{
//            //    ErrorTextCreateAccountIsEnable = true;
//            //    ErrorTextCreateAccount = "A request has already been sent.";
//            //    return;
//            //}


//            if (ErrorTextIsEnable)
//            {
//                return;
//            }

//            if (accountCreationRequested)
//            {
//                ErrorTextCreateAccountIsEnable = true;
//                ErrorTextCreateAccount = "A request has already been sent.";
//                return;
//            }
//            try
//            {
//                var requestData = new
//                {
//                    UserName = Username,
//                    Email = Email,
//                    Password = Password

//                };
//                var json = JsonConvert.SerializeObject(requestData);
//                var content = new StringContent(json, Encoding.UTF8, "application/json");
//                var response = await _httpClient.PostAsync(url, content);

//                if (response.IsSuccessStatusCode)
//                {
//                    var isCreated = await response.Content.ReadAsStringAsync();
//                    JObject responseData = JObject.Parse(isCreated);

//                    accountCreationRequested = true;

//                    await Shell.Current.GoToAsync(nameof(VerifyAccount));
//                }


//                else
//                {
//                    ErrorTextCreateAccountIsEnable = true;
//                    ErrorTextCreateAccount = "Error creating account. Please try again.";
//                }
//            }
//            catch (Exception ex)
//            {
//                ErrorTextCreateAccount = $"Error: {ex.Message}";
//            }
//        }


//       // [RelayCommand]
//        public async void TogglePasswordVisibility()
//        {
//            if (Password != null)
//            {
//                PasswordIsEnabled = !PasswordIsEnabled;
//            }
//        }

//       // [RelayCommand]
//        private async void ToggleRepeatPasswordVisibility()
//        {
//            if (RepeatPassword != null)
//            {
//                RepeatPasswordIsEnabled = !RepeatPasswordIsEnabled;
//            }
//        }


//    }
//}






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
        private float buttonCreateAccountOpacity = 0.5f;

        [ObservableProperty]
        private bool buttonCreateAccountIsEnabled = false;

        [ObservableProperty]
        private string errorTextCreateAccount;

        [ObservableProperty]
        private bool errorTextCreateAccountIsEnable = false;

        private readonly HttpClient _httpClient;
        private bool accountCreationRequested = false;

        public CreateAccountViewModel()
        {
            _httpClient = new HttpClient();
            CreateAccountCommand = new AsyncRelayCommand(CreateAccountAsync);
            TogglePasswordVisibilityCommand = new RelayCommand(TogglePasswordVisibility);
            ToggleRepeatPasswordVisibilityCommand = new RelayCommand(ToggleRepeatPasswordVisibility);

            // Inicializa la validación de entradas
            ValidateInputs();
        }

        public IAsyncRelayCommand CreateAccountCommand { get; }
        public IRelayCommand TogglePasswordVisibilityCommand { get; }
        public IRelayCommand ToggleRepeatPasswordVisibilityCommand { get; }

        private void ValidateInputs()
        {
            ErrorText = string.Empty;
            ErrorTextIsEnable = false;
            ButtonCreateAccountIsEnabled = true;
            ButtonCreateAccountOpacity = 1.0f;

            if (string.IsNullOrEmpty(Password))
            {
                ErrorText = "Password cannot be empty.";
                ErrorTextIsEnable = true;
                ButtonCreateAccountIsEnabled = false;
                ButtonCreateAccountOpacity = 0.5f;
            }

            if (string.IsNullOrEmpty(Email))
            {
                ErrorText = "Email cannot be empty.";
                ErrorTextIsEnable = true;
                ButtonCreateAccountIsEnabled = false;
                ButtonCreateAccountOpacity = 0.5f;
            }

            if (string.IsNullOrEmpty(Username))
            {
                ErrorText = "User cannot be empty.";
                ErrorTextIsEnable = true;
                ButtonCreateAccountIsEnabled = false;
                ButtonCreateAccountOpacity = 0.5f;
            }

            bool passwordsMatch = Password == RepeatPassword;
            if (!passwordsMatch)
            {
                ErrorText = "Passwords do not match.";
                ErrorTextIsEnable = true;
                ButtonCreateAccountIsEnabled = false;
                ButtonCreateAccountOpacity = 0.5f;
            }
        }

        partial void OnEmailChanged(string value) => ValidateInputs();
        partial void OnUsernameChanged(string value) => ValidateInputs();
        partial void OnPasswordChanged(string value) => ValidateInputs();
        partial void OnRepeatPasswordChanged(string value) => ValidateInputs();

        public async Task CreateAccountAsync()
        {
            // Primero valida los inputs
            ValidateInputs();

            // Si hay errores, no continúa
            if (ErrorTextIsEnable)
            {
                return;
            }

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
                var response = await _httpClient.PostAsync($"{DevTunnel.UrlDeborah}/User/create-user", content);

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
