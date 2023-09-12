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

        public static int GAME_STATE = 0; // 0: In game | 1: Die

        KeyboardState ks = new KeyboardState();
        KeyboardState oldKs = new KeyboardState();
        MouseState ms = new MouseState();

        public const int TILE_SIZE = 64;
        public const int MAP_WIDTH = 3200; //1600 * 2
        public const int MAP_HEIGHT = 1800; // 900 * 2

        //Camera
        public static OrthographicCamera _camera;
        public static Vector2 _cameraPosition;
        public static Vector2 _bgPosition;        

        //Scenes
        Level1Screen mLevel1;
        Screen mCurrentScreen;

        //Player
        Player player;
        PlayerSkills playerSkill;
        PlayerAttackEffect playerAtkEfx;
        //Ui
        HealthBar hpBar;
        StaminaBar staminaBar;

        //Enemy
        //Melee_Enemy ml_Enemy;

        //List
        List<GameObject> gameObjects= new List<GameObject>();
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
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
            _cameraPosition = Vector2.Zero;
            //Scenes
            mLevel1 = new Level1Screen(this.Content, new EventHandler(Level1ScreenEvent));
            mCurrentScreen = mLevel1;

            //Player
            player = new Player(this, new Vector2(0,0), 100, 100, 100, 10, 8, 8, 0.5f);
            playerSkill = new PlayerSkills(this,Vector2.Zero, new Vector2(0,64), 4, 8, 4, 0.5f);
            playerAtkEfx = new PlayerAttackEffect(this, Vector2.Zero, 10, 8, 2, 0.6f);
            //Ui
            hpBar = new HealthBar(this, Vector2.Zero, 250, 30, 1);
            staminaBar = new StaminaBar(this, new Vector2(0f, 30), 200, 25, 1);

            //Enemy
            gameObjects.Add(new Melee_Enemy(this, new Vector2(400, 200), Vector2.Zero, "Null", 125, 150, 5, 7, 4, 0.3f));
            gameObjects.Add(new Melee_Enemy(this, new Vector2(400, 1500), Vector2.Zero, "Null", 125, 150, 5, 7, 4, 0.3f));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            ks = Keyboard.GetState();
            ms = Mouse.GetState();

            switch (GAME_STATE)
            {
                case 0:
                    GamePlay(elapsed);
                    break;
                case 1:
                    break;
            }
            if (ks.IsKeyDown(Keys.F1))
            {
                GAME_STATE = 1;
            }
            if (ks.IsKeyDown(Keys.F2))
            {
                GAME_STATE = 0;
            }
            //Console.WriteLine(GAME_STATE);
            oldKs = ks;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            var transformMatrix = _camera.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: transformMatrix);
            mCurrentScreen.Draw(_spriteBatch);

            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.IsEnable)
                {
                    gameObject.Draw(_spriteBatch);
                }
            }
            //ml_Enemy.Draw(_spriteBatch);
            player.Draw(_spriteBatch);
            playerSkill.Draw(_spriteBatch);
            playerAtkEfx.Draw(_spriteBatch);
            staminaBar.Draw(_spriteBatch);
            hpBar.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void GamePlay(float elapsed)
        {
            //camera
            _camera.LookAt(_bgPosition + _cameraPosition);

            //Player
            player.Update(this, ks, oldKs, ms, playerSkill, elapsed);
            playerSkill.Update(elapsed, player, ks, ms);
            playerAtkEfx.Update(elapsed, player);                       
            staminaBar.Update(elapsed, player, GraphicsDevice);
            hpBar.Update(elapsed, player, GraphicsDevice);
            //ml_Enemy.Update(player, elapsed);
            
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.GetType().IsAssignableTo(typeof(AnimatedObject)))
                {
                    ((AnimatedObject)gameObject).UpdateFrame(elapsed);
                }
                player.CheckColiision(gameObject, ((Melee_Enemy)gameObject).DealDamage());
                playerAtkEfx.CheckColiision(gameObject);
                playerSkill.CheckColiision(gameObject);
                if (gameObject.GetType().IsAssignableTo(typeof(Melee_Enemy)))
                {                    
                    ((Melee_Enemy)gameObject).Update(player, elapsed);                    
                    ((Melee_Enemy)gameObject).CheckColiision(player, playerAtkEfx, playerSkill, playerAtkEfx, playerSkill);                   
                }
            }
        }

        protected void Restart()
        {
            player.Restart();
            Console.Clear();
            GAME_STATE = 0;
        }

        public void UpdateCamera(Vector2 move)
        {
            _cameraPosition += move;
        }
        public float GetCameraPosX()
        {
            return _cameraPosition.X;
        }
        public float GetCameraPosY()
        {
            return _cameraPosition.Y;
        }

        public void Level1ScreenEvent(object obj, EventArgs e)
        {
            mCurrentScreen = mLevel1;
        }
    }
}