using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Project
{
    public class ScrollingBackground
    {
        public Texture2D texture;
        public Rectangle rec;

        public void Initialize(Texture2D texture, Rectangle rec)
        {
            this.texture = texture;
            this.rec = rec;
        }

        public void Update()
        {
            rec.Y += 2;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texture, rec, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.1f);
        }
    }
}
