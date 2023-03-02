using System.Diagnostics;
using Celesteia.Game.Components;
using Celesteia.Game.Components.Items;
using Celesteia.Game.Components.Player;
using Celesteia.Game.Input;
using Celesteia.Game.Worlds;
using Celesteia.Graphics;
using Celesteia.Resources;
using Celesteia.Resources.Collections;
using Celesteia.Resources.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Input;

namespace Celesteia.Game.Systems {
    public class MouseClickSystem : EntityUpdateSystem
    {
        private readonly Camera2D _camera;
        private readonly GameWorld _world;

        private ComponentMapper<EntityInventory> entityInventoryMapper;

        public MouseClickSystem(Camera2D camera, GameWorld world) : base(Aspect.All(typeof(LocalPlayer), typeof(EntityInventory))) {
            _camera = camera;
            _world = world;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            entityInventoryMapper = mapperService.GetMapper<EntityInventory>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (int entityId in ActiveEntities) {
                EntityInventory inventory = entityInventoryMapper.Get(entityId);

                if (MouseWrapper.GetMouseHeld(MouseButton.Left)) {
                    Vector2 point = _camera.ScreenToWorld(MouseWrapper.GetPosition());
                    BlockType type = ResourceManager.Blocks.GetBlock(_world.GetBlock(point));
                    if (type.Item != null) {
                        bool couldAdd = inventory.Inventory.AddItem(new ItemStack(type.Item.ItemID, 1));
                        inventory.Inventory.DebugOutput();
                        if (!couldAdd) Debug.WriteLine("Inventory full!");
                    }
                    _world.RemoveBlock(point);
                }
            }
        }
    }
}