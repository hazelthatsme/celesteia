using System.Diagnostics;
using Celestia.GameInput;
using Celestia.Screens.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Celestia.Screens.Systems {
    public class LocalPlayerSystem : EntityUpdateSystem
    {
        private ComponentMapper<Transform2> transformMapper;
        private ComponentMapper<EntityAttributes> attributesMapper;
        private ComponentMapper<InputTest> inputMapper;

        public LocalPlayerSystem() : base(Aspect.All(typeof(Transform2), typeof(InputTest), typeof(LocalPlayer))) { }

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<Transform2>();
            attributesMapper = mapperService.GetMapper<EntityAttributes>();
            inputMapper = mapperService.GetMapper<InputTest>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (int entityId in ActiveEntities) {
                InputTest input = inputMapper.Get(entityId);
                EntityAttributes.EntityAttributeMap attributes = attributesMapper.Get(entityId).Attributes;
                transformMapper.Get(entityId).Position += new Vector2(
                    input.Definition.Test() * attributes.Get(EntityAttribute.MovementSpeed) * (gameTime.ElapsedGameTime.Milliseconds / 1000f),
                    0f
                );
            }
        }
    }
}