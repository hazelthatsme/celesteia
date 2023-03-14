using Celesteia.Graphics.Lighting;
using Celesteia.Resources.Sprites;
using MonoGame.Extended;
using MonoGame.Extended.TextureAtlases;

namespace Celesteia.Resources.Types {
    public class BlockType : IResourceType {
        private byte id;
        public readonly string Name;
        public void SetID(byte value) => id = value;
        public byte GetID() => id;

        public BlockType(string name) {
            Name = name;
        }

        public BlockFrames Frames = null;
        public NamespacedKey? DropKey = null;
        public RectangleF? BoundingBox = new RectangleF(0f, 0f, 1f, 1f);
        public int Strength = 1;
        public bool Translucent = false;
        public BlockLightProperties Light = new BlockLightProperties();


        public BlockType MakeFrames(TextureAtlas atlas, int frameStart, int frameCount) {
            Frames = new BlockFrames(atlas, frameStart, frameCount);
            return this;
        }

        public BlockType AddDrop(NamespacedKey dropKey) {
            DropKey = dropKey;
            return this;
        }

        public BlockType SetBoundingBox(RectangleF boundingBox) {
            BoundingBox = boundingBox;
            return this;
        }

        public BlockType SetStrength(int strength) {
            Strength = strength;
            return this;
        }

        public BlockType SetLightProperties(BlockLightProperties properties) {
            Light = properties;
            if (Light == null) Light = new BlockLightProperties();
            return this;
        }

        public BlockType SetTranslucent(bool translucent) {
            Translucent = translucent;
            return this;
        }

        public ItemType GetDrops() => DropKey.HasValue ? ResourceManager.Items.GetResource(DropKey.Value) as ItemType : null;
    }

    public class BlockLightProperties {
        public readonly bool Emits = false;
        public readonly bool Occludes = true;
        public readonly int Propagation = 0;
        public readonly LightColor Color = LightColor.black;

        public BlockLightProperties() {}
        public BlockLightProperties(LightColor color, int propagation = 0, bool occludes = true) {
            Emits = !color.Equals(LightColor.black);
            Propagation = propagation;
            Occludes = occludes;
            Color = color;
        }
    }
}