using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project
{
    public class Cursor : GameObject
    {
        public GameObject target;

        public override void Initialize()
        {
            //initialize all the variables
            name = "cursor";
            texture = Game1.assets["cursor"];
            position.X = Game1.window.ClientBounds.Width / 2;
            position.Y = Game1.window.ClientBounds.Height / 2;
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
            orientation = 0f;
            target = null;
        }

        public override void Update(GameTime gameTime)
        {
            if (target != null)
            {
                if (Game1.enemyList.Contains(target))
                    position = target.position;
                else
                    target = null;
            }
        }

        public void SelectTarget()
        {
            for (int i = 0; i < Game1.enemyList.Count; i++)
            {
                if (target != Game1.enemyList[i])
                {
                    target = Game1.enemyList[i];
                    break;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (target != null)
                spriteBatch.Draw(texture, position, null, Color.White, orientation, origin, 1.0f, SpriteEffects.None, 1.0f);
        }
    }
}
