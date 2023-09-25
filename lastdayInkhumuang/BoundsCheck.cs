using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lastdayInkhumuang
{
    public class BoundsCheck
    {
        Vector2 position;
        int boundWidth;
        int boundHeight;
        public BoundsCheck(Game1 game, Vector2 position, int boundHeight, int boundWidth)
        {
            this.position = position;
            this.boundWidth = boundWidth;
            this.boundHeight = boundHeight;
        }
        public Rectangle Bounds { get { return new Rectangle((int)position.X, (int)position.Y, boundWidth, boundHeight); } }
    }
}
