using System;
using System.Diagnostics;
using Celesteia.GameInput;
using Celesteia.Graphics;
using Celesteia.Resources;
using Celesteia.Screens.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Celesteia.Screens.Systems {
    public class CameraZoomSystem : UpdateSystem 
    {
        private readonly Camera2D _camera;

        public CameraZoomSystem(Camera2D camera) => _camera = camera;

        public override void Update(GameTime gameTime) => _camera.Zoom += MouseWrapper.GetScrollDelta();
    }
}