using Newtonsoft.Json;
using PTMobile.Models;

namespace PTMobile;

public partial class CodeVerification : ContentPage
{
	public CodeVerification()
	{
		InitializeComponent();
	}

    public async void OnVerifyCodeClicked(object sender, EventArgs e)
	{
        using (var httpClient = new HttpClient())
        {
            var code = codeEntry.Text;
            var token = TokenManager.Token;
            string url = $"https://cmg6rb8b-5250.uks1.devtunnels.ms/api/Code/VerifyCodeMobile?code={code}&token={token}";

            HttpResponseMessage response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                await Navigation.PushAsync(new AllProjects());
            }
        }
        
        //await DisplayAlert("Code", $"Código introducido: '{code}'", "OK!");       
    }
}