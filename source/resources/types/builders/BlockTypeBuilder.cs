using Celesteia.Graphics.Lighting;
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

        public BlockTypeBuilder WithTemplate(BlockTypeTemplate template) {
            current.MakeFrames(_atlas, template.StartFrame, template.FrameCount);
            current.BoundingBox = template.BoundingBox;
            current.DropKey = template.DropKey;
            current.SetLightProperties(template.LightProperties);
            current.Strength = template.Strength;

            return this;
        }

        public BlockTypeBuilder SetFrames(int start, int count) {
            current.MakeFrames(_atlas, start, count);
            return this;
        }

        public BlockTypeBuilder SetBoundingBox(RectangleF boundingBox) {
            current.BoundingBox = boundingBox;
            return this;
        }

        public BlockTypeBuilder SetDrop(NamespacedKey itemKey) {
            current.DropKey = itemKey;
            return this;
        }

        public BlockTypeBuilder SetStrength(int strength) {
            current.Strength = strength;
            return this;
        }

        public BlockTypeBuilder SetTranslucent(bool translucent) {
            current.Translucent = translucent;
            return this;
        }

        public BlockTypeBuilder SetLightProperties(BlockLightProperties lightProperties) {
            current.SetLightProperties(lightProperties);
            return this;
        }

        public BlockTypeBuilder SetLightProperties(bool occludes, int propagation, LightColor color) {
            current.SetLightProperties(new BlockLightProperties(color, propagation, occludes));
            return this;
        }

        public BlockType Get() {
            return current;
        }
    }

    public class BlockTypeTemplate
        {
            public static BlockTypeTemplate Invisible = new BlockTypeTemplate(0, 0, null, null, 0, true);
            public static BlockTypeTemplate Full = new BlockTypeTemplate(0, 1, new RectangleF(0f, 0f, 1f, 1f), null, 1, false, null);
            public static BlockTypeTemplate Walkthrough = new BlockTypeTemplate(0, 1, new RectangleF(0f, 0f, 1f, 1f));

            public int StartFrame;
            public int FrameCount;
            public RectangleF? BoundingBox;
            public NamespacedKey? DropKey;
            public int Strength;
            public bool Translucent;
            public BlockLightProperties LightProperties;

            public BlockTypeTemplate(
                int start = 0,
                int frameCount = 1,
                RectangleF? boundingBox = null,
                NamespacedKey? dropKey = null,
                int strength = 1,
                bool translucent = false,
                BlockLightProperties lightProperties = null
            ) {
                StartFrame = start;
                FrameCount = frameCount;
                BoundingBox = boundingBox;
                DropKey = dropKey;
                Strength = strength;
                Translucent = translucent;
                LightProperties = lightProperties;
            }
        }
}