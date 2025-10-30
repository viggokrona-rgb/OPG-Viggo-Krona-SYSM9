using CookMaster.Core;
using CookMaster.Managers;
using CookMaster.Model;
using CookMaster.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CookMaster.ViewModels
{
    public class AddRecipeViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;

        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _ingredients = string.Empty;
        public string Ingredients
        {
            get => _ingredients;
            set => SetProperty(ref _ingredients, value);
        }

        private string _instructions = string.Empty;
        public string Instructions
        {
            get => _instructions;
            set => SetProperty(ref _instructions, value);

        }

        private string _category = string.Empty;
        public string Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }

        private string _time = string.Empty;
        public string Time
        {
            get => _time;
            set => SetProperty(ref _time, value);
        }

        public ICommand SaveCommand  { get; }
        public ICommand CancelCommand { get; }

        public AddRecipeViewModel() : this(null) { }

        public AddRecipeViewModel(Recipe? template)
        {
            _navigationService = new NavigationService();
            SaveCommand = new RelayCommand(_ => Save(), _ => CanSave());
            CancelCommand = new RelayCommand(_ => _navigationService.ShowRecipesWindow());
            if (template != null)
            {
                Title = template.Title ?? string.Empty;
                Ingredients = template.Ingredients ?? string.Empty;
                Instructions = template.Instructions ?? string.Empty;
                Category = template.Category ?? string.Empty;
                Time = template.Time ?? string.Empty;
            }
        }
        private bool CanSave() => !string.IsNullOrWhiteSpace(Title) && !string.IsNullOrWhiteSpace(Ingredients) && !string.IsNullOrWhiteSpace(Instructions);

        private void Save()
        {
            if (UserManager.Instance.CurrentUser != null)
            {
                var recipe = new Recipe
                {
                    Title = Title,
                    Ingredients = Ingredients,
                    Instructions = Instructions,
                    CreatedBy = UserManager.Instance.CurrentUser
                };

                RecipeManager.Instance.Recipes.Add(recipe);
            }
            

            
            _navigationService.ShowRecipesWindow();
        }
    }
}
