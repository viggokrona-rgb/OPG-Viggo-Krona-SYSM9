using CookMaster.Core;
using CookMaster.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace CookMaster.Services
{
    public interface IRecipeManager
    {
        ObservableCollection<Recipe> Recipes { get; }
    }

    public sealed class RecipeManager : ObservableObject, IRecipeManager
    {
        private static readonly Lazy<RecipeManager> _instance = new(() => new RecipeManager());
        public static RecipeManager Instance => _instance.Value;

        public ObservableCollection<Recipe> Recipes { get; } = new();

        private RecipeManager()
        {
            // Seed with dummy recipes once at startup
            if (Recipes.Count == 0)
            {
                Recipes.Add(new Recipe { Title = "Spaghetti Bolognese", Ingredients = "Spaghetti, minced beef, tomato sauce, onion, garlic, olive oil, salt, pepper", Instructions = "Cook pasta. Brown beef with onion and garlic. Add sauce and simmer. Combine with pasta." });
                Recipes.Add(new Recipe { Title = "Chicken Curry", Ingredients = "Chicken, curry paste, coconut milk, onion, bell pepper, rice", Instructions = "Sauté onion and pepper, add chicken, then curry paste and coconut milk. Simmer. Serve with rice." });
                Recipes.Add(new Recipe { Title = "Vegetable Stir-fry", Ingredients = "Mixed vegetables, soy sauce, ginger, garlic, noodles", Instructions = "Stir-fry vegetables with garlic and ginger. Add soy sauce. Toss with cooked noodles." });
                Recipes.Add(new Recipe { Title = "Pancakes", Ingredients = "Flour, eggs, milk, sugar, butter", Instructions = "Mix batter, pour onto hot pan, flip when bubbles form, serve with toppings." });
                Recipes.Add(new Recipe { Title = "Caesar Salad", Ingredients = "Romaine lettuce, croutons, Parmesan, Caesar dressing, chicken (optional)", Instructions = "Toss lettuce with dressing, top with croutons, Parmesan, and optional chicken." });
            }
        }
    }
}

