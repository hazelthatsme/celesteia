using System;
using Celesteia.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celesteia.Graphics {
    public class Camera2D {
        private GraphicsDevice _graphicsDevice;
        
        // Viewport macros.
        private int ViewportX => _graphicsDevice.Viewport.X;
        private int ViewportY => _graphicsDevice.Viewport.Y;
        private int ViewportWidth => _graphicsDevice.Viewport.Width;
        private int ViewportHeight => _graphicsDevice.Viewport.Height;

        private int _zoom = 0;
        public int ScaledZoom { get; private set; } = 0;
        // The zoom value of the camera.
        public int Zoom {
            get { return _zoom; }
            set {
                _zoom = MathHelper.Clamp(value, 1, 8);
                ScaledZoom = _zoom * ResourceManager.INVERSE_SPRITE_SCALING;
            }
        }

        public Camera2D(GraphicsDevice graphicsDevice) {
            _graphicsDevice = graphicsDevice;
            Zoom = 2;
        }

        // The camera's center.
        public Vector2 Center = Vector2.Zero;

        private float _rotation;
        // The rotation applied to the camera.
        public float Rotation {
            get { return _rotation; }
            set { _rotation = value % 360f; }
        }

        /*
            Creates a matrix with the following steps:
                - Create a translation to match (0, 0) to the center point of the camera.
                - Apply Z rotation.
                - Scale according to zoom value and inverse sprite scaling.
                - Always round the viewport width and height to prevent half-pixel rounding issues.
        */
        private float maxScale = 0f;
        public Matrix GetViewMatrix() {
            maxScale = MathF.Max(MathF.Ceiling(ViewportWidth / 1920f), MathF.Ceiling(ViewportHeight / 1080f));
            return Matrix.CreateTranslation(-Center.X, -Center.Y, 0f) * 
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(ScaledZoom, ScaledZoom, 1f) * 
                Matrix.CreateScale(maxScale, maxScale, 1f) *
                Matrix.CreateTranslation(ViewportWidth / 2f, ViewportHeight / 2f, 0f);
        }

        // Transform the viewport relative mouse position to the inverse view matrix to get the pointer's position in the world.
        public Vector2 ScreenToWorld(Point point)
        => Vector2.Transform(new Vector2(point.X - ViewportX, point.Y - ViewportY), Matrix.Invert(GetViewMatrix()));
    }
}