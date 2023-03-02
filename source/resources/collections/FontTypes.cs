using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Celesteia.Resources.Collections {
    public abstract class FontProperties {
        public const int STANDARD_SIZE = 12;
    }

    public class FontTypes {        
        private List<FontType> Types;

        public void LoadContent(ContentManager Content) {
            Debug.WriteLine($"Loading fonts...");

            Types = new List<FontType>();

            Types.Add(new FontType("Hobo", Content.Load<SpriteFont>("Hobo")));
        }

        public FontType GetFontType(string name) {
            return Types.Find(x => x.Name == name);
        }
    }

    public struct FontType {
        public readonly string Name { get; }
        public readonly SpriteFont Font { get; }

        public FontType(string name, SpriteFont font) {
            Name = name;
            Font = font;

            Debug.WriteLine($"  Font '{name}' loaded.");
        }

        public float Scale(float targetFontSize) {
            return targetFontSize / FontProperties.STANDARD_SIZE;
        }
    }
}