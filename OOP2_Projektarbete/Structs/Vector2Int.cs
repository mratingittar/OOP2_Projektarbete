﻿using Skalm.Utilities;

namespace Skalm.Structs
{
    internal struct Vector2Int
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Vector2Int(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public void Normalize()
        {
            this.X = Math.Sign(this.X);
            this.Y = Math.Sign(this.Y);
        }

        public Vector2Int Add(Vector2Int added) => new Vector2Int(this.X + added.X, this.Y + added.Y);
        public Vector2Int Remove(Vector2Int removed) => new Vector2Int(this.X - removed.X, this.Y - removed.Y);

        public static Vector2Int DirectionFromTo(Vector2Int start, Vector2Int end)
        {
            int x, y;

            if (end.X < start.X)
                x = -1;
            else if (end.X > start.X)
                x = 1;
            else x = 0;

            if (end.Y < start.Y)
                y = -1;
            else if (end.Y > start.Y)
                y = 1;
            else
                y = 0;

            return new Vector2Int(x, y);
        }

        public static Vector2Int DirectionToVector2Int(Direction dir) => dir switch
        {
            Direction.North => new Vector2Int(0, -1),
            Direction.East => new Vector2Int(1, 0),
            Direction.South => new Vector2Int(0, 1),
            Direction.West => new Vector2Int(-1, 0),
            _ => throw new InvalidOperationException("Not a direction"),
        };

        public static Vector2Int Zero { get => new Vector2Int(0, 0); }
        public static Vector2Int Up { get => new Vector2Int(0, -1); }
        public static Vector2Int Right { get => new Vector2Int(1, 0); }
        public static Vector2Int Down { get => new Vector2Int(0, 1); }
        public static Vector2Int Left { get => new Vector2Int(-1, 0); }

        public static bool Equals(Vector2Int a, Vector2Int b)
        {
            return (a.X == b.X) && (a.Y == b.Y);
        }
    }
}
