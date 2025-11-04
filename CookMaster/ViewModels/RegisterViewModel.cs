using CookMaster.Core;
using CookMaster.Managers;
using CookMaster.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace CookMaster.ViewModels
{
    public class RegisterViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
       

        private string _username = string.Empty;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _country = string.Empty;
        public string Country
        {
            get => _country;
            set => SetProperty(ref _country, value);
        }

        private string _message = string.Empty;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);

        }

        private string _confirmPassword = string.Empty;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
            
                
            
        }
        public ICommand RegisterCommand { get; }
        public ICommand CancelCommand { get; }

        public RegisterViewModel()
        {
            _navigationService = new NavigationService();

            RegisterCommand = new RelayCommand(async _ => await RegisterAsync(), _ => CanRegister());
            CancelCommand = new RelayCommand(_ => _navigationService.ShowSignInWindow());
        }

        // Always enable registration for this simple example
        private bool CanRegister()
        {
            return true;
        }

        // Simple password complexity check
        private bool IsPasswordComplex(string pwd)
        {
            if (pwd.Length < 8) return false;
            bool hasDigit = false;
            bool hasSpecial = false;
            foreach (var c in pwd)
            {
                if (char.IsDigit(c)) hasDigit = true;
                if (!char.IsLetterOrDigit(c)) hasSpecial = true;
            }
            return hasDigit && hasSpecial;
        }

        // Perform registration
        private async Task RegisterAsync()
        {
            Message = string.Empty;

            if (Username.Length < 3)
            {
                Message = "Username must be at least 3 characters.";
                return;
            }

            if (!IsPasswordComplex(Password))
            {
                Message = "Password must be at least 8 characters and include at least one digit and one special character.";
                return;
            }

            if (!string.Equals(Password, ConfirmPassword))
            {
                Message = "Passwords do not match.";
                return;
            }

            var taken = await UserManager.Instance.IsUsernameTakenAsync(Username);
            if (taken)
            {
                Message = "Username is already taken.";
                return;
            }


            var user = new User { Username = Username, Password = Password, Country = Country };

            // Store user in UserManager temporarily and navigate back to sign-in
            UserManager.Instance.Users.Add(user);
            // In a real app, you would persist the user to a database
            _navigationService.ShowSignInWindow();
        }
    }
}

        


