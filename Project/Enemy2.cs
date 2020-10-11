using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project
{
    class Enemy2:GameObject
    {
        private List<Vector2> controlPointsList;
        private List<Vector2> positionList;
        private int wayPointCounter = 0;
        public override void Initialize()
        {
            //initialize all the variables
            health = 7;
            speed = 150.0f;
            fireTime = 0f;
            fireRate = 300.0f;
            orientation = 0f;
            texture = Game1.assets["enemy2"];
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
            controlPointsList = new List<Vector2>
            {
                new Vector2(0, 20),
                new Vector2(80, 90),
                new Vector2(160, 200),
                new Vector2(240, 90),
                new Vector2(320, 20),
                new Vector2(400, 90),
                new Vector2(480, 200),
                new Vector2(560, 90),
                new Vector2(640, 20),
                new Vector2(720, 90),
                new Vector2(800, 200),
                new Vector2(Game1.screenWidth + 100, Game1.screenHeight),
                new Vector2(Game1.screenWidth + 300, Game1.screenHeight)
                //new Vector2(650, 50),
                //new Vector2(750, 0),
            };
            positionList = new List<Vector2>();
            PatternMovement();
        }

        public override void Update(GameTime gameTime)
        {
            MoveAlongSpline(gameTime);
            LineOfSight(gameTime);
        }

        public void changeControlPoints()
        {
            controlPointsList.Clear();
            positionList.Clear();
            controlPointsList = new List<Vector2>
            {
                new Vector2(240, 20),
                new Vector2(160, 20),
                new Vector2(320, 240),
                new Vector2(480, 20),
                new Vector2(640, 240),
                new Vector2(800, 20),
                new Vector2(Game1.screenWidth + 100, 0),
                new Vector2(Game1.screenWidth + 300, 0)
                //new Vector2(650, 50),
                //new Vector2(750, 0),
            };
            PatternMovement();
        }
        private void MoveAlongSpline(GameTime gameTime)
        {
            
            Vector2 velocity = positionList[wayPointCounter] - position;
            float distance = velocity.Length();
            if (distance > 2.0f)
            {
                velocity.Normalize();
                velocity *= speed;
                position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Orientation(velocity);
            }
            else
            {
                if (wayPointCounter != positionList.Count - 1)
                    wayPointCounter++;
            }
        }

        //Pattern movement - Catmull-rom spline
        //Two ways of doing it (simplified version):
        // https://www.habrador.com/tutorials/interpolation/1-catmull-rom-splines/
        // https://medium.com/@PritishCh/camera-systems-part-2-adcf59aa8259
        // More complicated way (based on matrixes?):
        // https://andrewhungblog.wordpress.com/2017/03/03/catmull-rom-splines-in-plain-english/
        // How to make the object move along the spline
        // https://www.dreamincode.net/forums/topic/293148-getting-an-object-to-follow-a-bezier-curve/
        private void PatternMovement()
        {
            for (int i = 0; i < controlPointsList.Count; i++)
            {
                if (i == 0 || i == controlPointsList.Count - 1 || i == controlPointsList.Count - 2)
                    continue;
                FindCatmullRomSpline(i);
            }      
        }

        void FindCatmullRomSpline(int pos)
        {
            //Four points needed to form a spline between p1 and p2
            Vector2 p0 = controlPointsList[pos - 1];
            Vector2 p1 = controlPointsList[pos];
            Vector2 p2 = controlPointsList[pos + 1];
            Vector2 p3 = controlPointsList[pos + 2];

            //Start position
            Vector2 lastPos = p1;

            //The spline's resolution (make sure it adds up to 1, 0.2+0.2+0.2+0.2 = 0.8, or 0.3
            float resolution = 0.2f;

            //How many times to loop
            int loops = (int)Math.Floor(1f / resolution);
            for (int i = 1; i <= loops; i++)
            {
                float t = i * resolution;   // t position
                Vector2 newPos = GetCatmullRomPosition(t, p0, p1, p2, p3);
                positionList.Add(newPos);
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
            Vector2 pos = 0.5f * (a + (b * distance) + (c * distance * distance) + (d * distance * distance * distance));

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
