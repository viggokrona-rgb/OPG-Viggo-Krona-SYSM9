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
    internal class RecipeDetailsViewModel : ObservableObject
    {

        private readonly INavigationService _navigationService;
        private readonly Recipe _recipe;

        public RecipeDetailsViewModel(Recipe recipe)
        {
            _navigationService = new NavigationService();
            _recipe = recipe;

            // Initialize fields from recipe
            Title = recipe.Title;
            Ingredients = recipe.Ingredients;
            Instructions = recipe.Instructions;
            Category = recipe.Category ?? string.Empty;
            Time = recipe.Time ?? string.Empty;

            IsEditing = false;

            BackCommand = new RelayCommand(_ => _navigationService.ShowRecipesWindow());
            EditCommand = new RelayCommand(_ => BeginEdit(), _ => !IsEditing);
            SaveCommand = new RelayCommand(_ => Save(), _ => IsEditing);
            CancelCommand = new RelayCommand(_ => CancelEdit(), _ => IsEditing);
        }

        public ICommand BackCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

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

        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                if (SetProperty(ref _isEditing, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                    // Also notify that IsReadOnly changed
                    OnPropertyChanged(nameof(IsReadOnly));
                }
            }

        }

        public bool IsReadOnly => !IsEditing;

        private string _message = string.Empty;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        private void BeginEdit()
        {
            Message = string.Empty;
            IsEditing = true;
        }

        private void CancelEdit()
        {
            // Revert changes
            Title = _recipe.Title;
            Ingredients = _recipe.Ingredients;
            Instructions = _recipe.Instructions;
            Category = _recipe.Category ?? string.Empty;
            Time = _recipe.Time ?? string.Empty;
            Message = string.Empty;
            IsEditing = false;
        }

        private void Save()
        {
            Message = string.Empty;

            // Validation
            if (string.IsNullOrWhiteSpace(Title))
            {
                Message = "Title cannot be empty.";
                return;
            }

            if (!string.IsNullOrWhiteSpace(Time))
            {
                if (!int.TryParse(Time, out var t) || t < 0)
                {
                    Message = "Time must be a non-negative integer (minutes).";
                    return;
                }
            }

            // Save to underlying recipe
            _recipe.Title = Title;
            _recipe.Ingredients = Ingredients;
            _recipe.Instructions = Instructions;
            _recipe.Category = string.IsNullOrWhiteSpace(Category) ? null : Category;
            _recipe.Time = string.IsNullOrWhiteSpace(Time) ? null : Time;

            // Optionally update any managers or collections if needed - the Recipe object is updated in place

            IsEditing = false;
            Message = "Saved.";
        }
    }

}