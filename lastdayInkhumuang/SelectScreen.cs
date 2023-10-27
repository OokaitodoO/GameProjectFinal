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
        Texture2D button;
        Game1 game;

        Vector2 level1Pos;
        Vector2 level2Pos;
        Rectangle level1Rec;
        Rectangle level2Rec;

        SpriteFont text;

        KeyboardState ks;
        KeyboardState oldKs;

        bool warning;
        bool IsPlaySfx;
        public SelectScreen(Game1 game, EventHandler Level1, EventHandler Level2) : base(Level1, Level2)
        {
            this.game = game;
            bg = game.Content.Load<Texture2D>("Scenes/Select");
            text = game.Content.Load<SpriteFont>("Ui/File");
            button = game.Content.Load<Texture2D>("Ui/mapselect");
            warning = false;

            level1Pos = new Vector2(150, 250);
            level1Rec = new Rectangle((int)level1Pos.X, (int)level1Pos.Y, 300, 120);
            level2Pos = new Vector2(615, 250);
            level2Rec = new Rectangle((int)level2Pos.X, (int)level2Pos.Y, 300, 120);
        }
        public void Update(MouseState ms, MouseState oldMs, float elapsed)
        {
            ks = Keyboard.GetState();
            if (warning)
            {
                if (ks.IsKeyDown(Keys.Enter) && oldKs.IsKeyUp(Keys.Enter))
                {
                    ManageGameSaveFile.DeleteSave();
                    warning = false;
                }
                else if(ks.IsKeyDown(Keys.Escape) && oldKs.IsKeyUp(Keys.Escape))
                {
                    warning = false;
                }
            }
            else
            {
                if ((Game1.mouseRec.Intersects(level1Rec) || Game1.mouseRec.Intersects(level2Rec)) && !IsPlaySfx)
                {
                    if (ManageGameSaveFile.GetFileExists() && Game1.mouseRec.Intersects(level2Rec))
                    {
                        Sfx.PlaySfx(7);
                    }
                    else if (Game1.mouseRec.Intersects(level1Rec))
                    {
                        Sfx.PlaySfx(7);
                    }
                    IsPlaySfx = true;
                }
                else if (!(Game1.mouseRec.Intersects(level1Rec) || Game1.mouseRec.Intersects(level2Rec)))
                {
                    IsPlaySfx = false;
                }
                if (ks.IsKeyDown(Keys.F1) || Game1.mouseRec.Intersects(level1Rec) && ms.LeftButton == ButtonState.Pressed && oldMs.LeftButton != ButtonState.Pressed)
                {
                    Sfx.PlaySfx(0);
                    Sfx.InsStopSfx(19);
                    Sfx.InsPlaySfx(20);
                    Game1.GAME_STATE = 1;
                    ScreenEvent.Invoke(this, new EventArgs());
                    return;
                }
                else if ((ks.IsKeyDown(Keys.F2) || Game1.mouseRec.Intersects(level2Rec) && ms.LeftButton == ButtonState.Pressed && oldMs.LeftButton != ButtonState.Pressed) && ManageGameSaveFile.GetFileExists())
                {
                    if (ManageGameSaveFile.listText[0] == "UnlockLevel2")
                    {
                        Sfx.PlaySfx(0);
                        Sfx.InsStopSfx(19);
                        Sfx.InsPlaySfx(20);
                        Game1.GAME_STATE = 1;
                        ScreenEvent2.Invoke(this, new EventArgs());
                        return;
                    }
                }
                else if (ks.IsKeyDown(Keys.F5) && ManageGameSaveFile.GetFileExists())
                {
                    warning = true;
                }
                if (ks.IsKeyDown(Keys.Escape) && oldKs.IsKeyUp(Keys.Escape))
                {
                    Sfx.PlaySfx(0);
                    Game1.LOCK_CAM = true;
                    Game1.changeScreen = true;
                    Game1.GAME_STATE = 0;
                    FrontObject.timer = 1;
                    Game1._cameraPosition = Vector2.Zero;
                    Game1._camera.LookAt(Game1._bgPosition);
                    game.mCurrentScreen = game.mTitle;
                }
            }
            oldKs = ks;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bg, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, new Vector2(0.8f, 0.8f), 0, 0);

            if (Game1.mouseRec.Intersects(level1Rec))
            {
                spriteBatch.Draw(button, level1Pos, new Rectangle(300, 0, 300, 120), Color.White);
            }
            else
            {
                spriteBatch.Draw(button, level1Pos, new Rectangle(0, 0, 300, 120), Color.White);
            }
            if (ManageGameSaveFile.GetFileExists())
            {
                if (Game1.mouseRec.Intersects(level2Rec))
                {
                    spriteBatch.Draw(button, level2Pos, new Rectangle(300, 120, 300, 120), Color.White);
                }
                else
                {
                    spriteBatch.Draw(button, level2Pos, new Rectangle(0, 120, 300, 120), Color.White);
                }
            }
            else
            {
                spriteBatch.Draw(button, level2Pos, new Rectangle(0, 240, 300, 120), Color.White);
            }
           
            if (warning)
            {
                spriteBatch.DrawString(text, "Are you sure?\n Enter: to delete save\n Esc: to cancle", Vector2.Zero, Color.Red);
            }

            
        }
    }
}
