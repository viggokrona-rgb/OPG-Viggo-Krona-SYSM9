using System;
using System.Windows;
using CookMaster.Managers;

namespace CookMaster.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _username = string.Empty;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public UserManager UserManager { get; }

        public MainViewModel()
        {

            UserManager = (UserManager)Application.Current.Resources["UserManager"];

        }

        public void SignIn(string password)
        {
           
            string pass = password ?? string.Empty;

            bool success = UserManager.Login(Username, pass);

            if (success)
            {
                MessageBox.Show($"Signed in as {UserManager.CurrentUser.Username}", "Sign In");
            }
            else
            {
                MessageBox.Show("Sign in failed", "Sign In");
            }
        }

        public void SignOut()
        {
            UserManager.Logout();
        }
    }
}
