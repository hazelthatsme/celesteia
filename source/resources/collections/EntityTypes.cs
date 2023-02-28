using System;
using System.Collections.Generic;
using System.Diagnostics;
using Celesteia.Game.ECS;
using Celesteia.Game.Components;
using Celesteia.Game.Components.Player;
using Celesteia.Game.Input;
using Celesteia.Resources.Sprites;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.TextureAtlases;

namespace Celesteia.Resources.Collections {
    public class EntityTypes {
        public EntityType PLAYER;

        public List<EntityType> Types;

        public void LoadContent(ContentManager Content) {
            Debug.WriteLine($"Loading entity types...");

            Types = new List<EntityType>();

            Types.Add(PLAYER = new EntityType(0, "Player",
                (entity) => {
                    entity.Attach(new Transform2());

                    entity.Attach(new EntityFrames(
                        TextureAtlas.Create("player", Content.Load<Texture2D>("sprites/entities/player/astronaut"), 24, 24),
                        0, 1,
                        ResourceManager.SPRITE_SCALING
                    ));

                    entity.Attach(new PhysicsEntity(1f, true));

                    entity.Attach(new PlayerMovement()
                        .AddHorizontal(new KeyDefinition(Keys.A, Keys.D))
                        .AddVertical(new KeyDefinition(Keys.W, Keys.S))
                        .SetRun(new KeyDefinition(null, Keys.LeftShift))
                    );

                    entity.Attach(new LocalPlayer());

                    entity.Attach(new CameraFollow());

                    entity.Attach(new EntityAttributes(new EntityAttributes.EntityAttributeMap()
                        .Set(EntityAttribute.MovementSpeed, 5f)
                    ));
                }
            ));
            
            Debug.WriteLine("Entities loaded.");
        }
    }

    public struct EntityType {
        public readonly byte EntityID;
        public readonly string EntityName;
        private readonly Action<Entity> InstantiateAction;

        public EntityType(byte id, string name, Action<Entity> instantiate) {
            EntityID = id;
            EntityName = name;
            InstantiateAction = instantiate;

            Debug.WriteLine($"  Entity '{name}' loaded.");
        }

        public void Instantiate(Entity entity) {
            InstantiateAction.Invoke(entity);
        }
    }
}