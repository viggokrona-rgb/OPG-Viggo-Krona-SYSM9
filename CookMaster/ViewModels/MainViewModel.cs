using CookMaster.Core;
using CookMaster.Managers;
using CookMaster.Services;
using CookMaster.Views;
using System;
using System.Windows;
using System.Windows.Input;

namespace CookMaster.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly UserManager _userManager;
        private readonly RecipeManager _recipeManager;

        private string _username = "";
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        private string _password = "";
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public ICommand SignInCommand { get; }
        public ICommand RegisterCommand { get; }

        public MainViewModel(UserManager userManager, RecipeManager recipeManager)
        {
            _userManager = userManager;
            _recipeManager = recipeManager;

            SignInCommand = new RelayCommand(SignIn);
            RegisterCommand = new RelayCommand(OpenRegister);
        }

        private void SignIn(object? obj)
        {
            if (_userManager.ValidateUser(Username, Password))
            {
                var recipeWindow = new RecipeListWindow(Username, _userManager, _recipeManager);
                recipeWindow.Show();
                CloseWindow(obj);
            }
            else
            {
                MessageBox.Show("Fel användarnamn eller lösenord.", "Inloggning misslyckades", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void OpenRegister(object? obj)
        {
            var regWindow = new RegisterWindow(_userManager, _recipeManager);
            regWindow.Show();
            CloseWindow(obj);
        }

        private void CloseWindow(object? obj)
        {
            if (obj is Window w)
                w.Close();
        }
    }
}
}
