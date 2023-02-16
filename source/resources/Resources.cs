using System.Diagnostics;
using Celestia.Resources.Sprites;
using Celestia.Resources.Types;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Celestia.Resources {
    public static class ResourceManager {
        public static BlockTypes Blocks = new BlockTypes();
        public static EntityTypes Entities = new EntityTypes();

        public const float SPRITE_SCALING = 0.125f;
        public const float INVERSE_SPRITE_SCALING = 8f;

        public static void LoadContent(ContentManager content) {
            Blocks.LoadContent(content);
            
            Entities.LoadContent(content);
        }
    }
}