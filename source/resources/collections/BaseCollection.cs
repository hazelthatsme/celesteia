using System.Collections.Generic;
using Celesteia.Game;
using Celesteia.Game.Components;
using Celesteia.Game.ECS;
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
        private Dictionary<NamespacedKey, BlockType> blocks;
        private Dictionary<NamespacedKey, BlockType> LoadBlocks(int pixelSize = 8) {
            if (blocks != null) return blocks;

            void AddBlock(string index, BlockType type) => blocks.Add(GetKey(index), type);

            TextureAtlas _atlas = TextureAtlas.Create("blocks", _content.Load<Texture2D>("sprites/blocks"), pixelSize, pixelSize);
            BlockTypeBuilder builder = new BlockTypeBuilder(_atlas);
            
            blocks = new Dictionary<NamespacedKey, BlockType>();
            AddBlock("air", builder.WithName("Air").Invisible().Get());
            AddBlock("grown_soil", builder.WithName("Grown Soil").Full().Frames(0).Properties(
                strength: 5, 
                drop: GetKey("soil")
            ).Get());
            AddBlock("soil", builder.WithName("Soil").Full().Frames(1).Properties(
                strength: 5, 
                drop: GetKey("soil")
            ).Get());
            AddBlock("stone", builder.WithName("Stone").Full().Frames(2).Properties(
                strength: 7, 
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
                light: new BlockLightProperties(new LightColor(63f, 63f, 63f), 1, true)
            ).Get());
            AddBlock("copper_ore", builder.WithName("Copper Ore").Full().Frames(7).Properties(
                strength: 10, 
                drop: GetKey("copper_ore"),
                light: new BlockLightProperties(new LightColor(112f, 63f, 46f), 1, true)
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
                drop: GetKey("wooden_torch"),
                light: new BlockLightProperties(LightColor.white, 6, false)
            ).Get());
            AddBlock("test_tile_entity", builder.WithName("Test Tile Entity").Frames(5).Walkthrough().Properties(
                translucent: true,
                strength: -1
            ).Get());
            AddBlock("stone_bricks", builder.WithName("Stone Bricks").Frames(6).Full().Properties(
                strength: 7, 
                drop: GetKey("stone_bricks")
            ).Get());
            AddBlock("unyx_bricks", builder.WithName("Unyx Bricks").Frames(12).Full().Properties(
                strength: 10,
                drop: GetKey("unyx_bricks")
            ).Get());
            AddBlock("unyx_eyestone", builder.WithName("Unyx Eyestone").Frames(13).Full().Properties(
                strength: 10,
                drop: GetKey("unyx_eyestone"),
                light: new BlockLightProperties(new LightColor(230f, 74f, 255f), 5, true)
            ).Get());

            return blocks;
        }

        public Dictionary<NamespacedKey, ItemType> GetItems() => LoadItems();
        private Dictionary<NamespacedKey, ItemType> items;
        private Dictionary<NamespacedKey, ItemType> LoadItems(int pixelSize = 16) {
            if (items != null) return items;
            
            void AddItem(string index, ItemType type) => items.Add(GetKey(index), type);

            TextureAtlas _atlas = TextureAtlas.Create("items", _content.Load<Texture2D>("sprites/items"), pixelSize, pixelSize);
            ItemTypeBuilder builder = new ItemTypeBuilder(_atlas);
            
            items = new Dictionary<NamespacedKey, ItemType>();
            if (blocks != null) {
                foreach (KeyValuePair<NamespacedKey, BlockType> pair in blocks) {
                    if (pair.Value.Frames == null) continue;
                    AddItem(pair.Key.Index, builder.WithName(pair.Value.Name).Block(pair.Key).Frame(pair.Value.Frames.GetFrame(0).GetRegion()).Get());
                }
            }
            
            AddItem("iron_pickaxe", builder.WithName("Iron Pickaxe").Pickaxe(4).Frame(0).Get());
            AddItem("wooden_log", builder.WithName("Wooden Log").Frame(1).Block(NamespacedKey.Base("log")).Get());
            AddItem("coal", builder.WithName("Coal Lump").Frame(2).Get());
            AddItem("plank", builder.WithName("Plank").Frame(3).Get());
            AddItem("copper_ingot", builder.WithName("Copper Ingot").Frame(4).Get());
            AddItem("iron_ingot", builder.WithName("Iron Ingot").Frame(5).Get());
            AddItem("fuel_tank", builder.WithName("Fuel Tank").Frame(6).Upgrade(EntityAttribute.JumpFuel, 0.5f, 5f).Get());
            AddItem("wooden_torch", builder.WithName("Wooden Torch").Template(new ItemTypeTemplate(1000, true))
                .Frame(7).Actions(new TorchItemActions(NamespacedKey.Base("torch"))).Get());
            AddItem("tile_entity", builder.WithName("Test Tile Entity").Template(new ItemTypeTemplate(1, false))
                .Frame(8).Actions(new TileEntityItemActions(NamespacedKey.Base("test_tile_entity"))).Get());

            return items;
        }

        public Dictionary<NamespacedKey, Recipe> GetRecipes() => LoadRecipes();
        private Dictionary<NamespacedKey, Recipe> recipes;
        private Dictionary<NamespacedKey, Recipe> LoadRecipes() {
            if (recipes != null) return recipes;
            
            void AddRecipe(string index, Recipe type) => recipes.Add(GetKey(index), type);
            
            recipes = new Dictionary<NamespacedKey, Recipe>();
            AddRecipe("plank_ingredient", new Recipe(new Part(GetKey("plank"), 4), new Part(GetKey("log"), 1)));
            AddRecipe("plank_block", new Recipe(new Part(GetKey("wooden_planks"), 1), new Part(GetKey("plank"), 2)));
            AddRecipe("copper_smelt", new Recipe(new Part(GetKey("copper_ingot"), 1), new Part(GetKey("copper_ore"), 1)));
            AddRecipe("iron_smelt", new Recipe(new Part(GetKey("iron_ingot"), 1), new Part(GetKey("iron_ore"), 1)));
            AddRecipe("fuel_tank", new Recipe(new Part(GetKey("fuel_tank"), 1), new Part(GetKey("iron_ingot"), 10), new Part(GetKey("copper_ingot"), 5)));
            AddRecipe("torches", new Recipe(new Part(GetKey("wooden_torch"), 1), new Part(GetKey("plank"), 1), new Part(GetKey("coal"), 1)));
            AddRecipe("stone_brick", new Recipe(new Part(GetKey("stone_bricks"), 4), new Part(GetKey("stone"), 4)));

            return recipes;
        }
        
        public Dictionary<NamespacedKey, EntityType> GetEntities() => LoadEntities();
        private Dictionary<NamespacedKey, EntityType> entities;
        private Dictionary<NamespacedKey, EntityType> LoadEntities() {
            if (entities != null) return entities;
            
            void AddEntity(string index, EntityType type) => entities.Add(GetKey(index), type);
            
            entities = new Dictionary<NamespacedKey, EntityType>();
            AddEntity("player", new EntityType((e) => EntityFactory.BuildPlayer(e, _content.Load<Texture2D>("sprites/entities/player/astronaut"))));

            return entities;
        }
    }
}