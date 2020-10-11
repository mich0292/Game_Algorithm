using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

// References used:
// SceneManagement -> https://community.monogame.net/t/switch-scenes-in-monogame/2605/2
// Camera -> https://stackoverflow.com/questions/17452808/moving-a-camera-in-xna-c-sharp
// Camera -> https://www.youtube.com/watch?v=zPdmkFDT5Qo
//UI (Sprite Fonts) -> https://www.youtube.com/watch?v=x_c19loJ9Ds
//pause -> https://www.youtube.com/watch?v=RKmJsjLaNFM

namespace Project
{
    public enum GameState { MainMenu, Gameplay, GameOver, Win }

    public class Game1 : Game
    {
        public static GameState _state;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Dictionary<string, Texture2D> assets = new Dictionary<string, Texture2D>();
        public static Dictionary<string, SoundEffect> soundEffect = new Dictionary<string, SoundEffect>();
        public static Player player = new Player();
        public static List<GameObject> playerBulletList = new List<GameObject>();
        public static List<GameObject> enemyList = new List<GameObject>();
        public static List<GameObject> enemyBulletList = new List<GameObject>();
        public static List<Missile> missileList = new List<Missile>();

        //For measuring the screen
        public static int screenWidth;
        public static int screenHeight;

        //button
        private Button startButton;
        private Button endButton;
        private Button restartButton;
        private Button menuButton;

        private float lastPress;
        //enemyCounter
        private float counter;
        //turret counter
        private float turretCounter;
        //powerup Counter
        private int powerUpCounter;
        //Player collision
        private float collisionTime;
        //UI title
        private UI menuTitle;
        private UI pauseTitle;
        private UI winTitle;
        private UI loseTitle;
        private SpriteFont roboto;
        //Scrolling Background
        private ScrollingBackground bg1 = new ScrollingBackground();
        private ScrollingBackground bg2 = new ScrollingBackground();
        //pause variable
        private bool pauseGame;
        private float pauseCounter;
        //boss out
        private float distance;
        private bool bossOut;
        //background image
        private Texture2D bgImage;
        private Texture2D bgImage2;
        //level
        private int currentLevel;
        //score
        private int score;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            base.Initialize();

            screenWidth = graphics.GraphicsDevice.Viewport.Width; // or Window.ClientBounds.Width
            screenHeight = graphics.GraphicsDevice.Viewport.Height;
            score = 0;
            counter = 0;
            turretCounter = 0;
            powerUpCounter = 0;
            collisionTime = 0;
            lastPress = 0f;
            pauseCounter = 0;
            distance = 0f;
            currentLevel = 1;
            bossOut = false;

            startButton = new Button("startButton", Game1.assets["startButton"], screenWidth / 2 - Game1.assets["startButton"].Width / 2, screenHeight / 2 - Game1.assets["startButton"].Height / 2);
            endButton = new Button("endButton", Game1.assets["endButton"], screenWidth / 2 - Game1.assets["endButton"].Width / 2, screenHeight / 2 + Game1.assets["endButton"].Height / 2);
            restartButton = new Button("restartButton", Game1.assets["restartButton"], screenWidth / 2 - Game1.assets["restartButton"].Width / 2, screenHeight / 2 - Game1.assets["restartButton"].Height / 2);
            menuButton = new Button("menuButton", Game1.assets["menuButton"], screenWidth / 2 - Game1.assets["menuButton"].Width / 2, screenHeight / 2 - Game1.assets["menuButton"].Height / 2);
            player.Initialize();
        }

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
            assets.Add("boss2", Content.Load<Texture2D>("boss2"));
            assets.Add("powerup", Content.Load<Texture2D>("powerup"));
            assets.Add("powerBullet", Content.Load<Texture2D>("flowerBullet2"));
            assets.Add("heart", Content.Load<Texture2D>("heart"));
            menuTitle = new UI("Space Battle!", Content.Load<SpriteFont>("font"), Color.Black);
            pauseTitle = new UI("Pause Game", Content.Load<SpriteFont>("font"), Color.White);
            winTitle = new UI("You Win !", Content.Load<SpriteFont>("font"), Color.Black);
            loseTitle = new UI("You Lose !", Content.Load<SpriteFont>("font"), Color.Black);
            roboto = Content.Load<SpriteFont>("Roboto-Black");
            bgImage = Content.Load<Texture2D>("background1");
            bgImage2 = Content.Load<Texture2D>("sky");

            //load background here
            bg1.Initialize(bgImage2, new Rectangle(0, 500, 800, 500));
            bg2.Initialize(bgImage2, new Rectangle(0, 0, 800, 500));

            //load sound effect
            soundEffect.Add("player", Content.Load<SoundEffect>("bulletSoundEffect"));
            soundEffect.Add("enemy", Content.Load<SoundEffect>("enemyBulletSoundEffect"));
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

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
                case GameState.Win:
                    UpdateWin(gameTime);
                    break;
            }
        }

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
                case GameState.Win:
                    DrawWin(gameTime);
                    break;
            }
        }

        public void DetectCollision(GameTime gameTime)
        {
            //detect collision between player and enemy
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (player.BoundingBox.Intersects(enemyList[i].BoundingBox))
                {
                    //detect collision between player and powerup 
                    if (enemyList[i].GetType() == typeof(PowerUp))
                    {
                        player.powerUp = true;
                        enemyList[i].health--;
                        playerBulletList.Clear();
                        collisionTime = (float)gameTime.TotalGameTime.TotalSeconds + 0.5f;
                    }
                    else
                    {
                        player.health--;
                        enemyList[i].health--;
                        collisionTime = (float)gameTime.TotalGameTime.TotalSeconds + 0.5f;
                    }
                    if (enemyList[i].health <= 0)
                        enemyList.Remove(enemyList[i]);
                    break;
                }
            }

            //detect collision between player bullet and enemy
            for (int i = 0; i < enemyList.Count; i++)
            {
                for (int j = 0; j < playerBulletList.Count; j++)
                {
                    //Axis-Aligned Bounding Box
                    if (enemyList[i].BoundingBox.Intersects(playerBulletList[j].BoundingBox))
                    {
                        if (enemyList[i].GetType() != typeof(PowerUp))
                        {
                            enemyList[i].health--;
                            score += 10;
                        }

                        if (enemyList[i].health <= 0)
                        {
                            score += 50;
                            if (enemyList[i].GetType() == typeof(Boss))
                            {
                                if (currentLevel == 2)
                                {
                                    _state = GameState.Win;
                                }
                                distance = 0;
                                enemyList.Clear();
                                currentLevel = 2;
                                bossOut = false;
                                bg1.Initialize(bgImage, new Rectangle(0, 500, 800, 500));
                                bg2.Initialize(bgImage, new Rectangle(0, 0, 800, 500));
                            }
                            else
                                enemyList.Remove(enemyList[i]);
                        }

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
                        {
                            score += 20;
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
        }

        void UpdateMainMenu(GameTime deltaTime)
        {
            MouseState MouseInput = Mouse.GetState();

            if (MouseInput.LeftButton == ButtonState.Pressed && deltaTime.TotalGameTime.TotalMilliseconds > lastPress)
            {
                if (startButton.enterButton(MouseInput))
                {
                    lastPress = (float)deltaTime.TotalGameTime.TotalMilliseconds + 200;
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

            if (keyboard.IsKeyDown(Keys.Enter) && deltaTime.TotalGameTime.TotalMilliseconds > pauseCounter)
            {
                pauseCounter = (float)deltaTime.TotalGameTime.TotalMilliseconds + 200f;
                pauseGame = !pauseGame;
            }

            if (pauseGame)
            {
                MouseState MouseInput = Mouse.GetState();

                if (MouseInput.LeftButton == ButtonState.Pressed && deltaTime.TotalGameTime.TotalMilliseconds > lastPress)
                {
                    if (menuButton.enterButton(MouseInput))
                    {
                        _state = GameState.MainMenu;
                        pauseGame = false;
                        playerBulletList.Clear();
                        enemyBulletList.Clear();
                        enemyList.Clear();
                        missileList.Clear();
                        lastPress = (float)deltaTime.TotalGameTime.TotalMilliseconds + 200;
                    }
                    if (endButton.enterButton(MouseInput))
                    {
                        Exit();
                    }
                }
            }
            else if (!pauseGame)
            {
                counter += (float)deltaTime.ElapsedGameTime.TotalSeconds;
                turretCounter += (float)deltaTime.ElapsedGameTime.TotalSeconds;

                if (distance == 2000)
                {
                    //boss
                    enemyList.Clear();
                    enemyBulletList.Clear();
                    var boss = new Boss();
                    if (currentLevel == 1)
                        boss.Initialize2();
                    else
                        boss.Initialize();
                    enemyList.Add(boss);
                    bossOut = true;
                }

                if (turretCounter >= 10 && !bossOut)
                {
                    turretCounter = 0;
                    var turret = new Turret();
                    turret.Initialize();
                    enemyList.Add(turret);
                }

                if (currentLevel == 2 && distance == 500 && powerUpCounter == 0)
                {
                    var pu = new PowerUp();
                    pu.Initialize();
                    enemyList.Add(pu);
                    powerUpCounter++;
                }

                if (counter >= 2 && !bossOut)
                {
                    counter = 0;
                    var asteroid = new Asteroid();
                    //var enemy = new Enemy1();
                    asteroid.Initialize();
                    //enemy.Initialize();
                    //enemyList.Add(enemy);
                    enemyList.Add(asteroid);
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
                {
                    bg1.rec.Y = bg2.rec.Y - bg2.rec.Height;
                }

                if (bg2.rec.Y >= 500)
                {
                    bg2.rec.Y = bg1.rec.Y - bg1.rec.Height;
                }

                if (bg1.rec.Y % 10 == 0 || bg2.rec.Y % 10 == 0)
                {
                    distance += 10;
                }

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

            if (MouseInput.LeftButton == ButtonState.Pressed && deltaTime.TotalGameTime.TotalMilliseconds > lastPress)
            {
                if (restartButton.enterButton(MouseInput))
                {
                    _state = GameState.Gameplay;
                    lastPress = (float)deltaTime.TotalGameTime.TotalMilliseconds + 200;
                    //current player health is 0 ((because of gameover)
                    player.revivePlayer();
                    bossOut = false;
                    distance = 0;
                    currentLevel = 1;
                    powerUpCounter = 0;
                    counter = 0;
                    turretCounter = 0;
                    bg1.Initialize(bgImage2, new Rectangle(0, 500, 800, 500));
                    bg2.Initialize(bgImage2, new Rectangle(0, 0, 800, 500));
                }
                if (endButton.enterButton(MouseInput))
                {
                    Exit();
                }
            }
        }

        void UpdateWin(GameTime deltaTime)
        {
            MouseState MouseInput = Mouse.GetState();

            if (MouseInput.LeftButton == ButtonState.Pressed && deltaTime.TotalGameTime.TotalMilliseconds > lastPress)
            {
                if (menuButton.enterButton(MouseInput))
                {
                    _state = GameState.MainMenu;
                    playerBulletList.Clear();
                    enemyBulletList.Clear();
                    enemyList.Clear();
                    missileList.Clear();
                    lastPress = (float)deltaTime.TotalGameTime.TotalMilliseconds + 200;
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

            //draw simple UI
            spriteBatch.DrawString(roboto, "Health: ", new Vector2(10, 10), Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            Vector2 length = roboto.MeasureString("Health: ");

            for (int i = 0; i < player.health; i++)
            {
                spriteBatch.Draw(Game1.assets["heart"], new Vector2(length.X + 10 + 20 * i, 10), null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            }

            spriteBatch.DrawString(roboto, "Score: " + score, new Vector2(10, 30), Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            length = roboto.MeasureString(distance + "M");
            spriteBatch.DrawString(roboto, distance + "M", new Vector2(screenWidth - length.X - 10, 10), Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(roboto, "Missile: ", new Vector2(10, screenHeight - 30), Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            length = roboto.MeasureString("Missile: ");
            if (player.returnMissileCooldown() > 0.0f)
            {
                int temp = (int)player.returnMissileCooldown() + 1;
                spriteBatch.DrawString(roboto, temp + "s", new Vector2(length.X + 10, screenHeight - 30), Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            }
            else
            {
                spriteBatch.DrawString(roboto, "Ready", new Vector2(length.X + 10, screenHeight - 30), Color.Red, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            }


            spriteBatch.End();

            if (pauseGame)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                pauseTitle.Draw(spriteBatch, deltaTime);
                menuButton.Draw(spriteBatch);
                endButton.Draw(spriteBatch);
                spriteBatch.End();
            }
        }

        void DrawGameOver(GameTime deltaTime)
        {
            GraphicsDevice.Clear(Color.AntiqueWhite);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            loseTitle.Draw(spriteBatch, deltaTime);
            restartButton.Draw(spriteBatch);
            endButton.Draw(spriteBatch);
            spriteBatch.End();
            score = 0;
        }

        void DrawWin(GameTime deltaTime)
        {
            GraphicsDevice.Clear(Color.AntiqueWhite);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            winTitle.Draw(spriteBatch, deltaTime);
            menuButton.Draw(spriteBatch);
            endButton.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
