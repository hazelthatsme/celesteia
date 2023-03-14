using System.Collections.Generic;
using Celesteia.Game;
using Celesteia.Game.Components;
using Celesteia.Graphics.Lighting;
using Celesteia.Resources.Types;
using Celesteia.Resources.Types.Builders;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

/*
    A collection of resources for the base game.
*/
namespace Celesteia.Resources.Collections {
    public class BaseCollection : IResourceCollection
    {
        public NamespacedKey GetKey(string index) => NamespacedKey.Base(index);

        private ContentManager _content;
        public BaseCollection(ContentManager Content) {
            _content = Content;
        }

        public Dictionary<NamespacedKey, BlockType> GetBlocks() => LoadBlocks();
        public Dictionary<NamespacedKey, ItemType> GetItems() => LoadItems();

        private Dictionary<NamespacedKey, BlockType> blocks;
        private Dictionary<NamespacedKey, BlockType> LoadBlocks(int pixelSize = 8) {
            if (blocks != null) return blocks;

            void AddBlock(string index, BlockType type) => blocks.Add(GetKey(index), type);

            TextureAtlas _atlas = TextureAtlas.Create("blocks", _content.Load<Texture2D>("sprites/blocks"), pixelSize, pixelSize);
            BlockTypeBuilder builder = new BlockTypeBuilder(_atlas);
            
            blocks = new Dictionary<NamespacedKey, BlockType>();
            AddBlock("air", builder.WithName("Air").Invisible().Get());
            AddBlock("grown_soil", builder.WithName("Grown Soil").Full().Frames(0).Properties(
                strength: 3, 
                drop: GetKey("soil")
            ).Get());
            AddBlock("soil", builder.WithName("Soil").Full().Frames(1).Properties(
                strength: 3, 
                drop: GetKey("soil")
            ).Get());
            AddBlock("stone", builder.WithName("Stone").Full().Frames(2).Properties(
                strength: 5, 
                drop: GetKey("stone")
            ).Get());
            AddBlock("deepstone", builder.WithName("Deepstone").Full().Frames(3).Properties(
                strength: -1, 
                drop: GetKey("deepstone")
            ).Get());
            AddBlock("log", builder.WithName("Wooden Log").Full().Frames(10).Properties(
                strength: 2,
                drop: GetKey("log")
            ).Get());
            AddBlock("leaves", builder.WithName("Leaves").Walkthrough().Frames(11).Properties(
                strength: 1,
                light: new BlockLightProperties(LightColor.black, 0, false)
            ).Get());
            AddBlock("iron_ore", builder.WithName("Iron Ore").Full().Frames(8).Properties(
                strength: 15, 
                drop: GetKey("iron_ore"),
                light: new BlockLightProperties(new LightColor(63f, 63f, 63f), 3, true)
            ).Get());
            AddBlock("copper_ore", builder.WithName("Copper Ore").Full().Frames(7).Properties(
                strength: 10, 
                drop: GetKey("copper_ore"),
                light: new BlockLightProperties(new LightColor(112f, 63f, 46f), 3, true)
            ).Get());
            AddBlock("coal_ore", builder.WithName("Coal Ore").Full().Frames(14).Properties(
                strength: 10, 
                drop: GetKey("coal")
            ).Get());
            AddBlock("wooden_planks", builder.WithName("Wooden Planks").Full().Frames(4).Properties(
                strength: 4, 
                drop: GetKey("wooden_planks")
            ).Get());
            AddBlock("torch", builder.WithName("Torch").Walkthrough().Frames(9).Properties(
                translucent: true, 
                strength: 1, 
                drop: GetKey("torch"),
                light: new BlockLightProperties(LightColor.white, 6, false)
            ).Get());

            return blocks;
        }

        private Dictionary<NamespacedKey, ItemType> items;
        private Dictionary<NamespacedKey, ItemType> LoadItems(int pixelSize = 16) {
            if (items != null) return items;
            
            void AddItem(string index, ItemType type) => items.Add(GetKey(index), type);

            TextureAtlas _atlas = TextureAtlas.Create("items", _content.Load<Texture2D>("sprites/items"), pixelSize, pixelSize);
            ItemTypeBuilder builder = new ItemTypeBuilder(_atlas);
            
            items = new Dictionary<NamespacedKey, ItemType>();
            if (blocks != null) {
                foreach (KeyValuePair<NamespacedKey, BlockType> pair in blocks) {
                    AddItem(pair.Key.Index, builder.WithName(pair.Value.Name).Block(pair.Key).Frame(pair.Value.Frames.GetFrame(0).GetRegion()).Get());
                }
            }
            
            AddItem("iron_pickaxe", builder.WithName("Iron Pickaxe").Pickaxe(4).Frame(1).Get());
            AddItem("wooden_log", builder.WithName("Wooden Log").Frame(1).Block(NamespacedKey.Base("log")).Get());
            AddItem("coal", builder.WithName("Coal Lump").Frame(2).Get());
            AddItem("plank", builder.WithName("Plank").Frame(3).Get());
            AddItem("copper_ingot", builder.WithName("Copper Ingot").Frame(4).Get());
            AddItem("iron_ingot", builder.WithName("Iron Ingot").Frame(5).Get());
            AddItem("fuel_tank", builder.WithName("Fuel Tank").Frame(6).Upgrade(EntityAttribute.JumpFuel, 0.5f, 5f).Get());
            AddItem("wooden_torch", builder.WithName("Wooden Torch").Frame(7).Actions(new TorchItemActions(NamespacedKey.Base("torch"))).Get());

            return items;
        }
    }
}