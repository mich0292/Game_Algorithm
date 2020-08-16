using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Project
{
    public abstract class GameObject
    {
        public int health;
        public float speed;
        public Vector2 position; 
        public Vector2 direction;
        public Vector2 origin;
        public float orientation;
        public float fireRate;
        public float fireTime;
        public string name;
        public Texture2D texture;

        public abstract void Initialize();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
