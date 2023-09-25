using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lastdayInkhumuang
{
    internal class MapCollision : GameObject
    {
        public MapCollision(Game1 game, Vector2 position, Vector2 tileLocation, int boundHeight, int boundWidth, int DrawWidth, int drawHeight, float layerDepth) : base(game, position, tileLocation, boundHeight, boundWidth, DrawWidth, drawHeight, layerDepth)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
