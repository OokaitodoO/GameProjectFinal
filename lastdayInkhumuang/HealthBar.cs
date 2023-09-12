using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lastdayInkhumuang
{
    public class HealthBar : Ui, IGameFunction
    {
        Texture2D hpBar;
        Color[] data;
        Color hpColor;

        const int MAX_HP = 100;
        const int MAX_HPBAR = 250;
        const int HPBAR_HEIGHT = 30;
        float hpWidth;
        public HealthBar(Game1 game, Vector2 position, int width, int height, float layerDepth) : base(game, position, width, height, layerDepth)
        {
            this.drawHeight = height;
        }

        public void Update(float elapsed, Player player, GraphicsDevice gd)
        {
            position = Game1._cameraPosition;
            if (player.GetHp() > 0)
            {
                hpWidth = (player.GetHp() / MAX_HP) * MAX_HPBAR;
            }
            else
            {
                hpWidth = 1;
            }
            hpBar = new Texture2D(gd, (int)hpWidth, drawHeight);
            data = new Color[(int)hpWidth* HPBAR_HEIGHT];
            if (hpWidth > 125)
            {
                hpColor = Color.Green;
            }
            else if (hpWidth > 62)
            {
                hpColor = Color.Yellow;
            }
            else if (hpWidth > 0)
            {
                hpColor = Color.Red;
            }
            
            for (int i=0; i<data.Length; i++)
            {
                data[i] = hpColor;
            }
            hpBar.SetData(data);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(hpBar, position, null, Color.White, 0f, Vector2.Zero, Vector2.Zero, 0, layerDepth);
            spriteBatch.Draw(hpBar, position, Color.White);
        }

        public int GetHeight()
        {
            return HPBAR_HEIGHT;
        }
        public void Restart()
        {
            throw new NotImplementedException();
        }
    }
}
