using CookMaster.Managers;
using CookMaster.Services;
using System.Configuration;
using System.Data;
using System.Windows;

namespace CookMaster
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // Initialize managers (ensure singletons are created)
            _ = UserManager.Instance;
            _ = RecipeManager.Instance;
        }
    }

}
