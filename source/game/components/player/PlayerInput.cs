using System;
using System.Collections.Generic;
using Celesteia.Game.Input;
using Microsoft.Xna.Framework;

namespace Celesteia.Game.Components.Player {
    public class PlayerInput {
        public List<IInputDefinition> Horizontal = new List<IInputDefinition>();
        public List<IInputDefinition> Vertical = new List<IInputDefinition>();
        public List<IInputDefinition> Run = new List<IInputDefinition>();
        public List<IInputDefinition> Jump = new List<IInputDefinition>();
        public List<IInputDefinition> OpenInventory = new List<IInputDefinition>();
        public List<IInputDefinition> OpenCrafting = new List<IInputDefinition>();

        public PlayerInput AddHorizontal(params IInputDefinition[] defs) {
            Horizontal.AddRange(defs);
            return this;
        }

        public PlayerInput AddVertical(params IInputDefinition[] defs) {
            Vertical.AddRange(defs);
            return this;
        }

        public PlayerInput AddRun(params IInputDefinition[] defs) {
            Run.AddRange(defs);
            return this;
        }

        public PlayerInput AddJump(params IInputDefinition[] defs) {
            Jump.AddRange(defs);
            return this;
        }

        public PlayerInput AddOpenInventory(params IInputDefinition[] defs) {
            OpenInventory.AddRange(defs);
            return this;
        }

        public PlayerInput AddOpenCrafting(params IInputDefinition[] defs) {
            OpenCrafting.AddRange(defs);
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
            float val = 0f;
            Run.ForEach(d => { val += d.Test(); });
            return MathHelper.Clamp(val, -1f, 1f);
        }

        public float TestJump() {
            float val = 0f;
            Jump.ForEach(d => { val += d.Test(); });
            return MathHelper.Clamp(val, -1f, 1f);
        }

        public float TestOpenInventory() {
            float val = 0f;
            Jump.ForEach(d => { val += d.Test(); });
            return MathHelper.Clamp(val, -1f, 1f);
        }

        public float TestOpenCrafting() {
            float val = 0f;
            Jump.ForEach(d => { val += d.Test(); });
            return MathHelper.Clamp(val, -1f, 1f);
        }
    }
}