using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lastdayInkhumuang
{
    public class Level1Screen : Screen
    {
        Texture2D Level1;
        Texture2D tutorialKeys;
        public static List<GameObject> Enemy = new List<GameObject>();
        public static List<BoundsCheck> Bounds = new List<BoundsCheck>();
        Game1 game;

        Vector2 playerPos;

        bool mapClear;
        KeyboardState ks;
        KeyboardState oldks;
        public Level1Screen(Game1 game,EventHandler theScreenEvent) : base(theScreenEvent)
        {
            Level1 = game.Content.Load<Texture2D>("Scenes/Map_Scene1_NoRoof");
            tutorialKeys = game.Content.Load<Texture2D>("Ui/button&-cursor");
            mapClear = false;

            ////MonZone1
            //Enemy.Add(new Melee_Enemy(game, new Vector2(1030, 1535), Vector2.Zero, 90, 150, 5, 7, 5, 0.3f));
            //Enemy.Add(new Melee_Enemy(game, new Vector2(1030, 1560), Vector2.Zero, 90, 150, 5, 7, 5, 0.3f));
            //Enemy.Add(new Melee_Enemy(game, new Vector2(1255, 1660), Vector2.Zero, 125, 150, 5, 7, 5, 0.3f));
            //Enemy.Add(new Melee_Enemy(game, new Vector2(1400, 1660), Vector2.Zero, 125, 150, 5, 7, 5, 0.3f));
            //Enemy.Add(new Melee_Enemy(game, new Vector2(1540, 1660), Vector2.Zero, 125, 150, 5, 7, 5, 0.3f));
            ////MonZone2
            //Enemy.Add(new Melee_Enemy(game, new Vector2(2430, 1665), Vector2.Zero, 125, 150, 5, 7, 5, 0.3f));
            //Enemy.Add(new Melee_Enemy(game, new Vector2(2530, 1665), Vector2.Zero, 125, 150, 5, 7, 5, 0.3f));
            //Enemy.Add(new Melee_Enemy(game, new Vector2(2700, 1535), Vector2.Zero, 125, 150, 5, 7, 5, 0.3f));
            //Enemy.Add(new Melee_Enemy(game, new Vector2(2820, 1665), Vector2.Zero, 125, 150, 5, 7, 5, 0.3f));
            ////MonZone3(Middle)
            //Enemy.Add(new Melee_Enemy(game, new Vector2(1665, 1020), Vector2.Zero, 125, 150, 5, 7, 5, 0.3f));
            //Enemy.Add(new Melee_Enemy(game, new Vector2(1535, 905), Vector2.Zero, 125, 150, 5, 7, 5, 0.3f));
            //Enemy.Add(new Melee_Enemy(game, new Vector2(1290, 1025), Vector2.Zero, 125, 150, 5, 7, 5, 0.3f));
            //Enemy.Add(new Melee_Enemy(game, new Vector2(900, 900), Vector2.Zero, 125, 150, 5, 7, 5, 0.3f));
            //Enemy.Add(new Melee_Enemy(game, new Vector2(770, 1030), Vector2.Zero, 125, 150, 5, 7, 5, 0.3f));
            ////MonZone4(Top-Left)
            //Enemy.Add(new Melee_Enemy(game, new Vector2(340, 270), Vector2.Zero, 125, 150, 5, 7, 5, 0.3f));
            //Enemy.Add(new Melee_Enemy(game, new Vector2(230, 305), Vector2.Zero, 125, 150, 5, 7, 5, 0.3f));
            //Enemy.Add(new Melee_Enemy(game, new Vector2(134, 500), Vector2.Zero, 125, 150, 5, 7, 5, 0.3f));
            ////MonZome5(Top-Mid)
            //Enemy.Add(new Melee_Enemy(game, new Vector2(1030, 260), Vector2.Zero, 125, 150, 5, 7, 5, 0.3f));
            //Enemy.Add(new Melee_Enemy(game, new Vector2(1150, 500), Vector2.Zero, 125, 150, 5, 7, 5, 0.3f));
            //Enemy.Add(new Melee_Enemy(game, new Vector2(1300, 190), Vector2.Zero, 125, 150, 5, 7, 5, 0.3f));
            //Enemy.Add(new Melee_Enemy(game, new Vector2(1315, 455), Vector2.Zero, 125, 150, 5, 7, 5, 0.3f));
            //Enemy.Add(new Melee_Enemy(game, new Vector2(1475, 320), Vector2.Zero, 125, 150, 5, 7, 5, 0.3f));
            ////MonZone6(Top-Right)
            //Enemy.Add(new Melee_Enemy(game, new Vector2(2060, 240), Vector2.Zero, 125, 150, 5, 7, 5, 0.3f));
            //Enemy.Add(new Melee_Enemy(game, new Vector2(2056, 430), Vector2.Zero, 125, 150, 5, 7, 5, 0.3f));
            //Enemy.Add(new Melee_Enemy(game, new Vector2(2235, 190), Vector2.Zero, 125, 150, 5, 7, 5, 0.3f));
            //Enemy.Add(new Melee_Enemy(game, new Vector2(2225, 500), Vector2.Zero, 125, 150, 5, 7, 5, 0.3f));
            //Enemy.Add(new Melee_Enemy(game, new Vector2(2360, 360), Vector2.Zero, 125, 150, 5, 7, 5, 0.3f));
            //Enemy.Add(new Melee_Enemy(game, new Vector2(2430, 190), Vector2.Zero, 125, 150, 5, 7, 5, 0.3f));
            //Enemy.Add(new Melee_Enemy(game, new Vector2(2530, 470), Vector2.Zero, 125, 150, 5, 7, 5, 0.3f));
            //Enemy.Add(new Melee_Enemy(game, new Vector2(2645, 285), Vector2.Zero, 125, 150, 5, 7, 5, 0.3f));

            ////Bounds
            //Bounds.Add(new BoundsCheck(game, new Vector2(0, Game1.TILE_SIZE * 9), Game1.TILE_SIZE * 2, Game1.TILE_SIZE * 20));
            //Bounds.Add(new BoundsCheck(game, new Vector2(0, Game1.TILE_SIZE * 5), Game1.TILE_SIZE * 4, Game1.TILE_SIZE * 4));
            //Bounds.Add(new BoundsCheck(game, new Vector2(0, 0), Game1.TILE_SIZE * 5, Game1.TILE_SIZE));
            //Bounds.Add(new BoundsCheck(game, new Vector2(0, 0), Game1.TILE_SIZE, Game1.TILE_SIZE * 25));
            //Bounds.Add(new BoundsCheck(game, new Vector2(Game1.TILE_SIZE * 24, Game1.TILE_SIZE * 4), Game1.TILE_SIZE * 11, Game1.TILE_SIZE));
            //Bounds.Add(new BoundsCheck(game, new Vector2(Game1.TILE_SIZE * 6, Game1.TILE_SIZE * 5), Game1.TILE_SIZE * 2, Game1.TILE_SIZE * 14));
            //Bounds.Add(new BoundsCheck(game, new Vector2(Game1.TILE_SIZE * 20, Game1.TILE_SIZE * 5), Game1.TILE_SIZE, Game1.TILE_SIZE * 4));
            ////BoundsOutMap
            //Bounds.Add(new BoundsCheck(game, new Vector2(-(Game1.TILE_SIZE * 2), 0), Game1.TILE_SIZE * 15, Game1.TILE_SIZE * 2));
            //Bounds.Add(new BoundsCheck(game, new Vector2((Game1.TILE_SIZE * 25), 0), Game1.TILE_SIZE * 15, Game1.TILE_SIZE * 2));
            //Bounds.Add(new BoundsCheck(game, new Vector2(0, -(Game1.TILE_SIZE * 2)), Game1.TILE_SIZE * 2, Game1.TILE_SIZE * 25));
            //Bounds.Add(new BoundsCheck(game, new Vector2(0, (Game1.TILE_SIZE * 14)), Game1.TILE_SIZE * 2, Game1.TILE_SIZE * 25));

            Game1.monsterCount = Enemy.Count;
            this.game= game;
        }       
        public override void Update(GameTime gameTime, float elapsed, Player player)
        {            
            Game1.LOCK_CAM = false;
            ks = Keyboard.GetState();
            if (Game1.GAME_STATE == 1)
            {
                playerPos = player.GetPos();
                if (Game1.monsterCount == 0 && player.Bounds.X + 64 >= Game1.MAP_WIDTH - 10 && player.Bounds.Y > Game1.TILE_SIZE && player.Bounds.Y < Game1.TILE_SIZE * 4)
                {
                    Game1.IsCutscenes = true;
                    player.SetPos(new Vector2(0, 200));
                    Game1.monsterCount = 1;
                    ScreenEvent.Invoke(this, new EventArgs());
                    return;
                }
                foreach (GameObject gameObject in Enemy)
                {
                    game.player.CheckColiision(gameObject, ((Enemy)gameObject).DealDamage());
                    game.playerAtkEfx.CheckColiision(gameObject);
                    game.playerSkill.CheckColiision(gameObject);                    
                    if (gameObject.GetType().IsAssignableTo(typeof(Melee_Enemy)))
                    {
                        ((Melee_Enemy)gameObject).Update(game.player, elapsed);
                        ((Melee_Enemy)gameObject).CheckColiision(game.player);
                    }
                    if (gameObject.GetType().IsAssignableTo(typeof(Melee_Enemy)))
                    {
                        ((Melee_Enemy)gameObject).UpdateFrame(elapsed);
                    }
                    if (gameObject.GetType().IsAssignableTo(typeof(Range_Enemy)))
                    {
                        ((Range_Enemy)gameObject).Update(game.player, elapsed);
                        ((Range_Enemy)gameObject).CheckColiision(game.player);
                    }                    
                    if (gameObject.GetType().IsAssignableTo(typeof(Range_Enemy)))
                    {
                        ((Range_Enemy)gameObject).UpdateFrame(elapsed);
                    }
                }
                foreach (BoundsCheck bounds in Bounds)
                {
                    player.CheckMapColiision(bounds);
                    foreach (GameObject gameObject in Enemy)
                    {
                        if (gameObject.GetType().IsAssignableTo(typeof(Melee_Enemy)))
                        {
                            ((Melee_Enemy)gameObject).CheckMapColiision(bounds, elapsed);
                        }
                    }
                }
                if (ks.IsKeyDown(Keys.Escape) && oldks.IsKeyUp(Keys.Escape))
                {
                    Game1.GAME_STATE = 2;
                }
                
            }
            else if (Game1.GAME_STATE == 2)
            {
                if (ks.IsKeyDown(Keys.Enter) && !player.GetAlive())
                {
                    Game1.GAME_STATE = 1;
                    player.Restart();
                    foreach (GameObject gameObject in Enemy)
                    {
                        if (gameObject.GetType().IsAssignableTo(typeof(Melee_Enemy)))
                        {
                            ((Melee_Enemy)gameObject).Restart();
                        }
                        if (gameObject.GetType().IsAssignableTo(typeof(Range_Enemy)))
                        {
                            ((Range_Enemy)gameObject).Restart();
                        }
                    }
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
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Level1, Vector2.Zero, Color.White);
            foreach (GameObject gameObject in Enemy)
            {
                if (gameObject.GetType().IsAssignableTo(typeof(Range_Enemy)))
                {
                    ((Range_Enemy)gameObject).Draw(spriteBatch);
                }
                if (gameObject.GetType().IsAssignableTo(typeof(Melee_Enemy)))
                {
                    ((Melee_Enemy)gameObject).Draw(spriteBatch);
                }                

            }

            if (playerPos.X < 1060)
            {
                //W A S D
                spriteBatch.Draw(tutorialKeys, new Vector2(Game1.TILE_SIZE *5, Game1.TILE_SIZE * 12 +16),new Rectangle(16, 0, 16, 16), Color.White, 0f, Vector2.Zero, new Vector2(2f, 2f), 0, 0);
                spriteBatch.Draw(tutorialKeys, new Vector2(Game1.TILE_SIZE * 5, Game1.TILE_SIZE * 12 + 64), new Rectangle(48, 0, 16, 16), Color.White, 0f, Vector2.Zero, new Vector2(2f, 2f), 0, 0);
                spriteBatch.Draw(tutorialKeys, new Vector2(Game1.TILE_SIZE * 5 + 48, Game1.TILE_SIZE * 12 + 64), new Rectangle(64, 0, 16, 16), Color.White, 0f, Vector2.Zero, new Vector2(2f, 2f), 0, 0);
                spriteBatch.Draw(tutorialKeys, new Vector2(Game1.TILE_SIZE * 5 - 48, Game1.TILE_SIZE * 12 + 64), new Rectangle(32, 0, 16, 16), Color.White, 0f, Vector2.Zero, new Vector2(2f, 2f), 0, 0);
                //Rush E
                spriteBatch.Draw(tutorialKeys, new Vector2(Game1.TILE_SIZE * 5 + 48, Game1.TILE_SIZE * 12 + 16), new Rectangle(0, 0, 16, 16), Color.White, 0f, Vector2.Zero, new Vector2(2f, 2f), 0, 0);
                //Shift
                spriteBatch.Draw(tutorialKeys, new Vector2(Game1.TILE_SIZE * 4 - 80, Game1.TILE_SIZE * 12 + 64), new Rectangle(80, 0, 70, 16), Color.White, 0f, Vector2.Zero, new Vector2(2f, 2f), 0, 0);
                //Mouse
                spriteBatch.Draw(tutorialKeys, new Vector2(Game1.TILE_SIZE * 6, Game1.TILE_SIZE * 12 + 16), new Rectangle(112, 20, 29, 42), Color.White, 0f, Vector2.Zero, new Vector2(2f, 2f), 0, 0);
            }
            base.Draw(spriteBatch);
        }
        public static void ResetScreen(Player player)
        {
            player.Restart();
            foreach (GameObject gameObject in Enemy)
            {
                if (gameObject.GetType().IsAssignableTo(typeof(Melee_Enemy)))
                {
                    ((Melee_Enemy)gameObject).Restart();
                }
                if (gameObject.GetType().IsAssignableTo(typeof(Range_Enemy)))
                {
                    ((Range_Enemy)gameObject).Restart();
                }
            }
        }
    }
}
