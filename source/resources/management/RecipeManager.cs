using System.Collections.Generic;
using System.Diagnostics;
using Celesteia.Resources.Types;
using Microsoft.Xna.Framework.Content;

namespace Celesteia.Resources.Management {
    public class RecipeManager : IResourceManager {        
        public List<Recipe> Recipes;

        private List<IResourceCollection> _collections = new List<IResourceCollection>();
        public void AddCollection(IResourceCollection collection) => _collections.Add(collection);

        public void LoadContent(ContentManager Content) {
            Debug.WriteLine($"Loading crafting recipes...");

            Recipes = new List<Recipe>();

            foreach (IResourceCollection collection in _collections)
                LoadCollection(collection);
        }

        private void LoadCollection(IResourceCollection collection) {
            foreach (NamespacedKey key in collection.GetRecipes().Keys) {
                AddType(collection.GetRecipes()[key]);
            }
        }

        private void AddType(Recipe recipe) {
            Recipes.Add(recipe);
        }

        public IResourceType GetResource(NamespacedKey namespacedKey) => null;
    }
}