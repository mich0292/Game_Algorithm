using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

//References used for this class creation: https://www.youtube.com/watch?v=Mq2zYk5tW_E&t=208s and https://stackoverflow.com/questions/55010535/c-sharp-finding-angle-between-2-given-points
namespace Project
{
    class BulletPattern 
    {
        //BulletPool concept and the calculation of the pattern is based on this video, https://www.youtube.com/watch?v=Mq2zYk5tW_E&t=208s

        // Common attributes for all DotBullet instances (based on Lab, here we use playerbullet instead)
        public static BulletPattern BulletPatternInstance;
        private bool notEnoughBulletsInPool = true;

        private List<PlayerBullet> bullets;

        private float startAngle, endAngle;
        private static int bulletAmount = 10;

        public BulletPattern()
        {
            BulletPatternInstance = this;
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
                bul.texture = Game1.assets["powerBullet"];
                bullets.Add(bul);
                return bul;
            }
            return null;
        }

        public List<PlayerBullet> getBulletList()
        {
            return bullets;
        }

        // Player bullet handles deletion of bullets
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i].position.X < 0 || bullets[i].position.X >= Game1.screenWidth ||
                bullets[i].position.Y < 0 || bullets[i].position.Y >= Game1.screenHeight)
                {
                    bullets[i].alive = false;
                    Game1.playerBulletList.Remove(bullets[i]);
                }
            }     
        }

        public void fire()
        {
            startAngle = 275.896f;
            endAngle = startAngle + 180.0f;

            // angle difference between each bullet
            float angleStep = (endAngle - startAngle) / bulletAmount;
            // when the player shoots, the bullet pattern centers around the mouse cursor
            float angle = startAngle - 90.0f;

            for (int i = 0; i < bulletAmount ; i++)
            {
                float bulDirX = Game1.player.position.X + (float)Math.Cos((angle * Math.PI) / 180.0f);
                float bulDirY = Game1.player.position.Y + (float)Math.Sin((angle * Math.PI) / 180.0f);

                Vector2 bulMoveVector = new Vector2(bulDirX, bulDirY);
                // Calculate the vector (direction) of the bullet ( target - origin)
                Vector2 bulDir = bulMoveVector - Game1.player.position;
                bulDir.Normalize();

                PlayerBullet bul = BulletPatternInstance.getBullet();
                bul.position = Game1.player.position; //bullet starts from the player
                bul.heading = bulDir;
                bul.alive = true;

                angle += angleStep;
            }           
        }

    }
}
