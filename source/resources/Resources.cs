using System.Collections.Generic;
using Celesteia.Resources.Management;
using Celesteia.Resources.Types;
using Microsoft.Xna.Framework.Content;

namespace Celesteia.Resources {
    public static class ResourceManager {
        public static ItemManager Items = new ItemManager();
        public static BlockManager Blocks = new BlockManager();
        public static CraftingRecipes Recipes = new CraftingRecipes();
        public static EntityManager Entities = new EntityManager();
        public static FontTypes Fonts = new FontTypes();
        public static SkyboxAssets Skybox = new SkyboxAssets();

        public const float SPRITE_SCALING = 0.125f;
        public const float INVERSE_SPRITE_SCALING = 8f;

        public static void AddCollection(IResourceCollection collection) {
            Blocks.AddCollection(collection);
        }

        public static void LoadContent(ContentManager content) {
            Items.LoadContent(content);
            Blocks.LoadContent(content);
            Recipes.LoadContent(content);
            Entities.LoadContent(content);
            Fonts.LoadContent(content);
            Skybox.LoadContent(content);
        }
    }

    public struct NamespacedKey {
        public readonly string Namespace;
        public readonly string Index;

        public NamespacedKey(string ns, string index) {
            Namespace = ns;
            Index = index;
        }

        public static NamespacedKey Base(string index) {
            return new NamespacedKey("celesteia", index);
        }

        public string Qualify() {
            return $"{Namespace}:{Index}";
        }

        public override bool Equals(object obj)
        {
            return obj is NamespacedKey && ((NamespacedKey)obj).Namespace == Namespace && ((NamespacedKey)obj).Index == Index;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public interface IResourceType {
        public byte GetID();
        public void SetID(byte id);
    }

    public interface IResourceManager {
        public void AddCollection(IResourceCollection collection);
        public IResourceType GetResource(NamespacedKey namespacedKey);
    }

    public interface IResourceCollection {
        public Dictionary<NamespacedKey, BlockType> GetBlocks();
        public Dictionary<NamespacedKey, ItemType> GetItems();
        public NamespacedKey GetKey(string index);
    }
}