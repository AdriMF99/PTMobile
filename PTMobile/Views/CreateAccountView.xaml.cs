using PTMobile.ViewModels;

namespace PTMobile.Views;

public partial class CreateAccountView : ContentPage
{
    //private readonly HttpClient _httpClient = new();

    public CreateAccountView()
    {
        InitializeComponent();

        BindingContext = new CreateAccountViewModel();

        // CheckForm();

    }


    //private async void OnMoreClicked(object sender, EventArgs e)
    //{
    //    var action = await DisplayActionSheet("Options", "Cancel", null, "Option 1", "Option 2", "Option 3");
    //    // Manejar la selección de las opciones aquí
    //}


    //private void TogglePasswordVisibility1(object sender, EventArgs e)
    //{
    //    if (passwordEntry != null)
    //    {
    //        passwordEntry.IsPassword = !passwordEntry.IsPassword;
    //    }
    //}

    //private void TogglePasswordVisibility2(object sender, EventArgs e)
    //{
    //    if (repeatPasswordEntry != null)
    //    {
    //        repeatPasswordEntry.IsPassword = !repeatPasswordEntry.IsPassword;
    //    }
    //}

    //private void OnInputTextChanged(object sender, TextChangedEventArgs e)
    //{
    //    CheckForm();
    //}

    //private void CheckForm()
    //{
    //    bool passwordsMatch = passwordEntry.Text == repeatPasswordEntry.Text;
    //    bool isUsernameComplete = !string.IsNullOrEmpty(usernameEntry.Text);
    //    bool isEmailComplete = !string.IsNullOrEmpty(emailEntry.Text);

    //    bool isFormComplete = passwordsMatch && isUsernameComplete && isEmailComplete;

    //    createAccountButton.IsEnabled = isFormComplete;
    //    createAccountButton.Opacity = isFormComplete ? 1.0 : 0.5;
    //}


    //private bool accountCreationRequested = false;
    //private async void CreateAccountButton_Clicked(object sender, EventArgs e)
    //{
    //    string username = usernameEntry.Text;
    //    string email = emailEntry.Text;
    //    string password = passwordEntry.Text;
    //    string url = $"{DevTunnel.UrlDeborah}/User/create-user";

    //    if (accountCreationRequested)
    //    {
    //        createAccountResultLabel.Text = "Ya se ha enviado una solicitud.";
    //        return;
    //    }
    //    try
    //    {
    //        var requestData = new
    //        {
    //            UserName = username,
    //            Email = email,
    //            Password = password
    //        };
    //        var json = JsonConvert.SerializeObject(requestData);
    //        var content = new StringContent(json, Encoding.UTF8, "application/json");
    //        var response = await _httpClient.PostAsync(url, content);


    //        if (response.IsSuccessStatusCode)
    //        {
    //            var isCreated = await response.Content.ReadAsStringAsync();
    //            JObject responseData = JObject.Parse(isCreated);

    //            accountCreationRequested = true;

    //            await Navigation.PushAsync(new VerifyAccount());
    //        }


    //        else
    //        {
    //            createAccountResultLabel.Text = "Error al crear cuenta. Por favor, inténtalo de nuevo.";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        createAccountResultLabel.Text = $"Error: {ex.Message}";
    //    }

    //}
}
