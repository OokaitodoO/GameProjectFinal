using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lastdayInkhumuang
{
    public class Roof
    {
        Texture2D roof;
        Vector2 position;
        int boundWidth;
        int boundHeight;
        int drawX;
        int drawY;
        int drawWidth;
        int drawHeight;
        public Roof(Game1 game, Vector2 position, int drawX, int drawY, int boundHeight, int boundWidth, int drawHeight, int drawWidth)
        {
            roof = game.Content.Load<Texture2D>("Scenes/Map_TileSet_Object_RoofOnly");
            this.position = position;
            this.boundWidth = boundWidth;
            this.boundHeight = boundHeight;
            this.drawX = drawX;
            this.drawY = drawY;
            this.drawHeight = drawHeight;
            this.drawWidth = drawWidth;
        }
        public Rectangle Bounds { get { return new Rectangle((int)position.X, (int)position.Y + boundHeight/2, boundWidth, boundHeight); } }
        public Vector2 Position => position;
        public void Draw(SpriteBatch spriteBatch, float trans)
        {
            spriteBatch.Draw(roof, position, new Rectangle(drawX, drawY, drawWidth, drawHeight), Color.White * trans);
        }
    }
}
