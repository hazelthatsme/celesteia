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
    }
}