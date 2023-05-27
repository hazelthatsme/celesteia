using Celesteia.Game.Components;
using Celesteia.Game.Items;
using MonoGame.Extended.TextureAtlases;

namespace Celesteia.Resources.Types.Builders {
    public class ItemTypeBuilder {
        private TextureAtlas _atlas;
        public ItemTypeBuilder(TextureAtlas atlas) {
            _atlas = atlas;
        }

        private ItemType current;
        public ItemTypeBuilder WithName(string name) {
            current = new ItemType(name);
            return this;
        }

        public ItemTypeBuilder Block(NamespacedKey blockToPlace)
        => Template(ItemTypeTemplate.Block).Actions(new BlockItemActions(blockToPlace));
        public ItemTypeBuilder Pickaxe(int power)
        => Template(ItemTypeTemplate.Tool).Actions(new PickaxeItemActions(power));
        public ItemTypeBuilder Upgrade(EntityAttribute attribute, float increase, float max )
        => Template(ItemTypeTemplate.Tool).Actions(new UpgradeItemActions(attribute, increase, max));

        public ItemTypeBuilder Template(ItemTypeTemplate template) {
            current.MaxStackSize = template.MaxStackSize;
            current.ConsumeOnUse = template.ConsumeOnUse;
            return this;
        }

        public ItemTypeBuilder Frame(int frame) {
            return Frame(_atlas.GetRegion(frame));
        }

        public ItemTypeBuilder Frame(TextureRegion2D region) {
            current.Sprite = region;
            return this;
        }

        public ItemTypeBuilder Actions(IItemActions actions) {
            current.Actions = actions;
            return this;
        }

        public ItemType Get() {
            return current;
        }
    }

    public class ItemTypeTemplate
        {
            public static ItemTypeTemplate Block = new ItemTypeTemplate(1000, true);
            public static ItemTypeTemplate Tool = new ItemTypeTemplate(1, false);
            public static ItemTypeTemplate Upgrade = new ItemTypeTemplate(1, false);

            public int MaxStackSize = 99;
            public bool ConsumeOnUse = true;

            public ItemTypeTemplate(
                int maxStackSize = 99,
                bool consumeOnUse = true
            ) {
                MaxStackSize = maxStackSize;
                ConsumeOnUse = consumeOnUse;
            }
        }
}