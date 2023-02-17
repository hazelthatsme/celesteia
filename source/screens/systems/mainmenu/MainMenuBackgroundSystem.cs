using Celesteia.Graphics;
using Celesteia.Screens.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Celesteia.Screens.Systems.MainMenu {
    public class MainMenuBackgroundSystem : EntityUpdateSystem
    {
        private ComponentMapper<Transform2> transformMapper;
        private ComponentMapper<MainMenuRotateZ> rotatorMapper;

        public MainMenuBackgroundSystem() : base(Aspect.All(typeof(Transform2), typeof(MainMenuRotateZ))) {}

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<Transform2>();
            rotatorMapper = mapperService.GetMapper<MainMenuRotateZ>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (int entityId in ActiveEntities) {
                MainMenuRotateZ rotator = rotatorMapper.Get(entityId);
                Transform2 transform = transformMapper.Get(entityId);

                rotator.Current += rotator.Magnitude * (gameTime.GetElapsedSeconds() / 1000f) * 20f;

                transform.Rotation = rotator.Current;
            }
        }
    }
}