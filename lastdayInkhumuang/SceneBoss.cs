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
        List<GameObject> Boss = new List<GameObject>();
        public SceneBoss(Game1 game,EventHandler theScreenEvent) : base(theScreenEvent)
        {
            bossScene = game.Content.Load<Texture2D>("Scenes/Bg1");            
            Boss.Add(new MiniBoss1(game, new Vector2(), 256, 256, 1, 8, 1, 0.5f));
            this.game = game;
        }
        public override void Update(GameTime gameTime, float elapsed, Player player)
        {
            Game1.LOCK_CAM = true;
            if (Game1.GAME_STATE == 0)
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
            }
            

            //1stBoss
            //miniBoss.Update(elapsed, game.player);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bossScene, Vector2.Zero, Color.White);
            foreach (GameObject gameObject in Boss)
            {
                ((MiniBoss1)gameObject).Draw(spriteBatch);
            }
        }
    }
}
