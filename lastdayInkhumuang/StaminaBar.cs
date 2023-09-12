using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lastdayInkhumuang
{
    public class StaminaBar : Ui
    {
        Texture2D staminaBar;
        Color[] data;

        const int MAX_STAMINA = 100;
        float staminaWidth;
        public StaminaBar(Game1 game, Vector2 position, int width, int height, float layerDepth) : base(game, position, width, height, layerDepth)
        {
            this.drawWidth = width;
            this.drawHeight = height;
        }

        public void Update(float elapsed, Player player, GraphicsDevice gd)
        {
            position = new Vector2(Game1._cameraPosition.X, Game1._cameraPosition.Y + 30);
            if (player.GetStamina() > 0)
            {
                staminaWidth = (player.GetStamina() / MAX_STAMINA) * drawWidth;
            }
            else
            {
                staminaWidth = 1;
            }
            staminaBar = new Texture2D(gd, (int)staminaWidth, drawHeight);
            data = new Color[(int)staminaWidth * drawHeight];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = Color.LightGoldenrodYellow;
            }
            staminaBar.SetData(data);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(staminaBar, position, Color.White);
        }

        public int GetHeight()
        {
            return drawHeight;
        }
    }
}
