using Celesteia.Game.Components;
using Celesteia.Game.Components.Player;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Celesteia.Game.Systems {
    public class LocalPlayerSystem : EntityUpdateSystem
    {
        private ComponentMapper<Transform2> transformMapper;
        private ComponentMapper<EntityAttributes> attributesMapper;
        private ComponentMapper<PlayerMovement> movementMapper;

        public LocalPlayerSystem() : base(Aspect.All(typeof(Transform2), typeof(PlayerMovement), typeof(LocalPlayer))) { }

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<Transform2>();
            attributesMapper = mapperService.GetMapper<EntityAttributes>();
            movementMapper = mapperService.GetMapper<PlayerMovement>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (int entityId in ActiveEntities) {
                PlayerMovement input = movementMapper.Get(entityId);
                EntityAttributes.EntityAttributeMap attributes = attributesMapper.Get(entityId).Attributes;

                Vector2 movement = new Vector2(
                    input.TestHorizontal() * (1f + (input.TestRun() * 1.5f)),
                    0f
                );
                movement *= attributes.Get(EntityAttribute.MovementSpeed);
                movement *= (gameTime.ElapsedGameTime.Milliseconds / 1000f);

                transformMapper.Get(entityId).Position += movement;
            }
        }
    }
}