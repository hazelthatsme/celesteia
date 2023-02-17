using System;
using System.Collections.Generic;
using System.Diagnostics;
using Celesteia.GameInput;
using Celesteia.Resources.Sprites;
using Celesteia.Screens.Components;
using Celesteia.Screens.Components.Entities.Player.Movement;
using Celesteia.Utilities.ECS;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.TextureAtlases;

namespace Celesteia.Resources.Types {
    public class EntityTypes {
        public List<EntityType> Types;

        public void LoadContent(ContentManager Content) {
            Debug.WriteLine($"Loading entity types...");

            Types = new List<EntityType>();

            Types.Add(new EntityType(0, "Player", TextureAtlas.Create("player", Content.Load<Texture2D>("sprites/entities/player/astronaut"), 24, 24), 
                (world, atlas) => { new EntityBuilder(world)
                    .AddComponent(new Transform2())
                    .AddComponent(new EntityFrames(atlas, 0, 1, ResourceManager.SPRITE_SCALING))
                    .AddComponent(new PlayerMovement()
                        .AddHorizontal(new KeyDefinition(Keys.A, Keys.D))
                        .AddVertical(new KeyDefinition(Keys.W, Keys.S))
                    )
                    .AddComponent(new LocalPlayer())
                    .AddComponent(new CameraFollow())
                    .AddComponent(new EntityAttributes(new EntityAttributes.EntityAttributeMap()
                        .Set(EntityAttribute.MovementSpeed, 5f)
                    ))
                    .Build();
                }
            ));
            
            Debug.WriteLine("Entities loaded.");
        }
    }

    public struct EntityType {
        public readonly byte EntityID;
        public readonly string EntityName;
        public readonly TextureAtlas Atlas;
        private readonly Action<World, TextureAtlas> InstantiateAction;

        public EntityType(byte id, string name, TextureAtlas atlas, Action<World, TextureAtlas> instantiate) {
            EntityID = id;
            EntityName = name;
            Atlas = atlas;
            InstantiateAction = instantiate;

            Debug.WriteLine($"  Entity '{name}' loaded.");
        }

        public void Instantiate(World world) {
            InstantiateAction.Invoke(world, Atlas);
        }
    }
}