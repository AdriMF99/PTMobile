using PTMobile.Models;

namespace PTMobile.Views
{
    public partial class CodeVerification : ContentPage
    {
        public CodeVerification()
        {
            InitializeComponent();
            currentUser.Text = TokenManager.currentUser;
        }

        protected override bool OnBackButtonPressed() => true;


        //private async void OnMoreClicked(object sender, EventArgs e)
        //{
        //    var action = await DisplayActionSheet("Options", "Cancel", null, "Option 1", "Option 2", "Option 3");
        //    // Manejar la selección de las opciones aquí
        //}


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
                    await Shell.Current.GoToAsync("//AllProjects");
                }
            }

            //await DisplayAlert("Code", $"Código introducido: '{code}'", "OK!");       
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
                    string url = $"{DevTunnel.UrlFran}/api/Code/VerifyCodeMobile?code={first.Value}&token={token}";

                    HttpResponseMessage response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        TokenManager.TvCode = first.Value;
                        await Shell.Current.GoToAsync("//AllProjects");
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