using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project
{
    class Bullet : GameObject
    {
        public override void Initialize()
        {
            //initialize all the variables
            speed = 100.0f;
            name = "bullet";
            position = Game1.player.position;
            texture = Game1.assets["bullet"];
        }

        public override void Update(GameTime gameTime)
        {
            position.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texture, position,
                origin: new Vector2(texture.Width / 2.0f, texture.Height / 2.0f));
        }
    }
}
