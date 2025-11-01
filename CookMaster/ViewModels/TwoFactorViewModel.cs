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
    internal class TwoFactorViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;

        private string _username = string.Empty;
        private string _code = string.Empty;
        private string _expectedCode = string.Empty;
        private string _message = string.Empty;

        public string Code
        {
            get => _code;
            set => SetProperty(ref _code, value);
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public ICommand VerifyCommand { get; }
        public ICommand ResendCommand { get; }
        public ICommand CancelCommand { get; }

        public TwoFactorViewModel()
        {
            _navigationService = new NavigationService();
            
            VerifyCommand = new RelayCommand(_ =>  Verify());
            ResendCommand = new RelayCommand(_ => SendCode());
            CancelCommand = new RelayCommand(_ => _navigationService.ShowSignInWindow());
        }

        public void SetUsername()
        {           
            SendCode();
        }

        private void SendCode()
        {
            // Generate a 6-digit code and 'send' it (simulate)
            var rnd = new Random();
            _expectedCode = rnd.Next(0, 999999).ToString("D6");

            // In a real app you would send via email; here we just display in message for simulation
            Message = $"Verification code (simulated): {_expectedCode}";

            // Clear previous input
            Code = string.Empty;
        }

        private void Verify()
        {
            Message = string.Empty;
            if (string.IsNullOrWhiteSpace(Code) || Code.Length != 6)
            {
                Message = "Please enter the 6-digit code.";
                return;
            }

            if (Code != _expectedCode)
            {
                Message = "Invalid code.";
                return;
            }
 
            _navigationService.ShowRecipesWindow();
        }
    }
}
