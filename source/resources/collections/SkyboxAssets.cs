using System.Collections.Generic;
using System.Diagnostics;
using Celestia.Resources.Sprites;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace Celestia.Resources.Collections {
    public class SkyboxAssets {        
        private List<SkyboxAsset> Assets;

        public void LoadContent(ContentManager Content) {
            Debug.WriteLine($"Loading skybox assets...");

            Assets = new List<SkyboxAsset>();

            Assets.Add(new SkyboxAsset(0, "stars", TextureAtlas.Create("stars", Content.Load<Texture2D>("sprites/skybox/stars2"), 1024, 1024), 1024, 0));
            Assets.Add(new SkyboxAsset(1, "shadow", TextureAtlas.Create("shadow", Content.Load<Texture2D>("sprites/skybox/shadow"), 1024, 1024), 1024, 0));
            Assets.Add(new SkyboxAsset(2, "nebula", TextureAtlas.Create("nebula", Content.Load<Texture2D>("sprites/skybox/nebula"), 1024, 1024), 1024, 0));
        }

        public SkyboxAsset GetAsset(string name) {
            return Assets.Find(x => x.Name == name);
        }
    }

    public struct SkyboxAsset {
        public readonly byte AssetID;
        public readonly string Name;
        public readonly SkyboxPortionFrames Frames;

        public SkyboxAsset(byte id, string name, TextureAtlas atlas, int size, int frameStart, int frameCount) {
            AssetID = id;
            Name = name;
            Frames = new SkyboxPortionFrames(atlas, size, frameStart, frameCount);

            Debug.WriteLine($"  Skybox asset '{name}' loaded.");
        }

        public SkyboxAsset(byte id, string name, TextureAtlas atlas, int size, int frame) : this (id, name, atlas, size, frame, 1) {}
    }
}