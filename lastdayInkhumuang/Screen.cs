using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lastdayInkhumuang
{
    public class Screen
    {
        protected EventHandler ScreenEvent;
        public Screen(EventHandler theScreenEvent)
        {
            ScreenEvent = theScreenEvent;
        }
        public virtual void Update(GameTime gameTime, float elapsed, Player player)
        {

        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
