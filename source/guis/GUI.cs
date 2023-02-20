using System.Collections.Generic;
using System.Diagnostics;
using Celesteia.GameInput;
using Celesteia.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input;
using MonoGame.Extended.TextureAtlases;

namespace Celesteia.GUIs {
    public class GUI {
        public Game Game;

        public IContainer Root;

        public GUI(Game Game, Rect rect) {
            this.Game = Game;
            this.Root = new Container(rect);
        }

        public virtual void LoadContent() {
            Debug.WriteLine("Loaded GUI.");
        }

        public virtual void Update(GameTime gameTime) {
            Root.Update(gameTime);
        }

        // Draw all elements.
        public virtual void Draw(GameTime gameTime) {
            
            Game.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointWrap, null, null, null);

            if (Game.GUIEnabled) Root.Draw(Game.SpriteBatch);

            Game.SpriteBatch.End();
        }

        // If the menu is referred to as a boolean, return whether it is non-null (true) or null (false).
        public static implicit operator bool(GUI gui)
        {
            return !object.ReferenceEquals(gui, null);
        }
    }
}