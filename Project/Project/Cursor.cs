using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project
{
    public static class Cursor 
    {
        public static GameObject target;
        public static int counter;

        private static string name;
        private static Texture2D texture;
        public static Vector2 position;
        public static Vector2 origin;
        public static float orientation;

        public static void Initialize()
        {
            //initialize all the variables
            name = "cursor";
            texture = Game1.assets["cursor"];
            position.X = Game1.screenWidth / 2;
            position.Y = Game1.screenHeight / 2;
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
            orientation = 0f;
            target = null;
            counter = 0;
        }

        public static void Update(GameTime gameTime)
        {
            if (target != null)
            {
                if (Game1.enemyList.Contains(target))
                    position = target.position;
                else
                    target = null;
            }
        }

        public static void SelectTarget()
        {
            System.Diagnostics.Debug.WriteLine("Select Target");
            System.Diagnostics.Debug.WriteLine("Counter: " + counter);
            System.Diagnostics.Debug.WriteLine("Enemy Count: " + Game1.enemyList.Count);
            for (int i = counter; i < Game1.enemyList.Count; counter++, i++)
            {
                if (target != Game1.enemyList[i])
                {
                    target = Game1.enemyList[i];
                    break;
                }
            }
            if (counter >= Game1.enemyList.Count)
                counter = 0;
        }

        public static void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (target != null)
                spriteBatch.Draw(texture, position, null, Color.White, orientation, origin, 1.0f, SpriteEffects.None, 1.0f);                
        }
    }
}
