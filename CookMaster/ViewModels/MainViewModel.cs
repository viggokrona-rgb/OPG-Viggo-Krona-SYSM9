using System;
using System.Windows;

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

        public MainViewModel()
        {
        }

        public void SignIn(string password)
        {
            string user = Username;
            string pass = password ?? string.Empty;
        }
    }
}
