using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace ProjectTrackMaui.View;

public partial class LoginView : ContentPage
{

    private readonly HttpClient _httpClient = new();
    private const string BaseAddress = "https://7z9w9942-5250-inspect.uks1.devtunnels.ms";


    public LoginView()
	{
		InitializeComponent();
	}


    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string username = usernameEntry.Text;
        string password = passwordEntry.Text;

        try
        {
            var formData = new Dictionary<string, string>
        {
            { "UserName", "AAA" },
            { "Password", "12345678Aa" }
        };

            var jsonContent = new StringContent(JsonSerializer.Serialize(formData), Encoding.UTF8, "application/json");

           var response = await _httpClient.PostAsync($"{BaseAddress}/User/login", jsonContent);
            //var response = await _httpClient.GetAsync($"{BaseAddress}/User/all-users");

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
            }
            else
            {
            
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
         
            Console.WriteLine($"Error: {ex.Message}");
        }

       // var result = await _httpClient.PostAsync($"{BaseAddress}/User/login");
        //var result = await _httpClient.GetStringAsync(" https://bgm8xsbd-5250.uks1.devtunnels.ms/User/all-users");
    
        //Console.WriteLine( result );
    }
}