using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Project
{
    public class Asteroid : GameObject
    {
        private float displace;
        private Vector2 velocity;
        private static Random rand;

        public override void Initialize()
        {
            //initialize all the variables
            health = 1;
            speed = 100.0f;
            name = "asteroid";
            texture = Game1.assets["asteroid"];
            orientation = 0f;
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
            rand = new Random();
            velocity = new Vector2(0.0f, 1.0f);
            displace = 20f;
            position = new Vector2(rand.Next(0, Game1.screenWidth), Game1.screenHeight);
        }

        //reference from Lab code
        // https://gamedevelopment.tutsplus.com/tutorials/understanding-steering-behaviors-wander--gamedev-1624
        public override void Update(GameTime gameTime)
        {
            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 displacement = new Vector2(rand.Next(-1, 2) * displace, rand.Next(-1, 2) * displace);
            velocity += displacement;
            velocity.Normalize();
            velocity *= speed;
            Orientation(velocity);

            if (position.X < 0)
                position.X = Game1.window.ClientBounds.Width - 30;
            else if (position.X > Game1.window.ClientBounds.Width)
                position.X = 0;

            if (position.Y < 0)
                position.Y = Game1.window.ClientBounds.Height - 30;
            else if (position.Y > Game1.window.ClientBounds.Height)
                position.Y = 0;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texture, position, null, Color.White, orientation, origin, 1.0f, SpriteEffects.None, 1.0f);
        }

        public void Orientation(Vector2 velocity)
        {
            orientation = (float)Math.Atan2(velocity.Y, velocity.X);
        }
    }
}
