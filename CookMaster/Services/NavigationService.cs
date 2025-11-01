

using CookMaster.Model;
using CookMaster.Views;

namespace CookMaster.Services
{
    public interface INavigationService
    {
        void ShowRecipesWindow();
        void ShowSignInWindow();
        void ShowAddRecipeWindow(Recipe? template = null);
        void ShowRegisterWindow();

        void ShowUserDetailsWindow();

        void ShowResetPasswordWindow(string? username = null);
        void ShowForgotPasswordWindow();

        void ShowTwoFactorWindow();

    }

    public class NavigationService : INavigationService
    {
        public void ShowRecipesWindow()
        {
            var win = new RecipeListWindow();
            win.Show();
            // Close current main window if desired
            System.Windows.Application.Current.MainWindow?.Close();
            System.Windows.Application.Current.MainWindow = win;
        }

        public void ShowSignInWindow()
        {
            var win = new MainWindow();
            win.Show();
            System.Windows.Application.Current.MainWindow?.Close();
            System.Windows.Application.Current.MainWindow = win;
        }

        public void ShowAddRecipeWindow(Recipe? template = null)
        {
            var win = new AddRecipeWindow();
            win.DataContext = new ViewModels.AddRecipeViewModel(template);
            win.Show();
            System.Windows.Application.Current.MainWindow?.Close();
            System.Windows.Application.Current.MainWindow = win;
        }

        public void ShowRegisterWindow()
        {
            var win = new RegisterWindow();
            win.Show();
            System.Windows.Application.Current.MainWindow?.Close();
            System.Windows.Application.Current.MainWindow = win;
        }

        public void ShowUserDetailsWindow()
        {
            var win = new UserDetailsWindow();
            win.Show();
            System.Windows.Application.Current.MainWindow?.Close();
            System.Windows.Application.Current.MainWindow = win;

        }

        public void ShowResetPasswordWindow(string? username = null)
        {
            var win = new ResetPasswordWindow();
            if (win.DataContext is ViewModels.ResetPasswordViewModel vm && !string.IsNullOrWhiteSpace(username))
            {
                vm.Username = username;
                // trigger lookup to show security question
                if (vm.LookupCommand.CanExecute(null)) vm.LookupCommand.Execute(null);
            }
            win.Show();
            System.Windows.Application.Current.MainWindow?.Close();
            System.Windows.Application.Current.MainWindow = win;
        }
        public void ShowForgotPasswordWindow()
        {
            var win = new ForgotPasswordWindow();
            win.Show();
            System.Windows.Application.Current.MainWindow?.Close();
            System.Windows.Application.Current.MainWindow = win;

        }

        public void ShowTwoFactorWindow()
        {
            var win = new TwoFactorWindow();
            win.Show();
            System.Windows.Application.Current.MainWindow?.Close();
            System.Windows.Application.Current.MainWindow = win;
        }
    }

}
