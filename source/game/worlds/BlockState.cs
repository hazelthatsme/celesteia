using Celesteia.Resources;
using Celesteia.Resources.Types;

namespace Celesteia.Game.Worlds {
    public struct BlockState {
        private byte _id;
        public byte BlockID {
            get => _id;
            set {
                _id = value;
                _type = ResourceManager.Blocks.GetBlock(BlockID) as BlockType;
            }
        }

        private BlockType _type;
        public BlockType Type => _type;
        
        public int BreakProgress;

        public bool DoDraw() => _type.Frames != null;
    }
}