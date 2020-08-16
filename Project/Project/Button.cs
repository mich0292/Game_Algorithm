using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//used this as reference, https://stackoverflow.com/questions/32430598/making-a-button-xna-monogame

namespace Project
{
    class Button
    {
        private int posX, posY;
        private string name;
        private Texture2D texture;


        public int ButtonX
        {
            get
            {
                return posX;
            }
        }

        public int ButtonY
        {
            get
            {
                return posY;
            }
        }

        public Button(string name, Texture2D texture, int buttonX, int buttonY)
        {
            this.name = name;
            this.texture = texture;
            this.posX = buttonX;
            this.posY = buttonY;
        }

        public bool enterButton(MouseState mouseInput)
        {
          
            if (mouseInput.X < posX + texture.Width &&
                    mouseInput.X > posX &&
                    mouseInput.Y < posY + texture.Height &&
                    mouseInput.Y > posY)
            {
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)ButtonX, (int)ButtonY, texture.Width, texture.Height), Color.White);
        }
    }
}
