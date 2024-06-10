using PTMobile.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PTMobile.Models;
using PTMobile.ViewModels;
using System.Text;
namespace PTMobile.View;

public partial class SettingsUpdateFields : ContentPage
{
    private readonly HttpClient _httpClient = new();
    public SettingsUpdateFields()
    {
        InitializeComponent();

        currentUser.Text = TokenManager.currentUser;

        BindingContext = new SettingsUpdateFieldsViewModel();

        // LoadDataUser();
    }
}