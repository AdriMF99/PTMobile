using PTMobile.Interfaces;
using PTMobile.Models;
using PTMobile.ViewModels;

namespace PTMobile.Views;

public partial class ForgotPasswordView : ContentPage
{

    //private readonly HttpClient _httpClient = new();
    public ForgotPasswordView()
    {
        InitializeComponent();
        currentUser.Text = TokenManager.currentUser;

        BindingContext = new ForgotPasswordViewModel();
    }
}


