using PTMobile.Models;
using Microsoft.Maui.Graphics;
using Xamarin.Essentials;
using Newtonsoft.Json;

namespace PTMobile.View
{
    public partial class CodeVerification : ContentPage
    {
        public CodeVerification()
        {
            InitializeComponent();
            currentUser.Text = TokenManager.currentUser;
        }

        public async void OnVerifyCodeClicked(object sender, EventArgs e)
        {
            using (var httpClient = new HttpClient())
            {
                var code = codeEntry.Text;
                var token = TokenManager.Token;
                string url = $"{DevTunnel.UrlFran}/api/Code/VerifyCodeMobile?code={Uri.EscapeDataString(code)}&token={Uri.EscapeDataString(token)}";

                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<dynamic>(content);

                        if (result.success == true)
                        {
                            TokenManager.TvCode = code;
                            await Navigation.PushAsync(new AllProjects());
                        }
                        else
                        {
                            await DisplayAlert("Error", "Código incorrecto. Por favor, inténtalo de nuevo.", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "Error al verificar el código. Por favor, inténtalo de nuevo.", "OK");
                    }
                }
                catch (HttpRequestException ex)
                {
                    await DisplayAlert("Error de red", $"No se pudo conectar con el servidor: {ex.Message}", "OK");
                }
                catch (JsonException ex)
                {
                    await DisplayAlert("Error", $"Error al procesar la respuesta del servidor: {ex.Message}", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Error inesperado: {ex.Message}", "OK");
                }
            }
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
                        barcodeReader.IsVisible = false;
                        return;
                    }

                    var token = TokenManager.Token;
                    string url = $"{DevTunnel.UrlDeborah}/api/Code/VerifyCodeMobile?code={first.Value}&token={token}";

                    HttpResponseMessage response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        TokenManager.TvCode = first.Value;
                        await Navigation.PushAsync(new AllProjects());
                    }
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    barcodeReader.IsVisible = false;
                });

            });
        }

        private void Click_camera(object sender, EventArgs e)
        {
            barcodeReader.IsVisible = true;
        }
        
    }
}