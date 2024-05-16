using PTMobile.Models;
using Microsoft.Maui.Graphics;
using Xamarin.Essentials;

namespace PTMobile
{
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
                string url = $"{DevTunnel.UrlFran}/api/Code/VerifyCodeMobile?code={code}&token={token}";

                HttpResponseMessage response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    TokenManager.TvCode = code;
                    await Navigation.PushAsync(new AllProjects());
                }
            }

            //await DisplayAlert("Code", $"C�digo introducido: '{code}'", "OK!");       
        }

        /*public void OnQRClicked(object sender, EventArgs e)
        {
            barcodeReader.IsVisible = false;
        }*/

        [Obsolete]
        private async void barcodeReader_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                using (var httpClient = new HttpClient())
                {
                    var first = e.Results?.FirstOrDefault();

                    if (first == null)
                    {
                        return;
                    }

                    var token = TokenManager.Token;
                    string url = $"{DevTunnel.UrlFran}/api/Code/VerifyCodeMobile?code={first.Value}&token={token}";

                    HttpResponseMessage response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        TokenManager.TvCode = first.Value;
                        await Navigation.PushAsync(new AllProjects());
                    }
                }
            });
        }

    }
}