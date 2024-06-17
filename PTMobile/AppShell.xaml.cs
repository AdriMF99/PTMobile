using PTMobile.Views;

namespace PTMobile
{
    public partial class AppShell : Shell
    {
        private FlyoutItem _allUsersFlyout;
        private FlyoutItem _allProjectsToAddFlyout;

        public AppShell()
        {
            InitializeComponent();
            InitializeRoutes();
            AddAndHideFlyoutItems();
        }

        private void InitializeRoutes()
        {
            Routing.RegisterRoute(nameof(LoginView), typeof(LoginView));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(SettingsUpdateFields), typeof(SettingsUpdateFields));
            Routing.RegisterRoute(nameof(AllProjects), typeof(AllProjects));
            Routing.RegisterRoute(nameof(AllProjectsToAdd), typeof(AllProjectsToAdd));
            Routing.RegisterRoute(nameof(AllUsersView), typeof(AllUsersView));
            Routing.RegisterRoute(nameof(CreateAccountView), typeof(CreateAccountView));
            Routing.RegisterRoute(nameof(EntryCodeForgotPassword), typeof(EntryCodeForgotPassword));
            Routing.RegisterRoute(nameof(ForgotPasswordView), typeof(ForgotPasswordView));
            Routing.RegisterRoute(nameof(PasswordAddProject), typeof(PasswordAddProject));
            Routing.RegisterRoute(nameof(VerifyAccount), typeof(VerifyAccount));
            Routing.RegisterRoute(nameof(CodeVerification), typeof(CodeVerification));
        }

        private void AddAndHideFlyoutItems()
        {
            _allUsersFlyout = new FlyoutItem()
            {
                Title = "All Users",
                Route = "AllUsersView",
                Items =
                {
                    new ShellContent()
                    {
                        Title = "All Users",
                        ContentTemplate = new DataTemplate(typeof(AllUsersView))
                    }
                }
            };

            _allProjectsToAddFlyout = new FlyoutItem()
            {
                Title = "All Projects to Add",
                Route = "AllProjectsToAdd",
                Items =
                {
                    new ShellContent()
                    {
                        Title = "All Projects to Add",
                        ContentTemplate = new DataTemplate(typeof(AllProjectsToAdd))
                    }
                }
            };

            // Hide the items initially by not adding them to the Shell.Items
        }

        public void ShowAdminFlyoutItems()
        {
            // Show the admin flyout items by adding them to the Shell.Items
            if (!Shell.Current.Items.Contains(_allUsersFlyout))
                Shell.Current.Items.Add(_allUsersFlyout);

            if (!Shell.Current.Items.Contains(_allProjectsToAddFlyout))
                Shell.Current.Items.Add(_allProjectsToAddFlyout);
        }

        public void HideAdminFlyoutItems()
        {
            // Hide the admin flyout items by removing them from the Shell.Items
            Shell.Current.Items.Remove(_allUsersFlyout);
            Shell.Current.Items.Remove(_allProjectsToAddFlyout);
        }
    }
}
