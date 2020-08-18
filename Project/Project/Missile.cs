﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project
{
    public class Missile : GameObject
    {
        public GameObject target;

        public override void Initialize()
        {
            //initialize all the variables
            speed = 300.0f;
            name = "missile";
            texture = Game1.assets["missile"];
            position.X = Game1.window.ClientBounds.Width / 2;
            position.Y = Game1.window.ClientBounds.Height / 2;
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
            orientation = 0f;
        }

        public override void Update(GameTime gameTime)
        {
            if (Game1.enemyList.Contains(target))
            {
                //path finding
            }
            else
                Game1.missileList.Remove(this);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texture, position, null, Color.White, orientation, origin, 1.0f, SpriteEffects.None, 1.0f);
        }
    }
}
