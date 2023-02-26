using System;
using System.Diagnostics;
using Celesteia.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celesteia.Graphics {
    public class Camera2D {
        public const float FOLLOW_SPEED = 5f;

        private int _zoom = 1;
        private Matrix _transform;
        private Vector2 _center = Vector2.Zero;
        private float _rotation;
        private GraphicsDevice _graphicsDevice;
        
        private int ViewportX => _graphicsDevice.Viewport.X;
        private int ViewportY => _graphicsDevice.Viewport.Y;
        private int ViewportWidth => _graphicsDevice.Viewport.Width;
        private int ViewportHeight => _graphicsDevice.Viewport.Height;

        public Camera2D(GraphicsDevice graphicsDevice) {
            _graphicsDevice = graphicsDevice;
        }

        public Vector2 Center {
            get { return _center; }
        }

        public int Zoom {
            get { return _zoom; }
            set { _zoom = MathHelper.Clamp(value, 1, 8); }
        }

        public float Rotation {
            get { return _rotation; }
            set { _rotation = value % 360f; }
        }

        public void MoveTo(Vector2 vector2) {
            _center = vector2;
        }

        private float ScaledZoom => _zoom * ResourceManager.INVERSE_SPRITE_SCALING;
        public Matrix GetViewMatrix() {
            _transform = Matrix.CreateTranslation(new Vector3(-_center.X, -_center.Y, 0)) * 
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(ScaledZoom, ScaledZoom, 1f) * 
                Matrix.CreateTranslation((int)Math.Round(ViewportWidth / 2f), (int)Math.Round(ViewportHeight / 2f), 0f);

            return _transform;
        }

        public Vector2 GetDrawingPosition(Vector2 position) {
            return new Vector2(
                (int)Math.Round(position.X * ResourceManager.INVERSE_SPRITE_SCALING * _zoom) / (ResourceManager.INVERSE_SPRITE_SCALING * _zoom),
                (int)Math.Round(position.Y * ResourceManager.INVERSE_SPRITE_SCALING * _zoom) / (ResourceManager.INVERSE_SPRITE_SCALING * _zoom)
            );
        }

        public Vector2 ScreenToWorld(Point point) {
            return ScreenToWorld(point.X, point.Y);
        }

        public Vector2 ScreenToWorld(float x, float y) {
            return ScreenToWorld(new Vector2(x, y));
        }
        
        public Vector2 ScreenToWorld(Vector2 screenPosition) {
            return Vector2.Transform(screenPosition - new Vector2(ViewportX, ViewportY), Matrix.Invert(GetViewMatrix()));
        }
    }
}