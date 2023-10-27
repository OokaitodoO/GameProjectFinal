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
        Texture2D Font;
        Game1 game;
        public static List<GameObject> Boss = new List<GameObject>();
        public static List<BoundsCheck> Bounds = new List<BoundsCheck>();
        KeyboardState ks;
        KeyboardState oldks;


        public SceneBoss(Game1 game,EventHandler theScreenEvent) : base(theScreenEvent)
        {
            bossScene = game.Content.Load<Texture2D>("Scenes/Map_Boss");            
            
            this.game = game;

            //MapCollision
            Bounds.Add(new BoundsCheck(game, new Vector2(-(Game1.TILE_SIZE * 2), 0), Game1.TILE_SIZE * 15, Game1.TILE_SIZE * 2));
            Bounds.Add(new BoundsCheck(game, new Vector2((Game1.TILE_SIZE * 8), 0), Game1.TILE_SIZE * 15, Game1.TILE_SIZE * 2));
            Bounds.Add(new BoundsCheck(game, new Vector2(0, -(Game1.TILE_SIZE)), Game1.TILE_SIZE * 2, Game1.TILE_SIZE * 25));
            Bounds.Add(new BoundsCheck(game, new Vector2(0, (Game1.TILE_SIZE * 14)), Game1.TILE_SIZE * 2, Game1.TILE_SIZE * 25));
            
            Bounds.Add(new BoundsCheck(game, new Vector2(0, (Game1.TILE_SIZE * 5)), Game1.TILE_SIZE, Game1.TILE_SIZE * 25));
            Bounds.Add(new BoundsCheck(game, new Vector2(0, (Game1.TILE_SIZE * 10)), Game1.TILE_SIZE, Game1.TILE_SIZE * 25));
        }
        public override void Update(GameTime gameTime, float elapsed, Player player)
        {
            ks = Keyboard.GetState();
            Game1.LOCK_CAM = true;
            if (Game1.GAME_STATE == 1)
            {
                //ChangeMap
                if (game.oldScreen == game.mLevel1)
                {
                    if (Game1.monsterCount == 0 && player.Bounds.X + 64 >= Game1.TILE_SIZE * 6 && player.Bounds.X <= Game1.TILE_SIZE * 9 && player.Bounds.Y >= Game1.TILE_SIZE * 4)
                    {
                        player.SetPos(new Vector2(350, 250));
                        player.Restart(new Vector2(350, 250));
                        Game1._cameraPosition = new Vector2(0, 0);
                        Boss.Clear();
                        Sfx.InsStopSfx(21);
                        Sfx.InsPlaySfx(20);
                        ScreenEvent.Invoke(this, new EventArgs());
                        return;
                    }
                    
                }
                else if (game.oldScreen == game.mLevel2)
                {
                    if (Game1.monsterCount == 0)
                    {
                        Boss.Clear();
                        CreditsScreen.SetCreditsPos();
                        Game1.LOCK_CAM = true;
                        Game1.GAME_STATE = 0;
                        Game1._cameraPosition = Vector2.Zero;
                        Game1._camera.LookAt(Game1._bgPosition);
                        PlayerSkills.Restart();
                        game.mCurrentScreen = game.mTitle;
                        Sfx.InsStopSfx(21);
                        Sfx.InsPlaySfx(19);
                        ScreenEvent.Invoke(this, new EventArgs());
                        return;
                    }
                }

                //UpdateBoss
                foreach (GameObject gameObject in Boss)
                {                    
                    if (game.oldScreen == game.mLevel1)
                    {                        
                        game.player.CheckColiision((MiniBoss1)gameObject, ((MiniBoss1)gameObject).DealDamage());
                        game.playerAtkEfx.CheckColiision((MiniBoss1)gameObject);
                        game.playerSkill.CheckColiision((MiniBoss1)gameObject);
                        if (gameObject.GetType().IsAssignableTo(typeof(MiniBoss1)))
                        {
                            ((MiniBoss1)gameObject).Update(elapsed, game.player);
                            ((MiniBoss1)gameObject).CheckColiision(game.player);
                            ((MiniBoss1)gameObject).UpdateFrame(elapsed);                            
                        }                        
                    }
                    else if (game.oldScreen == game.mLevel2)
                    {
                        game.player.CheckColiision((MiniBoss2)gameObject, ((MiniBoss2)gameObject).DealDamage());
                        //game.player.CheckColiision(gameObject);
                        game.playerAtkEfx.CheckColiision((MiniBoss2)gameObject);
                        game.playerSkill.CheckColiision((MiniBoss2)gameObject);
                        if (gameObject.GetType().IsAssignableTo(typeof(MiniBoss2)))
                        {
                            ((MiniBoss2)gameObject).Update(elapsed, game.player);
                            ((MiniBoss2)gameObject).CheckColiision(game.player);
                            ((MiniBoss2)gameObject).UpdateFrame(elapsed);
                        }
                    }

                }
                
                //CheckMapCollision
                foreach (BoundsCheck bounds in Bounds)
                {
                    player.CheckMapColiision(bounds);
                }
                //Pause
                if (ks.IsKeyDown(Keys.Escape) && oldks.IsKeyUp(Keys.Escape))
                {
                    Game1.GAME_STATE = 2;
                    Sfx.PlaySfx(0);
                    Sfx.InsStopSfx(14);
                }                
            }
            else if (Game1.GAME_STATE == 2) //Pausing
            {
                //Restart
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !player.GetAlive())
                {
                    if (game.oldScreen == game.mLevel1)
                    {
                        game.mCurrentScreen = game.mLevel1;
                        player.Restart(new Vector2(0, 1500));
                        Game1._cameraPosition = new Vector2(0, 1200);
                        Game1.monsterCount = Level1Screen.Enemy.Count;
                        Boss.Clear();
                    }
                    else if(game.oldScreen == game.mLevel2)
                    {
                        game.mCurrentScreen = game.mLevel2;
                        player.Restart(new Vector2(350, 250));
                        Game1._cameraPosition = new Vector2(0, 0);
                        Game1.monsterCount = Level2Screen.Enemy.Count;
                        Boss.Clear();
                    }
                    
                }
                else if (ks.IsKeyDown(Keys.Enter) && player.GetAlive()) //BackToMenu
                {
                    Game1.LOCK_CAM = true;
                    Game1.GAME_STATE = 0;
                    Game1._cameraPosition = Vector2.Zero;
                    Game1._camera.LookAt(Game1._bgPosition);
                    game.mCurrentScreen = game.mTitle;
                    Sfx.PlaySfx(0);
                    Sfx.InsStopSfx(21);
                    Sfx.InsPlaySfx(19);
                    Sfx.InsStopSfx(14);
                    Boss.Clear();
                }
                else if (ks.IsKeyDown(Keys.Escape) && oldks.IsKeyUp(Keys.Escape)) //Resume
                {
                    Game1.GAME_STATE = 1;
                    Sfx.PlaySfx(0);
                }
            }
            oldks = ks;
        }
        public static void ResetScreen(Player player, Game1 game)
        {            
            if (game.oldScreen == game.mLevel1)
            {
                Boss.Add(new MiniBoss1(game, new Vector2(Game1.MAP_WIDTH / 3, Game1.MAP_HEIGHT / 3 - 450), 300, 300, 5, 7, 4, 0.5f));
            }
            else if (game.oldScreen == game.mLevel2)
            {
                Boss.Add(new MiniBoss2(game, new Vector2(Game1.MAP_WIDTH / 3 - 300, (Game1.MAP_HEIGHT / 3)), 300, 300, 5, 7, 4, 0.5f));
            }
            foreach (GameObject gameObject in Boss)
            {
                if (game.oldScreen == game.mLevel1)
                {                    
                    if (gameObject.GetType().IsAssignableTo(typeof(MiniBoss1)))
                    {
                        ((MiniBoss1)gameObject).Restart();
                    }
                }
                else if (game.oldScreen == game.mLevel2)
                {                    
                    if (gameObject.GetType().IsAssignableTo(typeof(MiniBoss2)))
                    {
                        ((MiniBoss2)gameObject).Restart();
                    }
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bossScene, Vector2.Zero, Color.White);
            foreach (GameObject gameObject in Boss)
            {
                if (game.oldScreen == game.mLevel1)
                {
                    if (gameObject.GetType().IsAssignableTo(typeof(MiniBoss1)))
                    {
                        ((MiniBoss1)gameObject).Draw(spriteBatch);

                    }
                }
                else if (game.oldScreen == game.mLevel2)
                {
                    if (gameObject.GetType().IsAssignableTo(typeof(MiniBoss2)))
                    {
                        ((MiniBoss2)gameObject).Draw(spriteBatch);

                    }
                }
            }                    
        }
    }
}
