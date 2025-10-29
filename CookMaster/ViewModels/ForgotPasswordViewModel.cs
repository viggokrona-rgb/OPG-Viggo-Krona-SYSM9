using CookMaster.Core;
using CookMaster.Managers;
using CookMaster.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CookMaster.ViewModels
{
    internal class ForgotPasswordViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        private string _username = string.Empty;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string _message = string.Empty;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public ICommand SubmitCommand { get; }
        public ICommand BackCommand { get; }

        public ForgotPasswordViewModel()
        {
            _navigationService = new NavigationService();
           
            SubmitCommand = new RelayCommand(async _ => await SubmitAsync(), _ => CanSubmit());
            BackCommand = new RelayCommand(_ => _navigationService.ShowSignInWindow());
        }

        private bool CanSubmit() => !string.IsNullOrWhiteSpace(Username);

        private async Task SubmitAsync()
        {
            Message = string.Empty;
            var exists = await UserManager.Instance.ForgotPasswordAsync(Username);
            if (exists)
            {
                // Navigate to full reset flow where user answers security question
                _navigationService.ShowResetPasswordWindow(Username);
            }
            else
            {
                Message = "User not found.";
            }
        }
    }
}
