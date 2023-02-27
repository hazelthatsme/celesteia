using Celesteia.Resources;
using Celesteia.Game.Systems.MainMenu;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using Celesteia.Game.Skybox;
using Celesteia.Resources.Sprites;
using Celesteia.Game.Components.Player;
using MonoGame.Extended.TextureAtlases;
using Microsoft.Xna.Framework.Graphics;
using Celesteia.Game.Input;
using Celesteia.Game.Components;
using Microsoft.Xna.Framework.Input;
using Celesteia.Resources.Collections;

namespace Celesteia.Game.ECS {
    /*
        Contains various commonly used prefabrications for entities.
        Many of the functions were moved to EntityTypes.
    */

    public class EntityFactory {
        private readonly World World;
        private readonly GameInstance Game;

        public EntityFactory(World world, GameInstance game) {
            World = world;
            Game = game;
        }

        public Entity CreateSkyboxPortion(string name, Color color, float rotation, float depth)
        {
            return new EntityBuilder(World)
                .AddComponent(new Transform2(Vector2.Zero, 0F, new Vector2(3f, 3f)))
                .AddComponent(new SkyboxRotateZ(rotation))
                .AddComponent(ResourceManager.Skybox.GetAsset(name).Frames.Clone().SetColor(color).SetDepth(depth))
                .Build();
        }

        public Entity CreateEntity(EntityType type)
        {
            Entity entity = World.CreateEntity();
            type.Instantiate(entity);
            
            return entity;
        }
    }
}