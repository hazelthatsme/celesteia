using System.Diagnostics;
using Celesteia.Game.Components;
using Celesteia.Game.Planets;
using Celesteia.Resources;
using Celesteia.Resources.Types;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;

namespace Celesteia.Game.Items {
    public class TileEntityItemActions : CooldownItemActions {
        private NamespacedKey _entityKey;
        private TileEntityType _tileEntity = null;

        public TileEntityItemActions(NamespacedKey entityKey) {
            UseTime = 0.2;
            _entityKey = entityKey;
        }
        
        private void TryQualify() {
            _tileEntity = new TileEntityType(NamespacedKey.Base("test_tile_entity"), (e) => {}, 3, 3, 1, 2);
            //_tileEntity = ResourceManager.TileEntities.GetResource(_entityKey) as TileEntityType;
        }
        
        public override bool Primary(GameTime gameTime, ChunkMap chunkMap, Point cursor, Entity user) {
            TryQualify();
            return Assert(gameTime, chunkMap, cursor, user, false) && Place(chunkMap, cursor, false);
        }

        public virtual bool Assert(GameTime gameTime, ChunkMap chunkMap, Point cursor, Entity user, bool forWall) {
            if (!base.Assert(gameTime)) return false;
            if (_tileEntity == null) return false;

            if (!user.Has<Transform2>() || !user.Has<EntityAttributes>()) return false;

            Transform2 entityTransform = user.Get<Transform2>();
            EntityAttributes.EntityAttributeMap attributes = user.Get<EntityAttributes>().Attributes;

            if (Vector2.Distance(entityTransform.Position, cursor.ToVector2()) > attributes.Get(EntityAttribute.BlockRange)) return false;

            for (int i = 0; i < _tileEntity.Bounds.X; i++) {
                for (int j = 0; j < _tileEntity.Bounds.Y; j++) {
                    if (chunkMap.GetForeground(cursor.X - _tileEntity.Origin.X + i, cursor.Y - _tileEntity.Origin.Y - j) != 0) {
                        Debug.WriteLine($"Block found at {cursor.X - _tileEntity.Origin.X + i}, {cursor.Y - _tileEntity.Origin.Y - j}");
                        return false;
                    } else Debug.WriteLine($"No block found at {cursor.X - _tileEntity.Origin.X + i}, {cursor.Y - _tileEntity.Origin.Y - j}");
                }
            }

            /*if (!world.GetAnyBlock(cursor + new Vector2(-1, 0), true) && 
                !world.GetAnyBlock(cursor + new Vector2(1, 0), true) &&
                !world.GetAnyBlock(cursor + new Vector2(0, -1), true) && 
                !world.GetAnyBlock(cursor + new Vector2(0, 1), true)) {
                if (!forWall && world.GetWallBlock(cursor) == 0) return false;
                else if (forWall && world.GetBlock(cursor) == 0) return false;
            }*/
            
            base.UpdateLastUse(gameTime);
            
            return true;
        }

        public bool Place(ChunkMap chunkMap, Point cursor, bool wall) {
            Point pos = cursor - _tileEntity.Origin;
            BlockType part = ResourceManager.Blocks.GetResource(_tileEntity.PartKey) as BlockType;

            for (int i = 0; i < _tileEntity.Bounds.X; i++)
                for (int j = 0; j < _tileEntity.Bounds.Y; j++) {
                    chunkMap.SetForeground(cursor.X - _tileEntity.Origin.X + i, cursor.Y - _tileEntity.Origin.Y + j, part.GetID());
                    Debug.WriteLine($"Set block at at {cursor.X - _tileEntity.Origin.X + i}, {cursor.Y - _tileEntity.Origin.Y - j}");
                }
            
            return true;
        }
    }
}