using Celesteia.Game.Input.Conditions;

namespace Celesteia.Game.Components.Player {
    public class PlayerInput {
        public ICondition<float> Horizontal;
        public ICondition<bool> Run;
        public ICondition<bool> Jump;
        public ICondition<bool> Inventory;
        public ICondition<bool> Crafting;
        public ICondition<bool> Pause;
        public ICondition<bool> PrimaryUse;
        public ICondition<bool> SecondaryUse;
    }
}