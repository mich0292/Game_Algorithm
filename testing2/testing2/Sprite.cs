using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace testing2
{
    public class Sprite : Component
    {
        public float _layer;
        public  Texture2D _texture;
        public Vector2 position;

        public Sprite(Texture2D texture)
        {
            _texture = texture;
        }

        public float Layer { get => _layer; set => _layer = value; }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, _texture.Width, _texture.Height);
            }
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(_texture, position, null, Color.White, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, Layer);
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
