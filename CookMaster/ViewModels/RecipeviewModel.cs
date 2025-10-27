using CookMaster.Core;
using CookMaster.Managers;
using CookMaster.Model;
using CookMaster.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace CookMaster.ViewModels
{
    public class RecipeViewModel : ObservableObject
    {
        private Recipe? _selectedRecipe;
        private ObservableCollection<Recipe> recipes;

        public string Username { get; }

        public bool IsAdmin { get; }

       

        public Recipe? SelectedRecipe
        {
            get => _selectedRecipe;
            set => SetProperty(ref _selectedRecipe, value);
        }
        public ObservableCollection<Recipe> Recipes => RecipeManager.Instance.Recipes;

        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand DetailsCommand { get; }
        public ICommand InfoCommand { get; }
        public ICommand SignOutCommand { get; }
        public ICommand UserCommand { get; }

        public RecipeViewModel()
        {
           // Username = username;
            IsAdmin = true; // enkel admin-check           
            AddCommand = new RelayCommand(_ => AddRecipe());
            RemoveCommand = new RelayCommand(_ => RemoveRecipe());
            DetailsCommand = new RelayCommand(_ => ShowDetails());
            InfoCommand = new RelayCommand(_ => ShowInfo());
            SignOutCommand = new RelayCommand(window => SignOut(window as Window));
            UserCommand = new RelayCommand(_ => OpenUserDetails());
        }

        private void AddRecipe()
        {
            //var addWindow = new AddRecipeWindow(Username);
            //addWindow.Show();
            Application.Current.Windows[0]?.Close(); // stäng RecipeListWindow
        }

        private void RemoveRecipe()
        {
            if (SelectedRecipe == null)
            {
                MessageBox.Show("Vänligen välj ett recept att ta bort.");
                return;

            }
            //RecipeManager.RemoveRecipe(SelectedRecipe);
            Recipes.Remove(SelectedRecipe);
        }

        private void ShowDetails()
        {
            if (SelectedRecipe == null)
            {
                MessageBox.Show("Vänligen välj ett recept för att se detaljer.");
                return;
            }
           // var details = new RecipeDetailsWindow(SelectedRecipe);
            //details.Show();
        }

        private void ShowInfo()
        {
            MessageBox.Show("CookMaster hjälper dig att skapa, visa och hantera recept.\n\n" +
                "• Add Recipe: Lägg till nya recept\n" +
                "• Details: Visa receptdetaljer\n" +
                "• Remove: Ta bort markerat recept\n\n" +
                "CookMaster AB © 2025",
                "Om CookMaster",
                 MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SignOut(Window? window)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            window?.Close();
        }

        private void OpenUserDetails()
        {
            //var userDetailsWindow = new UserDetailsWindow(Username);
            //userDetailsWindow.Show();
        }



    }
}
