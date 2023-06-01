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
        private int _scaledZoom = 0;
        // The zoom value of the camera.
        public int Zoom {
            get { return _zoom; }
            set {
                _zoom = MathHelper.Clamp(value, 2, 8);
                _scaledZoom = _zoom * ResourceManager.INVERSE_SPRITE_SCALING;
            }
        }

        public Camera2D(GraphicsDevice graphicsDevice) {
            _graphicsDevice = graphicsDevice;
            Zoom = 2;
        }

        private Vector2 _center = Vector2.Zero;
        // The camera's center, exposed to other classes.
        public Vector2 Center {
            get { return _center; }
            set { _center = value; }
        }

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
        public Matrix GetViewMatrix() {
            return Matrix.CreateTranslation(new Vector3(-_center.X, -_center.Y, 0)) * 
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(_scaledZoom, _scaledZoom, 1f) * 
                Matrix.CreateTranslation((int)Math.Round(ViewportWidth / 2f), (int)Math.Round(ViewportHeight / 2f), 0f);
        }

        // Forward to ScreenToWorld(Vector2)
        public Vector2 ScreenToWorld(Point point) {
            return ScreenToWorld(new Vector2(point.X, point.Y));
        }
        
        // Transform the viewport relative mouse position to the inverse view matrix to get the pointer's position in the world.
        public Vector2 ScreenToWorld(Vector2 screenPosition) {
            return Vector2.Transform(screenPosition - new Vector2(ViewportX, ViewportY), Matrix.Invert(GetViewMatrix()));
        }
    }
}