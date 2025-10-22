using CookMaster.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace CookMaster.Services
{
    public  class RecipeManager
    {
        private static readonly ObservableCollection<Recipe> _recipes = new();

        public static ObservableCollection<Recipe> GetRecipes(string username, bool isAdmin)
        {
            return isAdmin
                ? _recipes
                : new ObservableCollection<Recipe>(_recipes.Where(r => r.Author == username));
        }

        public static void AddRecipe(Recipe recipe) => _recipes.Add(recipe);

        public static void RemoveRecipe(Recipe recipe)
        {
            if (_recipes.Contains(recipe))
                _recipes.Remove(recipe);
        }
    }
}

