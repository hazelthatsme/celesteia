using Celestia.Screens.Components;
using Celestia.Screens.Components.Entities.Player.Movement;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Celestia.Screens.Systems {
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
                transformMapper.Get(entityId).Position += new Vector2(
                    input.TestHorizontal(),
                    input.TestVertical()
                ) * attributes.Get(EntityAttribute.MovementSpeed) * (gameTime.ElapsedGameTime.Milliseconds / 1000f);
            }
        }
    }
}