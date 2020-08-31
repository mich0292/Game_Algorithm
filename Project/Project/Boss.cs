﻿using System;
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

        public override void Initialize()
        {
            //initialize all the variables
            health = 50;
            fireRate = 150f; //in miliseconds
            fireTime = 0f;  //in miliseconds
            speed = 300.0f;
            name = "boss";
            texture = Game1.assets["boss"];
            position.X = Game1.screenWidth / 2;
            position.Y = Game1.screenHeight / 2;
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
            orientation = 0f;
        }

        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
            switch (currentState)
            {
                case BossState.avoid: UpdateAvoid(); break;
                case BossState.attack: UpdateAttack(); break;
                case BossState.attack_faster: UpdateAttackFaster(); break;                 
            }
        }

        void UpdateAvoid()
        {

        }

        void UpdateAttack()
        {

        }

        void UpdateAttackFaster()
        {
            fireRate = 75f;
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texture, position, null, Color.White, orientation, origin, 1.0f, SpriteEffects.None, 1.0f);
        }
    }
}
