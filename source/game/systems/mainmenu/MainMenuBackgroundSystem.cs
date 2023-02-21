using Celesteia.Game.Skybox;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Celesteia.Game.Systems.MainMenu {
    public class MainMenuBackgroundSystem : EntityUpdateSystem
    {
        private ComponentMapper<Transform2> transformMapper;
        private ComponentMapper<SkyboxRotateZ> rotatorMapper;

        public MainMenuBackgroundSystem() : base(Aspect.All(typeof(Transform2), typeof(SkyboxRotateZ))) {}

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<Transform2>();
            rotatorMapper = mapperService.GetMapper<SkyboxRotateZ>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (int entityId in ActiveEntities) {
                SkyboxRotateZ rotator = rotatorMapper.Get(entityId);
                Transform2 transform = transformMapper.Get(entityId);

                rotator.Current += rotator.Magnitude * (gameTime.GetElapsedSeconds() / 1000f) * 20f;

                transform.Rotation = rotator.Current;
            }
        }
    }
}