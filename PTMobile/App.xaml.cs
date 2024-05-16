using PTMobile.View;

namespace PTMobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginView());
        }
    }
}
