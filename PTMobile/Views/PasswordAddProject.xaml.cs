using PTMobile.Models;

namespace PTMobile.Views;

public partial class PasswordAddProject : ContentPage
{
    public PasswordAddProject()
    {
        InitializeComponent();
        currentUser.Text = TokenManager.currentUser;
    }

    //private async void OnMoreClicked(object sender, EventArgs e)
    //{
    //    var action = await DisplayActionSheet("Options", "Cancel", null, "Option 1", "Option 2", "Option 3");
    //    // Manejar la selecci�n de las opciones aqu�
    //}

    private async void checkButtonClicked(object sender, EventArgs e)
    {
        if (passEntry.Text == "1234a")
        {
            await Navigation.PushAsync(new AllProjectsToAdd());
        }
        else
        {
            await DisplayAlert("Error", "Contrase�a Incorrecta!", "Ok");
        }
    }
}