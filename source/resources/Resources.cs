using System.Diagnostics;
using Celesteia.Resources.Collections;
using Celesteia.Resources.Sprites;
using Celesteia.Resources.Types;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Celesteia.Resources {
    public static class ResourceManager {
        public static BlockTypes Blocks = new BlockTypes();
        public static EntityTypes Entities = new EntityTypes();
        public static FontTypes Fonts = new FontTypes();
        public static SkyboxAssets Skybox = new SkyboxAssets();

        public const float SPRITE_SCALING = 0.125f;
        public const float INVERSE_SPRITE_SCALING = 8f;

        public static void LoadContent(ContentManager content) {
            Blocks.LoadContent(content);
            Entities.LoadContent(content);
            Fonts.LoadContent(content);
            Skybox.LoadContent(content);
        }
    }
}