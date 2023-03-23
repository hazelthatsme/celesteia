using Celesteia.Game.Worlds;
using Celesteia.Resources.Types;
using Microsoft.Xna.Framework;

public class TileEntity {
    public readonly Point Position;
    private TileEntityType _type;

    public TileEntity(int x, int y, TileEntityType type) {
        Position = new Point(x, y);
        _type = type;
    }

    public bool CheckIfBroken(GameWorld world) {
        for (int i = 0; i < _type.Bounds.X; i++) {
            for (int j = 0; j < _type.Bounds.Y; j++) {
                if (world.GetBlock(Position.X - _type.Origin.X + i, Position.Y - _type.Origin.Y + j) == 0) return true;
            }
        }

        return false;
    }
}