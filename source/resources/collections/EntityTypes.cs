using System;
using System.Collections.Generic;
using System.Diagnostics;
using Celesteia.Game.ECS;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;

namespace Celesteia.Resources.Collections {
    public class EntityTypes : IResourceCollection {
        public EntityType PLAYER;

        public List<EntityType> Types;
        private EntityType[] BakedTypes;
        private Dictionary<string, byte> keys = new Dictionary<string, byte>();

        public void LoadContent(ContentManager Content) {
            Debug.WriteLine($"Loading entity types...");

            Types = new List<EntityType>();

            AddEntity("player", "Player", (entity) => EntityFactory.BuildPlayer(entity, Content.Load<Texture2D>("sprites/entities/player/astronaut")));

            BakedTypes = Types.ToArray();
        }

        private void AddKey(NamespacedKey key, byte id) {
            keys.Add(key.Qualify(), id);
            Debug.WriteLine($"  Loading block '{key.Qualify()}' ({id})...");
        }

        private byte next;
        private byte AddType(string name, Action<Entity> instantiate) {
            Types.Add(new EntityType(next, name, instantiate));
            return next++;
        }

        private void AddEntity(string key, string name, Action<Entity> instantiate) {
            AddKey(NamespacedKey.Base(key), AddType(name, instantiate));
        }

        public IResourceType GetResource(NamespacedKey key) {
            if (!keys.ContainsKey(key.Qualify())) throw new NullReferenceException();
            return BakedTypes[keys[key.Qualify()]];
        }
    }

    public class EntityType : IResourceType {
        public readonly byte EntityID;
        public readonly string EntityName;
        private readonly Action<Entity> InstantiateAction;

        public EntityType(byte id, string name, Action<Entity> instantiate) {
            EntityID = id;
            EntityName = name;
            InstantiateAction = instantiate;

            Debug.WriteLine($"  Entity '{name}' ({id}) loaded.");
        }

        public void Instantiate(Entity entity) {
            InstantiateAction.Invoke(entity);
        }

        public byte GetID() => EntityID;
    }
}