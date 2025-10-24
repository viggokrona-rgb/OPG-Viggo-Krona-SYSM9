

using CookMaster.Views;

namespace CookMaster.Services
{
    public interface INavigationService
    {
        void ShowRecipesWindow();
        void ShowSignInWindow();
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

     
    }
}
