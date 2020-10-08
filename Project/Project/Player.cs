using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Project
{
    public class Player : GameObject
    {
        private float missileTime; //in seconds
        private float missileRate; //in seconds             
        private float missileCooldown; //in seconds
        private float tabTime;
        private BulletPattern _BulletPattern;

        public static bool playerAlive = true;   
        public static int m_bulletCount = 0;
        public Vector2 previousPos;
        public bool powerUp = false;

        public override void Initialize()
        {
            //initialize all the variables
            health = 5;
            fireRate = 150f; //in miliseconds
            fireTime = 0f;  //in miliseconds
            missileTime = 0f; //in seconds
            missileRate = 5f; //in seconds
            missileCooldown = 0f;
            tabTime = 0f;
            _BulletPattern = new BulletPattern();
            speed = 300.0f;
            name = "player";
            texture = Game1.assets["player"];
            position.X = Game1.screenWidth / 2;
            position.Y = Game1.screenHeight / 2;
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
            orientation = 0f;           
            Cursor.Initialize();
        }

        public float returnMissileCooldown() { return missileCooldown; }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            previousPos = position;

            //moving the player
            if (keyboard.IsKeyDown(Keys.Up) && position.Y > 0 + texture.Height / 2)
                position.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (keyboard.IsKeyDown(Keys.Down) && position.Y < Game1.screenHeight - texture.Height / 2)
                position.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (keyboard.IsKeyDown(Keys.Left) && position.X > 0 + texture.Width / 2)
                position.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (keyboard.IsKeyDown(Keys.Right) && position.X < Game1.screenWidth - texture.Width / 2)
                position.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Update fire rate for powerup
            if (powerUp)
            {
                fireRate = 350.0f;
            }

            //player fire bullet
            if (keyboard.IsKeyDown(Keys.Space))
            {
                if(gameTime.TotalGameTime.TotalMilliseconds > fireTime)
                {
                    Console.WriteLine(powerUp);
                    if (!powerUp)
                    {
                        fireTime = (float)gameTime.TotalGameTime.TotalMilliseconds + fireRate;
                        Game1.playerBulletList.Add(new PlayerBullet());
                        Game1.playerBulletList[Game1.playerBulletList.Count - 1].Initialize();
                    }
                    else {
                        fireTime = (float)gameTime.TotalGameTime.TotalMilliseconds + fireRate;
                        _BulletPattern.fire();

                        for (int i = 0; i < BulletPattern.BulletPatternInstance.getBulletList().Count; i++)
                        {
                            PlayerBullet bulletInstance = BulletPattern.BulletPatternInstance.getBulletList()[i];
                            bulletInstance.name = "bullet" + m_bulletCount;
                            m_bulletCount++;
                            Game1.playerBulletList.Add(BulletPattern.BulletPatternInstance.getBulletList()[i]);
                            // Check all collisions between bullet and enemy
                            //Collider.Listen(bulletInstance, World.Objects["enemy1"], bulletInstance.HitTarget);
                        }
                    }
                }
            }

            //player fire missile

            if (missileTime > 0.0f)
            {
                missileCooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (keyboard.IsKeyDown(Keys.Z) && gameTime.TotalGameTime.TotalSeconds > missileTime)
            {
                if ((keyboard.IsKeyDown(Keys.Tab) || Cursor.target == null) && gameTime.TotalGameTime.TotalMilliseconds > tabTime)
                {
                    tabTime = (float)gameTime.TotalGameTime.TotalMilliseconds + 200f;
                    Cursor.SelectTarget();
                }                    

                Cursor.Update(gameTime);
            }

            if (keyboard.IsKeyUp(Keys.Z) && Cursor.target != null)
            {                
                missileTime = (float)gameTime.TotalGameTime.TotalSeconds + missileRate;
                missileCooldown = missileRate;
                Console.WriteLine(missileTime);
                var missile = new Missile();
                missile.Initialize();
                missile.target = Cursor.target;
                Cursor.target = null;
                Cursor.counter = 0; //after fire missile, reset the counter
                Game1.missileList.Add(missile);
            }

            //Check whether the player is dead to end the game
            if (health == 0)
                playerAlive = false;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Cursor.Draw(spriteBatch, gameTime);
            spriteBatch.Draw(texture, position, null, Color.White, orientation, origin, 1.0f, SpriteEffects.None, 1.0f);
        }

        public void revivePlayer()
        {
            health = 5;
            playerAlive = true;
        }
    }
}
