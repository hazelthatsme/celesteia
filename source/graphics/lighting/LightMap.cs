using System;
using Celesteia.Resources.Collections;
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

        public void AddLight(int x, int y, BlockType blockType) {
            AddLight(x, y, blockType.Light.Emits, blockType.Light.Color, blockType.Light.Propagation);
        }

        public void AddLight(int x, int y, bool emit, LightColor color, int propagation) {
            _emit[x, y] = emit;
            _lightColors[x, y] = color;
            _propagation[x, y] = propagation;
        }

        public void AddDarkness(int x, int y) {
            _emit[x, y] = false;
            _lightColors[x, y] = LightColor.black;
            _propagation[x, y] = 0;
        }

        private Color[] _colorMap;
        public void CreateColorMap() {
            _colorMap = new Color[Width * Height];

            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    _colorMap[y * Width + x] = _lightColors[x, y].GetColor();
        }

        public Color[] GetColors() => _colorMap;
        public int GetColorCount() => _colorMap.Length;

        private Vector2 _position;
        public Vector2 Position => _position;
        public void SetPosition(Vector2 v) => _position = v;
    }

    public struct LightColor {
        public static LightColor black = new LightColor(0, 0, 0);
        public static LightColor ambient = new LightColor(51f, 51f, 51f);
        public static LightColor cave = new LightColor(10f, 10f, 10f);

        public float R;
        public float G;
        public float B;

        public LightColor(float r, float g, float b) {
            R = Math.Clamp(r, 0, 255f);
            G = Math.Clamp(g, 0, 255f);
            B = Math.Clamp(b, 0, 255f);
        }

        public Color GetColor() {
            return new Color(R / 255f, G / 255f, B / 255f);
        }

        public bool IsCutoff(float cutoff) {
            return R > cutoff || G > cutoff || B > cutoff;
        }

        public bool Overpowers(LightColor other) {
            return R > other.R || G > other.G || B > other.B;
        }

        public bool Equals(LightColor other)
        {
            return R == other.R && G == other.G && B == other.B;
        }

        public static LightColor FromColor(Color color) {
            return new LightColor(color.R, color.G, color.B);
        }

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