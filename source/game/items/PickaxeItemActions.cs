using Celesteia.Game.Components;
using Celesteia.Game.Components.Items;
using Celesteia.Game.Worlds;
using Celesteia.Resources;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;

namespace Celesteia.Game {
    public class PickaxeItemActions : ItemActions {
        private int _power;

        public PickaxeItemActions(int power) {
            UseTime = 0.2;
            _power = power;
        }
        
        public override bool OnLeftClick(GameTime gameTime, GameWorld world, Vector2 cursor, Entity user) {
            return Check(gameTime, world, cursor, user, false) && Break(world, cursor, user, false);
        }
        public override bool OnRightClick(GameTime gameTime, GameWorld world, Vector2 cursor, Entity user) {
            return Check(gameTime, world, cursor, user, true) && Break(world, cursor, user, true);
        }

        // Check if the conditions to use this item's action are met.
        public bool Check(GameTime gameTime, GameWorld world, Vector2 cursor, Entity user, bool forWall) {
            if (!CheckUseTime(gameTime)) return false;

            // If the user has no transform or attributes, the rest of the function will not work, so check if they're there first.
            if (!user.Has<Transform2>() || !user.Has<EntityAttributes>()) return false;

            Transform2 entityTransform = user.Get<Transform2>();
            EntityAttributes.EntityAttributeMap attributes = user.Get<EntityAttributes>().Attributes;

            // Check if the targeted location is within the entity's block manipulation range.
            if (Vector2.Distance(entityTransform.Position, cursor) > attributes.Get(EntityAttribute.BlockRange)) return false;

            byte id = forWall ? world.GetWallBlock(cursor) : world.GetBlock(cursor);

            // If there is no tile in the given location, the action will not continue.
            if (id == 0) return false;

            // If the block is unbreakable, don't break it. Duh.
            if (ResourceManager.Blocks.GetBlock(id).Strength < 0) return false;

            UpdateLastUse(gameTime);
            
            return true;
        }


        public bool Break(GameWorld world, Vector2 cursor, Entity user, bool wall) {
            // Add breaking progress accordingly.
            ItemStack drops = null;
            world.AddBreakProgress(cursor, _power, wall, out drops);

            if (drops != null && user.Has<EntityInventory>())
                user.Get<EntityInventory>().Inventory.AddItem(drops);

            return true;
        }
    }
}