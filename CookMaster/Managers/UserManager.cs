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

        private UserManager() { }
    }
}
