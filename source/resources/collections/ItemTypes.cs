using System;
using System.Collections.Generic;
using System.Diagnostics;
using Celesteia.Game;
using Celesteia.Game.Components;
using Celesteia.Game.Components.Items;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace Celesteia.Resources.Collections {
    public abstract class ItemSpriteProperties {
        public const int SIZE = 16; 
    }

    public class ItemTypes {        
        private List<ItemType> Types;
        private ItemType[] BakedTypes;
        private Dictionary<string, byte> keys = new Dictionary<string, byte>();

        public void LoadContent(ContentManager Content) {
            Debug.WriteLine($"Loading item types...");

            Types = new List<ItemType>();

            TextureAtlas atlas = TextureAtlas.Create("items",
                Content.Load<Texture2D>("sprites/items"),
                ItemSpriteProperties.SIZE,
                ItemSpriteProperties.SIZE
            );

            AddBlockItem("stone", "Stone", atlas.GetRegion(4), NamespacedKey.Base("stone"), "Tough as rock.");
            AddBlockItem("soil", "Soil", atlas.GetRegion(3), NamespacedKey.Base("soil"), "Rich in nutrients.");
            AddBlockItem("grown_soil", "Grown Soil", atlas.GetRegion(2), NamespacedKey.Base("grown_soil"), "Grassy.");
            AddBlockItem("deepstone", "Deepstone", atlas.GetRegion(5), NamespacedKey.Base("deepstone"), "Tougher than rock.");
            AddBlockItem("log", "Wooden Log", atlas.GetRegion(11), NamespacedKey.Base("log"), "Good for making planks.");
            AddBlockItem("iron_ore", "Iron Ore", atlas.GetRegion(10), NamespacedKey.Base("iron_ore"), "Contains iron.");
            AddBlockItem("copper_ore", "Copper Ore", atlas.GetRegion(9), NamespacedKey.Base("copper_ore"), "Contains copper.");
            AddIngredientItem("coal", "Coal", atlas.GetRegion(14), "A primitive fuel source.");
            AddToolItem("iron_pickaxe", "Iron Pickaxe", atlas.GetRegion(33), 4, "Used to harvest blocks.");
            AddBlockItem("wooden_planks", "Wooden Planks", atlas.GetRegion(6), NamespacedKey.Base("wooden_planks"), "Great for building.");
            AddIngredientItem("plank", "Plank", atlas.GetRegion(15), "Good for making blocks.");
            AddIngredientItem("copper_ingot", "Copper Ingot", atlas.GetRegion(17), "Conducts currents.");
            AddIngredientItem("iron_ingot", "Iron Ingot", atlas.GetRegion(18), "Strong and sturdy.");
            AddUpgradeItem("jetpack_tank", "Jetpack Tank", atlas.GetRegion(16), "Upgrades jetpack fuel.", EntityAttribute.JumpFuel, .1f, 1f);
            AddBlockItem("torch", "Torch", atlas.GetRegion(1), NamespacedKey.Base("torch"), "Light it up!");

            BakedTypes = Types.ToArray();
        }

        private void AddKey(NamespacedKey key, byte id) {
            keys.Add(key.Qualify(), id);
            Debug.WriteLine($"  Loading block '{key.Qualify()}' ({id})...");
        }

        byte next = 0;
        private byte AddType(string name, string lore, TextureRegion2D sprite, ItemActions actions, bool consumeOnUse, int maxStack) {
            Types.Add(new ItemType(next, name, lore, sprite, actions, consumeOnUse, maxStack));
            return next++;
        }

        private void AddBlockItem(string key, string name, TextureRegion2D sprite, NamespacedKey blockKey, string lore) {
            AddKey(NamespacedKey.Base(key), AddType(name, lore, sprite, new BlockItemActions(blockKey), true, 99));
        }

        private void AddToolItem(string key, string name, TextureRegion2D sprite, int power, string lore) {
            AddKey(NamespacedKey.Base(key), AddType(name, lore, sprite, new PickaxeItemActions(power), false, 1));
        }

        private void AddUpgradeItem(string key, string name, TextureRegion2D sprite, string lore, EntityAttribute attribute, float increase, float max) {
            AddKey(NamespacedKey.Base(key), AddType(name, lore, sprite, new UpgradeItemActions(increase, attribute, max), true, 1));
        }

        private void AddIngredientItem(string key, string name, TextureRegion2D sprite, string lore) {
            AddKey(NamespacedKey.Base(key), AddType(name, lore, sprite, null, true, 99));
        }

        public ItemType GetItem(byte id) {
            return Types.Find(x => x.ItemID == id);
        }

        public ItemType GetItem(string name) {
            return Types.Find(x => x.Name == name);
        }

        public IResourceType GetResource(NamespacedKey key) {
            if (!keys.ContainsKey(key.Qualify())) throw new NullReferenceException();
            return BakedTypes[keys[key.Qualify()]];
        }
    }

    public class ItemType : IResourceType {
        public readonly byte ItemID;
        public readonly string Name;
        public readonly string Lore;
        public readonly TextureRegion2D Sprite;
        public readonly int MaxStackSize;
        public ItemActions Actions;
        public readonly bool ConsumeOnUse;

        public ItemType(byte id, string name, string lore, TextureRegion2D sprite, ItemActions actions, bool consumeOnUse, int maxStack) {
            Debug.WriteLine($"  Loading item '{name}' ({id})...");

            ItemID = id;
            Name = name;
            Lore = lore;
            Sprite = sprite;
            Actions = actions;
            MaxStackSize = maxStack;
            ConsumeOnUse = consumeOnUse;
        }

        public ItemStack GetStack(int amount) {
            return new ItemStack(ItemID, amount);
        }

        public byte GetID() => ItemID;
    }
}