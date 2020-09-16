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
    public abstract class GameObject
    {
        public int health;
        public float speed;
        public Vector2 position; 
        public Vector2 velocity; // the direction vector
        public Vector2 origin;
        //Creating bounding box
        //https://www.dreamincode.net/forums/topic/180069-xna-2d-bounding-box-collision-detection/
        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    texture.Width,
                    texture.Height
                    );
            }
        }
        public float orientation;
        public float fireRate;
        public float fireTime;
        public string name;
        public Texture2D texture;

        public abstract void Initialize();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);

        //https://deepnight.net/tutorial/bresenham-magic-raycasting-line-of-sight-pathfinding/
        //https://gamedev.stackexchange.com/questions/26813/xna-2d-line-of-sight-check
        //https://en.wikipedia.org/wiki/Bresenham's_line_algorithm
        //https://community.monogame.net/t/building-boundingbox/8276/8
        //https://www.redblobgames.com/articles/visibility/
        public bool InLOS(float AngleDistance, float PositionDistance, Vector2 PositionA, Vector2 PositionB, float AngleB)
        {
            //Check the distance and angle between the enemy and the player
            //If both true, run bresenham check to see if anything is in between
            float AngleBetween = (float)Math.Atan2((PositionA.Y - PositionB.Y), (PositionA.X - PositionB.X));
            if ((AngleBetween <= (AngleB + (AngleDistance / 2f / 100f))) && (AngleBetween >= (AngleB - (AngleDistance / 2f / 100f)))
                && (Vector2.Distance(PositionA, PositionB) <= PositionDistance))
            {
                foreach (Point p in BresenhamLine((int)PositionA.X, (int)PositionA.Y, (int)PositionB.X, (int)PositionB.Y))
                {
                    //Console.WriteLine(p);                  
                    for (int i = 0; i < Game1.enemyList.Count; i++)
                    {
                        if (Game1.enemyList[i].position != position && Game1.enemyList[i].BoundingBox.Contains(p))
                        {
                            //Console.WriteLine(Game1.enemyList[i].BoundingBox.Contains(p));
                            return false;
                        }
                    }
                }
                return true;
            }
            else return false;
        }

        //https://www.youtube.com/watch?v=jNZh9wppUkY
        //https://gamedev.stackexchange.com/questions/11234/2d-ray-intersection
        public List<Point> BresenhamLine(int x0, int y0, int x1, int y1)
        {
            List<Point> result = new List<Point>();
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);

            if (steep)
            {
                int temp = x0;
                x0 = y0;
                y0 = temp;

                temp = x1;
                x1 = y1;
                y1 = temp;
            }

            //Check whether the line go left/ right
            if (x0 > x1)
            {
                int temp = x0;
                x0 = x1;
                x1 = temp;

                temp = y0;
                y0 = y1;
                y1 = temp;
            }

            int deltax = x1 - x0;               //difference between x value
            int deltay = Math.Abs(y1 - y0);     //difference between y value
            int error = 0;                      //the increment of x value before y increase
            int ystep;
            int y = y0;
            if (y0 < y1) ystep = 1; else ystep = -1;    //increment of y value to reach endPoint
            for (int x = x0; x <= x1; x++)
            {
                if (steep) result.Add(new Point(y, x));
                else result.Add(new Point(x, y));
                error += deltay;
                if (2 * error >= deltax)
                {
                    y += ystep;
                    error -= deltax;
                }
            }

            return result;
        }
    }
}
