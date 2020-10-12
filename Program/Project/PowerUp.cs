using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project
{
    class PowerUp : GameObject
    {
        public bool alive;
        public override void Initialize()
        {
            //initialize all the variables
            health = 1;
            speed = 0.0f;
            name = "powerUp";
            position = Game1.player.position - new Vector2 (100, 100);
            texture = Game1.assets["powerup"];
            orientation = 0f;
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texture, position, null, Color.White, orientation, origin, 1.0f, SpriteEffects.None, 0.99f);
        }
    }              
   
}
