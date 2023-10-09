using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace lastdayInkhumuang
{
    public class SelectScreen : Screen
    {
        Texture2D bg;
        Game1 game;
        public SelectScreen(Game1 game, EventHandler Level1, EventHandler Level2) : base(Level1, Level2)
        {
            this.game = game;
            bg = game.Content.Load<Texture2D>("Scenes/Bg1");
        }
        public void Update(float elapsed)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.F1))
            {
                Game1.GAME_STATE = 1;
                ScreenEvent.Invoke(this, new EventArgs());
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.F2) && ManageGameSaveFile.GetFileExists())
            {
                if (ManageGameSaveFile.listText[0] == "UnlockLevel2")
                {
                    Game1.GAME_STATE = 1;
                    ScreenEvent2.Invoke(this, new EventArgs());
                }                
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bg, Vector2.Zero, Color.White);
        }
    }
}
