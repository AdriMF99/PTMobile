using Newtonsoft.Json.Linq;
using PTMobile.Models;

namespace PTMobile
{
    public partial class MainPage : ContentPage
    {
        private readonly HttpClient _httpClient = new();
        public MainPage()
        {
            InitializeComponent();
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            string username = usernameEntry.Text;
            string password = passwordEntry.Text;
            string url = $"https://dkhgf4x3-5250.uks1.devtunnels.ms/User/login?username={Uri.EscapeDataString(username)}&password={Uri.EscapeDataString(password)}";

            try
            {
                var response = await _httpClient.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    var isLogin = await response.Content.ReadAsStringAsync();
                    JObject responseData = JObject.Parse(isLogin);

                    loginResultLabel.IsVisible = true;
                    string token = responseData?.SelectToken("Value").Value<string>();

                    // Guardar el token generado
                    TokenManager.Token = token;

                    loginResultLabel.Text = token;

                    // Esperar dos segundos
                    //await Task.Delay(2000);

                    await Navigation.PushAsync(new CodeVerification());
                }
                else
                {
                    loginResultLabel.IsVisible = true;
                    loginResultLabel.Text = "Error en la autenticación. Por favor, inténtalo de nuevo.";
                }
            }
            catch (Exception ex)
            {
                loginResultLabel.IsVisible = true;
                loginResultLabel.Text = $"Error: {ex.Message}";
            }

        }

        private async void DirectoNoLog(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CodeVerification());
        }
    }

}
