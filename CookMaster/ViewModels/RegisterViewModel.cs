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

        public ICommand RegisterCommand { get; }
        public ICommand CancelCommand { get; }

        public RegisterViewModel()
        {
            _navigationService = new NavigationService();

            RegisterCommand = new RelayCommand(async _ => await RegisterAsync(), _ => CanRegister());
            CancelCommand = new RelayCommand(_ => _navigationService.ShowSignInWindow());
        }

        private bool CanRegister()
        {
            return !string.IsNullOrWhiteSpace(Username)
                && !string.IsNullOrWhiteSpace(Password);
        }

        private async Task RegisterAsync()
        {
            Message = string.Empty;

            
            var user = new User { Username = Username, Password = Password, Country = Country };

            // Store user in UserManager temporarily and navigate back to sign-in
            UserManager.Instance.Users.Add(user);

            _navigationService.ShowSignInWindow();
        }
    }
}

        


