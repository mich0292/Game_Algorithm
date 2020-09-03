using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project
{
    public class Boss : GameObject
    {
        public enum BossState { avoid, attack, attack_faster};
        public static BossState currentState;

        private int oriHealth;
        private float avoidSpeed;

        public override void Initialize()
        {
            //initialize all the variables
            health = 50;
            oriHealth = health;
            fireRate = 150f; //in miliseconds
            fireTime = 0f;  //in miliseconds
            speed = 300.0f;
            avoidSpeed = 1000.0f;
            name = "boss";
            texture = Game1.assets["boss"];
            position.X = Game1.screenWidth / 2;
            position.Y = 50;
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
            orientation = 0f;
        }

        public override void Update(GameTime gameTime)
        {
            int temp = oriHealth * 30 / 100;

            if (health < temp && currentState == BossState.attack)
                currentState = BossState.attack_faster;

            switch (currentState)
            {
                case BossState.avoid:
                    UpdateAvoid(gameTime);
                    break;
                case BossState.attack:
                    //UpdateAttack(gameTime);
                    break;
                case BossState.attack_faster:
                    //UpdateAttackFaster(gameTime);
                    break;                 
            }
        }

        void UpdateAvoid(GameTime gameTime)
        {            
            if (Game1.player.previousPos.X - Game1.player.position.X < 0 && position.X < Game1.screenWidth - texture.Width / 2 
                && Game1.player.position.X < position.X) //player move from left to right and player is on left side of boss
                position.X += avoidSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds; //move to right to avoid the bullet
            else if (Game1.player.previousPos.X - Game1.player.position.X < 0 && position.X > 0 + texture.Width / 2 && Game1.player.position.X > position.X) //player move from left to right and player is on right side of boss
                position.X -= avoidSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds; //move to left to avoid the bullet
            else if (Game1.player.previousPos.X - Game1.player.position.X > 0 && position.X < Game1.screenWidth - texture.Width / 2 && Game1.player.position.X < position.X) //player move from right to left and player is on left side of boss
                position.X += avoidSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds; //move to right to avoid the bullet
            else if (Game1.player.previousPos.X - Game1.player.position.X > 0 && position.X > 0 + texture.Width / 2 && Game1.player.position.X > position.X) //player move from right to left and player is on right side of boss
                position.X -= avoidSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds; //move to left to avoid the bullet
            else if(Game1.player.previousPos.X - Game1.player.position.X == 0) //player stand still
            {
                if(Game1.player.position.X >= Game1.screenWidth - Game1.player.texture.Width / 2 && position.X > 0 + texture.Width / 2)  //player reach the maximum right side
                    position.X -= avoidSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds; //move to left to avoid the bullet
                else if(Game1.player.position.X <= 0 + Game1.player.texture.Width / 2 && position.X < Game1.screenWidth - texture.Width / 2) //player reach the maximum left side
                    position.X += avoidSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds; //move to right to avoid the bullet
                else if (Game1.player.position.X < position.X && position.X < Game1.screenWidth - texture.Width / 2) //player on the boss left side
                    position.X += avoidSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds; //move to right to avoid the bullet                   
                else if (Game1.player.position.X > position.X && position.X > 0 + texture.Width / 2) //player on the boss right side
                    position.X -= avoidSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds; //move to left to avoid the bullet                                      
            }
        }

        void UpdateAttack(GameTime gameTime)
        {
            System.Diagnostics.Debug.WriteLine("Attack");
            //MoveToPlayer(gameTime); 
            Fire();
        }

        void UpdateAttackFaster(GameTime gameTime)
        {
            System.Diagnostics.Debug.WriteLine("Attack");
            fireRate = 75f;
            //MoveToPlayer(gameTime);
            Fire();
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texture, position, null, Color.White, orientation, origin, 1.0f, SpriteEffects.None, 1.0f);
        }

        public void MoveToPlayer(GameTime gameTime)
        {
            if (Game1.player.position.X < position.X) 
                position.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds; //move to left to chase the player
            else if (Game1.player.position.X > position.X) 
                position.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds; //move to right to chase the player
        }

        public void Fire()
        {
            //line of sight here
        }
    }
}
