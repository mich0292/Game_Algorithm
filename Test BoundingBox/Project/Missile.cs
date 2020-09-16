using System;
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
        List<Vector2> path;
        private bool called = false;

        public override void Initialize()
        {
            //initialize all the variables
            speed = 300.0f;
            name = "missile";
            texture = Game1.assets["missile"];
            position.X = Game1.player.position.X;
            position.Y = Game1.player.position.Y;
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
            orientation = 0f;
        }

        public override void Update(GameTime gameTime)
        {
            if (Game1.enemyList.Contains(target))
            {
                Vector2 moveToward;
                if (!called)
                {
                    var stopWatch = new System.Diagnostics.Stopwatch();
                    stopWatch.Start();
                    AStar.Initialize(position, target.position);
                    path = AStar.Compute(position, target.position);
                    stopWatch.Stop();
                    System.Diagnostics.Debug.WriteLine(stopWatch.Elapsed);
                    called = true;
                }

                if (path.Count > 0)
                {
                    moveToward = path[0];
                    path.RemoveAt(0);

                    Vector2 diff = moveToward - position;
                    //Vector2 diff = target.position - position;
                    orientation = (float)Math.Atan2(diff.Y, diff.X);
                    diff.Normalize();

                    position += speed * diff * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
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
