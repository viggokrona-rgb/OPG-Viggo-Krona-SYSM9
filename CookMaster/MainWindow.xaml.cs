using CookMaster.Managers;
using CookMaster.Services;
using CookMaster.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;

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

            DataContext = new MainViewModel(new UserManager(), new RecipeManager());
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm && sender is System.Windows.Controls.PasswordBox pb)
            {
                vm.Password = pb.Password;
            }
        }
    }
}