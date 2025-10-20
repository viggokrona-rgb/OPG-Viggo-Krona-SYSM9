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
      

        public MainWindow()
        {
            InitializeComponent();
     

           
            PasswordBox.PasswordChanged += (s, e) => {  };
        }

       
    }
}