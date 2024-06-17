
using PTMobile.Interfaces;
using PTMobile.Services;

namespace PTMobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();

            DependencyService.Register<IDialogService, DialogService>();
        }
    }
}
