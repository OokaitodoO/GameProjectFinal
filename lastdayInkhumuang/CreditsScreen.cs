using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lastdayInkhumuang
{
    public class CreditsScreen : Screen
    {
        Game1 game;
        Texture2D Bg;

        private Texture2D creditsText;
        public static Vector2 creditsPos;
        public CreditsScreen(Game1 game, EventHandler theScreenEvent) : base(theScreenEvent)
        {
            this.game = game;
            Bg = game.Content.Load<Texture2D>("Scenes/Credits");
            creditsText = game.Content.Load<Texture2D>("Ui/Text/credit");
        }

        public override void Update(GameTime gameTime, float elapsed, Player player)
        {
            if (game.oldScreen == game.mLevel2 && ManageGameSaveFile.listText.Count == 2 )
            {
                Sfx.PlaySfx(0);
                ScreenEvent.Invoke(this, new EventArgs());
                return;
            }
            if (game.oldScreen == game.mLevel2 && ManageGameSaveFile.listText.Count == 1)
            {
                if (creditsPos.Y + creditsText.Height > -40)
                {
                    creditsPos += new Vector2(0, -1);
                }
                else
                {
                    Sfx.PlaySfx(0);
                    ManageGameSaveFile.AddSave("Credited");
                    ScreenEvent.Invoke(this, new EventArgs());
                    return;
                }
            }
            else
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    Sfx.PlaySfx(0);
                    ScreenEvent.Invoke(this, new EventArgs());
                    return;
                }
                if (creditsPos.Y + creditsText.Height > -40)
                {
                    creditsPos += new Vector2(0, -1);
                }
                else
                {
                    creditsPos = new Vector2(0, Game1.MAP_HEIGHT / 3);
                }
            }            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Bg, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, new Vector2(0.8f, 0.8f), 0, 0);
            spriteBatch.Draw(creditsText, creditsPos, Color.White);
        }

        public static void SetCreditsPos()
        {
            creditsPos = new Vector2(0, 600);
        }
    }
}
