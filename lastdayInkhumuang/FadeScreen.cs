using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace lastdayInkhumuang
{
    public class FadeScreen
    {
        static Texture2D fade;
        static float timer;
        static bool done;
        public FadeScreen()
        {
            timer = 0;
            done = false;
        }
        public static void Fade(string asset, Game1 game, SpriteBatch spriteBatch, float elapsed)
        {
            fade = game.Content.Load<Texture2D>(asset);
            CalFadeTiming(elapsed);
            spriteBatch.Draw(fade, Game1._cameraPosition, Color.Black * timer);
        }

        public static void CalFadeTiming(float elapsed)
        {
            if (timer <= 0 && !done)
            {
                timer += elapsed / 2;
            }
            else
            {

            }
        }
    }
}
