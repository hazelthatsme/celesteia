using System;
using Microsoft.Xna.Framework;

namespace Celesteia.Game.Worlds.Generators {
    public class TerranWorldGenerator : IWorldGenerator {
        private GameWorld _world;
        public GameWorld GetWorld() => _world;
        public void SetWorld(GameWorld world) => _world = world;
        private FastNoiseLite _noise;

        public TerranWorldGenerator(GameWorld world) {
            SetWorld(world);
            _noise = new FastNoiseLite(world.GetSeed());
        }

        public byte GetNaturalBlock(int x, int y)
        {
            return SecondPass(x, y, FirstPass(x, y));
        }

        public void GenerateStructures() {
            Random rand = new Random(_world.GetSeed());

            GenerateTrees(rand);
        }

        public Vector2 GetSpawnpoint()
        {
            float x;
            Vector2 spawn = new Vector2(
                x = (float)Math.Floor(_world.GetWidthInBlocks() / 2f) + 0.5f,
                (_world.GetHeightInBlocks()) - GetHeightValue((int)Math.Floor(x)) - 2f
            );
            return spawn;
        }

        public byte FirstPass(int x, int y) {
            byte value = 0;

            int h = GetHeightValue(x);

            if (_world.GetHeightInBlocks() - y <= h) {
                if (_world.GetHeightInBlocks() - y == h) value = 3;
                else if (_world.GetHeightInBlocks() - y >= h - 3) value = 2;
                else value = 1;
            }

            return value;
        }
        public byte SecondPass(int x, int y, byte prev) {
            byte value = prev;
            float threshold = 0.667f;

            if (prev == 0 || prev == 4) return value;
            if (prev == 2 || prev == 3) threshold += .2f;

            float c = GetCaveValue(x, y);

            if (c > threshold) value = 0;

            return value;
        }
        private int defaultOffset => _world.GetHeightInBlocks() / 3;
        public int GetHeightValue(int x) {
            return (int)Math.Round((_noise.GetNoise(x / 1f, 0f) * 24f) + defaultOffset);
        }
        public float GetCaveValue(int x, int y) {
            return _noise.GetNoise(x / 0.6f, y / 0.4f);
        }

        private int blocksBetweenTrees = 5;
        private int treeGrowthSteps = 10;
        public void GenerateTrees(Random rand) {
            int j = 0;
            int randomNumber = 0;
            int lastTree = 0;
            for (int i = 0; i < _world.GetWidthInBlocks(); i++) {
                j = _world.GetHeightInBlocks() - GetHeightValue(i);

                if (Math.Abs(i - GetSpawnpoint().X) < 10f) continue;            // Don't grow trees too close to spawn.
                if (i < 10 || i > _world.GetWidthInBlocks() - 10) continue;     // Don't grow trees too close to world borders.
                if (_world.GetBlock(i, j) != 3) continue;                       // Only grow trees on grass.
                if (i - lastTree < blocksBetweenTrees) continue;                // Force a certain number of blocks between trees.

                lastTree = i;

                randomNumber = rand.Next(0, 6);

                if (randomNumber == 1) GrowTreeRecursively(i, j - 1, treeGrowthSteps - rand.Next(0, 7), rand, false);
            }
        }

        public void GrowTreeRecursively(int x, int y, int steps, Random rand, bool branch) {
            if (steps == 0) {
                for (int i = -2; i <= 2; i++)
                    for (int j = -2; j <= 2; j++) {
                        if (_world.GetBlock(x + i, y + j) == 0) _world.SetBlock(x + i, y + j, 6);
                    }
                return;
            }

            if (_world.GetBlock(x, y) != 0) return;

            _world.SetBlock(x, y, 5);

            if (!branch) GrowTreeRecursively(x, y - 1, steps - 1, rand, false);     // Grow upwards.
            if (rand.Next(0, 6) > steps) GrowTreeRecursively(x - 1, y, steps - 1, rand, true);     // Grow to the left.
            if (rand.Next(0, 6) > steps) GrowTreeRecursively(x + 1, y, steps - 1, rand, true);     // Grow to the right.
        }
    }
}