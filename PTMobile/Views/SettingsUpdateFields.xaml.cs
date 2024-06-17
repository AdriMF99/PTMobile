using PTMobile.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PTMobile.Models;
using PTMobile.ViewModels;
using System.Text;

namespace PTMobile.Views;

public partial class SettingsUpdateFields : ContentPage
{
    private readonly HttpClient _httpClient = new();
    public SettingsUpdateFields()
    {
        InitializeComponent();

        //currentUser.Text = TokenManager.currentUser;

        BindingContext = new SettingsUpdateFieldsViewModel();

        // LoadDataUser();
    }

    protected override bool OnBackButtonPressed() => true;

    //private async void OnMoreClicked(object sender, EventArgs e)
    //{
    //    var action = await DisplayActionSheet("Options", "Cancel", null, "Option 1", "Option 2", "Option 3");
    //    // Manejar la selección de las opciones aquí
    //}

}