using CookMaster.Core;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CookMaster.Managers
{
    public interface IUserManager
    {
        User? CurrentUser { get; set; }

        Task<bool> UpdateUserAsync(string originalUsername, User updatedUser);

        Task<User?> GetUserByUsernameAsync(string username);

        Task<bool> IsUsernameTakenAsync(string username);
        Task<bool> ValidateSecurityAnswerAsync(string username, string answer);
        Task<bool> ResetPasswordAsync(string username, string newPassword);
    }

    public sealed class UserManager : ObservableObject, IUserManager
    {
        private static readonly Lazy<UserManager> _instance = new(() => new UserManager());
        public static UserManager Instance => _instance.Value;

        private User? _currentUser;
        public User? CurrentUser
        {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
        }

        private UserManager()
        {
            Users = new()
            {
                new Admin { Username = "admin", Password = "password", Country = "Sweden",SecurityQuestion ="Vad är min favorit färg?",SecurityAnswer ="blå" },
                new User { Username = "user", Password = "password", Country = "USA", SecurityQuestion = "Vilken Kurs studerar jag?", SecurityAnswer = "OPG"},
                new User { Username = "bob", Password = "secret2", Country = "UK" },
            };
        }


        public List<User> Users { get; set; }

        public Task<bool> SignInAsync(string username, string password)
        {
            var user = Users.FirstOrDefault(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase)
                                   && u.Password == password);

            UserManager.Instance.CurrentUser = user;

            return Task.FromResult(user != null);
        }

        public Task<bool> ForgotPasswordAsync(string username)
        {
            // In a real app you would send a reset link or similar
            var exists = Users.Any(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(exists);
        }

        public Task<bool> UpdateUserAsync(string originalUsername, User updatedUser)
        {
            if (updatedUser == null || string.IsNullOrWhiteSpace(originalUsername)) return Task.FromResult(false);

            var existing = Users.FirstOrDefault(u => string.Equals(u.Username, originalUsername, StringComparison.OrdinalIgnoreCase));
            if (existing == null) return Task.FromResult(false);

            // If username changed, ensure new username isn't taken by another user
            if (!string.Equals(originalUsername, updatedUser.Username, StringComparison.OrdinalIgnoreCase))
            {
                var taken = Users.Any(u => string.Equals(u.Username, updatedUser.Username, StringComparison.OrdinalIgnoreCase));
                if (taken) return Task.FromResult(false);
            }

            existing.Username = updatedUser.Username;
            existing.Password = updatedUser.Password;
            existing.Country = updatedUser.Country;

            return Task.FromResult(true);
        }

        public Task<bool> IsUsernameTakenAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return Task.FromResult(false);
            var taken = Users.Any(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(taken);
        }

        public Task<User?> GetUserByUsernameAsync(string username)
        {
            var user = Users.FirstOrDefault(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(user);
        }
        public Task<bool> ValidateSecurityAnswerAsync(string username, string answer)
        {
            var user = Users.FirstOrDefault(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));
            if (user == null) return Task.FromResult(false);
            var ok = string.Equals(user.SecurityAnswer ?? string.Empty, answer ?? string.Empty, StringComparison.OrdinalIgnoreCase);
            return Task.FromResult(ok);
        }

        public Task<bool> ResetPasswordAsync(string username, string newPassword)
        {
            var user = Users.FirstOrDefault(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));
            if (user == null) return Task.FromResult(false);
            user.Password = newPassword;
            return Task.FromResult(true);
        }
    }

}
