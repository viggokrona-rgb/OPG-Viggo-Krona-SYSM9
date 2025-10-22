using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CookMaster.Managers
{
    public class UserManager : INotifyPropertyChanged
    {
        private List<User> _users;

        public UserManager()
        {
            
            _users = new List<User>();
            CreateDefaultUsers();


        }

        private void CreateDefaultUsers()
        {
            _users.Add(new Admin
            {
                Username = "admin",
                Password = "password",
                Country = "Sweden"
            });

            _users.Add(new User
            {

                Username = "user",
                Password = "password",
                Country = "Sweden"
            });
        }




        private User? _currentUser;

        public User? CurrentUser
        {
            get { return _currentUser; }
            set
            {
                _currentUser = value;
                OnPropertyChanged();

            }



        }






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


        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool Login(string username, string password)
        {
          

            var user = _users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                CurrentUser = user;
                IsLoggedIn = true;
                return true;
            }
            return false;




        }

        public void Logout()
        {
            CurrentUser = null;
            IsLoggedIn = false;
        }
    }
}
