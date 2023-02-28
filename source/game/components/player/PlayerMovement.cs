using System;
using System.Collections.Generic;
using Celesteia.Game.Input;
using Microsoft.Xna.Framework;

namespace Celesteia.Game.Components.Player {
    public class PlayerMovement {
        public List<IInputDefinition> Horizontal;
        public List<IInputDefinition> Vertical;
        public IInputDefinition Run;
        public IInputDefinition Jump;

        public PlayerMovement() {
            Horizontal = new List<IInputDefinition>();
            Vertical = new List<IInputDefinition>();
        }

        public PlayerMovement AddHorizontal(IInputDefinition def) {
            Horizontal.Add(def);
            return this;
        }

        public PlayerMovement AddVertical(IInputDefinition def) {
            Vertical.Add(def);
            return this;
        }

        public PlayerMovement SetRun(IInputDefinition def) {
            Run = def;
            return this;
        }

        public PlayerMovement SetJump(IInputDefinition def) {
            Jump = def;
            return this;
        }

        public float TestHorizontal() {
            float val = 0f;
            Horizontal.ForEach(d => { val += d.Test(); });
            return MathHelper.Clamp(val, -1f, 1f);
        }

        public float TestVertical() {
            float val = 0f;
            Vertical.ForEach(d => { val += d.Test(); });
            return MathHelper.Clamp(val, -1f, 1f);
        }

        public float TestRun() {
            float val = Run.Test();
            return MathHelper.Clamp(val, -1f, 1f);
        }

        public float TestJump() {
            float val = Jump.Test();
            return MathHelper.Clamp(val, -1f, 1f);
        }
    }
}