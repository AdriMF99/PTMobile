using Newtonsoft.Json;
using PTMobile.Models;
using PTMobile.ViewModels;
using PTMobile.Views;

namespace PTMobile.Views;

public partial class AllProjects : ContentPage
{

    public AllProjects()
    {
        InitializeComponent();
    }

    protected override bool OnBackButtonPressed() => true;

    //private async void OnMoreClicked(object sender, EventArgs e)
    //{
    //    var action = await DisplayActionSheet("Options", "Cancel", null, "Option 1", "Option 2", "Option 3");
    //    // Manejar la selección de las opciones aquí
    //}


    protected override async void OnAppearing()
    {
        base.OnAppearing();
        {
        }
        else
        {
            adminbutton.IsVisible = false;
    }

    {
        }

        ShowSwipeAnimation();
    }

    //protected override async void OnAppearing()
    //{
    //    base.OnAppearing();




    public async void OnShowConfirmation(object sender, EventArgs e)
    {
        var button = sender as SwipeItem;
        var project = button?.BindingContext as Project;




            if (answer)
            {
                using (var httpClient = new HttpClient())
                {
                    string urlDelete = $"{DevTunnel.UrlDeborah}/api/Project/deleteProject?projectId={project.Id}";
                    HttpResponseMessage response = await httpClient.DeleteAsync(urlDelete);
                    if (response.IsSuccessStatusCode)
                    {
                        await DisplayAlert("Info", $"¡{project.ProjectName}' está borrado!", "OK!");
                        List<Project> projects = await GetProjectsAsync(TokenManager.currentUser);
                        TokenManager.Allprojects = projects;
                        if (projects != null)
                        {
                            projectsList.ItemsSource = projects;
                        }
                    }
                    else
                    {
                        await DisplayAlert("Info", $"¡{project.ProjectName}' tuvo problemas al borrar!", "OK!");
                    }
                }
            }
            else
            {
                // Acción si se pulsa "NO"
            }
        }
        else
        {
            await DisplayAlert("Error", "No se pudo obtener la información del proyecto", "OK");
        }
    }





    //private async void OnAddProjectButtonClicked(object sender, EventArgs e)
    //{
    //    TokenManager.isFromAdmin = false;
    //    await Navigation.PushAsync(new PasswordAddProject());
    //}




    //        await projectsList.TranslateTo(-50, 0, 250, Easing.SinInOut);
    //        await projectsList.TranslateTo(0, 0, 250, Easing.SinInOut);
    //        await Task.Delay(500);

}