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
    internal class ResetPasswordViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;

        private string _username = string.Empty;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string _securityQuestion = string.Empty;
        public string SecurityQuestion
        {
            get => _securityQuestion;
            set => SetProperty(ref _securityQuestion, value);
        }

        private string _securityAnswer = string.Empty;
        public string SecurityAnswer
        {
            get => _securityAnswer;
            set => SetProperty(ref _securityAnswer, value);
        }

        private string _newPassword = string.Empty;
        public string NewPassword
        {
            get => _newPassword;
            set => SetProperty(ref _newPassword, value);
        }

        private string _confirmNewPassword = string.Empty;
        public string ConfirmNewPassword
        {
            get => _confirmNewPassword;
            set => SetProperty(ref _confirmNewPassword, value);
        }

        private string _message = string.Empty;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        private bool _isSecurityStepVisible;
        public bool IsSecurityStepVisible
        {
            get => _isSecurityStepVisible;
            set => SetProperty(ref _isSecurityStepVisible, value);
        }

        private bool _isResetStepVisible;
        public bool IsResetStepVisible
        {
            get => _isResetStepVisible;
            set => SetProperty(ref _isResetStepVisible, value);
        }

        public ICommand LookupCommand { get; }
        public ICommand ValidateAnswerCommand { get; }
        public ICommand ResetPasswordCommand { get; }

        public ResetPasswordViewModel()
        {
            _navigationService = new NavigationService();
            

            LookupCommand = new RelayCommand(async _ => await LookupAsync());
            ValidateAnswerCommand = new RelayCommand(async _ => await ValidateAnswerAsync());
            ResetPasswordCommand = new RelayCommand(async _ => await ResetPasswordAsync());

            IsSecurityStepVisible = false;
            IsResetStepVisible = false;
        }

        // Lookup the user and load the security question
        private async Task LookupAsync()
        {
            Message = string.Empty;
            IsSecurityStepVisible = false;
            IsResetStepVisible = false;

            var user = await UserManager.Instance.GetUserByUsernameAsync(Username);
            if (user == null)
            {
                Message = "User not found.";
                return;
            }

            SecurityQuestion = user.SecurityQuestion ?? string.Empty;
            IsSecurityStepVisible = true;
        }

        // Validate the security answer
        private async Task ValidateAnswerAsync()
        {
            Message = string.Empty;
            var ok = await UserManager.Instance.ValidateSecurityAnswerAsync(Username, SecurityAnswer);
            if (!ok)
            {
                Message = "Incorrect answer.";
                return;
            }

            IsResetStepVisible = true;
        }

        // Reset the password
        private async Task ResetPasswordAsync()
        {
            Message = string.Empty;

            if (string.IsNullOrWhiteSpace(NewPassword) || NewPassword.Length < 8)
            {
                Message = "Password must be at least 8 characters.";
                return;
            }

            if (!string.Equals(NewPassword, ConfirmNewPassword))
            {
                Message = "Passwords do not match.";
                return;
            }

            var ok = await UserManager.Instance.ResetPasswordAsync(Username, NewPassword);
            if (ok)
            {
                _navigationService.ShowSignInWindow();
            }
            else
            {
                Message = "Failed to reset password.";
            }
        }
    }
}
