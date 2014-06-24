using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Prototype
{
    internal class Map
    {
        public int[,] mapArray;
        private Controller controller;
        private int width;
        private int height;
        public List<Path> paths = new List<Path>();
        public Random ran = new Random();
        private KeyboardState state, prevState;

        private int chanceToTurn;
        private int chanceToBranch;

        private int timer, maxTimer = 2;

        public Map(Controller controller, int width, int height, int chanceToTurn, int chanceToBranch)
        {
            timer = maxTimer;
            state = Keyboard.GetState();

            this.controller = controller;
            this.width = width;
            this.height = height;
            this.chanceToBranch = chanceToBranch;
            this.chanceToTurn = chanceToTurn;

            mapArray = new int[width, height];
            if (width < 1) width = 1;
            if (height < 1) height = 1;
            Reset();
        }

        public void Reset()
        {
            paths.Clear();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    mapArray[x, y] = 0; //0 == not path
                }
            }
            paths.Add(new Path(this, Path.Direction.Down, new Point(width / 2, height / 2), chanceToTurn, chanceToBranch));
            paths.Add(new Path(this, Path.Direction.Up, new Point(width / 2, height / 2), chanceToTurn, chanceToBranch));
            paths.Add(new Path(this, Path.Direction.Left, new Point(width / 2, height / 2), chanceToTurn, chanceToBranch));
            paths.Add(new Path(this, Path.Direction.Right, new Point(width / 2, height / 2), chanceToTurn, chanceToBranch));
            timer = maxTimer;
        }

        public void Update()
        {
            prevState = state;
            state = Keyboard.GetState();
            if (timer > 0)
            {
                timer--;
            }
            if (state.IsKeyDown(Keys.Space) && timer <= 0)
            {
                Step();
                timer = maxTimer;
            }
            if (state.IsKeyDown(Keys.R) && prevState.IsKeyUp(Keys.R))
            {
                Reset();
            }
        }

        public void Step()
        {
            //  while (paths.Count > 0)
            // {
            for (int i = paths.Count - 1; i > -1; i--)
            {
                if (!paths[i].Step())
                {
                    paths.RemoveAt(i);
                }
            }
            //   }
        }

        public bool OutOfBounds(int x, int y)
        {
            return x < 0 || y < 0 || x >= width || y >= height;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            float scale = 0.5f;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (mapArray[x, y] == 0)
                    {
                        spriteBatch.Draw(Art.wall, new Vector2(x * 32 * scale, y * 32 * scale), null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                    }
                    else
                    {
                        int color = mapArray[x, y];
                        string colorStr = color.ToString();
                        while (colorStr.Length < 9)
                        {
                            colorStr = "0" + colorStr;
                        }

                        int r = int.Parse(colorStr.Substring(0, 3));
                        int g = int.Parse(colorStr.Substring(3, 3));
                        int b = int.Parse(colorStr.Substring(6, 3));

                        spriteBatch.Draw(Art.blank, new Vector2(x * 32 * scale, y * 32 * scale), null, new Color(r, g, b), 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                    }
                }
            }
        }
    }
}