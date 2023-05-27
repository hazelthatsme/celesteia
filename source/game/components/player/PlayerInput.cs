using System.Collections.Generic;
using Celesteia.Game.Input;
using Microsoft.Xna.Framework;

namespace Celesteia.Game.Components.Player {
    public class PlayerInput {
        public List<IInputDefinition> Horizontal = new List<IInputDefinition>();
        public List<IInputDefinition> Run = new List<IInputDefinition>();
        public List<IInputDefinition> Jump = new List<IInputDefinition>();
        public List<IInputDefinition> Inventory = new List<IInputDefinition>();
        public List<IInputDefinition> Crafting = new List<IInputDefinition>();
        public List<IInputDefinition> Pause = new List<IInputDefinition>();
        public List<IInputDefinition> ManipulateForeground = new List<IInputDefinition>();
        public List<IInputDefinition> ManipulateBackground = new List<IInputDefinition>();

        public float TestHorizontal() {
            float val = 0f;
            Horizontal.ForEach(d => { val += d.Test(); });
            return MathHelper.Clamp(val, -1f, 1f);
        }

        public float TestRun() {
            float val = 0f;
            Run.ForEach(d => { val += d.Test(); });
            return MathHelper.Clamp(val, -1f, 1f);
        }

        public float TestJump() {
            float val = 0f;
            Jump.ForEach(d => { val += d.Test(); });
            return MathHelper.Clamp(val, -1f, 1f);
        }

        public float TestInventory() {
            float val = 0f;
            Inventory.ForEach(d => { val += d.Test(); });
            return MathHelper.Clamp(val, -1f, 1f);
        }

        public float TestCrafting() {
            float val = 0f;
            Crafting.ForEach(d => { val += d.Test(); });
            return MathHelper.Clamp(val, -1f, 1f);
        }

        public float TestPause() {
            float val = 0f;
            Pause.ForEach(d => { val += d.Test(); });
            return MathHelper.Clamp(val, -1f, 1f);
        }
    }
}