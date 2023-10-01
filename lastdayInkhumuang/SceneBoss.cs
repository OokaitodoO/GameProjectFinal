using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace lastdayInkhumuang
{
    public class SceneBoss : Screen
    {
        Texture2D bossScene;
        MiniBoss1 miniBoss;
        Game1 game;
        public static List<GameObject> Boss = new List<GameObject>();
        public static List<BoundsCheck> Bounds = new List<BoundsCheck>();
        KeyboardState ks;
        KeyboardState oldks;
        
       

        public SceneBoss(Game1 game,EventHandler theScreenEvent) : base(theScreenEvent)
        {
            bossScene = game.Content.Load<Texture2D>("Scenes/Map_Boss");            
            Boss.Add(new MiniBoss1(game, new Vector2(), 300, 300, 5, 7, 4, 0.5f));
            this.game = game;

            //MapCollision
            Bounds.Add(new BoundsCheck(game, new Vector2(-(Game1.TILE_SIZE * 2), 0), Game1.TILE_SIZE * 15, Game1.TILE_SIZE * 2));
            Bounds.Add(new BoundsCheck(game, new Vector2((Game1.TILE_SIZE * 8), 0), Game1.TILE_SIZE * 15, Game1.TILE_SIZE * 2));
            Bounds.Add(new BoundsCheck(game, new Vector2(0, -(Game1.TILE_SIZE)), Game1.TILE_SIZE * 2, Game1.TILE_SIZE * 25));
            Bounds.Add(new BoundsCheck(game, new Vector2(0, (Game1.TILE_SIZE * 14)), Game1.TILE_SIZE * 2, Game1.TILE_SIZE * 25));

            Bounds.Add(new BoundsCheck(game, new Vector2(0, (Game1.TILE_SIZE * 5)), Game1.TILE_SIZE, Game1.TILE_SIZE * 25));
        }
        public override void Update(GameTime gameTime, float elapsed, Player player)
        {
            ks = Keyboard.GetState();
            Game1.LOCK_CAM = true;
            if (Game1.GAME_STATE == 1)
            {
                if (Game1.monsterCount == 0 && player.Bounds.X + 64 >= (Game1.MAP_WIDTH/3) - 10)
                {
                    if (game.oldScreen == game.mLevel1)
                    {
                        player.SetPos(new Vector2(0, 200));
                        Game1._cameraPosition = new Vector2(0,0);
                    }
                    ScreenEvent.Invoke(this, new EventArgs());
                    return;
                }

                foreach (GameObject gameObject in Boss)
                {
                    game.player.CheckColiision(gameObject, ((MiniBoss1)gameObject).DealDamage());
                    game.playerAtkEfx.CheckColiision(gameObject);
                    game.playerSkill.CheckColiision(gameObject);
                    if (game.mCurrentScreen == game.mBoss1)
                    {
                        if (gameObject.GetType().IsAssignableTo(typeof(MiniBoss1)))
                        {
                            ((MiniBoss1)gameObject).Update(elapsed, game.player);
                            ((MiniBoss1)gameObject).CheckColiision(game.player);
                            ((MiniBoss1)gameObject).UpdateFrame(elapsed);                            
                        }                        
                    }                    
                }
                foreach (BoundsCheck bounds in Bounds)
                {
                    player.CheckMapColiision(bounds);
                }
                if (ks.IsKeyDown(Keys.Escape) && oldks.IsKeyUp(Keys.Escape))
                {
                    Game1.GAME_STATE = 2;
                }

                //test
                //if (Keyboard.GetState().IsKeyDown(Keys.Left))
                //{
                //    x -= 5 * elapsed;
                //}
                //if (Keyboard.GetState().IsKeyDown(Keys.Right))
                //{
                //    x += 5 * elapsed;
                //}
                //if (Keyboard.GetState().IsKeyDown(Keys.Up))
                //{
                //    y -= 5 * elapsed;
                //}
                //if (Keyboard.GetState().IsKeyDown(Keys.Down))
                //{
                //    y += 5 * elapsed;
                //}
            }
            else if (Game1.GAME_STATE == 2)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !player.GetAlive())
                {
                    game.mCurrentScreen = game.mLevel1;
                    player.Restart();
                    Game1._cameraPosition = new Vector2(0, 1200);
                    Game1.monsterCount = Level1Screen.Enemy.Count;
                }
                else if (ks.IsKeyDown(Keys.Enter) && player.GetAlive())
                {
                    Game1.LOCK_CAM = true;
                    Game1.GAME_STATE = 0;
                    Game1._cameraPosition = Vector2.Zero;
                    Game1._camera.LookAt(Game1._bgPosition);
                    game.mCurrentScreen = game.mTitle;
                }
                else if (ks.IsKeyDown(Keys.Escape) && oldks.IsKeyUp(Keys.Escape))
                {
                    Game1.GAME_STATE = 1;
                }
            }
            oldks = ks;
            //1stBoss
            //miniBoss.Update(elapsed, game.player);
        }
        public static void ResetScreen(Player player)
        {            
            foreach (GameObject gameObject in Boss)
            {
                if (gameObject.GetType().IsAssignableTo(typeof(MiniBoss1)))
                {
                    ((MiniBoss1)gameObject).Restart();
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bossScene, Vector2.Zero, Color.White);
            foreach (GameObject gameObject in Boss)
            {
                if (gameObject.GetType().IsAssignableTo(typeof(MiniBoss1)))
                {
                    ((MiniBoss1)gameObject).Draw(spriteBatch);
                    
                }                
            }
        }
    }
}
