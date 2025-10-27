using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookMaster.Managers;
using CookMaster.Model;



namespace CookMaster.Services
{
    public interface IAuthService
    {
        Task<bool> SignInAsync(string username, string password);
        Task<bool> ForgotPasswordAsync(string username);
    }

    public class AuthService : IAuthService
    {
        private readonly List<User> _users = new()
        {
            new User { Username = "admin", Password = "password", Country = "Sweden" },
            new User { Username = "alice", Password = "secret1", Country = "USA" },
            new User { Username = "bob", Password = "secret2", Country = "UK" },
        };

        public Task<bool> SignInAsync(string username, string password) 
        {
            var user = _users.FirstOrDefault(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase)
                                   && u.Password == password);

            UserManager.Instance.CurrentUser = user;

            return Task.FromResult(user != null);
        }

        public Task<bool> ForgotPasswordAsync(string username)
        {
            // In a real app you would send a reset link or similar
            var exists = _users.Any(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(exists);
        }
    }
}
