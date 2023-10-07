using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended;

namespace lastdayInkhumuang
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        //private GraphicsDevice _graphicDevice;
        //TestCommit
        public static int GAME_STATE = 0; // 0: Tile | 1: Gameplay | 2: Pause
        public static bool LOCK_CAM;

        KeyboardState ks = new KeyboardState();
        KeyboardState oldKs = new KeyboardState();
        MouseState ms = new MouseState();
        MouseState oldMs = new MouseState();
        public static Rectangle mouseRec;

        public const int TILE_SIZE = 128;
        public const int MAP_WIDTH = 3200; //1600 * 2
        public const int MAP_HEIGHT = 1800; // 900 * 2

        //Camera
        public static OrthographicCamera _camera;
        public static Vector2 _cameraPosition;
        public static Vector2 _bgPosition;

        //Scenes
        //public static string Map;
        public static bool changeScreen = false;
        public Texture2D pauseScreen;
        public TitleScreen mTitle;
        public Level1Screen mLevel1;
        public Level2Screen mLevel2;
        public SceneBoss mBoss1;
        public Screen mCurrentScreen;
        public Screen oldScreen;
        public FrontObject frontObject;
        

        //Player
        public Player player;
        public Player_Boss_StatusBar playerStatusBar;
        public PlayerSkills playerSkill;
        public PlayerAttackEffect playerAtkEfx;

        //Ui
        HealthBar hpBar;
        StaminaBar staminaBar;
        SpriteFont pause;
        Texture2D Cursor;
        Vector2 cursorPos;

        //Enemy
        public static int monsterCount;

        //List
        //List<GameObject> gameObjects= new List<GameObject>();
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false ;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = MAP_HEIGHT/3; //600
            _graphics.PreferredBackBufferWidth = (int)MAP_WIDTH/3; //1066            
            _graphics.ApplyChanges();


            var viewportadapter = new BoxingViewportAdapter(Window, GraphicsDevice, MAP_WIDTH/3, MAP_HEIGHT/3);
            _camera = new OrthographicCamera(viewportadapter);
            _bgPosition = new Vector2(533, 300);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _cameraPosition = new Vector2(0, 0);
            mouseRec = new Rectangle(0,0, 1,1);

            //Scenes
            frontObject = new FrontObject(this);
            mTitle = new TitleScreen(this, new EventHandler(TitleScreenEvent));
            mLevel1 = new Level1Screen(this, new EventHandler(Level1ScreenEvent));
            mLevel2 = new Level2Screen(this.Content, new EventHandler(Level2ScreenEvent));
            mBoss1 = new SceneBoss(this, new EventHandler(SceneBossScreenEvent));
            pauseScreen = Content.Load<Texture2D>("Scenes/Bg1");
            mCurrentScreen = mTitle;


            //Player
            player = new Player(this, new Vector2(0,1500), 100, 100, 100, 5, 8, 13, 0.5f);
            playerSkill = new PlayerSkills(this,Vector2.Zero, new Vector2(0,64), 4, 8, 4, 0.5f);
            playerAtkEfx = new PlayerAttackEffect(this, Vector2.Zero, 5, 8, 3, 0.6f);
            //Ui
            playerStatusBar = new Player_Boss_StatusBar(this, Vector2.Zero, 580,  190, 1f);
            Cursor = Content.Load<Texture2D>("Ui/button&-cursor");
            //hpBar = new HealthBar(this, Vector2.Zero, 250, 30, 1);
            //staminaBar = new StaminaBar(this, new Vector2(0f, 30), 200, 25, 1);
            pause = Content.Load<SpriteFont>("Ui/File");            
        }

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            ks = Keyboard.GetState();
            ms = Mouse.GetState();
            mouseRec.Location = ms.Position;
            cursorPos.X = ms.Position.X + _cameraPosition.X;
            cursorPos.Y = ms.Position.Y + _cameraPosition.Y;
            //Scenes
            mCurrentScreen.Update(gameTime, elapsed, player);

            switch (GAME_STATE)
            {
                case 0:
                    mTitle.Update(ms, oldMs, elapsed);
                    break;
                case 1:
                    GamePlay(elapsed);
                    break;
                case 2:
                    break;
            }
            if (ks.IsKeyDown(Keys.F1))
            {
                GAME_STATE = 1;
            }
            //if (ks.IsKeyDown(Keys.Escape))
            //{
            //    GAME_STATE = 2;
            //}
            //Console.WriteLine(GAME_STATE);
            oldKs = ks;
            oldMs = ms;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            var transformMatrix = _camera.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: transformMatrix);
            //Screen
            mCurrentScreen.Draw(_spriteBatch);

            //Player
            if (mCurrentScreen != mTitle)
            {
                player.Draw(_spriteBatch);
                playerSkill.Draw(_spriteBatch);
                playerAtkEfx.Draw(_spriteBatch);               
                //staminaBar.Draw(_spriteBatch);
                //hpBar.Draw(_spriteBatch);
            }

            //FrontObj
            frontObject.Draw(_spriteBatch, player, this);

            //Pause
            if (mCurrentScreen != mTitle)
            {
                playerStatusBar.Draw(_spriteBatch);
            }
            if (GAME_STATE == 2)
            {
                _spriteBatch.Draw(pauseScreen, _cameraPosition, Color.Gray * 0.5f);               
                if (player.GetAlive())
                {
                    _spriteBatch.DrawString(pause, "Pause...", _cameraPosition + new Vector2(410, 0), Color.White);
                    _spriteBatch.DrawString(pause, "Press Enter : Return to Title", _cameraPosition + new Vector2(0, 500), Color.White);
                }
                else if (!player.GetAlive())
                {
                    _spriteBatch.DrawString(pause, "Nice try...", _cameraPosition + new Vector2(410, 0), Color.White);
                    _spriteBatch.DrawString(pause, "Press Enter : Respawn", _cameraPosition + new Vector2(0, 500), Color.White);
                }
                
            }

            //Cursor
            if (ms.LeftButton == ButtonState.Pressed)
            {
                _spriteBatch.Draw(Cursor, cursorPos, new Rectangle(18, 17, 16, 22), Color.White, 0f, new Vector2(3, 5), new Vector2(2f, 2f), 0, 0);
            }
            else
            {
                _spriteBatch.Draw(Cursor, cursorPos, new Rectangle(0, 17, 16, 22), Color.White, 0f, new Vector2(3, 5), new Vector2(2f, 2f), 0, 0);
            }
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void GamePlay(float elapsed)
        {
            frontObject.Update(elapsed);
            //camera
            if (!LOCK_CAM)
            {
                _camera.LookAt(_bgPosition + _cameraPosition);
            }

            //Player
            playerStatusBar.Update(elapsed, player, GraphicsDevice, this);
            player.Update(this, ks, oldKs, ms, playerSkill, elapsed);
            playerSkill.Update(elapsed, player, ks, ms);
            playerAtkEfx.Update(elapsed, player);                       
            //staminaBar.Update(elapsed, player, GraphicsDevice);
            //hpBar.Update(elapsed, player, GraphicsDevice);           
        }
        protected void Pause()
        {
            if (ks.IsKeyDown(Keys.Enter))
            {
                LOCK_CAM = true;
                mCurrentScreen = mTitle;
                _cameraPosition = Vector2.Zero;
                _camera.LookAt(_bgPosition);
                GAME_STATE = 0;
            }            
        }
        protected void Restart()
        {
            if (ks.IsKeyDown(Keys.Enter))
            {
                player.Restart();
                Console.Clear();
                GAME_STATE = 1;
            }
            
        }

        public void UpdateCamera(Vector2 move)
        {
            if (!LOCK_CAM)
            {
                _cameraPosition += move;
            }            
        }
        public float GetCameraPosX()
        {
            return _cameraPosition.X;
        }
        public float GetCameraPosY()
        {
            return _cameraPosition.Y;
        }
        public void TitleScreenEvent(object obj, EventArgs e)
        {
            mCurrentScreen = mLevel1;
            oldScreen = mTitle;
            changeScreen = true;
            FrontObject.timer = 1;
            Level1Screen.ResetScreen(player);
            _cameraPosition = new Vector2(0, 1200);
            monsterCount = Level1Screen.Enemy.Count;                        
        }
        public void Level1ScreenEvent(object obj, EventArgs e)
        {
            mCurrentScreen = mBoss1;
            oldScreen = mLevel1;
            changeScreen = true;
            FrontObject.timer = 1;
            SceneBoss.ResetScreen(player);
            _cameraPosition = new Vector2(0, 0);
            monsterCount = 1;
        }
        public void Level2ScreenEvent(object obj, EventArgs e)
        {
            mCurrentScreen = mBoss1;
            oldScreen = mLevel2;
        }
        public void SceneBossScreenEvent(object obj, EventArgs e)
        {
            if (oldScreen == mLevel1)
            {
                mCurrentScreen = mTitle;
                GAME_STATE = 0;
            }
            
        }
    }
}