using MonoGame.Extended;
using MonoGame.Extended.TextureAtlases;

namespace Celesteia.Resources.Types.Builders {
    public class BlockTypeBuilder {
        private TextureAtlas _atlas;
        public BlockTypeBuilder(TextureAtlas atlas) {
            _atlas = atlas;
        }

        private BlockType current;
        public BlockTypeBuilder WithName(string name) {
            current = new BlockType(name);
            return this;
        }

        public BlockTypeBuilder Full() => WithTemplate(BlockTypeTemplate.Full);
        public BlockTypeBuilder Invisible() => WithTemplate(BlockTypeTemplate.Invisible);
        public BlockTypeBuilder Walkthrough() => WithTemplate(BlockTypeTemplate.Walkthrough);

        public BlockTypeBuilder WithTemplate(BlockTypeTemplate template) {
            current.Translucent = template.Translucent;
            current.BoundingBox = template.BoundingBox;
            current.DropKey = template.DropKey;
            current.SetLightProperties(template.LightProperties);
            current.Strength = template.Strength;

            return this;
        }

        public BlockTypeBuilder Frames(int start, int count = 1) {
            current.MakeFrames(_atlas, start, count);
            return this;
        }

        public BlockTypeBuilder Properties(bool translucent = false, int strength = 1, NamespacedKey? drop = null, BlockLightProperties light = null) {
            current.Translucent = translucent;
            current.Strength = strength;
            current.DropKey = drop;
            current.SetLightProperties(light);

            return this;
        }

        public BlockType Get() {
            return current;
        }
    }

    public class BlockTypeTemplate
        {
            public static BlockTypeTemplate Invisible = new BlockTypeTemplate(null, null, 0, true);
            public static BlockTypeTemplate Full = new BlockTypeTemplate(new RectangleF(0f, 0f, 1f, 1f), null, 1, false, null);
            public static BlockTypeTemplate Walkthrough = new BlockTypeTemplate(null);

            public RectangleF? BoundingBox;
            public NamespacedKey? DropKey;
            public int Strength;
            public bool Translucent;
            public BlockLightProperties LightProperties;

            public BlockTypeTemplate(
                RectangleF? boundingBox = null,
                NamespacedKey? dropKey = null,
                int strength = 1,
                bool translucent = false,
                BlockLightProperties lightProperties = null
            ) {
                BoundingBox = boundingBox;
                DropKey = dropKey;
                Strength = strength;
                Translucent = translucent;
                LightProperties = lightProperties;
            }
        }
}