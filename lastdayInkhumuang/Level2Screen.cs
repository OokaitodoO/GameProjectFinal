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
    public class Level2Screen : Screen
    {
        Texture2D level2;


        public Level2Screen(ContentManager content, EventHandler theScreenEvent) : base(theScreenEvent)
        {
            level2 = content.Load<Texture2D>("Scenes/draft-map");
            
        }
        public override void Update(GameTime gameTime, float elapsed, Player player)
        {
            Game1.LOCK_CAM = false;
            if (Keyboard.GetState().IsKeyDown(Keys.F6))
            {
                ScreenEvent.Invoke(this, new EventArgs());
                return;
            }            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(level2, Vector2.Zero, Color.White);
            base.Draw(spriteBatch);
        }
    }
}
