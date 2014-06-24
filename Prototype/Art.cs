using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Prototype
{
    internal class Art
    {
        public static Texture2D wall, blank;
        public static SpriteFont font;

        public void Load(ContentManager Content)
        {
            wall = Content.Load<Texture2D>("wall");
            blank = Content.Load<Texture2D>("floor");
            font = Content.Load<SpriteFont>("tahoma");
        }
    }
}