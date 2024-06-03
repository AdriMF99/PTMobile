using PTMobile.Models;

namespace PTMobile.Views;

public partial class PasswordAddProject : ContentPage
{
	public PasswordAddProject()
	{
		InitializeComponent();
        currentUser.Text = TokenManager.currentUser;
    }

	private async void checkButtonClicked(object sender, EventArgs e)
	{
        if (passEntry.Text == "1234a")
		{
            await Navigation.PushAsync(new AllProjectsToAdd());
        }
		else
		{
			await DisplayAlert("Error", "Contraseña Incorrecta!", "Ok");
		}
    }
}