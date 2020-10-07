using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project
{
    class Enemy2:GameObject
    {
        public override void Initialize()
        {
            //initialize all the variables
            health = 7;
            speed = 100.0f;
            fireTime = 0f;
            fireRate = 300.0f;
            orientation = 0f;
            texture = Game1.assets["enemy2"];
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
        }

        public override void Update(GameTime gameTime)
        {
            PatternMovement(gameTime);
            LineOfSight(gameTime);
        }

        //Pattern movement - Catmull-rom spline
        //Two ways of doing it (simplified version):
        // https://www.habrador.com/tutorials/interpolation/1-catmull-rom-splines/
        // https://medium.com/@PritishCh/camera-systems-part-2-adcf59aa8259
        // More complicated way (based on matrixes?):
        // https://andrewhungblog.wordpress.com/2017/03/03/catmull-rom-splines-in-plain-english/
        public void PatternMovement(GameTime gameTime)
        {
            //Find the coordinate between the end points with a Catmull-Rom spline
            Vector2 newPos = GetCatmullRomPosition(200f * (float)gameTime.TotalGameTime.TotalMilliseconds, new Vector2(0, 0), new Vector2(100, 100),
            new Vector2(50, 50), new Vector2(Game1.screenWidth, Game1.screenHeight));
            Vector2 velocity = newPos - position;
            velocity.Normalize();

            velocity *= speed;
            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Orientation(velocity);

            //Save this pos so we can draw the next line segment        
        }

        //Returns a position between 4 Vector3 with Catmull-Rom spline algorithm
        //http://www.iquilezles.org/www/articles/minispline/minispline.htm
        Vector2 GetCatmullRomPosition(float distance, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            //The coefficients of the cubic polynomial 
            Vector2 a = 2f * p1;
            Vector2 b = p2 - p0;
            Vector2 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
            Vector2 d = -p0 + 3f * p1 - 3f * p2 + p3;

            //The cubic polynomial: a + b * t + c * t^2 + d * t^3
            Vector2 pos =  a + (b * distance) + (c * distance * distance) + (d * distance * distance * distance);

            return pos;
        }

        public void LineOfSight(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds > fireTime)
            {
                fireTime = (float)gameTime.TotalGameTime.TotalMilliseconds + fireRate;

                if (InLOS(90, 300, Game1.player.position, position, orientation))
                {
                    EnemyBullet tempBullet = new EnemyBullet();
                    tempBullet.setOwner(this);
                    tempBullet.Initialize();
                    Game1.enemyBulletList.Add(tempBullet);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texture, position, null, Color.White, orientation, origin, 1.0f, SpriteEffects.None, 1.0f);
        }

        //Reference from notes Lecture 3, part of kinematic seek
        public void Orientation(Vector2 velocity)
        {
            orientation = (float)Math.Atan2(velocity.Y, velocity.X);
        }
    }
}
