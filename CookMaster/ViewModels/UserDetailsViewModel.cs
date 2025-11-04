using CookMaster.Core;
using CookMaster.Managers;
using CookMaster.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CookMaster.ViewModels
{
    public class UserDetailsViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        public UserDetailsViewModel()
        {
            _navigationService = new NavigationService();


            SaveCommand = new RelayCommand(async _ => await SaveAsync());
            CancelCommand = new RelayCommand(_ => _navigationService.ShowRecipesWindow());

            // Load initial data
            var current = UserManager.Instance.CurrentUser;
            CurrentUsername = current?.Username ?? string.Empty;
            CurrentCountry = current?.Country ?? string.Empty;

            NewUsername = CurrentUsername;
            SelectedCountry = CurrentCountry;
            // Predefined list of countries
            AvailableCountries = new ObservableCollection<string>(new[] { "Sweden", "USA", "UK", "Germany", "France", "Norway" });
        }

        private string _currentUsername = string.Empty;
        public string CurrentUsername
        {
            get => _currentUsername;
            set => SetProperty(ref _currentUsername, value);
        }

        private string _currentCountry = string.Empty;
        public string CurrentCountry
        {
            get => _currentCountry;
            set => SetProperty(ref _currentCountry, value);
        }

        private string _newUsername = string.Empty;
        public string NewUsername
        {
            get => _newUsername;
            set => SetProperty(ref _newUsername, value);
        }

        private string _newPassword = string.Empty;
        public string NewPassword
        {
            get => _newPassword;
            set => SetProperty(ref _newPassword, value);
        }

        private string _confirmPassword = string.Empty;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        private ObservableCollection<string> _availableCountries = new();
        public ObservableCollection<string> AvailableCountries
        {
            get => _availableCountries;
            set => SetProperty(ref _availableCountries, value);
        }

        private string _selectedCountry = string.Empty;
        public string SelectedCountry
        {
            get => _selectedCountry;
            set => SetProperty(ref _selectedCountry, value);
        }

        private string _message = string.Empty;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        // Validate user inputs
        private bool ValidateInputs(out string validationMessage)
        {
            validationMessage = string.Empty;

            // Username validation
            if (!string.IsNullOrWhiteSpace(NewUsername) && NewUsername.Length < 3)
            {
                validationMessage = "Username must be at least 3 characters.";
                return false;
            }

            // Check if username is taken (only if changed)
            if (!string.Equals(CurrentUsername, NewUsername, System.StringComparison.OrdinalIgnoreCase))
            {
                   var takenTask = UserManager.Instance.IsUsernameTakenAsync(NewUsername);
                 takenTask.Wait();
                if (takenTask.Result)
                {
                    validationMessage = "Username is already taken.";
                     return false;
                }
            }

            // Password validation - only if user provided something
            if (!string.IsNullOrEmpty(NewPassword) || !string.IsNullOrEmpty(ConfirmPassword))
            {
                if (NewPassword.Length < 5)
                {
                    validationMessage = "Password must be at least 5 characters.";
                    return false;
                }

                if (!string.Equals(NewPassword, ConfirmPassword))
                {
                    validationMessage = "Passwords do not match.";
                    return false;
                }
            }

            return true;
        }

        // Save the updated user details
        private async Task SaveAsync()
        {
            Message = string.Empty;

            if (!ValidateInputs(out var validationMessage))
            {
                Message = validationMessage;
                return;
            }

            var original = CurrentUsername;

            var updated = new User
            {
                Username = NewUsername,
                Password = string.IsNullOrEmpty(NewPassword) ? UserManager.Instance.CurrentUser?.Password : NewPassword,
                Country = SelectedCountry
            };

            var ok = await UserManager.Instance.UpdateUserAsync(original, updated);
            if (!ok)
            {
                Message = "Failed to update user (username may be taken).";
                return;
            }

            // Close window and go back to recipes
            _navigationService.ShowRecipesWindow();
        }
    }

}
