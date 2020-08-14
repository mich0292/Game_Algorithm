using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.UI.Forms;

namespace Project
{
    class Menu : ControlManager
    {
        public Menu(Game game) : base(game)
        {

        }

        public override void InitializeComponent()
        {
            Button startButton = new Button()
            {
                Text = "Start Game",
                Size = new Vector2(200, 50),
                BackgroundColor = Color.Black
            };

            startButton.Clicked += StartGame;
            Controls.Add(startButton);

            Button quitButton = new Button()
            {
                Text = "Quit Game",
                Size = new Vector2(200, 50),
                BackgroundColor = Color.Black,
                Location = new Vector2(0, 100)
            };

            quitButton.Clicked += QuitGame;
            Controls.Add(quitButton);
        }

        private void StartGame(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("blabla");
        }

        private void QuitGame(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("hihi");
        }
    }
}
