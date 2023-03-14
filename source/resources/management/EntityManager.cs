using System;
using System.Collections.Generic;
using System.Diagnostics;
using Celesteia.Resources.Types;
using Microsoft.Xna.Framework.Content;

namespace Celesteia.Resources.Management {
    public class EntityManager : IResourceManager {
        public List<EntityType> Types;
        private EntityType[] BakedTypes;
        private Dictionary<string, byte> keys = new Dictionary<string, byte>();

        private List<IResourceCollection> _collections = new List<IResourceCollection>();
        public void AddCollection(IResourceCollection collection) => _collections.Add(collection);

        public void LoadContent(ContentManager Content) {
            Debug.WriteLine($"Loading entity types...");

            Types = new List<EntityType>();

            foreach (IResourceCollection collection in _collections)
                LoadCollection(collection);

            BakedTypes = Types.ToArray();
        }

        private void LoadCollection(IResourceCollection collection) {
            foreach (NamespacedKey key in collection.GetEntities().Keys) {
                AddType(key, collection.GetEntities()[key]);
            }
        }

        private byte next = 0;
        private void AddType(NamespacedKey key, EntityType type) {
            type.SetID(next++);
            keys.Add(key.Qualify(), type.GetID());

            Types.Add(type);
        }

        public IResourceType GetResource(NamespacedKey key) {
            if (!keys.ContainsKey(key.Qualify())) throw new NullReferenceException();
            return BakedTypes[keys[key.Qualify()]];
        }
    }
}