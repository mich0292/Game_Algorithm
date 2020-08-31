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
    class Enemy1:GameObject
    {
        public override void Initialize()
        {
            //initialize all the variables
            health = 1;
            speed = 100.0f;
            fireTime = 0f;
            fireRate = 100.0f;
            orientation = 0f;
            texture = Game1.assets["enemy1"];
            position = new Vector2(0, 0);
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
        }

        public override void Update(GameTime gameTime)
        {
            KinematicSeek(gameTime);
            LineOfSight(gameTime);
        }

        //Reference from notes Lecture 3
        public void KinematicSeek(GameTime gameTime)
        {
            Vector2 velocity = Game1.player.position - position;
            velocity.Normalize();
            velocity *= speed;
            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Orientation(velocity);
        }

        //https://deepnight.net/tutorial/bresenham-magic-raycasting-line-of-sight-pathfinding/
        //https://gamedev.stackexchange.com/questions/26813/xna-2d-line-of-sight-check
        //https://en.wikipedia.org/wiki/Bresenham's_line_algorithm
        //https://community.monogame.net/t/building-boundingbox/8276/8
        public void LineOfSight(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds > fireTime)
            {
                fireTime = (float)gameTime.TotalGameTime.TotalMilliseconds + fireRate;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texture, position, null, Color.White, orientation, origin, 1.0f, SpriteEffects.None, 1.0f);
        }

        //Reference from notes Lecture 3, part of kinematic seek
        public void Orientation(Vector2 velocity)
        {
            //https://stackoverflow.com/questions/2276855/xna-2d-vector-angles-whats-the-correct-way-to-calculate
            orientation = (float)Math.Atan2(velocity.X, -velocity.Y);
        }
    }
}
