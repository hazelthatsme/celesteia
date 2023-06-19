using System;
using Microsoft.Xna.Framework;
using Celesteia.Resources;
using System.Collections.Generic;
using System.Linq;

namespace Celesteia.Game.Planets.Generation {
    public class TerranPlanetGenerator : IChunkProvider {
        private ChunkMap _chunkMap;
        private int _seed;
        private FastNoiseLite _noise;

        public TerranPlanetGenerator(GeneratedPlanet chunkMap) {
            _chunkMap = chunkMap;
            _noise = new FastNoiseLite(_seed = chunkMap.Seed);

            LoadBlockIndices();
        }

        private byte top;
        private byte soil;
        private byte stone;
        private byte deepstone;
        private byte log;
        private byte leaves;
        private byte planks;
        private byte coal_ore;
        private byte copper_ore;
        private byte iron_ore;
        private byte[] foliage;
        private byte landing_floor;
        private byte cc_base;
        private byte cc_frame;
        private void LoadBlockIndices() {
            top = ResourceManager.Blocks.GetResource(NamespacedKey.Base("grown_soil")).GetID();
            soil = ResourceManager.Blocks.GetResource(NamespacedKey.Base("soil")).GetID();
            stone = ResourceManager.Blocks.GetResource(NamespacedKey.Base("stone")).GetID();
            deepstone = ResourceManager.Blocks.GetResource(NamespacedKey.Base("deepstone")).GetID();
            log = ResourceManager.Blocks.GetResource(NamespacedKey.Base("log")).GetID();
            leaves = ResourceManager.Blocks.GetResource(NamespacedKey.Base("leaves")).GetID();
            planks = ResourceManager.Blocks.GetResource(NamespacedKey.Base("wooden_planks")).GetID();
            coal_ore = ResourceManager.Blocks.GetResource(NamespacedKey.Base("coal_ore")).GetID();
            copper_ore = ResourceManager.Blocks.GetResource(NamespacedKey.Base("copper_ore")).GetID();
            iron_ore = ResourceManager.Blocks.GetResource(NamespacedKey.Base("iron_ore")).GetID();

            foliage = new byte[5] {
                0,
                ResourceManager.Blocks.GetResource(NamespacedKey.Base("grass")).GetID(),
                ResourceManager.Blocks.GetResource(NamespacedKey.Base("blue_flower")).GetID(),
                ResourceManager.Blocks.GetResource(NamespacedKey.Base("red_flower")).GetID(),
                ResourceManager.Blocks.GetResource(NamespacedKey.Base("violet_flower")).GetID()
            };

            landing_floor = ResourceManager.Blocks.GetResource(NamespacedKey.Base("scorched_soil")).GetID();
            cc_base = ResourceManager.Blocks.GetResource(NamespacedKey.Base("crashed_capsule_base")).GetID();
            cc_frame = ResourceManager.Blocks.GetResource(NamespacedKey.Base("crashed_capsule_frame")).GetID();
        }

        public void ProvideChunk(Chunk c) {
            byte[] natural;
            for (int i = 0; i < Chunk.CHUNK_SIZE; i++)
                for (int j = 0; j < Chunk.CHUNK_SIZE; j++) {
                    natural = GetNaturalBlocks(c.TruePosition.X + i, c.TruePosition.Y + j);
                    c.SetForeground(i, j, natural[0]);
                    c.SetBackground(i, j, natural[1]);
                }
        }

        public byte[] GetNaturalBlocks(int x, int y) => ThirdPass(x, y, SecondPass(x, y, FirstPass(x, y)));

        public void GenerateStructures(Action<string> progressReport) {
            Random rand = new Random(_seed);

            progressReport("Planting trees...");
            GenerateTrees(rand);
            progressReport("Abandoning houses...");
            GenerateAbandonedHomes(rand);
            progressReport("Planting foliage...");
            GenerateFoliage(rand);
            progressReport("Landing light...");
            GenerateLanding();
        }

        public Vector2 GetSpawnpoint()
        {
            float x;
            return new Vector2(
                x = MathF.Floor(_chunkMap.BlockWidth / 2f) + 0.5f,
                (_chunkMap.BlockHeight) - GetHeightValue((int)MathF.Floor(x)) - 2f
            );
        }

        public byte[] FirstPass(int x, int y) {
            if (y > _chunkMap.BlockHeight - 5) return new byte[2] { deepstone, deepstone };

            byte[] values = new byte[2] { 0, 0 };

            int h = GetHeightValue(x);

            if (_chunkMap.BlockHeight - y <= h) {
                if (_chunkMap.BlockHeight - y == h) { values[0] = top; values[1] = soil; }
                else if (_chunkMap.BlockHeight - y >= h - 3) { values[0] = soil; values[1] = soil; }
                else { values[0] = stone; values[1] = stone; }
            }

            return values;
        }
        public byte[] SecondPass(int x, int y, byte[] values) {
            float threshold = 0.667f;

            if (values[0] == 0 || values[0] == deepstone) return values;
            if (values[0] == soil || values[0] == top) threshold += .2f;

            float c = GetCaveValue(x, y);

            if (c > threshold) values[0] = 0;

            return values;
        }

        public byte[] ThirdPass(int x, int y, byte[] values) {
            if (values[0] != stone) return values;

            float coalValue = GetOreValue(x, y, 498538f, 985898f);
            if (coalValue > 0.95f) values[0] = coal_ore;
            else {
                float copperValue = GetOreValue(x, y, 3089279f, 579486f);
                if (copperValue > 0.95f) values[0] = copper_ore;

                else
                {
                    float ironValue = GetOreValue(x, y, 243984f, 223957f);
                    if (ironValue > 0.95f) values[0] = iron_ore;
                }
            }

            return values;
        }

        private int defaultOffset => _chunkMap.BlockHeight / 3;
        public int GetHeightValue(int x) => (int)Math.Round((_noise.GetNoise(x / 1f, 0f) * 24f) + defaultOffset);
        public float GetCaveValue(int x, int y) => _noise.GetNoise(x / 0.6f, y / 0.7f);
        public float GetOreValue(int x, int y, float offsetX, float offsetY) => (_noise.GetNoise((x + offsetX) * 5f, (y + offsetY) * 5f) + 1) / 2f;

        private int blocksBetweenTrees = 5;
        private int treeGrowthSteps = 10;
        public void GenerateTrees(Random rand) {
            int j = 0;
            int randomNumber = 0;
            int lastTree = 0;
            for (int i = 0; i < _chunkMap.BlockWidth; i++) {
                j = _chunkMap.BlockHeight - GetHeightValue(i);

                if (MathF.Abs(i - GetSpawnpoint().X) < 10f) continue;            // Don't grow trees too close to spawn.
                if (i < 10 || i > _chunkMap.BlockWidth - 10) continue;     // Don't grow trees too close to world borders.
                if (_chunkMap.GetForeground(i, j).BlockID != top) continue;                     // Only grow trees on grass.
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
                        if (_chunkMap.GetForeground(x + i, y + j).Empty) _chunkMap.SetForegroundID(x + i, y + j, leaves);
                    }
                return;
            }

            if (!_chunkMap.GetForeground(x, y).Empty) return;

            _chunkMap.SetForegroundID(x, y, log);

            if (!branch) GrowTreeRecursively(x, y - 1, steps - 1, rand, false);     // Grow upwards.
            if (rand.Next(0, 6) > steps) GrowTreeRecursively(x - 1, y, steps - 1, rand, true);     // Grow to the left.
            if (rand.Next(0, 6) > steps) GrowTreeRecursively(x + 1, y, steps - 1, rand, true);     // Grow to the right.
        }

        private int blocksBetweenHomes = 150;
        public void GenerateAbandonedHomes(Random rand) {
            int j = 0;
            int randomNumber = 0;
            int lastHome = 0;
            for (int i = 0; i < _chunkMap.BlockWidth; i++) {
                j = _chunkMap.BlockHeight - GetHeightValue(i);

                if (MathF.Abs(i - GetSpawnpoint().X) < 10f) continue;            // Don't grow trees too close to spawn.
                if (i < 10 || i > _chunkMap.BlockWidth - 10) continue;     // Don't grow trees too close to world borders.
                if (i - lastHome < blocksBetweenHomes) continue;                // Force a certain number of blocks between trees.

                int homeWidth = rand.Next(10, 15);
                int homeHeight = rand.Next(6, 10);
                int buryAmount = rand.Next(15, 40);

                j -= homeHeight;    // Raise the home to be built on the ground first.
                j += buryAmount;    // Bury the home by a random amount.

                lastHome = i;

                randomNumber = rand.Next(0, 5);

                if (randomNumber == 1) BuildAbandonedHome(i, j, homeWidth, homeHeight, rand);
            }
        }

        public void BuildAbandonedHome(int originX, int originY, int homeWidth, int homeHeight, Random rand) {
            int maxX = originX + homeWidth;

            for (int i = originX; i < maxX; i++) originY = Math.Max(originY, _chunkMap.BlockHeight - GetHeightValue(i));
            int maxY = originY + homeHeight;

            for (int i = originX; i < maxX; i++)
                for (int j = originY; j < maxY; j++) {
                    _chunkMap.SetBackgroundID(i, j, planks);
                    _chunkMap.SetForegroundID(i, j, 0);

                    // Apply some random decay by skipping tiles at random.
                    if (rand.Next(0, 5) > 3) {
                        continue;
                    }

                    if (j == originY || j == maxY - 1) _chunkMap.SetForegroundID(i, j, planks);
                    if (i == originX || i == maxX - 1) _chunkMap.SetForegroundID(i, j, log);
                }
        }
        
        private Dictionary<double, int> foliageDistribution = new Dictionary<double, int> {
            { 0.3, 0 },
            { 0.6, 1 },
            { 0.7, 2 },
            { 0.85, 4 },
            { 0.99, 3 },
        };

        public void GenerateFoliage(Random rand) {
            int j = 0;

            double randomNumber = 0;
            int foliageIndex = 0;
            for (int i = 0; i < _chunkMap.BlockWidth; i++) {
                j = _chunkMap.BlockHeight - GetHeightValue(i);

                if (_chunkMap.GetForeground(i, j).BlockID != top) continue; // If there is anything but foreground grown soil, continue.
                if (!_chunkMap.GetForeground(i, j - 1).Empty) continue;     // If the foreground is already taken, continue.

                randomNumber = rand.NextDouble();
                for (int f = 0; f < foliageDistribution.Keys.Count; f++) {
                    if (randomNumber > foliageDistribution.Keys.ElementAt(f)) foliageIndex = foliageDistribution[foliageDistribution.Keys.ElementAt(f)];
                }
                
                _chunkMap.SetForegroundID(i, j - 1, foliage[foliageIndex]);
            }
        }
        
        public void GenerateLanding() {
            int x = GetSpawnpoint().ToPoint().X;
            int j = _chunkMap.BlockHeight - GetHeightValue(x);
            for (int i = -1; i <= 1; i++) {
                _chunkMap.SetForegroundID(x + i, j + 1, soil);
                _chunkMap.SetForegroundID(x + i, j, landing_floor);
                for (int h = 1; h <= 3; h++) {
                    _chunkMap.SetForegroundID(x + i, j - h, cc_frame);
                    _chunkMap.SetBackgroundID(x + i, j - h, 0);
                }
            }
            _chunkMap.SetForegroundID(x, j - 1, cc_base);
        }
    }
}