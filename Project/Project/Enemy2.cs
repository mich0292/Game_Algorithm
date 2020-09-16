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
            fireTime = 0f;
            fireRate = 100.0f;
            orientation = 0f;
            texture = Game1.assets["player"];
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
        //
        public void PatternMovement(GameTime gameTime)
        {            
            Vector2 velocity = Game1.player.position - position;
            velocity.Normalize();

            velocity *= speed;
            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Orientation(velocity);            
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
