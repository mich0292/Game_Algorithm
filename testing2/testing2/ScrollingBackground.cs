using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace testing2
{
    public class ScrollingBackground : Component
    {
        /// <summary>
        /// if this true means the background constant moving even the player is stationary
        /// </summary>
        private bool constantSpeed;
        private float layer;
        private float scrollingSpeed;
        private List<Sprite> spritesList;
        private float speed;

        public float Layer {
            get { return layer; }
            set {
                layer = value;
                foreach (var sprite in spritesList)
                {
                    sprite.Layer = layer;
                }
            }
        }

        public ScrollingBackground(Texture2D texture, float scrollingSpeed, bool constantSpeed = true)
            :this(new List<Texture2D>() { texture, texture}, scrollingSpeed, constantSpeed)
        {

        }

        public ScrollingBackground(List<Texture2D> textures, float scrollingSpeed, bool constantSpeed = true)
        {            
            this.scrollingSpeed = scrollingSpeed;
            this.constantSpeed = constantSpeed;
            spritesList = new List<Sprite>();
            for (int i = 0; i < textures.Count; i++)
            {
                var texture = textures[i];
                spritesList.Add(new Sprite(texture)
                {
                    position = new Vector2(0, Game1.window.ClientBounds.Height - texture.Height)
                });
            }
        }


        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var sprite in spritesList)
                sprite.Draw(spriteBatch, gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            ApplySpeed(gameTime);
            CheckPosition();
        }

        private void CheckPosition()
        {
            var mouse = Mouse.GetState();
            for(int i = 0; i < spritesList.Count; i++)
            {
                var sprite = spritesList[i];

                if (sprite.Rectangle.Center.Y >= Game1.window.ClientBounds.Height)
                {
                    sprite.position.Y = Game1.window.ClientBounds.Height - sprite._texture.Height;
                }
            }
        }

        private void ApplySpeed(GameTime gameTime)
        {
            speed = scrollingSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (var sprite in spritesList)
                sprite.position.Y += speed;
        }
    }
}
