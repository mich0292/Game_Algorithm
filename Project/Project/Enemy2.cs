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
    class Enemy2:GameObject
    {
        public override void Initialize()
        {
            //initialize all the variables
            health = 1;
            speed = 100.0f;


        }

        public override void Update(GameTime gameTime)
        {
            //Kinematic Seek 
            Vector2 velocity = Player.player.position - position;
            velocity.Normalize();

            velocity *= speed;
            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Orientation(velocity);

            //line of sight
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void Orientation(Vector2 velocity)
        {
            orientation = (float)Math.Atan2(velocity.Y, velocity.X);
        }
    }
}
