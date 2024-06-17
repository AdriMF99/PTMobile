using Newtonsoft.Json;
using PTMobile.Interfaces;
using PTMobile.Models;
using PTMobile.ViewModels;
using System.Text;

namespace PTMobile.Views;

public partial class EntryCodeForgotPassword : ContentPage
{
    public EntryCodeForgotPassword()
    {
        InitializeComponent();

        BindingContext = new EntryCodeForgotPasswordViewModel();
        //CheckForm();
    }
}