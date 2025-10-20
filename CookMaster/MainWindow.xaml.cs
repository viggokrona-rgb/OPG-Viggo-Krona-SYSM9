using System.Text;
using System.Windows;
using System.Windows.Controls;
using CookMaster.ViewModels;

namespace CookMaster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _vm;

        public MainWindow()
        {
            InitializeComponent();
            _vm = new MainViewModel();
            DataContext = _vm;
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            _vm.SignIn(PasswordBox.Password);
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
           
        }
    }
}