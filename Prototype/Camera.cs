using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Prototype
{
    internal class Camera
    {
        public Point position;
        private int tileSize;
        public float scale = 0.2f;
        private KeyboardState state, prevState;
        private int timer, maxTimer = 3;

        public Camera(Point position)
        {
            state = Keyboard.GetState();
            this.position = position;
            tileSize = 32;
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
            if (state.IsKeyDown(Keys.W) && timer <= 0)
            {
                position.Y += 2;
                timer = maxTimer;
            }
            if (state.IsKeyDown(Keys.A) && timer <= 0)
            {
                position.X += 2;
                timer = maxTimer;
            }
            if (state.IsKeyDown(Keys.S) && timer <= 0)
            {
                position.Y -= 2;
                timer = maxTimer;
            }
            if (state.IsKeyDown(Keys.D) && timer <= 0)
            {
                position.X -= 2;
                timer = maxTimer;
            }

            if (state.IsKeyDown(Keys.PageUp))
            {
                scale += 0.02f;
            }

            if (state.IsKeyDown(Keys.PageDown))
            {
                scale -= 0.02f;
            }
        }

        public Matrix GetTransformMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(-position.X * tileSize, -position.Y * tileSize, 0)) * Matrix.CreateScale(scale, scale, 0);
        }
    }
}