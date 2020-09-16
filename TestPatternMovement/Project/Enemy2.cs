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
            health = 1;
            speed = 100.0f;
            fireTime = 0f;
            fireRate = 100.0f;
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
            //Make sure it's is adding up to 1, so 0.3 will give a gap, but 0.2 will work
            float resolution = 0.2f;

            //How many times should we loop?
            int loops = (int)Math.Floor(1f / resolution);

            Vector2 lastPos = position;

            for (int i = 1; i <= loops; i++)
            {
                //Which t position are we at?
                float t = i * resolution;

                //Find the coordinate between the end points with a Catmull-Rom spline
                Vector2 newPos = GetCatmullRomPosition(t, new Vector2(0, 0), new Vector2(100, 100),
                new Vector2(50, 50), new Vector2(Game1.screenWidth, Game1.screenHeight));
                //Vector2 velocity = newPos - position;
                newPos.Normalize();

                newPos *= speed;
                position += newPos * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Orientation(velocity);

                //Save this pos so we can draw the next line segment
                lastPos = newPos;
            }
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
