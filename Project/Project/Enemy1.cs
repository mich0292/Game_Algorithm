using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project
{
    class Enemy1:GameObject
    {
        public override void Initialize()
        {
            //initialize all the variables
            health = 5;
            speed = 100.0f;
            fireTime = 0.0f;
            fireRate = 500.0f;
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
            spriteBatch.Draw(texture, position, null, Color.White, orientation, origin, 1.0f, SpriteEffects.None, 0.99f);
        }

        //Reference from notes Lecture 3, part of kinematic seek
        public void Orientation(Vector2 velocity)
        {
            //https://stackoverflow.com/questions/2276855/xna-2d-vector-angles-whats-the-correct-way-to-calculate
            orientation = (float)Math.Atan2(velocity.Y, velocity.X);
        }
    }
}
