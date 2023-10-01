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
    public class Player_Boss_StatusBar : Ui
    {
        Texture2D statusBar;
        Vector2 hpPos;
        Color hpColor;
        const int MAX_HP = 100;
        const int MAX_HPBAR_WIDTH = 375;
        const int HPBAR_HEIGHT = 50;
        const int MAX_STM = 100;
        const int MAX_STM_WIDTH = 375;
        const int STMBAR_HEIGHT = 14;
        float staminaWidth;
        float hpWidth;
        Vector2 staminaPos;
        int height;
        int width;

        Vector2 bossHpPos;
        //BossHp
        
        public Player_Boss_StatusBar(Game1 game, Vector2 position, int width, int height, float layerDepth) : base(game, position, width, height, layerDepth)
        {
            statusBar = game.Content.Load<Texture2D>("Ui/UI-characrtet_&Boss");
            this.height = height;
            this.width = width;
            bossHpPos = new Vector2();
        }
        public void Update(float elapsed, Player player, GraphicsDevice gd, Game1 game)
        {           
            position = Game1._cameraPosition;
            hpPos = position + new Vector2(130, 13);
            staminaPos = position + new Vector2(132, 54);
            hpWidth = (player.GetHp() / MAX_HP) * MAX_HPBAR_WIDTH;
            staminaWidth = (player.GetStamina() / MAX_STM) * MAX_STM_WIDTH;            

            if (hpWidth >= 188)
            {
                hpColor = Color.LimeGreen;
            }
            else if (hpWidth >= 94)
            {
                hpColor = Color.Yellow;
            }
            else if (hpWidth >= 0)
            {
                hpColor = Color.Red;
            }

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(statusBar, hpPos, new Rectangle(57, 252, (int)hpWidth, HPBAR_HEIGHT), hpColor, 0f, Vector2.Zero, new Vector2(0.7f, 0.7f), 0, 0);
            spriteBatch.Draw(statusBar, staminaPos, new Rectangle(57, 252, (int)staminaWidth, STMBAR_HEIGHT), Color.LightGoldenrodYellow, 0f, Vector2.Zero, new Vector2(0.702f, 0.7f), 0, 0);
            spriteBatch.Draw(statusBar, position, new Rectangle(0, 0, width, height), Color.White, 0f, Vector2.Zero, new Vector2(0.7f, 0.7f), 0, 0);
        }
    }
}
