namespace PTMobile;

public partial class PasswordAddProject : ContentPage
{
	public PasswordAddProject()
	{
		InitializeComponent();
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