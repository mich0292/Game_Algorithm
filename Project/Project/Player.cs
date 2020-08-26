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
    public class Player : GameObject
    {
        private float missileTime; //in seconds
        private float missileRate; //in seconds
        private Cursor cursor;
        public static bool playerAlive = true;

        public override void Initialize()
        {
            //initialize all the variables
            health = 5;
            fireRate = 150f; //in miliseconds
            fireTime = 0f;  //in miliseconds
            missileTime = 0f;
            missileRate = 10f;
            speed = 300.0f;
            name = "player";
            texture = Game1.assets["player"];
            position.X = Game1.screenWidth / 2;
            position.Y = Game1.screenHeight / 2;
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
            orientation = 0f;
            cursor = new Cursor();
            cursor.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();

            //moving the player
            if (keyboard.IsKeyDown(Keys.Up) && position.Y > 0 + texture.Height / 2)
                position.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (keyboard.IsKeyDown(Keys.Down) && position.Y < Game1.screenHeight - texture.Height / 2)
                position.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (keyboard.IsKeyDown(Keys.Left) && position.X > 0 + texture.Width / 2)
                position.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (keyboard.IsKeyDown(Keys.Right) && position.X < Game1.screenWidth - texture.Width / 2)
                position.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            //player fire bullet
            if (keyboard.IsKeyDown(Keys.Space) && gameTime.TotalGameTime.TotalMilliseconds > fireTime)
            {
                fireTime = (float)gameTime.TotalGameTime.TotalMilliseconds + fireRate;
                Game1.playerBulletList.Add(new PlayerBullet());
                Game1.playerBulletList[Game1.playerBulletList.Count - 1].Initialize();
            }

            //player fire missile
            if (keyboard.IsKeyDown(Keys.Z) && gameTime.TotalGameTime.TotalSeconds > missileTime)
            {
                if (keyboard.IsKeyDown(Keys.Tab) || cursor.target == null)
                    cursor.SelectTarget();

                cursor.Update(gameTime);
            }

            if (keyboard.IsKeyUp(Keys.Z) && cursor.target != null)
            {
                System.Diagnostics.Debug.WriteLine("Fire missile");
                //missileTime = (float)gameTime.TotalGameTime.TotalSeconds + missileRate;
                //var missile = new Missile();
                //missile.Initialize();
                //missile.target = cursor.target;
                //cursor.target = null;
                //Game1.missileList.Add(missile);
            }

            //Check whether the player is dead to end the game
            if (health == 0)
            { playerAlive = false; }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            cursor.Draw(spriteBatch, gameTime);
            spriteBatch.Draw(texture, position, null, Color.White, orientation, origin, 1.0f, SpriteEffects.None, 1.0f);
        }

        public void revivePlayer()
        {
            health = 5;
            playerAlive = true;
        }
    }
}
