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
    public class Level2Screen : Screen
    {
        Game1 game;

        Texture2D level2;


        public static List<GameObject> Enemy = new List<GameObject>();
        public static List<BoundsCheck> Bounds = new List<BoundsCheck>();

        KeyboardState ks;
        KeyboardState oldKs;
        public Level2Screen(Game1 game, EventHandler theScreenEvent) : base(theScreenEvent)
        {
            level2 = game.Content.Load<Texture2D>("Scenes/Map_Scene3_Layout");
            this.game = game;

            //BoundsMap
            //Bounds.Add(new BoundsCheck(game, new Vector2(0, 0), Game1.TILE_SIZE * 10, Game1.TILE_SIZE));
            //Bounds.Add(new BoundsCheck(game, new Vector2(0, Game1.TILE_SIZE * 9), Game1.TILE_SIZE, Game1.TILE_SIZE * 20));
            //Bounds.Add(new BoundsCheck(game, new Vector2(Game1.TILE_SIZE * 5, Game1.TILE_SIZE * 8), Game1.TILE_SIZE, Game1.TILE_SIZE * 9));
            //Bounds.Add(new BoundsCheck(game, new Vector2(Game1.TILE_SIZE * 9, Game1.TILE_SIZE * 7), Game1.TILE_SIZE, Game1.TILE_SIZE * 5));
            //Bounds.Add(new BoundsCheck(game, new Vector2(Game1.TILE_SIZE * 18, Game1.TILE_SIZE * 7), Game1.TILE_SIZE * 2, Game1.TILE_SIZE * 2));
            //Bounds.Add(new BoundsCheck(game, new Vector2(Game1.TILE_SIZE * 5, 0), Game1.TILE_SIZE * 7, Game1.TILE_SIZE * 3));
            //Bounds.Add(new BoundsCheck(game, new Vector2(Game1.TILE_SIZE * 8, 0), Game1.TILE_SIZE * 3, Game1.TILE_SIZE * 12));
            //Bounds.Add(new BoundsCheck(game, new Vector2(Game1.TILE_SIZE * 20, 0), Game1.TILE_SIZE * 2, Game1.TILE_SIZE * 5));
            //Bounds.Add(new BoundsCheck(game, new Vector2(Game1.TILE_SIZE * 24, 0), Game1.TILE_SIZE * 15, Game1.TILE_SIZE));

            //BoundsOutMap
            Bounds.Add(new BoundsCheck(game, new Vector2(-(Game1.TILE_SIZE * 2), 0), Game1.TILE_SIZE * 15, Game1.TILE_SIZE * 2));
            Bounds.Add(new BoundsCheck(game, new Vector2((Game1.TILE_SIZE * 25), 0), Game1.TILE_SIZE * 15, Game1.TILE_SIZE * 2));
            Bounds.Add(new BoundsCheck(game, new Vector2(0, -(Game1.TILE_SIZE * 2)), Game1.TILE_SIZE * 2, Game1.TILE_SIZE * 25));
            Bounds.Add(new BoundsCheck(game, new Vector2(0, (Game1.TILE_SIZE * 13)), Game1.TILE_SIZE * 3, Game1.TILE_SIZE * 25));
        }
        public override void Update(GameTime gameTime, float elapsed, Player player)
        {
            ks = Keyboard.GetState();
            Game1.LOCK_CAM = false;
            if (Game1.GAME_STATE == 1)
            {
                //ChangeMap
                if (Game1.monsterCount == 0 && player.Bounds.X + 64 <= Game1.TILE_SIZE && player.Bounds.Y > Game1.TILE_SIZE * 9 && player.Bounds.Y < Game1.TILE_SIZE * 14)
                {
                    Game1.IsCutscenes = false;
                    player.SetPos(new Vector2(0, 960));
                    Game1.monsterCount = 1;
                    Sfx.InsPlaySfx(21);
                    Sfx.InsStopSfx(20);
                    ScreenEvent.Invoke(this, new EventArgs());
                    return;
                }
                //CheckCollisionMap
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
                //CheckCollisionEnemy
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

                //Pause
                if (ks.IsKeyDown(Keys.Escape) && oldKs.IsKeyUp(Keys.Escape))
                {
                    Game1.GAME_STATE = 2;
                    Sfx.PlaySfx(0);
                    Sfx.InsStopSfx(14);
                }
            }
            else if (Game1.GAME_STATE == 2) //Pausing
            {
                //RestartMap
                if (ks.IsKeyDown(Keys.Enter) && !player.GetAlive())
                {
                    Game1.GAME_STATE = 1;
                    player.Restart(new Vector2(350, 250));                    
                }
                else if (ks.IsKeyDown(Keys.Enter) && player.GetAlive()) //BackToMenu
                {
                    Game1.LOCK_CAM = true;
                    Game1.GAME_STATE = 0;
                    Game1._cameraPosition = Vector2.Zero;
                    Game1._camera.LookAt(Game1._bgPosition);
                    game.mCurrentScreen = game.mTitle;
                    player.Restart();
                    Sfx.PlaySfx(0);
                    Sfx.InsStopSfx(20);
                    Sfx.InsPlaySfx(19);
                    Sfx.InsStopSfx(14);
                }
                else if (ks.IsKeyDown(Keys.Escape) && oldKs.IsKeyUp(Keys.Escape)) //Resume
                {
                    Game1.GAME_STATE = 1;
                    Sfx.PlaySfx(0);
                }
            }
            oldKs = ks;
        }
        public static void ResetScreen(Player player)
        {
            player.Restart(new Vector2(350, 250));           
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(level2, Vector2.Zero, Color.White);
            base.Draw(spriteBatch);
        }
    }
}
