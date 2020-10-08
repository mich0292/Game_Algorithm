using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Project
{
    public class Boss : GameObject
    {
        public enum BossState { avoid, attack, attack_faster };
        public static BossState currentState = BossState.attack;

        private int attackFasterHealth;
        private float avoidCounter; //if the boss avoid for more than 10 seconds, boss cannot avoid and enter attack state for 10 seconds
        private bool canAvoid; //boss can avoid the bullet if this boolean is true

        public override void Initialize() //initialize for level 2
        {
            //initialize all the variables
            health = 50;
            attackFasterHealth = health * 30/100;
            fireRate = 200f; //in miliseconds
            fireTime = 0f;  //in miliseconds
            speed = 200.0f;
            name = "boss";
            texture = Game1.assets["boss"];
            position.X = Game1.screenWidth / 2;
            position.Y = 50;
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
            orientation = 0f;
            avoidCounter = 0f;
            canAvoid = true;
        }

        public void Initialize2() //initialize for level 1
        {
            //initialize all the variables
            health = 25;
            attackFasterHealth = health * 30 / 100;
            fireRate = 250f; //in miliseconds
            fireTime = 0f;  //in miliseconds
            speed = 200.0f;
            name = "boss";
            texture = Game1.assets["boss2"];
            position.X = Game1.screenWidth / 2;
            position.Y = 50;
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
            orientation = 0f;
            avoidCounter = 0f;
            canAvoid = true;
        }

        public override void Update(GameTime gameTime)
        {
            Console.WriteLine(currentState);
            switch (currentState)
            {
                case BossState.avoid:
                    UpdateAvoid(gameTime);
                    break;
                case BossState.attack:
                    UpdateAttack(gameTime);
                    break;
                case BossState.attack_faster:
                    UpdateAttackFaster(gameTime);
                    break;
            }
        }

        void UpdateAvoid(GameTime gameTime)
        {

            if (Game1.player.previousPos.X - Game1.player.position.X < 0 && position.X < Game1.screenWidth - texture.Width / 2
                && Game1.player.position.X < position.X) //player move from left to right and player is on left side of boss
                position.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds; //move to right to avoid the bullet
            else if (Game1.player.previousPos.X - Game1.player.position.X < 0 && position.X > 0 + texture.Width / 2 && Game1.player.position.X > position.X) //player move from left to right and player is on right side of boss
                position.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds; //move to left to avoid the bullet
            else if (Game1.player.previousPos.X - Game1.player.position.X > 0 && position.X < Game1.screenWidth - texture.Width / 2 && Game1.player.position.X < position.X) //player move from right to left and player is on left side of boss
                position.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds; //move to right to avoid the bullet
            else if (Game1.player.previousPos.X - Game1.player.position.X > 0 && position.X > 0 + texture.Width / 2 && Game1.player.position.X > position.X) //player move from right to left and player is on right side of boss
                position.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds; //move to left to avoid the bullet
            else if (Game1.player.previousPos.X - Game1.player.position.X == 0) //player stand still
            {
                if (Game1.player.position.X >= Game1.screenWidth - Game1.player.texture.Width / 2 && position.X > 0 + texture.Width / 2)  //player reach the maximum right side
                    position.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds; //move to left to avoid the bullet
                else if (Game1.player.position.X <= 0 + Game1.player.texture.Width / 2 && position.X < Game1.screenWidth - texture.Width / 2) //player reach the maximum left side
                    position.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds; //move to right to avoid the bullet
                else if (Game1.player.position.X < position.X && position.X < Game1.screenWidth - texture.Width / 2) //player on the boss left side
                    position.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds; //move to right to avoid the bullet                   
                else if (Game1.player.position.X > position.X && position.X > 0 + texture.Width / 2) //player on the boss right side
                    position.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds; //move to left to avoid the bullet 
                else if (Math.Abs(Game1.player.position.X - position.X) < 0.1) //player in front of boss
                    position.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds; //move to left to avoid the bullet 
            }

            //Prevent boss from forever avoiding player's bullets
            KeyboardState keyboard = Keyboard.GetState();
            if (!keyboard.IsKeyDown(Keys.Space) || !canAvoid){
                if (health > attackFasterHealth)
                    currentState = BossState.attack;
                else
                    currentState = BossState.attack_faster;
                avoidCounter = 0.0f;
            }

            if (keyboard.IsKeyDown(Keys.Space))
            {
                avoidCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (avoidCounter >= 1.0f)
                {
                    canAvoid = false;
                    avoidCounter = 0.0f;
                }
            }
        }

        void UpdateAttack(GameTime gameTime)
        {
            MoveToPlayer(gameTime);
            Fire(gameTime);

            KeyboardState keyboard = Keyboard.GetState();
            //player fire bullet
            if (keyboard.IsKeyDown(Keys.Space) && canAvoid)
            {
                currentState = BossState.avoid; //boss dodge the bullet 
            }

            //boss cannot avoid player's bullet
            if (!canAvoid && health <= attackFasterHealth)
            {
                 currentState = BossState.attack_faster;
            }

            avoidCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (avoidCounter >= 2.0f)
            {
                canAvoid = true;
                avoidCounter = 0.0f;
            }
        }

        void UpdateAttackFaster(GameTime gameTime)
        {
            fireRate = 160f;
            MoveToPlayer(gameTime);
            Fire(gameTime);

            KeyboardState keyboard = Keyboard.GetState();
            //player fire bullet
            if (keyboard.IsKeyDown(Keys.Space) && canAvoid)
            {
                currentState = BossState.avoid; //boss dodge the bullet 
            }

            avoidCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (avoidCounter >= 1.0f)
            {
                canAvoid = true;
                avoidCounter = 0.0f;
            }
        }

        public void MoveToPlayer(GameTime gameTime)
        {
            if (Game1.player.position.X < position.X && Math.Abs(Game1.player.position.X - position.X) > 0.1)
                position.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds; //move to left to chase the player
            else if (Game1.player.position.X > position.X && Math.Abs(Game1.player.position.X - position.X) > 0.1)
                position.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds; //move to right to chase the player
        }

        public void Fire(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds > fireTime)
            {
                fireTime = (float)gameTime.TotalGameTime.TotalMilliseconds + fireRate;
                if (InLOS(360, Game1.screenHeight, Game1.player.position, position, orientation))
                {
                    EnemyBullet tempBullet = new EnemyBullet();
                    tempBullet.setOwner(this);
                    tempBullet.Initialize();
                    if (currentState == BossState.attack)
                        tempBullet.speed = 350.0f;
                    else
                        tempBullet.speed = 450.0f;
                    Game1.enemyBulletList.Add(tempBullet);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texture, position, null, Color.White, orientation, origin, 1.0f, SpriteEffects.None, 0.99f);
        }        
    }
}
