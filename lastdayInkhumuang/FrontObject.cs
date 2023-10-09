using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace lastdayInkhumuang
{
    public class FrontObject
    {
        List<Roof> roof1 = new List<Roof>();
        
        Texture2D statusBar;
        Texture2D fade;
        public static float timer;
        float elapsed;
        const int boss1_MaxHp = 500;
        const int boss1_MaxHpWidth = 519;
        const int boss1_MaxHpHeight = 14;
        Vector2 bossHpPos = new Vector2(((Game1.MAP_WIDTH / 3) / 2) - (boss1_MaxHpWidth / 2), 550);
        float boss1HpWidth;
        public FrontObject(Game1 game)
        {
            //BossBar
            statusBar = game.Content.Load<Texture2D>("Ui/UI-characrtet_&Boss");
            //FadeScreen
            fade = game.Content.Load<Texture2D>("Scenes/Bg1");
            //Down-Line
            roof1.Add(new Roof(game, new Vector2(Game1.TILE_SIZE*18, Game1.TILE_SIZE*8), Game1.TILE_SIZE * 8, Game1.TILE_SIZE, Game1.TILE_SIZE/2, Game1.TILE_SIZE *2, Game1.TILE_SIZE, Game1.TILE_SIZE*2));
            roof1.Add(new Roof(game, new Vector2(Game1.TILE_SIZE * 14, Game1.TILE_SIZE * 8), Game1.TILE_SIZE * 3, 0, Game1.TILE_SIZE / 2, Game1.TILE_SIZE * 3, Game1.TILE_SIZE, Game1.TILE_SIZE * 3));
            roof1.Add(new Roof(game, new Vector2(Game1.TILE_SIZE * 12, Game1.TILE_SIZE * 8), Game1.TILE_SIZE * 6, 0, Game1.TILE_SIZE / 2, Game1.TILE_SIZE * 2, Game1.TILE_SIZE, Game1.TILE_SIZE * 2));
            roof1.Add(new Roof(game, new Vector2(Game1.TILE_SIZE * 9, Game1.TILE_SIZE * 8), Game1.TILE_SIZE * 8, 0, Game1.TILE_SIZE / 2, Game1.TILE_SIZE * 2, Game1.TILE_SIZE, Game1.TILE_SIZE * 2));
            roof1.Add(new Roof(game, new Vector2(Game1.TILE_SIZE * 7, Game1.TILE_SIZE * 8), Game1.TILE_SIZE * 8, Game1.TILE_SIZE, Game1.TILE_SIZE / 2, Game1.TILE_SIZE * 2, Game1.TILE_SIZE, Game1.TILE_SIZE * 2));
            roof1.Add(new Roof(game, new Vector2(Game1.TILE_SIZE * 4, Game1.TILE_SIZE * 8), 0, 0, Game1.TILE_SIZE / 2, Game1.TILE_SIZE * 3, Game1.TILE_SIZE, Game1.TILE_SIZE * 3));
            //Top-Line
            roof1.Add(new Roof(game, new Vector2(Game1.TILE_SIZE, Game1.TILE_SIZE * 4), Game1.TILE_SIZE * 3, Game1.TILE_SIZE, Game1.TILE_SIZE / 2, Game1.TILE_SIZE * 3, Game1.TILE_SIZE, Game1.TILE_SIZE * 3));
            roof1.Add(new Roof(game, new Vector2(Game1.TILE_SIZE * 6, Game1.TILE_SIZE * 4), Game1.TILE_SIZE * 3, 0, Game1.TILE_SIZE / 2, Game1.TILE_SIZE * 3, Game1.TILE_SIZE, Game1.TILE_SIZE * 3));
            roof1.Add(new Roof(game, new Vector2(Game1.TILE_SIZE * 9, Game1.TILE_SIZE * 4), Game1.TILE_SIZE * 6, Game1.TILE_SIZE, Game1.TILE_SIZE / 2, Game1.TILE_SIZE * 2, Game1.TILE_SIZE, Game1.TILE_SIZE * 2));
            roof1.Add(new Roof(game, new Vector2(Game1.TILE_SIZE * 11, Game1.TILE_SIZE * 4), Game1.TILE_SIZE * 6, 0, Game1.TILE_SIZE / 2, Game1.TILE_SIZE * 2, Game1.TILE_SIZE, Game1.TILE_SIZE * 2));
            roof1.Add(new Roof(game, new Vector2(Game1.TILE_SIZE * 13, Game1.TILE_SIZE * 4), Game1.TILE_SIZE * 6, Game1.TILE_SIZE * 2, Game1.TILE_SIZE / 2, Game1.TILE_SIZE * 2, Game1.TILE_SIZE, Game1.TILE_SIZE * 2));
            roof1.Add(new Roof(game, new Vector2(Game1.TILE_SIZE * 16, Game1.TILE_SIZE * 4), Game1.TILE_SIZE * 8, 0, Game1.TILE_SIZE / 2, Game1.TILE_SIZE * 2, Game1.TILE_SIZE, Game1.TILE_SIZE * 2));
            roof1.Add(new Roof(game, new Vector2(Game1.TILE_SIZE * 18, Game1.TILE_SIZE * 4), Game1.TILE_SIZE * 6, 0, Game1.TILE_SIZE / 2, Game1.TILE_SIZE * 2, Game1.TILE_SIZE, Game1.TILE_SIZE * 2));
            roof1.Add(new Roof(game, new Vector2(Game1.TILE_SIZE * 20, Game1.TILE_SIZE * 4), Game1.TILE_SIZE * 6, Game1.TILE_SIZE * 2, Game1.TILE_SIZE / 2, Game1.TILE_SIZE * 2, Game1.TILE_SIZE, Game1.TILE_SIZE * 2));
            roof1.Add(new Roof(game, new Vector2(Game1.TILE_SIZE * 22, Game1.TILE_SIZE * 4), Game1.TILE_SIZE * 6, Game1.TILE_SIZE * 2, Game1.TILE_SIZE / 2, Game1.TILE_SIZE * 2, Game1.TILE_SIZE, Game1.TILE_SIZE * 2));
            roof1.Add(new Roof(game, new Vector2(Game1.TILE_SIZE * 24, Game1.TILE_SIZE * 3), Game1.TILE_SIZE * 3, Game1.TILE_SIZE * 2, Game1.TILE_SIZE / 2, Game1.TILE_SIZE, Game1.TILE_SIZE, Game1.TILE_SIZE));
        }        
        public void Update(float elapsed) 
        {
            //this.elapsed = elapsed;            
            boss1HpWidth = (MiniBoss1.hp / boss1_MaxHp) * boss1_MaxHpWidth;

            if (timer > 0)
            {
                timer -= elapsed / 2.5f;
            }
            if (timer <= 0)
            {
                timer = 0;
                Game1.changeScreen = false;
            }
        }


        public void Draw(SpriteBatch spriteBatch, Player player, Game1 game)
        {
            if (game.mCurrentScreen == game.mLevel1)
            {
                foreach (Roof roof in roof1)
                {
                    if (roof.Bounds.Intersects(player.Bounds))
                    {
                        roof.Draw(spriteBatch, 0.5f);
                    }
                    else
                    {
                        roof.Draw(spriteBatch, 1f);
                    }
                }
            }
            if (game.mCurrentScreen == game.mBoss1)
            {
                spriteBatch.Draw(statusBar, bossHpPos + new Vector2(9, 4), new Rectangle(57, 252, (int)boss1HpWidth, 48), Color.Red, 0f, Vector2.Zero, new Vector2(1f, 0.6f), 0, 0);
                spriteBatch.Draw(statusBar, bossHpPos, new Rectangle(45, 191, 519, 48), Color.White, 0f, Vector2.Zero, new Vector2(1f, 0.6f), 0, 0);
            }

            //DrawFade
            if (Game1.changeScreen)
            {
                spriteBatch.Draw(fade, Game1._cameraPosition - new Vector2(30, 30), Color.Black * timer);
            }
        }
    }
}
