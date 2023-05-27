using Celesteia.Resources;
using Celesteia.Resources.Sprites;
using Celesteia.Resources.Types;

namespace Celesteia.Game.Worlds {
    public struct BlockState {
        private byte _id;
        public byte BlockID {
            get => _id;
            set {
                _id = value;
                Type = ResourceManager.Blocks.GetBlock(BlockID) as BlockType;

                Translucent = Type.Translucent;
                Frames = Type.Frames;
            }
        }

        public BlockType Type { get; private set; }
        public bool Translucent { get; private set; }
        public BlockFrames Frames { get; private set; }
        
        public int BreakProgress;

        public bool Draw;
        public bool HasFrames() => Frames != null;
    }
}