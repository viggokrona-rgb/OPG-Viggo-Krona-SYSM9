using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CookMaster.Managers
{
    public class UserManager : INotifyPropertyChanged
    {
        private bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            private set
            {
                if (_isLoggedIn == value) return;
                _isLoggedIn = value;
                OnPropertyChanged();
            }
        }

        private string? _currentUsername;
        public string? CurrentUsername
        {
            get => _currentUsername;
            private set
            {
                if (_currentUsername == value) return;
                _currentUsername = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool Login(string username, string password)
        {
            // TODO: Replace with real authentication logic.
            // For demonstration, accept any non-empty username/password.
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                IsLoggedIn = false;
                CurrentUsername = null;
                return false;
            }

            CurrentUsername = username;
            IsLoggedIn = true;
            return true;
        }

        public void Logout()
        {
            CurrentUsername = null;
            IsLoggedIn = false;
        }
    }
}
