
using PTMobile.ViewModels;
using PTMobile.Views;

namespace PTMobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginView), typeof(LoginView));
            Routing.RegisterRoute(nameof(LoginViewModel), typeof(LoginViewModel));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(UpdateFields), typeof(UpdateFields));
            Routing.RegisterRoute(nameof(AllProjects), typeof(AllProjects));
            Routing.RegisterRoute(nameof(AllProjectsToAdd), typeof(AllProjectsToAdd));
            Routing.RegisterRoute(nameof(AllUsersView), typeof(AllUsersView));
            Routing.RegisterRoute(nameof(CodeValidationView), typeof(CodeValidationView));
            Routing.RegisterRoute(nameof(CreateAccountView), typeof(CreateAccountView));
            Routing.RegisterRoute(nameof(EntryCodeForgotPassword), typeof(EntryCodeForgotPassword));
            Routing.RegisterRoute(nameof(ForgotPasswordView), typeof(ForgotPasswordView));
            Routing.RegisterRoute(nameof(PasswordAddProject), typeof(PasswordAddProject));
            Routing.RegisterRoute(nameof(VerifyAccount), typeof(VerifyAccount));
        }
    }
}
