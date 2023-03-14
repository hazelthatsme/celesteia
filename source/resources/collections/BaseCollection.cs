using System.Collections.Generic;
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
        public Dictionary<NamespacedKey, BlockType> GetBlocks() => blocks;
        public Dictionary<NamespacedKey, ItemType> GetItems() => items;

        public Dictionary<NamespacedKey, BlockType> blocks;
        public Dictionary<NamespacedKey, ItemType> items;
        public void LoadContent(ContentManager Content) {
            LoadBlocks(Content);
        }

        private void LoadBlocks(ContentManager Content, int pixelSize = 8) {
            TextureAtlas _atlas = TextureAtlas.Create("blocks", Content.Load<Texture2D>("sprites/blocks"), pixelSize, pixelSize);
            BlockTypeBuilder builder = new BlockTypeBuilder(_atlas);
            
            blocks = new Dictionary<NamespacedKey, BlockType>();
            blocks.Add(GetKey("air"),
                builder.WithName("Air").WithTemplate(BlockTypeTemplate.Invisible).Get());
            blocks.Add(GetKey("grown_soil"),
                builder.WithName("Grown Soil").WithTemplate(BlockTypeTemplate.Full).SetFrames(0, 1).SetDrop(GetKey("soil")).SetStrength(3).Get());
            blocks.Add(GetKey("soil"),
                builder.WithName("Soil").WithTemplate(BlockTypeTemplate.Full).SetFrames(1, 1).SetDrop(GetKey("soil")).SetStrength(3).Get());
            blocks.Add(GetKey("stone"), 
                builder.WithName("Stone").WithTemplate(BlockTypeTemplate.Full).SetFrames(2, 1).SetDrop(GetKey("stone")).SetStrength(3).Get());
            blocks.Add(GetKey("deepstone"),
                builder.WithName("Deepstone").WithTemplate(BlockTypeTemplate.Full).SetFrames(3, 1).SetDrop(GetKey("deepstone")).SetStrength(-1).Get());
            blocks.Add(GetKey("log"),
                builder.WithName("Wooden Log").WithTemplate(BlockTypeTemplate.Full).SetFrames(10, 1).SetDrop(GetKey("log")).SetStrength(2).Get());
            blocks.Add(GetKey("leaves"),
                builder.WithName("Leaves").WithTemplate(BlockTypeTemplate.Walkthrough).SetFrames(11, 1).SetLightProperties(false, 0, LightColor.black).SetStrength(1).Get());
            blocks.Add(GetKey("iron_ore"),
                builder.WithName("Iron Ore").WithTemplate(BlockTypeTemplate.Full).SetFrames(8, 1).SetDrop(GetKey("iron_ore")).SetStrength(15).SetLightProperties(true, 3, new LightColor(63f, 63f, 63f)).Get());
            blocks.Add(GetKey("copper_ore"),
                builder.WithName("Copper Ore").WithTemplate(BlockTypeTemplate.Full).SetFrames(7, 1).SetDrop(GetKey("copper_ore")).SetStrength(10).SetLightProperties(true, 3, new LightColor(112f, 63f, 46f)).Get());
            blocks.Add(GetKey("coal_ore"),
                builder.WithName("Coal Ore").WithTemplate(BlockTypeTemplate.Full).SetFrames(14, 1).SetDrop(GetKey("coal")).SetStrength(10).Get());
            blocks.Add(GetKey("wooden_planks"),
                builder.WithName("Wooden Planks").WithTemplate(BlockTypeTemplate.Full).SetFrames(4, 1).SetDrop(GetKey("wooden_planks")).SetStrength(4).Get());
            blocks.Add(GetKey("torch"),
                builder.WithName("Torch").WithTemplate(BlockTypeTemplate.Walkthrough).SetFrames(9, 1).SetDrop(GetKey("torch")).SetLightProperties(false, 6, LightColor.white).SetTranslucent(true).Get());
        }

        public NamespacedKey GetKey(string index) => NamespacedKey.Base(index);
    }
}