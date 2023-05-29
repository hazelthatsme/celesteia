using System.Threading;
using Celesteia.Game.World;
using Celesteia.Graphics;
using Celesteia.Graphics.Lighting;
using Celesteia.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities.Systems;
using Celesteia.Resources.Types;
using Celesteia.Game.World.Planet;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Celesteia.Game.Systems {
    public class LightingSystem : IUpdateSystem, IDrawSystem
    {
        private readonly Camera2D _camera;
        private readonly SpriteBatch _spriteBatch;
        private readonly GameWorld _gameWorld;

        public LightingSystem(Camera2D camera, SpriteBatch spriteBatch, GameWorld gameWorld) {
            _camera = camera;
            _spriteBatch = spriteBatch;
            _gameWorld = gameWorld;
        }
        public void Dispose() { }
        
        private int _lightRenderDistance = 5;

        private Dictionary<byte, BlockLightProperties> lightingDictionary;
        public void Initialize(MonoGame.Extended.Entities.World world) {
            int _size = 2 * _lightRenderDistance * Chunk.CHUNK_SIZE;

            _lightMap = new LightMap(_size, _size);
            _texture = new Texture2D(_spriteBatch.GraphicsDevice, _size, _size);

            lightingDictionary = new Dictionary<byte, BlockLightProperties>();
        }

        private LightMap _lightMap;
        private Texture2D _texture;

        private bool drawTexture = false;
        private Task _lightUpdate;
        public void Update(GameTime gameTime)
        {
            if (_lightUpdate == null || (_lightUpdate != null && _lightUpdate.IsCompleted))
                    _lightUpdate = Task.Factory.StartNew(() => UpdateLight());
                
            if (drawTexture) UpdateTexture();
        }
        
        private Point _position;
        private void UpdatePosition() {
            _position = ChunkVector.FromVector2(_camera.Center).Resolve() - new Point(_lightRenderDistance * Chunk.CHUNK_SIZE);
        }

        private void UpdateTexture() {
            _drawPosition = _position.ToVector2();
            _texture.SetData<Color>(_lightMap.GetColors(), 0, _lightMap.GetColorCount());
            drawTexture = false;
        }

        private void UpdateLight() {
            UpdatePosition();
            
            UpdateEmission();
            _lightMap.Propagate();
            _lightMap.CreateColorMap();

            drawTexture = true;
        }

        private byte _blockID;
        private BlockLightProperties _light;

        // Foreground != 0 -> (Emits -> emit light) OR (Occludes -> emit darkness).
        // Background != 0 -> Occludes -> (Emits -> emit light) OR emit darkness.
        private void UpdateEmission() {
            for (int i = 0; i < _lightMap.Width; i++) {
                for (int j = 0; j < _lightMap.Height; j++) {
                    // Foreground
                    _blockID = _gameWorld.ChunkMap.GetForeground(i + _position.X, j + _position.Y);

                    if (_blockID != 0) {
                        _light = GetBlockLightProperties(_blockID);
                        if (_light.Emits) {
                            _lightMap.AddLight(i, j, _light); continue;
                        }
                        else {
                            if (_light.Occludes) {
                                _lightMap.AddDarkness(i, j); continue;
                            }
                        }
                    }

                    // Background
                    _blockID = _gameWorld.ChunkMap.GetBackground(i + _position.X, j + _position.Y);
                    if (_blockID != 0) {
                        _light = GetBlockLightProperties(_blockID);
                        if (_light.Occludes) {
                            if (_light.Emits) {
                                _lightMap.AddLight(i, j, _light); continue;
                            }
                            else {
                                _lightMap.AddDarkness(i, j); continue;
                            }
                        }
                    }

                    _lightMap.AddLight(i, j, true, LightColor.ambient, 4);
                }
            }

        }

        private BlockLightProperties GetBlockLightProperties(byte id) {
            if (!lightingDictionary.ContainsKey(id)) lightingDictionary.Add(id, ResourceManager.Blocks.GetBlock(id).Light);
            return lightingDictionary[id];
        }

        private BlendState multiply = new BlendState() {
            ColorBlendFunction = BlendFunction.Add,
            ColorSourceBlend = Blend.DestinationColor,
            ColorDestinationBlend = Blend.Zero,
        };

        private Vector2 _drawPosition;
        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, multiply, SamplerState.LinearClamp, null, null, null, _camera.GetViewMatrix());

            _spriteBatch.Draw(_texture, _drawPosition, Color.White);

            _spriteBatch.End();
        }
    }
}