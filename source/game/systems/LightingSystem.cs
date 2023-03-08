using System.Threading;
using Celesteia.Game.Worlds;
using Celesteia.Graphics;
using Celesteia.Graphics.Lighting;
using Celesteia.Resources;
using Celesteia.Resources.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended;

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
        
        private Semaphore _semaphore;
        public void Initialize(World world) {
            _semaphore = new Semaphore(0, 1);
            UpdateFrame();
            StartLightMapUpdates();
            _semaphore.Release(1);
        }

        private LightMap _lightMap;
        private Texture2D _texture;

        private double _lightUpdateRate = 0.0333;

        private bool drawTexture = false;
        private bool calculationDone = false;
        private double lastUpdate = 0;
        public void Update(GameTime gameTime)
        {
            if (drawTexture) UpdateTexture();

            if (gameTime.TotalGameTime.TotalSeconds - lastUpdate > _lightUpdateRate) {
                if (calculationDone) {
                    _position = _offset;
                    _semaphore.Release();
                    lastUpdate = gameTime.TotalGameTime.TotalSeconds;
                }
            }
        }

        private int _lightRenderDistance = 150;
        private int _size => 2 * _lightRenderDistance;
        private void UpdateFrame() {
            _lightMap = new LightMap(_size, _size);
            _texture = new Texture2D(_spriteBatch.GraphicsDevice, _size, _size);
        }

        private void UpdateTexture() {
            _texture.SetData<Color>(_lightMap.GetColors(), 0, _lightMap.GetColorCount());
            drawTexture = false;
        }
        
        private Vector2 _position;
        private Vector2 UpdatePosition() {
            _position = new Vector2(
                (int)System.Math.Floor(_camera.Center.X - _lightRenderDistance),
                (int)System.Math.Floor(_camera.Center.Y - _lightRenderDistance)
            );
            return _position;
        }

        private Thread updateLightThread;
        private Vector2 _offset;
        private void StartLightMapUpdates() {
            updateLightThread = new Thread(() => {
                while (true) {
                    _semaphore.WaitOne();
                    calculationDone = false;

                    _lightMap.SetPosition(UpdatePosition());

                    UpdateEmission();
                    _lightMap.CreateColorMap();

                    calculationDone = true;
                    drawTexture = true;
                }
            });
            
            updateLightThread.IsBackground = true;
            updateLightThread.Start();
        }

        private Vector2 _lookAt;
        private byte _blockID;
        private BlockType _block;
        private byte _wallID;
        private BlockType _wall;
        private void UpdateEmission() {
            for (int i = 0; i < _lightMap.Width; i++) {
                _lookAt.X = i;
                for (int j = 0; j < _lightMap.Height; j++) {
                    _lookAt.Y = j;

                    _blockID = _gameWorld.GetBlock(_lightMap.Position + _lookAt);
                    _block = ResourceManager.Blocks.GetBlock(_blockID);

                    if (_block.Light.Emits) _lightMap.AddLight(i, j, _block);
                    else {
                        if (_block.Light.Occludes) _lightMap.AddDarkness(i, j);
                        else {
                            _wallID = _gameWorld.GetWallBlock(_lightMap.Position + _lookAt);
                            _wall = ResourceManager.Blocks.GetBlock(_wallID);

                            if (_block.Light.Occludes) {
                                if (_block.Light.Emits) _lightMap.AddLight(i, j, _block);
                                else _lightMap.AddDarkness(i, j);
                            } else _lightMap.AddLight(i, j, true, LightColor.ambient, 4);
                        }
                    }
                }
            }

        }

        private Vector2 origin = new Vector2(0.5f);
        private Color lightColor = new Color(255, 255, 255, 255);
        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearClamp, null, null, null, _camera.GetViewMatrix());

            _spriteBatch.Draw(_texture, _position, lightColor);

            _spriteBatch.End();
        }
    }
}