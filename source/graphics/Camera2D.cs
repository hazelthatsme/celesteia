using System.Diagnostics;
using Celesteia.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celesteia.Graphics {
    public class Camera2D {
        public const float FOLLOW_SPEED = 5f;

        private float _zoom = 1f;
        private Matrix _transform;
        private Vector2 _center = Vector2.Zero;
        private float _rotation;
        private GraphicsDevice _graphicsDevice;
        
        private int ViewportWidth => _graphicsDevice.Viewport.Width;
        private int ViewportHeight => _graphicsDevice.Viewport.Height;

        public Camera2D(GraphicsDevice graphicsDevice) {
            _graphicsDevice = graphicsDevice;
        }

        public Vector2 Center {
            get { return _center; }
        }

        public float Zoom {
            get { return _zoom * 2f * ResourceManager.INVERSE_SPRITE_SCALING; }
            set { _zoom = value <= 0.1f ? 0.1f : value; }
        }

        public float Rotation {
            get { return _rotation; }
            set { _rotation = value % 360f; }
        }

        public void MoveTo(Vector2 vector2) {
            _center = vector2;
        }

        public Matrix GetViewMatrix() {
            _transform = Matrix.CreateTranslation(new Vector3(-_center.X, -_center.Y, 0)) * 
                Matrix.CreateRotationZ(Rotation) * 
                Matrix.CreateScale(Zoom, Zoom, 1f) * 
                Matrix.CreateTranslation(ViewportWidth / 2f, ViewportHeight / 2f, 0f);

            return _transform;
        }
    }
}