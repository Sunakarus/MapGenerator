using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Prototype
{
    internal class Path
    {
        public enum Direction { Left, Right, Up, Down };

        private Direction direction;
        private Point position;
        private Map map;

        private int chanceToTurn; // probability = 1/chanceToTurn
        private int chanceToBranch; // probability = 1/chanceToBranch
        public int color;

        public Path(Map map, Direction direction, Point position, int chanceToTurn, int chanceToBranch)
        {
            this.direction = direction;
            this.position = position;
            this.map = map;
            this.chanceToBranch = chanceToBranch;
            this.chanceToTurn = chanceToTurn;

            color = GenerateRandomColor(map);

            map.mapArray[position.X, position.Y] = color;
        }

        private int GenerateRandomColor(Map map)
        {
            string temp = "";
            for (int i = 0; i < 3; i++)
            {
                color = map.ran.Next(0, 255);

                if (color.ToString().Length == 1)
                {
                    temp += "00";
                }
                else if (color.ToString().Length == 2)
                {
                    temp += "0";
                }
                temp += color;
            }
            return int.Parse(temp);
        }

        public bool Step()
        {
            Point temp = new Point(position.X, position.Y);
            Point future = Move(position, direction);
            if (temp.Equals(future))
            {
                return false;
            }
            else
            {
                position = future;
            }
            map.mapArray[position.X, position.Y] = color;
            if (map.ran.Next(chanceToTurn) == 0)
            {
                Turn();
            }
            if (map.ran.Next(chanceToBranch) == 0)
            {
                Path newBranch;
                if (Branch(out newBranch))
                {
                    map.paths.Add(newBranch);
                }
            }
            return true;
        }

        public bool Branch(out Path result)
        {
            List<Direction> dirs = GetPerpDirs();
            while (dirs.Count > 0)
            {
                Direction chosen = dirs[map.ran.Next(0, dirs.Count)];
                Point temp = Move(position, chosen);
                if (!CheckNeighbours(temp, chosen))
                {
                    result = new Path(map, chosen, temp, chanceToTurn, chanceToBranch);
                    return true;
                }
                else
                {
                    dirs.Remove(chosen);
                }
            }
            result = new Path(map, direction, position, chanceToTurn, chanceToBranch);
            return false;
        }

        private List<Direction> GetPerpDirs()
        {
            List<Direction> dirs = new List<Direction>();
            if (direction == Direction.Down || direction == Direction.Up)
            {
                dirs.Add(Direction.Right);
                dirs.Add(Direction.Left);
            }
            else if (direction == Direction.Left || direction == Direction.Right)
            {
                dirs.Add(Direction.Down);
                dirs.Add(Direction.Up);
            }
            return dirs;
        }

        public void Turn()
        {
            List<Direction> dirs = GetPerpDirs();
            while (dirs.Count > 0)
            {
                Direction chosen = dirs[map.ran.Next(0, dirs.Count)];
                Point temp = Move(position, chosen);
                if (!CheckNeighbours(temp, chosen))
                {
                    direction = chosen;
                    return;
                }
                else
                {
                    dirs.Remove(chosen);
                }
            }
        }

        public bool CheckNeighbours(Point pos, Direction dir)
        {
            Point tempPoint = pos;
            if (!map.OutOfBounds(pos.X - 1, pos.Y) && map.mapArray[pos.X - 1, pos.Y] != 0 && dir != Direction.Right)
            {
                return true;
            }
            if (!map.OutOfBounds(pos.X + 1, pos.Y) && map.mapArray[pos.X + 1, pos.Y] != 0 && dir != Direction.Left)
            {
                return true;
            }
            if (!map.OutOfBounds(pos.X, pos.Y - 1) && map.mapArray[pos.X, pos.Y - 1] != 0 && dir != Direction.Down)
            {
                return true;
            }
            if (!map.OutOfBounds(pos.X, pos.Y + 1) && map.mapArray[pos.X, pos.Y + 1] != 0 && dir != Direction.Up)
            {
                return true;
            }
            return false;
        }

        private Point Move(Point orig, Direction direction)
        {
            Point result = new Point(orig.X, orig.Y);
            switch (direction)
            {
                case Direction.Down:
                    if (!map.OutOfBounds(orig.X, orig.Y + 1) && map.mapArray[orig.X, orig.Y + 1] == 0)
                    {
                        result.Y++;
                    }
                    break;

                case Direction.Left:
                    if (!map.OutOfBounds(orig.X - 1, orig.Y) && map.mapArray[orig.X - 1, orig.Y] == 0)
                    {
                        result.X--;
                    }
                    break;

                case Direction.Up:
                    if (!map.OutOfBounds(orig.X, orig.Y - 1) && map.mapArray[orig.X, orig.Y - 1] == 0)
                    {
                        result.Y--;
                    }
                    break;

                case Direction.Right:
                    if (!map.OutOfBounds(orig.X + 1, orig.Y) && map.mapArray[orig.X + 1, orig.Y] == 0)
                    {
                        result.X++;
                    }
                    break;
            }
            return result;
        }
    }
}