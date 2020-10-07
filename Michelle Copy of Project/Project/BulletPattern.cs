using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//References used for this class creation: https://www.youtube.com/watch?v=Mq2zYk5tW_E&t=208s and https://stackoverflow.com/questions/55010535/c-sharp-finding-angle-between-2-given-points
namespace Project
{
    class MichelleChai 
    {
        //BulletPool concept and the calculation of the pattern is based on this video, https://www.youtube.com/watch?v=Mq2zYk5tW_E&t=208s

        // Common attributes for all DotBullet instances
        public static MichelleChai MichelleChaiInstance;
        private bool notEnoughBulletsInPool = true;

        private List<PlayerBullet> bullets;

        private float startAngle, endAngle;
        private static int bulletAmount = 10;

        public MichelleChai()
        {
            MichelleChaiInstance = this;
            bullets = new List<PlayerBullet>();
        }

        public PlayerBullet getBullet()
        {
            if (bullets.Count > 0)
            {
                for (int i = 0; i < bullets.Count; i++)
                {
                    if (!bullets[i].alive)
                    {
                        return bullets[i];
                    }   
                }
            }

            if (notEnoughBulletsInPool)
            {
                PlayerBullet bul = new PlayerBullet();
                bul.Initialize();
                bullets.Add(bul);
                return bul;
            }
            return null;
        }

        public List<PlayerBullet> getBulletList()
        {
            return bullets;
        }
        /* Player bullet handles deletion of bullets
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i].position.X < 0 || bullets[i].position.X >= Game1.Screen.ClientBounds.Width ||
                bullets[i].position.Y < 0 || bullets[i].position.Y >= Game1.Screen.ClientBounds.Height)
                {
                    bullets[i].Alive = false;
                    Collider.RemoveAllOf(bullets[i]);
                }
            }     
        }
        */
        public void fire( Vector2 playerPosition, Vector2 mousePosition)
        {
            //Calculation of the angle of the mousePosition from the player is based on https://stackoverflow.com/questions/55010535/c-sharp-finding-angle-between-2-given-points
            //tan = opposite/
            float startRad = (float)Math.Atan2(mousePosition.Y - playerPosition.Y, mousePosition.X - playerPosition.X);
            startAngle = (startRad * (180.0f / (float)Math.PI) + 360 ) % 360; 
            //( + 360 ) % 360 -> doesn't affect anything, just made it +ve ( from 0 to 360) instead of from -180 to 180,;

            Console.WriteLine(startAngle);
            endAngle = startAngle + 180.0f;

            // angle difference between each bullet
            float angleStep = (endAngle - startAngle) / bulletAmount;
            // when the player shoots, the bullet pattern centers around the mouse cursor
            float angle = startAngle - 90.0f;

            for (int i = 0; i < bulletAmount ; i++)
            {
                float bulDirX = playerPosition.X + (float)Math.Cos((angle * Math.PI) / 180.0f);
                float bulDirY = playerPosition.Y + (float)Math.Sin((angle * Math.PI) / 180.0f);

                Vector2 bulMoveVector = new Vector2(bulDirX, bulDirY);
                // Calculate the vector (direction) of the bullet ( target - origin)
                Vector2 bulDir = bulMoveVector - playerPosition;
                bulDir.Normalize();

                PlayerBullet bul = MichelleChaiInstance.getBullet();
                bul.position = playerPosition; //bullet starts from the player
                //bul.orientation = bulDir;
                bul.alive = true;

                angle += angleStep;
            }
            
        }

    }
}
