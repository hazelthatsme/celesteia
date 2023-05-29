using System;
using System.Collections.Generic;
using Celesteia.Game.Components.Entity;
using Celesteia.Game.Planets;
using Celesteia.Game.Systems;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Celesteia.Game.ECS {
    public class GameWorld : IDisposable {
        public ChunkMap ChunkMap { get; private set; }
        public GameWorld(ChunkMap map) => ChunkMap = map;

        public void Dispose() {
            _w.Dispose();
            ChunkMap = null;
        }

        private World _w;
        private WorldBuilder _builder;
        
        public WorldBuilder BeginBuilder() => _builder = new WorldBuilder();

        public WorldBuilder AddSystem(ISystem system) => _builder.AddSystem(system);

        public void EndBuilder() => _w = _builder.Build();

        public Entity CreateEntity() {
            Entity e = _w.CreateEntity();
            e.Attach(new GameWorldEntity(this, e.Id));
            
            return e;
        }
        
        public void DestroyEntity(int id) => _w.DestroyEntity(id);

        public void Update(GameTime gt) => _w.Update(gt);
        public void Draw(GameTime gt) => _w.Draw(gt);
    }
}