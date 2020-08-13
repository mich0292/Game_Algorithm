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
    class Player:GameObject
    {
        private float missileTime; //in seconds
        private float missileRate; //in seconds

        public override void Initialize()
        {
            //initialize all the variables
            health = 5;
            fireRate = 100f; //in miliseconds
            fireTime = 0f;  //in miliseconds
            missileTime = 0f;
            missileRate = 10f;
            speed = 100.0f;
            name = "player";
            texture = Game1.assets["player"];
        }

        public override void Update(GameTime gameTime)
        {            
            KeyboardState keyboard = Keyboard.GetState();

            //moving the player
            if (keyboard.IsKeyDown(Keys.Up))
                position.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;                
            else if(keyboard.IsKeyDown(Keys.Down))
                position.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (keyboard.IsKeyDown(Keys.Left))
                position.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (keyboard.IsKeyDown(Keys.Right))
                position.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            //player fire bullet
            if(keyboard.IsKeyDown(Keys.Space) && gameTime.TotalGameTime.TotalMilliseconds > fireTime)
            {
                fireTime = (float)gameTime.TotalGameTime.TotalMilliseconds + fireRate;
                Game1.playerBulletList.Add(new Bullet());
                Game1.playerBulletList[Game1.playerBulletList.Count - 1].Initialize();
            }

            //player fire missile
            if (keyboard.IsKeyDown(Keys.Z) && gameTime.TotalGameTime.TotalSeconds > missileTime)
            {
                missileTime = (float)gameTime.TotalGameTime.TotalSeconds + missileRate;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texture, position,
                origin: new Vector2(texture.Width / 2.0f, texture.Height / 2.0f));
        }
    }
}
