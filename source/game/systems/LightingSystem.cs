using System.Threading;
using Celesteia.Game.Worlds;
using Celesteia.Graphics;
using Celesteia.Graphics.Lighting;
using Celesteia.Resources;
using Celesteia.Resources.Management;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended;
using System.Diagnostics;
using Celesteia.Resources.Types;

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
            if (drawTexture) {
                UpdateTexture();
            }

            if (gameTime.TotalGameTime.TotalSeconds - lastUpdate > _lightUpdateRate && calculationDone) {
                _position = UpdatePosition();
                _semaphore.Release();
                lastUpdate = gameTime.TotalGameTime.TotalSeconds;
            }
        }
        
        private Point _position;
        private Vector2 _drawPosition;
        private Point UpdatePosition() {
            return ChunkVector.FromVector2(_camera.Center).Resolve() - new Point(_lightRenderDistance);
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
            _drawPosition = _position.ToVector2();
        }

        private Thread updateLightThread;
        private void StartLightMapUpdates() {
            updateLightThread = new Thread(() => {
                while (true) {
                    _semaphore.WaitOne();
                    calculationDone = false;

                    UpdateEmission();
                    _lightMap.Propagate();
                    _lightMap.CreateColorMap();

                    calculationDone = true;
                    drawTexture = true;
                }
            });
            
            updateLightThread.IsBackground = true;
            updateLightThread.Start();
        }

        private int x;
        private int y;
        private byte _blockID;
        private BlockType _block;
        private byte _wallID;
        private BlockType _wall;
        private void UpdateEmission() {
            for (int i = 0; i < _lightMap.Width; i++) {
                x = i + _position.X;
                for (int j = 0; j < _lightMap.Height; j++) {
                    y = j + _position.Y;

                    _blockID = _gameWorld.GetBlock(x, y);

                    if (_blockID != 0) {
                        _block = ResourceManager.Blocks.GetBlock(_blockID);
                        if (_block.Light.Emits) {
                            _lightMap.AddLight(i, j, _block.Light); continue;
                        }
                        else {
                            if (_block.Light.Occludes) {
                                _lightMap.AddDarkness(i, j); continue;
                            }
                        }
                    }

                    _wallID = _gameWorld.GetWallBlock(x, y);
                    if (_wallID != 0) {
                        _wall = ResourceManager.Blocks.GetBlock(_wallID);
                        if (_wall.Light.Occludes) {
                            if (_wall.Light.Emits) {
                                _lightMap.AddLight(i, j, _wall.Light); continue;
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

        private Vector2 origin = new Vector2(0.5f);
        private Color lightColor = new Color(255, 255, 255, 255);
        private BlendState multiply = new BlendState() {
            ColorBlendFunction = BlendFunction.Add,
            ColorSourceBlend = Blend.DestinationColor,
            ColorDestinationBlend = Blend.Zero,
        };

        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, multiply, SamplerState.LinearClamp, null, null, null, _camera.GetViewMatrix());

            _spriteBatch.Draw(_texture, _drawPosition, lightColor);

            _spriteBatch.End();
        }
    }
}