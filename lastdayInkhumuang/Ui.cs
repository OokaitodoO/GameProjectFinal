using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lastdayInkhumuang
{
    public abstract class Ui
    {
        Game1 game;
        protected Vector2 position;
        protected int drawHeight;
        protected int drawWidth;
        protected string objType;
        protected float layerDepth;

        public Ui(Game1 game, Vector2 position, int width, int height, float layerDepth)
        {
            this.game = game;
            this.position = position;
            this.drawWidth = width;
            this.drawHeight = height;
            this.layerDepth = layerDepth;
            objType = "Ui";
        }

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
