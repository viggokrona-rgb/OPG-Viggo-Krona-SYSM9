using CookMaster.Core;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CookMaster.Managers
{
    public interface IUserManager
    {
        User? CurrentUser { get; set; }
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
                new Admin { Username = "admin", Password = "password", Country = "Sweden" },
                new User { Username = "alice", Password = "secret1", Country = "USA" },
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
    }

}
