using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Prototype
{
    internal class Controller
    {
        public Map map { get; set; }

        public Settings settings;
        public Camera camera;

        public const int mapSize = 100;
        public const int chanceToTurn = 5; // probability = 1/chanceToTurn
        public const int chanceToBranch = 5; // probability = 1/chanceToBranch

        public Controller()
        {
            map = new Map(this, mapSize, mapSize, chanceToTurn, chanceToBranch);
            camera = new Camera(Point.Zero);
            settings = new Settings(this);
        }

        public void Update()
        {
            map.Update();
            camera.Update();
            settings.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            map.Draw(spriteBatch);
            settings.Draw(spriteBatch);
        }
    }
}