using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project
{
    public class EnemyBullet : GameObject
    {
        private GameObject owner;
        private Vector2 direction;

        public override void Initialize()
        {
            //initialize all the variables
            speed = 300.0f;
            name = "enemyBullet";           
            texture = Game1.assets["enemyBullet"];           
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
            if (owner != null)
            {
                orientation = owner.orientation;
                position = owner.position;
            }
            direction = Game1.player.position + new Vector2 (Game1.screenWidth, Game1.screenHeight);
        }

        public void setOwner(GameObject owner)
        {
            this.owner = owner;
        }
       
        public override void Update(GameTime gameTime)
        {
            Vector2 velocity = direction - position;
            velocity.Normalize();
            velocity *= speed;
            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;          

            if (position.X > Game1.screenWidth || position.X < 0 || position.Y > Game1.screenHeight)
            {
                Game1.enemyBulletList.Remove(this);
            }
        }


        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texture, position, null, Color.White, orientation, origin, 1.0f, SpriteEffects.None, 1.0f);
        }

    }
  
}
