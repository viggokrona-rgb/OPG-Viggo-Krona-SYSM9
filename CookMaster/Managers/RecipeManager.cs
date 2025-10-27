using CookMaster.Core;
using CookMaster.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace CookMaster.Services
{
    public class RecipeManager
    {
        private static readonly ObservableCollection<Recipe> _recipes = new();

        public static ObservableCollection<Recipe> GetRecipes(string username, bool isAdmin)
        {
            if (_recipes.Count == 0)
            {
                _recipes.Add(new Recipe { Title = "Spaghetti Bolognese", Ingredients = "Spaghetti, minced beef, tomato sauce, onion, garlic, olive oil, salt, pepper", Instructions = "Cook pasta. Brown beef with onion and garlic. Add sauce and simmer. Combine with pasta." });
                _recipes.Add(new Recipe { Title = "Chicken Curry", Ingredients = "Chicken, curry paste, coconut milk, onion, bell pepper, rice", Instructions = "Sauté onion and pepper, add chicken, then curry paste and coconut milk. Simmer. Serve with rice." });
                _recipes.Add(new Recipe { Title = "Vegetable Stir-fry", Ingredients = "Mixed vegetables, soy sauce, ginger, garlic, noodles", Instructions = "Stir-fry vegetables with garlic and ginger. Add soy sauce. Toss with cooked noodles." });
                _recipes.Add(new Recipe { Title = "Pancakes", Ingredients = "Flour, eggs, milk, sugar, butter", Instructions = "Mix batter, pour onto hot pan, flip when bubbles form, serve with toppings." });
                _recipes.Add(new Recipe { Title = "Caesar Salad", Ingredients = "Romaine lettuce, croutons, Parmesan, Caesar dressing, chicken (optional)", Instructions = "Toss lettuce with dressing, top with croutons, Parmesan, and optional chicken." });
            }

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

