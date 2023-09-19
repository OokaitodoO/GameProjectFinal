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
        List<GameObject> Enemy = new List<GameObject>();
        Game1 game;

        bool mapClear;
        public Level1Screen(Game1 game,EventHandler theScreenEvent) : base(theScreenEvent)
        {
            Level1 = game.Content.Load<Texture2D>("Scenes/1st_Level");
            mapClear = false;
            //rasengan = new Rasengan(game, 25, 25, 0.5f);
            Enemy.Add(new Melee_Enemy(game, new Vector2(800, 600), Vector2.Zero, 90, 150, 5, 7, 5, 0.3f));
            Enemy.Add(new Range_Enemy(game, new Vector2(800, 400), Vector2.Zero, 90, 150, 5, 7, 5, 0.3f));
            //Enemy.Add(new Melee_Enemy(game, new Vector2(800, 200), Vector2.Zero, "Null", 125, 150, 5, 7, 5, 0.3f));
            //Enemy.Add(new Melee_Enemy(game, new Vector2(1200, 200), Vector2.Zero, "Null", 125, 150, 5, 7, 5, 0.3f));
            Game1.monsterCount = Enemy.Count;
            this.game= game;
        }
        public override void Update(GameTime gameTime, float elapsed, Player player)
        {
            //Console.WriteLine(Game1.monsterCount);
            Game1.LOCK_CAM = false;
            if (Game1.GAME_STATE == 0)
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
                    Console.WriteLine(((Enemy)gameObject).DealDamage());
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
                        //rasengan.Update(elapsed, player, (Range_Enemy)gameObject);
                    }                    
                    if (gameObject.GetType().IsAssignableTo(typeof(Range_Enemy)))
                    {
                        ((Range_Enemy)gameObject).UpdateFrame(elapsed);
                    }
                }                
            }                      
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
    }
}
