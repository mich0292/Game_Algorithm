using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

//References for the Dynamic Wandering
//https://gamedevelopment.tutsplus.com/tutorials/understanding-steering-behaviors-wander--gamedev-1624
//https://answers.unity.com/questions/794603/dynamic-wander-ai.html
namespace Project
{
    public class Asteroid : GameObject
    {
        private float wanderAngle, wanderTime;
        private static Random rand;

        private const float WANDER_OFFSET = 5.0f;
        private const float WANDER_RADIUS = 2.0f;
        private const float WANDER_RATE = 50.0f; //in miliseconds
        private const float ANGLE_CHANGE = 0.5f;

        public override void Initialize()
        {
            //initialize all the variables
            health = 1;
            speed = 100.0f;
            name = "asteroid";
            texture = Game1.assets["asteroid"];
            orientation = 0f;
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
            rand = new Random(DateTime.Now.Ticks.GetHashCode());
            velocity = new Vector2(0.0f, 1.0f);
            position = new Vector2(rand.Next(0, Game1.screenWidth), 0);
            wanderTime = 0.0f;
        }

        public override void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds > wanderTime )
            {
                wanderTime = (float)gameTime.TotalGameTime.TotalMilliseconds + WANDER_RATE;
                Vector2 wanderForce = Wander();
                velocity += wanderForce;
                velocity.Normalize();
                velocity *= speed;
            }
            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Orientation(velocity);

            if (position.X > Game1.screenWidth|| position.X < 0 || position.Y > Game1.screenHeight
                || position.Y < 0)
            {
                Game1.enemyList.Remove(this);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texture, position, null, Color.White, orientation, origin, 1.0f, SpriteEffects.None, 0.99f);
        }

        public void Orientation(Vector2 velocity)
        {
            orientation = (float)Math.Atan2(velocity.Y, velocity.X);
        }

        public Vector2 Wander()
        {
            //Offset so that the vector starts from the circle of the center,
            //which is slightly in front of the player
            velocity.Normalize();
            Vector2 circleCenter = velocity * WANDER_OFFSET;

            //Debug.WriteLine("circle Center = ", circleCenter.ToString());
            //Debug.WriteLine("velocity = ", velocity.ToString());

            //Find the displacement (a point around the circle circumference)
            double randomNumber = rand.NextDouble();
            float numberInRightRange = MathHelper.Lerp(-1, +1, (float)randomNumber);
            //Debug.WriteLine(numberInRightRange);
            Vector2 displacement = new Vector2(numberInRightRange, 0) * WANDER_RADIUS;
            displacement = setAngle(displacement, wanderAngle);

            wanderAngle += ((float)rand.NextDouble() * ANGLE_CHANGE) - (ANGLE_CHANGE * 0.5f);

            Vector2 wanderForce = circleCenter + displacement;
            return wanderForce;

        }

        public Vector2 setAngle(Vector2 vector, float num)
        {
            float len = vector.Length();
            vector.X = (float)Math.Cos(num) * len;
            vector.Y = (float)Math.Sin(num) * len;
            return vector;
        }

    }
}
