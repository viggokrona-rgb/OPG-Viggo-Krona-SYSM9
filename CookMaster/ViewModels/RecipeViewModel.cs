using CookMaster.Core;
using CookMaster.Managers;
using CookMaster.Model;
using CookMaster.Services;
using CookMaster.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Collections.Specialized;

namespace CookMaster.ViewModels
{
    public class RecipeViewModel : ObservableObject
    {
        private Recipe? _selectedRecipe;
        private ObservableCollection<Recipe> _recipes = new();
        private string username;

        public string Username
        {

            get => username;
            set => SetProperty(ref username, value);
        }


        public bool IsAdmin { get; }

        private string _selectedCategory = string.Empty;
        public string SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
        }

        private ObservableCollection<Recipe> _filteredRecipes = new();
        public ObservableCollection<Recipe> FilteredRecipes
        {
            get => _filteredRecipes;
            set => SetProperty(ref _filteredRecipes, value);
        }

        private string _filterText = string.Empty;
        public string FilterText
        {
            get => _filterText;
            set => SetProperty(ref _filterText, value);
        }

        private bool _sortByDateDesc = true;
        public bool SortByDateDesc
        {
            get => _sortByDateDesc;
            set => SetProperty(ref _sortByDateDesc, value);
        }

        public ICommand ApplyFilterCommand { get; }
        public ICommand ClearFilterCommand { get; }

        // Get distinct categories from recipes for filtering
        public IEnumerable<string> AvailableCategories => Recipes.Select(r => r.Category).Where(t => !string.IsNullOrWhiteSpace(t)).Distinct();



        public Recipe? SelectedRecipe
        {
            get => _selectedRecipe;
            set => SetProperty(ref _selectedRecipe, value);
        }

        public ObservableCollection<Recipe> Recipes
        {
            get => _recipes;
            set
            {
                if (SetProperty(ref _recipes, value))
                {
                    // ensure filtering whenever the collection changes
                    ApplyFilter();
                    _recipes.CollectionChanged -= Recipes_CollectionChanged;
                    _recipes.CollectionChanged += Recipes_CollectionChanged;
                }
            }
        }

        // React to changes in the Recipes collection
        private void Recipes_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            ApplyFilter();
        }

        private readonly INavigationService _navigationService;

        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand DetailsCommand { get; }
        public ICommand InfoCommand { get; }
        public ICommand SignOutCommand { get; }
        public ICommand UserCommand { get; }
        public ICommand CopyRecipeCommand { get; }

        public RecipeViewModel()
        {
            _navigationService = new NavigationService();
            Username = UserManager.Instance.CurrentUser?.Username ?? string.Empty;

            IsAdmin = true; // Simple admin-check           
            AddCommand = new RelayCommand(_ => _navigationService.ShowAddRecipeWindow());
            RemoveCommand = new RelayCommand(_ => RemoveRecipe());
            DetailsCommand = new RelayCommand(_ => ShowDetails());
            InfoCommand = new RelayCommand(_ => ShowInfo());
            SignOutCommand = new RelayCommand(window => SignOut(window as Window));
            UserCommand = new RelayCommand(_ => _navigationService.ShowUserDetailsWindow());
            CopyRecipeCommand = new RelayCommand(_ => _navigationService.ShowAddRecipeWindow(SelectedRecipe), _ => SelectedRecipe != null);


            var recipes = RecipeManager.Instance.Recipes;
            // If the user is admin, show all recipes, otherwise filter by current user
            if (UserManager.Instance.CurrentUser is Admin)
            {
                Recipes = new ObservableCollection<Recipe>(recipes);
            }
            else
            {
                Recipes = new ObservableCollection<Recipe>(recipes.Where(x => x.CreatedBy?.Username == Username));
            }

            ApplyFilterCommand = new RelayCommand(_ => ApplyFilter());
            ClearFilterCommand = new RelayCommand(_ => ClearFilter());

            // Initialize filtered list
            ApplyFilter();

            // ensure we react to adds/removes on the manager collection as well
            RecipeManager.Instance.Recipes.CollectionChanged += (s, e) =>
            {
                // recreate local Recipes collection to reflect current user view
                if (UserManager.Instance.CurrentUser is Admin)
                {
                    Recipes = new ObservableCollection<Recipe>(RecipeManager.Instance.Recipes);
                }
                else
                {
                    Recipes = new ObservableCollection<Recipe>(RecipeManager.Instance.Recipes.Where(x => x.CreatedBy?.Username == Username));
                }
            };
        }

        // Remove selected recipe
        private void RemoveRecipe()
        {
            if (SelectedRecipe == null)
            {
                MessageBox.Show("Vänligen välj ett recept att ta bort.");
                return;

            }

            Recipes.Remove(SelectedRecipe);
        }
        // Show details of selected recipe
        private void ShowDetails()
        {
            if (SelectedRecipe == null)
            {
                MessageBox.Show("Vänligen välj ett recept för att se detaljer.");
                return;
            }
            var details = new RecipeDetailsWindow(SelectedRecipe);
            details.Show();
            Application.Current.MainWindow?.Close();
            Application.Current.MainWindow = details;


        }
        // Show application info
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
        // Sign out and return to main window
        private void SignOut(Window? window)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            window?.Close();
        }

        // Apply current filters to the recipe list
        private void ApplyFilter()
        {
            var query = Recipes.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(FilterText))
            {
                var ft = FilterText.Trim().ToLowerInvariant();
                query = query.Where(r => (r.Title ?? string.Empty).ToLowerInvariant().Contains(ft) || (r.Ingredients ?? string.Empty).ToLowerInvariant().Contains(ft));
            }

            if (!string.IsNullOrWhiteSpace(SelectedCategory))
            {
                query = query.Where(r => string.Equals(r.Category, SelectedCategory, StringComparison.OrdinalIgnoreCase));
            }

            if (SortByDateDesc)
                query = query.OrderByDescending(r => r.DateCreated);
            else
                query = query.OrderBy(r => r.DateCreated);

            FilteredRecipes = new ObservableCollection<Recipe>(query);
            // Notify that AvailableCategories might have changed
            OnPropertyChanged(nameof(AvailableCategories));
        }
        // Clear all filters
        private void ClearFilter()
        {
            FilterText = string.Empty;
            SelectedCategory = string.Empty;
            SortByDateDesc = true;
            ApplyFilter();
        }

    }
}
