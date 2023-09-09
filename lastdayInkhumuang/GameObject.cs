using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lastdayInkhumuang
{
    public abstract class GameObject
    {
        Game1 game;
        public const int TILE_SIZE = Game1.TILE_SIZE;
        protected bool enable;
        protected Vector2 position;
        protected Vector2 tileLoaction;
        protected int boundHeight;
        protected int boundWidth;
        protected int drawHeight;
        protected int drawWidth;
        protected string objType;
        protected float layerDepth;


        public GameObject(Game1 game, Vector2 position, int width, int height, float layerDepth)
        {
            this.game = game;
            this.position = position;
            this.boundHeight = height;
            this.boundWidth = width;
            this.drawHeight = height;
            this.drawWidth = width;
            this.objType = "GameObject";
            this.layerDepth = layerDepth;
        }

        public GameObject(Game1 game, Vector2 position, Vector2 tileLocation, int width, int height, float layerDepth)
        {
            this.game = game;
            this.position = position;
            this.tileLoaction = tileLocation;
            this.boundHeight = height;
            this.boundWidth = width;
            this.drawHeight = height;
            this.drawWidth = width;
            this.objType = "GameObject";
            this.layerDepth = layerDepth;
        }

        public GameObject(Game1 game, Vector2 position, Vector2 tileLocation, int boundHeight, int boundWidth, int DrawWidth, int drawHeight, float layerDepth)
        {
            this.game = game;
            this.position = position;
            this.tileLoaction = tileLocation;
            this.boundHeight = boundHeight;
            this.boundWidth = boundWidth;
            this.drawHeight = drawHeight;
            this.drawWidth = DrawWidth;
            this.objType = "GameObject";
            this.layerDepth = layerDepth;
        }

        public virtual Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, boundWidth, boundHeight); }
        }

        public bool IsEnable
        {
            get { return true; }
        }

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
