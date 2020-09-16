using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

// References used:
// SceneManagement -> https://community.monogame.net/t/switch-scenes-in-monogame/2605/2
// Camera -> https://stackoverflow.com/questions/17452808/moving-a-camera-in-xna-c-sharp
// Camera -> https://www.youtube.com/watch?v=zPdmkFDT5Qo
//UI (Sprite Fonts) -> https://www.youtube.com/watch?v=x_c19loJ9Ds
//pause -> https://www.youtube.com/watch?v=RKmJsjLaNFM

namespace Project
{
    public enum GameState
    {
        MainMenu,
        Gameplay,
        GameOver,
    }

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public static GameState _state;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Dictionary<string, Texture2D> assets = new Dictionary<string, Texture2D>();
        public static Player player = new Player();
        public static List<GameObject> playerBulletList = new List<GameObject>();
        public static List<GameObject> enemyList = new List<GameObject>();
        public static List<GameObject> enemyBulletList = new List<GameObject>();
        public static List<Missile> missileList = new List<Missile>();

        //For measuring the screen
        public static int screenWidth;
        public static int screenHeight;

        private Button startButton;
        private Button endButton;
        private Button restartButton;
        //private Button menuButton;
        //Asteroid
        private float counter;
        //Player collision
        private float collisionTime;
        //Menu title
        private UI menuTitle;
        //Scrolling Background
        private ScrollingBackground bg1 = new ScrollingBackground();
        private ScrollingBackground bg2 = new ScrollingBackground();
        //pause texture
        private Texture2D pauseTexture;
        //pause variable
        private bool pauseGame;
        private float pauseCounter;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            base.Initialize();

            screenWidth = graphics.GraphicsDevice.Viewport.Width; // or Window.ClientBounds.Width
            screenHeight = graphics.GraphicsDevice.Viewport.Height;
            counter = 0;
            collisionTime = 0;
            pauseCounter = 0f;
            pauseGame = false;

            startButton = new Button("startButton", Game1.assets["startButton"], screenWidth / 2 - Game1.assets["startButton"].Width / 2, screenHeight / 2 - Game1.assets["startButton"].Height / 2);
            endButton = new Button("endButton", Game1.assets["endButton"], screenWidth / 2 - Game1.assets["endButton"].Width / 2, screenHeight / 2 + Game1.assets["endButton"].Height / 2);
            restartButton = new Button("restartButton", Game1.assets["restartButton"], screenWidth / 2 - Game1.assets["restartButton"].Width / 2, screenHeight / 2 - Game1.assets["restartButton"].Height / 2);
            player.Initialize();
            //boss
            var boss = new Boss();
            boss.Initialize();
            enemyList.Add(boss);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            // http://pixelartmaker.com/offshoot/a8bcafa2d30a8be -> for making buttons 
            //Load content here
            assets.Add("player", Content.Load<Texture2D>("Player"));
            assets.Add("playerBullet", Content.Load<Texture2D>("bullet"));
            assets.Add("enemyBullet", Content.Load<Texture2D>("enemy_bullet")); //change the file!!!
            assets.Add("startButton", Content.Load<Texture2D>("start"));
            assets.Add("endButton", Content.Load<Texture2D>("exit"));
            assets.Add("restartButton", Content.Load<Texture2D>("restart"));
            assets.Add("menuButton", Content.Load<Texture2D>("menu"));
            assets.Add("asteroid", Content.Load<Texture2D>("asteroid"));
            assets.Add("cursor", Content.Load<Texture2D>("cursor"));
            assets.Add("enemy1", Content.Load<Texture2D>("enemy1")); 
            assets.Add("enemy2", Content.Load<Texture2D>("enemy2"));
            assets.Add("turret", Content.Load<Texture2D>("turret"));
            assets.Add("missile", Content.Load<Texture2D>("rocket")); 
            assets.Add("boss", Content.Load<Texture2D>("boss")); 
            menuTitle = new UI("Space Battle", Content.Load<SpriteFont>("font"));
            pauseTexture = Content.Load<Texture2D>("pause");

            //load background here
            bg1.Initialize(Content.Load<Texture2D>("background1"), new Rectangle(0, 500, 800, 500));
            bg2.Initialize(Content.Load<Texture2D>("background1"), new Rectangle(0, 0, 800, 500));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);

            switch (_state)
            {
                case GameState.MainMenu:
                    UpdateMainMenu(gameTime);
                    break;
                case GameState.Gameplay:
                    UpdateGameplay(gameTime);
                    break;
                case GameState.GameOver:
                    UpdateGameOver(gameTime);
                    break;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            switch (_state)
            {
                case GameState.MainMenu:
                    DrawMainMenu(gameTime);
                    break;
                case GameState.Gameplay:
                    DrawGameplay(gameTime);
                    break;
                case GameState.GameOver:
                    DrawGameOver(gameTime);
                    break;
            }    
        }

        public void DetectCollision(GameTime gameTime)
        {
            //detect collision between player and enemy
            for(int i = 0; i < enemyList.Count; i++)
            {
                if (player.BoundingBox.Intersects(enemyList[i].BoundingBox))
                {
                    player.health--;
                    enemyList[i].health--;
                    collisionTime = (float)gameTime.TotalGameTime.TotalSeconds + 0.5f;

                    if(enemyList[i].health <= 0)
                        enemyList.Remove(enemyList[i]);
                        

                    break;
                }
            }

            //detect collision between player bullet and enemy
            for(int i = 0; i < enemyList.Count; i++)
            {
                for(int j = 0; j < playerBulletList.Count; j++)
                {
                    //Axis-Aligned Bounding Box
                    if (enemyList[i].BoundingBox.Intersects(playerBulletList[j].BoundingBox))
                    {
                        enemyList[i].health--;

                        if (enemyList[i].health <= 0)
                            enemyList.Remove(enemyList[i]);                            

                        playerBulletList.Remove(playerBulletList[j]);
                        break;
                    }
                }
            }

            //detect collision between enemy bullet and player
            for (int i = 0; i < enemyBulletList.Count; i++)
            {
                if (player.BoundingBox.Intersects(enemyBulletList[i].BoundingBox))
                {
                    player.health--;

                    enemyBulletList.Remove(enemyBulletList[i]);

                    break;
                }
            }

            //detect collision between missile and target
            for (int i = 0; i < missileList.Count; i++)
            {
                for (int j = 0; j < enemyList.Count; j++)
                {
                    if (object.ReferenceEquals(missileList[i].target, enemyList[j]))
                    {
                        if (missileList[i].BoundingBox.Intersects(enemyList[j].BoundingBox))
                            enemyList[j].health -= 3;
                            if (enemyList[j].health <= 0)
                                enemyList.Remove(enemyList[j]);

                            missileList[i].target = null;
                            missileList.Remove(missileList[i]);

                            break;                       
                    }
                }
            }
        }

        void UpdateMainMenu(GameTime deltaTime)
        {
            MouseState MouseInput = Mouse.GetState();

            if (MouseInput.LeftButton == ButtonState.Pressed)
            {
                if (startButton.enterButton(MouseInput))
                {
                    _state = GameState.Gameplay;
                }
                if (endButton.enterButton(MouseInput))
                {
                    Exit();
                }
            }
        }

        void UpdateGameplay(GameTime deltaTime)
        {
            KeyboardState keyboard = Keyboard.GetState();

            if(keyboard.IsKeyDown(Keys.Enter) && deltaTime.TotalGameTime.TotalMilliseconds > pauseCounter)
            {
                pauseCounter = (float)deltaTime.TotalGameTime.TotalMilliseconds + 200f;
                pauseGame = !pauseGame;
            }                
            if (!pauseGame)
            {
                counter += (float)deltaTime.ElapsedGameTime.TotalSeconds;

                if (counter >= 2)
                {
                    counter = 0;
                    for (int i = 0; i < 1; i++)
                    {
                        var asteroid = new Asteroid();
                        var enemy = new Enemy1();
                        asteroid.Initialize();
                        enemy.Initialize();
                        enemyList.Add(enemy);
                        enemyList.Add(asteroid);
                    }
                }
                //update player
                player.Update(deltaTime);
                //update player bullet
                for (int i = 0; i < playerBulletList.Count; i++)
                    playerBulletList[i].Update(deltaTime);
                //update enemy bullet
                for (int i = 0; i < enemyBulletList.Count; i++)
                    enemyBulletList[i].Update(deltaTime);
                //update enemy 
                for (int i = 0; i < enemyList.Count; i++)
                    enemyList[i].Update(deltaTime);
                //update missile
                for (int i = 0; i < missileList.Count; i++)
                    missileList[i].Update(deltaTime);
                //update background
                if (bg1.rec.Y >= 500)
                    bg1.rec.Y = bg2.rec.Y - bg2.rec.Height;
                if (bg2.rec.Y >= 500)
                    bg2.rec.Y = bg1.rec.Y - bg1.rec.Height;
                bg1.Update();
                bg2.Update();

                //detect collision
                DetectCollision(deltaTime);

                if (!Player.playerAlive)
                {
                    _state = GameState.GameOver;
                    playerBulletList.Clear();
                    enemyBulletList.Clear();
                    enemyList.Clear();
                    missileList.Clear();
                }
            }
        }

        void UpdateGameOver(GameTime deltaTime)
        {
            MouseState MouseInput = Mouse.GetState();

            if (MouseInput.LeftButton == ButtonState.Pressed)
            {
                if (restartButton.enterButton(MouseInput))
                {
                    _state = GameState.Gameplay;
                    //current player health is 0 ((because of gameover)
                    player.revivePlayer();
                }
                if (endButton.enterButton(MouseInput))
                {
                    Exit();
                }
            }
        }

        void DrawMainMenu(GameTime deltaTime)
        {
            GraphicsDevice.Clear(Color.Coral);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            startButton.Draw(spriteBatch);
            menuTitle.Draw(spriteBatch, deltaTime);
            endButton.Draw(spriteBatch);
            spriteBatch.End();
        }

        void DrawGameplay(GameTime deltaTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.FrontToBack);            

            //draw player
            player.Draw(spriteBatch, deltaTime);
            //draw player bullet
            for (int i = 0; i < playerBulletList.Count; i++)
                playerBulletList[i].Draw(spriteBatch, deltaTime);
            //draw enemy bullet
            for (int i = 0; i < enemyBulletList.Count; i++)
                enemyBulletList[i].Draw(spriteBatch, deltaTime);
            //draw enemy 
            for (int i = 0; i < enemyList.Count; i++)
                enemyList[i].Draw(spriteBatch, deltaTime);
            //draw missile
            for (int i = 0; i < missileList.Count; i++)
                missileList[i].Draw(spriteBatch, deltaTime);

            //draw background
            bg1.Draw(spriteBatch, deltaTime);
            bg2.Draw(spriteBatch, deltaTime);
            //draw pause
            if (pauseGame)
                spriteBatch.Draw(pauseTexture, new Vector2(screenWidth / 2 - pauseTexture.Width / 2, screenHeight / 2 - pauseTexture.Height / 2), Color.White);
            spriteBatch.End();
        }

        void DrawGameOver(GameTime deltaTime)
        {
            GraphicsDevice.Clear(Color.AntiqueWhite);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            restartButton.Draw(spriteBatch);
            endButton.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
