using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project
{

    public class UI
    {
        public SpriteFont font;
        public string word;
        public Color color;

        public UI(string word, SpriteFont font, Color color)
        {
            this.word = word;
            this.font = font;
            this.color = color;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Vector2 length = font.MeasureString(word);
            spriteBatch.DrawString(font, word, new Vector2(Game1.screenWidth / 2 - length.X / 2, Game1.screenHeight / 4 - length.Y / 2), color);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Vector2 position)
        {
            Vector2 length = font.MeasureString(word);
            spriteBatch.DrawString(font, word, position, color);
        }
    }
}
