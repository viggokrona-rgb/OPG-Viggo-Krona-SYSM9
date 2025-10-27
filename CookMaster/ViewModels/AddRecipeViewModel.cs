using CookMaster.Core;
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

        public ICommand SaveCommand  { get; }
        public ICommand CancelCommand { get; }

        public AddRecipeViewModel()
        {
            _navigationService = new NavigationService();
            SaveCommand = new RelayCommand(_ => Save(), _ => CanSave());
            CancelCommand = new RelayCommand(_ => _navigationService.ShowRecipesWindow());

        }
        private bool CanSave() => !string.IsNullOrWhiteSpace(Title);

        private void Save()
        {
            var recipe = new Recipe
            {
                Title = Title,
                Ingredients = Ingredients,
                Instructions = Instructions
            };

            RecipeManager.Instance.Recipes.Add(recipe);
            _navigationService.ShowRecipesWindow();
        }
    }
}
