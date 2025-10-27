using CookMaster.Core;
using CookMaster.Managers;
using CookMaster.Services;
using CookMaster.Views;
using System;
using System.Windows;
using System.Windows.Input;


namespace CookMaster.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly IAuthService _authService;
        private readonly INavigationService _navigationService;

        private string _username = string.Empty;
        public string Username
        {
            get => _username;
            set
            {
                if (SetProperty(ref _username, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set
            {
                if (SetProperty(ref _password, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (SetProperty(ref _isBusy, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        private string _errorMessage = string.Empty;
        
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand SignInCommand { get; }
        public ICommand ForgotPasswordCommand { get; }

        public MainViewModel()
        {
            _authService = new AuthService();
            _navigationService = new NavigationService();

            SignInCommand = new RelayCommand(async _ => await SignInAsync(), _ => CanSignIn());
           

#if DEBUG
            // Prefill credentials in Debug builds to speed up testing
            Username = "admin";
            Password = "password";
#endif
        }

        private bool CanSignIn()
        {
            return !IsBusy && !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }

        private async Task SignInAsync()
        {
            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;
                bool ok = await _authService.SignInAsync(Username, Password);
                
                if (ok)
                {
                    _navigationService.ShowRecipesWindow();
                    
                }
                else
                {
                    ErrorMessage = "Ogiltigt användarnamn eller lösenord.";
                }


            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally
            {
                IsBusy = false;
            }


        }
    }
}

