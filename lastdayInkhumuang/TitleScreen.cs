using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lastdayInkhumuang
{
    public class TitleScreen : Screen
    {
        Game1 game;
        Texture2D menu;
        Rectangle playButton;
        Rectangle creditsButton;
        Rectangle exitButton;
        Vector2 playPos;
        Vector2 creditsPos;
        Vector2 exitPos;
        const int buttonWidth = 185;
        const int buttonHeight = 38;
        public TitleScreen(Game1 game, EventHandler theScreenEvent) : base(theScreenEvent)
        {
            this.game = game;
            menu = game.Content.Load<Texture2D>("menu");
            
            playPos = new Vector2(250, 250);
            playButton = new Rectangle((int)playPos.X, (int)playPos.Y, buttonWidth, buttonHeight);
            creditsPos = new Vector2(250, 300);
            creditsButton = new Rectangle((int)creditsPos.X, (int)creditsPos.Y, buttonWidth, buttonHeight);
            exitPos = new Vector2(250, 350);
            exitButton = new Rectangle((int)exitPos.X, (int)exitPos.Y, buttonWidth, buttonHeight);
        }
        public void Update(MouseState ms, float elapsed)
        {
            if (Game1.mouseRec.Intersects(playButton) && ms.LeftButton == ButtonState.Pressed) //Play
            {
                Game1.GAME_STATE = 1;
                ScreenEvent.Invoke(this, new EventArgs());
                return;
            }
            if (Game1.mouseRec.Intersects(exitButton) && ms.LeftButton == ButtonState.Pressed) //Exit
            {
                game.Exit();                
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Game1.mouseRec.Intersects(playButton)) //play
            {
                spriteBatch.Draw(menu, playPos, new Rectangle(0, 0, buttonWidth, buttonHeight), Color.White);
            }
            else
            {
                spriteBatch.Draw(menu, playPos, new Rectangle(buttonWidth, 0, buttonWidth, buttonHeight), Color.White);
            }

            if (!Game1.mouseRec.Intersects(exitButton)) // Exit
            {
                spriteBatch.Draw(menu, exitPos, new Rectangle(0, buttonHeight*4, buttonWidth, buttonHeight), Color.White);
            }
            else
            {
                spriteBatch.Draw(menu, exitPos, new Rectangle(buttonWidth, buttonHeight * 4, buttonWidth, buttonHeight), Color.White);
            }
        }
    }
}
