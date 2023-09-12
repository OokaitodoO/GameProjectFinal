using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lastdayInkhumuang
{
    internal class Level1Screen : Screen
    {
        Texture2D Level1;
        public Level1Screen(ContentManager theContent,EventHandler theScreenEvent) : base(theScreenEvent)
        {
            Level1 = theContent.Load<Texture2D>("Scenes/1st_Level");
        }
        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyUp(Keys.F5))
            {
                ScreenEvent.Invoke(this, new EventArgs());
            }
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Level1, Vector2.Zero, Color.White); base.Draw(spriteBatch);
        }
    }
}
