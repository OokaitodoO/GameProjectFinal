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
    public class PlayerDash : Player
    {
        public static bool IsDash;
        public PlayerDash(Game1 game, Vector2 origin, int frames, int framesPerSec, int framesRow, float layerDepth) : base(game, Vector2.Zero, origin, frames, framesPerSec, framesRow, layerDepth)
        {
            spriteTexture.Load(game.Content, "Player/Effect_Warp", frames, framesRow, framesPerSec);
        }
        public static void SetIsDash(bool isDash)
        {
            IsDash = isDash;
        }
        public void Update(float elapsed, Player player)
        {         
            if (!player.dash && player.dashCd == 0)
            {
                position = new Vector2(player.Bounds.X - 48, player.Bounds.Y - 64);
                spriteTexture.Reset();
            }
            else if (player.dash && player.dashCd != 0)
            {               
                if (spriteTexture.GetFrame() >= 3)
                {
                    IsDash = false;
                }
                else
                {                    
                    IsDash = true;
                    spriteTexture.UpdateFrame(elapsed);
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch, Player player)
        {
            if (spriteTexture.GetFrame() < 4 && IsDash)
            {
                spriteTexture.DrawFrame(spriteBatch, position);
            }                          
        }
    }
}
