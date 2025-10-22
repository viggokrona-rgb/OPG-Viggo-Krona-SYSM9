using CookMaster.Managers;
using CookMaster.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace CookMaster.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private readonly UserManager _userManager;
        private readonly RecipeManager _recipeManager;

        public ObservableCollection<string> Countries { get; } = new()
        {
            "Sverige", "Norge", "Danmark", "Finland", "Tyskland", "USA"
        };

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

        private string _country = "";
        public string Country
        {
            get => _country;
            set { _country = value; OnPropertyChanged(); }
        }

        public ICommand RegisterCommand { get; }
        public ICommand CancelCommand { get; }

        public RegisterViewModel(UserManager userManager, RecipeManager recipeManager)
        {
            _userManager = userManager;
            _recipeManager = recipeManager;

            RegisterCommand = new RelayCommand(RegisterUser);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void RegisterUser(object? obj)
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Fyll i alla fält.", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _userManager.AddUser(new UserInfo
            {
                Username = Username,
                Password = Password,
                Country = Country,
                IsAdmin = false
            });

            MessageBox.Show("Användare skapad!", "CookMaster", MessageBoxButton.OK, MessageBoxImage.Information);
            var main = new MainWindow();
            main.Show();
            CloseWindow(obj);
        }

        private void Cancel(object? obj)
        {
            var main = new MainWindow();
            main.Show();
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
