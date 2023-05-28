using System;
using Celesteia.Game.Components.Items;
using Microsoft.Xna.Framework;

namespace Celesteia.Game.World.Planet {
    public class ChunkMap {
        public int Width, Height;
        public int BlockWidth => Width * Chunk.CHUNK_SIZE;
        public int BlockHeight => Height * Chunk.CHUNK_SIZE;
        
        public Chunk[,] Map;

        public ChunkMap(int w, int h) {
            Width = w;
            Height = h;

            Map = new Chunk[w, h];
        }

        public Chunk GetChunk(int chunkX, int chunkY) => ChunkIsInMap(chunkX, chunkY) ? Map[chunkX, chunkY] : null;
        public Chunk GetChunk(ChunkVector cv) => GetChunk(cv.X, cv.Y);
        public Chunk GetChunkAtCoordinates(int x, int y) => GetChunk(x / Chunk.CHUNK_SIZE, y / Chunk.CHUNK_SIZE);
        public Chunk GetChunkAtPoint(Point point) => GetChunkAtCoordinates(point.X, point.Y);

        // BACKGROUND MANAGEMENT
        public byte GetBackground(int blockX, int blockY) {
            Chunk c = GetChunkAtCoordinates(blockX, blockY);
            return c != null ? c.GetBackground(blockX % Chunk.CHUNK_SIZE, blockY % Chunk.CHUNK_SIZE) : (byte)0;
        }
        public byte GetBackground(Point point) => GetBackground(point.X, point.Y);

        public void SetBackground(int blockX, int blockY, byte id) {
            Chunk c = GetChunkAtCoordinates(blockX, blockY);
            if (c != null) c.SetBackground(blockX % Chunk.CHUNK_SIZE, blockY % Chunk.CHUNK_SIZE, id);
        }
        public void SetBackground(Point point, byte id) => SetBackground(point.X, point.Y, id);

        public void AddBackgroundBreakProgress(int blockX, int blockY, int power, out ItemStack drops) {
            drops = null;
            Chunk c = GetChunkAtCoordinates(blockX, blockY);
            if (c != null) c.AddBreakProgress(blockX % Chunk.CHUNK_SIZE, blockY % Chunk.CHUNK_SIZE, power, true, out drops);
        }
        public void AddBackgroundBreakProgress(Point point, int power, out ItemStack drops) => AddBackgroundBreakProgress(point.X, point.Y, power, out drops);

        // FOREGROUND MANAGEMENT
        public byte GetForeground(int blockX, int blockY) {
            Chunk c = GetChunkAtCoordinates(blockX, blockY);
            return c != null ? c.GetForeground(blockX % Chunk.CHUNK_SIZE, blockY % Chunk.CHUNK_SIZE) : (byte)0;
        }
        public byte GetForeground(Point point) => GetForeground(point.X, point.Y);

        public void SetForeground(int blockX, int blockY, byte id) {
            Chunk c = GetChunkAtCoordinates(blockX, blockY);
            if (c != null) c.SetForeground(blockX % Chunk.CHUNK_SIZE, blockY % Chunk.CHUNK_SIZE, id);
        }
        public void SetForeground(Point point, byte id) => SetForeground(point.X, point.Y, id);

        public void AddForegroundBreakProgress(int blockX, int blockY, int power, out ItemStack drops) {
            drops = null;
            Chunk c = GetChunkAtCoordinates(blockX, blockY);
            if (c != null) c.AddBreakProgress(blockX % Chunk.CHUNK_SIZE, blockY % Chunk.CHUNK_SIZE, power, false, out drops);
        }
        public void AddForegroundBreakProgress(Point point, int power, out ItemStack drops) => AddForegroundBreakProgress(point.X, point.Y, power, out drops);

        // FOR ADJACENCY CHECKS
        public bool GetAny(int blockX, int blockY) {
            Chunk c = GetChunkAtCoordinates(blockX, blockY);
            return c != null && (
                c.GetForeground(blockX % Chunk.CHUNK_SIZE, blockY % Chunk.CHUNK_SIZE) != 0 ||
                c.GetBackground(blockX % Chunk.CHUNK_SIZE, blockY % Chunk.CHUNK_SIZE) != 0
            );
        }

        // CHUNK IN MAP CHECKS
        public bool ChunkIsInMap(int chunkX, int chunkY) => chunkX >= 0 && chunkY >= 0 && chunkX < Width && chunkY < Height;
        public bool ChunkIsInMap(ChunkVector cv) => ChunkIsInMap(cv.X, cv.Y);
    }
}