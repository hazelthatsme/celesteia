using System;
using System.Collections.Generic;
using System.Diagnostics;
using Celesteia.Resources.Types;
using Microsoft.Xna.Framework.Content;

namespace Celesteia.Resources.Management {

    public class ItemManager : IResourceManager {    
        private List<ItemType> Types;
        private ItemType[] BakedTypes;
        private Dictionary<string, byte> keys = new Dictionary<string, byte>();

        private List<IResourceCollection> _collections;
        public void AddCollection(IResourceCollection collection) => _collections.Add(collection);

        public void LoadContent(ContentManager Content) {
            Debug.WriteLine($"Loading item types...");

            Types = new List<ItemType>();

            foreach (IResourceCollection collection in _collections)
                LoadCollection(collection);

            BakedTypes = Types.ToArray();
        }

        private void LoadCollection(IResourceCollection collection) {
            foreach (NamespacedKey key in collection.GetBlocks().Keys) {
                AddType(key, collection.GetBlocks()[key]);
            }
        }

        private byte next = 0;
        private void AddType(NamespacedKey key, IResourceType type) {
            type.SetID(next++);
            keys.Add(key.Qualify(), type.GetID());
        }

        private void AddKey(NamespacedKey key, byte id) {
            keys.Add(key.Qualify(), id);
            Debug.WriteLine($"  Loading block '{key.Qualify()}' ({id})...");
        }

        public IResourceType GetResource(NamespacedKey key) {
            if (!keys.ContainsKey(key.Qualify())) throw new NullReferenceException();
            return BakedTypes[keys[key.Qualify()]];
        }
    }
}