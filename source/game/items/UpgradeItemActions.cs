using System;
using Celesteia.Game.Components;
using Celesteia.Game.Planets;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;

namespace Celesteia.Game.Items {
    public class UpgradeItemActions : CooldownItemActions {
        private float _increase;
        private EntityAttribute _attribute;
        private float _max;

        public UpgradeItemActions(EntityAttribute attribute, float increase, float max) {
            UseTime = 0.2;
            _attribute = attribute;
            _increase = increase;
            _max = max;
        }
        
        public override bool Primary(GameTime gameTime, ChunkMap chunkMap, Point cursor, Entity user) => Assert(gameTime, user) && Use(user);

        // Check if the conditions to use this item's action are met.
        public bool Assert(GameTime gameTime, Entity user) {
            if (!base.Assert(gameTime)) return false;

            // If the user has no attributes, the rest of the function will not work, so check if they're there first.
            if (!user.Has<EntityAttributes>()) return false;

            EntityAttributes.EntityAttributeMap attributes = user.Get<EntityAttributes>().Attributes;

            // If the attribute is maxed out, don't upgrade.
            if (attributes.Get(_attribute) >= _max) return false;

            UpdateLastUse(gameTime);
            
            return true;
        }


        public bool Use(Entity user) {
            // Use the upgrade.
            EntityAttributes.EntityAttributeMap attributes = user.Get<EntityAttributes>().Attributes;
            attributes.Set(_attribute, Math.Clamp(attributes.Get(_attribute) + _increase, 0f, _max));

            return true;
        }
    }
}