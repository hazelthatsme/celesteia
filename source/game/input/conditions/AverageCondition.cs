using Celesteia.Game.Input.Definitions;

namespace Celesteia.Game.Input.Conditions {
    public class AverageCondition : ICondition<float> {
        private IFloatInputDefinition[] _definitions;

        public AverageCondition(params IFloatInputDefinition[] definitions)
        => _definitions = definitions;

        public float Poll() {
            float total = 0f;
            for (int i = 0; i < _definitions.Length; i++) total += _definitions[i].Test();
            return total / _definitions.Length;
        }
    }
}