using System;
using Celesteia.Resources.Types;
using Microsoft.Xna.Framework;

namespace Celesteia.Graphics.Lighting {
    public class LightMap {
        private bool[,] _emit;
        private LightColor[,] _lightColors;
        private int[,] _propagation;

        public readonly int Width;
        public readonly int Height;

        public LightMap(int width, int height) {
            Width = width;
            Height = height;

            _emit = new bool[width, height];
            _lightColors = new LightColor[width, height];
            _propagation = new int[width, height];
        }

        public bool AddForeground(int x, int y, BlockLightProperties blockLight) {
            if (blockLight.Emits) AddLight(x, y, blockLight.Emits, blockLight.Color, blockLight.Propagation);
            else if (blockLight.Occludes) AddDarkness(x, y);

            return blockLight.Emits || blockLight.Occludes;
        }

        public bool AddBackground(int x, int y, BlockLightProperties blockLight) {
            if (blockLight.Occludes) {
                if (blockLight.Emits) AddLight(x, y, blockLight.Emits, blockLight.Color, blockLight.Propagation);
                else AddDarkness(x, y);
            }
            
            return blockLight.Occludes;
        }

        public void AddLight(int x, int y, bool emit, LightColor color, int propagation) {
            if (!InMap(x, y)) return;

            _emit[x, y] = emit;
            _lightColors[x, y] = color;
            _propagation[x, y] = propagation;
        }

        public void AddDarkness(int x, int y) => AddLight(x, y, false, LightColor.black, 0);

        public void Propagate() {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    if (_emit[x, y])
                        PropagateFrom(x, y, _lightColors[x, y], _propagation[x, y]);
        }

        public bool InMap(int x, int y) => !(x < 0 || x >= Width || y < 0 || y >= Height);

        private float _normalDropoff = 0.7f;
        private float _diagonalDropoff => _normalDropoff * _normalDropoff;
        private int lookX;
        private int lookY;
        private LightColor _target;
        private int distance;
        private void PropagateFrom(int x, int y, LightColor color, int propagation) {
            for (int i = -propagation; i <= propagation; i++) {
                lookX = x + i;
                for (int j = -propagation; j <= propagation; j++) {
                    lookY = y + j;

                    if (!InMap(lookX, lookY)) continue;
                    if (_emit[lookX, lookY]) continue;

                    distance = Math.Max(Math.Abs(i), Math.Abs(j));

                    _target = color * (float)(distance > propagation - 3 ? Math.Pow((i != 0 && j != 0) ? _diagonalDropoff : _normalDropoff, distance - (propagation - 3)) : 1);

                    if (!_lightColors[lookX, lookY].Equals(LightColor.black))
                    {
                        _target.R = MathF.Max(_target.R, _lightColors[lookX, lookY].R);
                        _target.G = MathF.Max(_target.G, _lightColors[lookX, lookY].G);
                        _target.B = MathF.Max(_target.B, _lightColors[lookX, lookY].B);
                    }

                    _lightColors[lookX, lookY] = _target;
                }
            }
        }

        private Color[] _colorMap;
        public void CreateColorMap() {
            _colorMap = new Color[Width * Height];

            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    _colorMap[y * Width + x] = _lightColors[x, y].Color;
        }

        public Color[] GetColors() => _colorMap;
        public int GetColorCount() => _colorMap.Length;
    }

    public struct LightColor {
        public static LightColor black = new LightColor(0, 0, 0);
        public static LightColor white = new LightColor(255f, 255f, 255f);
        public static LightColor ambient = new LightColor(255f, 255f, 255f);
        public static LightColor cave = new LightColor(40f, 40f, 40f);

        public float R;
        public float G;
        public float B;

        public Color Color => new Color(R / 255f, G / 255f, B / 255f);

        public LightColor(float r, float g, float b) {
            R = Math.Clamp(r, 0, 255f);
            G = Math.Clamp(g, 0, 255f);
            B = Math.Clamp(b, 0, 255f);
        }

        public bool IsCutoff(float cutoff) => R > cutoff || G > cutoff || B > cutoff;
        public bool Overpowers(LightColor other) => R > other.R || G > other.G || B > other.B;
        public bool Equals(LightColor other) => R == other.R && G == other.G && B == other.B;
        public static LightColor FromColor(Color color) => new LightColor(color.R, color.G, color.B);

        public static LightColor operator *(LightColor a, LightColor b) {
            a.R *= b.R;
            a.G *= b.G;
            a.B *= b.B;
            return a;
        }

        public static LightColor operator *(LightColor a, float multiplier) {
            a.R *= multiplier;
            a.G *= multiplier;
            a.B *= multiplier;
            return a;
        }
    }
}