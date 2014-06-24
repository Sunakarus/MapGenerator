using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Prototype
{
    internal class Settings
    {
        private KeyboardState state, prevState;
        private Controller controller;
        private int chosen = 0;
        private bool changing = false;

        private const int MAXSETTINGS = 200;

        private enum Setting { MapSize, ChanceToTurn, ChanceToBranch };

        private Setting[] optKeys = { Setting.MapSize, Setting.ChanceToTurn, Setting.ChanceToBranch };

        private Dictionary<Setting, int> options = new Dictionary<Setting, int>{
                    {Setting.MapSize, 100},
                    {Setting.ChanceToTurn, 5},
                    {Setting.ChanceToBranch, 5}};

        public Settings(Controller controller)
        {
            state = Keyboard.GetState();
            this.controller = controller;
        }

        public void Update()
        {
            prevState = state;
            state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Enter) && prevState.IsKeyUp(Keys.Enter))
            {
                if (changing)
                {
                    controller.map = new Map(controller, options[Setting.MapSize], options[Setting.MapSize], options[Setting.ChanceToTurn], options[Setting.ChanceToBranch]);
                }
                changing = !changing;
            }
            if (state.IsKeyDown(Keys.Left) && prevState.IsKeyUp(Keys.Left) && changing)
            {
                if (options[optKeys[chosen]] > 10)
                {
                    options[optKeys[chosen]] -= 10;
                }
                else
                {
                    options[optKeys[chosen]] = 1;
                }
            }
            if (state.IsKeyDown(Keys.Right) && prevState.IsKeyUp(Keys.Right) && changing)
            {
                if (options[optKeys[chosen]] < MAXSETTINGS - 10)
                {
                    options[optKeys[chosen]] += 10;
                }
                else
                {
                    options[optKeys[chosen]] = MAXSETTINGS;
                }
            }
            if (state.IsKeyDown(Keys.Up) && prevState.IsKeyUp(Keys.Up))
            {
                if (changing)
                {
                    if (options[optKeys[chosen]] < MAXSETTINGS)
                    {
                        options[optKeys[chosen]]++;
                    }
                }
                else
                {
                    if (chosen > 0)
                    {
                        chosen--;
                    }
                    else
                    {
                        chosen = options.Count - 1;
                    }
                }
            }
            if (state.IsKeyDown(Keys.Down) && prevState.IsKeyUp(Keys.Down))
            {
                if (changing)
                {
                    if (options[optKeys[chosen]] > 1)
                    {
                        options[optKeys[chosen]]--;
                    }
                }
                else
                {
                    if (chosen < options.Count - 1)
                    {
                        chosen++;
                    }
                    else
                    {
                        chosen = 0;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int i = -1;
            Color color = Color.Orange;
            float scale = 1 / controller.camera.scale;

            foreach (KeyValuePair<Setting, int> kvp in options)
            {
                i++;
                if (i == chosen)
                {
                    if (changing)
                    {
                        color = Color.Red;
                    }
                    else
                    {
                        color = Color.Gold;
                    }
                }
                else
                {
                    color = Color.Orange;
                }

                string result = string.Format("{0}: {1}", kvp.Key, kvp.Value);
                if (i == 1 || i == 2)
                {
                    result = string.Format("{0}: 1/{1}", kvp.Key, kvp.Value);
                }
                spriteBatch.DrawString(Art.font, result, new Vector2(controller.camera.position.X * 32 + 400 * scale, controller.camera.position.Y * 32 + 20 * i * scale), color, 0, Vector2.Zero, scale, SpriteEffects.None, -10);
            }
        }

        public object List { get; set; }
    }
}