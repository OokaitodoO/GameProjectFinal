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
        protected EventHandler ScreenEvent2;
        public Screen(EventHandler theScreenEvent)
        {
            ScreenEvent = theScreenEvent;
        }
        public Screen(EventHandler ScreenEvent, EventHandler ScreenEvent2)
        {
            this.ScreenEvent = ScreenEvent;
            this.ScreenEvent2 = ScreenEvent2;
        }
        public virtual void Update(GameTime gameTime, float elapsed, Player player)
        {

        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
