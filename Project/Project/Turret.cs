using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Project
{
    class Turret:GameObject
    {
        private Random random = new Random();
        private SoundEffect soundEffect;

        public override void Initialize()
        {
            //initialize all the variables
            health = 10;
            speed = 0.0f;
            fireTime = 0.0f;
            fireRate = 600.0f;
            orientation = 0f;
            texture = Game1.assets["turret"];
            position = new Vector2(700, random.Next(0, Game1.screenHeight/2));
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
            soundEffect = Game1.soundEffect["enemy"];
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 velocity = Game1.player.position - position;
            position.Y += 2;
            if (position.Y > Game1.screenHeight)
            {
                Game1.enemyList.Remove(this);
            }
            velocity.Normalize();
            Orientation(velocity);
            LineOfSight(gameTime);           
        }

        public void LineOfSight(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds > fireTime)
            {
                fireTime = (float)gameTime.TotalGameTime.TotalMilliseconds + fireRate;
                if (InLOS(90, 500, Game1.player.position, position, orientation))
                {
                    EnemyBullet tempBullet = new EnemyBullet();
                    tempBullet.setOwner(this);
                    tempBullet.Initialize();
                    Game1.enemyBulletList.Add(tempBullet);
                    soundEffect.Play();
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
