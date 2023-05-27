using Celesteia.Game.Input.Definitions;

namespace Celesteia.Game.Input.Conditions {
    public class AnyCondition : ICondition<bool> {
        private IBinaryInputDefinition[] _definitions;

        public AnyCondition(params IBinaryInputDefinition[] definitions)
        => _definitions = definitions;

        public bool Poll() {
            for (int i = 0; i < _definitions.Length; i++) if (_definitions[i].Test()) return true;
            return false;
        }
    }
}