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
        public static List<GameObject> Enemy = new List<GameObject>();
        public static List<BoundsCheck> Bounds = new List<BoundsCheck>();
        Game1 game;

        bool mapClear;
        KeyboardState ks;
        KeyboardState oldks;
        public Level1Screen(Game1 game,EventHandler theScreenEvent) : base(theScreenEvent)
        {
            Level1 = game.Content.Load<Texture2D>("Scenes/1st_Level");
            mapClear = false;
            //rasengan = new Rasengan(game, 25, 25, 0.5f);
            Enemy.Add(new Melee_Enemy(game, new Vector2(800, 400), Vector2.Zero, 90, 150, 5, 7, 5, 0.3f));
            Enemy.Add(new Range_Enemy(game, new Vector2(800, 400), Vector2.Zero, 90, 150, 5, 7, 5, 0.3f));
            Enemy.Add(new Melee_Enemy(game, new Vector2(800, 200), Vector2.Zero, 125, 150, 5, 7, 5, 0.3f));
            Enemy.Add(new Melee_Enemy(game, new Vector2(1200, 200), Vector2.Zero, 125, 150, 5, 7, 5, 0.3f));

            //Bounds
            Bounds.Add(new BoundsCheck(game, new Vector2(0, Game1.TILE_SIZE * 9), Game1.TILE_SIZE * 2, Game1.TILE_SIZE * 20));
            Bounds.Add(new BoundsCheck(game, new Vector2(0, Game1.TILE_SIZE * 5), Game1.TILE_SIZE * 4, Game1.TILE_SIZE * 4));
            Bounds.Add(new BoundsCheck(game, new Vector2(0, 0), Game1.TILE_SIZE * 5, Game1.TILE_SIZE));
            Bounds.Add(new BoundsCheck(game, new Vector2(0, 0), Game1.TILE_SIZE, Game1.TILE_SIZE * 25));
            Bounds.Add(new BoundsCheck(game, new Vector2(Game1.TILE_SIZE * 24, Game1.TILE_SIZE * 4), Game1.TILE_SIZE * 11, Game1.TILE_SIZE));
            Bounds.Add(new BoundsCheck(game, new Vector2(Game1.TILE_SIZE * 6, Game1.TILE_SIZE * 5), Game1.TILE_SIZE * 2, Game1.TILE_SIZE * 18));
            //BoundsOutMap
            Bounds.Add(new BoundsCheck(game, new Vector2(-(Game1.TILE_SIZE * 2), 0), Game1.TILE_SIZE * 15, Game1.TILE_SIZE * 2));
            Bounds.Add(new BoundsCheck(game, new Vector2((Game1.TILE_SIZE * 25), 0), Game1.TILE_SIZE * 15, Game1.TILE_SIZE * 2));
            Bounds.Add(new BoundsCheck(game, new Vector2(0, -(Game1.TILE_SIZE * 2)), Game1.TILE_SIZE * 2, Game1.TILE_SIZE * 25));
            Bounds.Add(new BoundsCheck(game, new Vector2(0, (Game1.TILE_SIZE * 14)), Game1.TILE_SIZE * 2, Game1.TILE_SIZE * 25));

            Game1.monsterCount = Enemy.Count;
            this.game= game;
        }
        public override void Update(GameTime gameTime, float elapsed, Player player)
        {
            Game1.LOCK_CAM = false;
            ks = Keyboard.GetState();
            if (Game1.GAME_STATE == 1)
            {                
                if (Game1.monsterCount == 0 && player.Bounds.X + 64 >= Game1.MAP_WIDTH - 10 && player.Bounds.Y > 128 && player.Bounds.Y < 384)
                {
                    Game1._cameraPosition = new Vector2(0,0);
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
                foreach (BoundsCheck bounds in Level1Screen.Bounds)
                {
                    player.CheckMapColiision(bounds);
                    foreach (GameObject gameObject in Enemy)
                    {
                        if (gameObject.GetType().IsAssignableTo(typeof(Melee_Enemy)))
                        {
                            ((Melee_Enemy)gameObject).CheckMapColiision(bounds);
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
