using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lastdayInkhumuang
{
    public class AnimatedObject : GameObject
    {
        protected AnimatedTexture spriteTexture;

        private const float Rotation = 0;
        private const float Scale = 1;
        protected int frames;
        protected int framesPerSecond;
        protected int framesRow;
        protected string direction;

        public AnimatedObject(Game1 game, Vector2 position, Vector2 tileLocation, int width, int height, int frames, int framesPerSec, int framesRow, float layerDepth) : base(game, position, tileLocation, width, height, layerDepth)
        {
            this.frames = frames;
            this.framesPerSecond = framesPerSec;
            this.framesRow = framesRow;
            spriteTexture = new AnimatedTexture(Vector2.Zero, Rotation, Scale, layerDepth);
        }

        public AnimatedObject(Game1 game, Vector2 position, Vector2 tileLocation, int boundHeight, int boundWidth, int DrawWidth, int drawHeight, int frames, int framesPerSec, int framesRow, float layerDepth) : base(game, position, tileLocation, boundHeight, boundWidth, DrawWidth, drawHeight, layerDepth)
        {
            this.frames = frames;
            this.framesPerSecond = framesPerSec;
            this.framesRow = framesRow;
            spriteTexture = new AnimatedTexture(Vector2.Zero, Rotation, Scale, layerDepth);
        }
        public AnimatedObject(Game1 game, Vector2 position, Vector2 origin, Vector2 tileLocation, int boundHeight, int boundWidth, int DrawWidth, int drawHeight, int frames, int framesPerSec, int framesRow, float layerDepth) : base(game, position, tileLocation, boundHeight, boundWidth, DrawWidth, drawHeight, layerDepth)
        {
            this.frames = frames;
            this.framesPerSecond = framesPerSec;
            this.framesRow = framesRow;
            spriteTexture = new AnimatedTexture(origin, Rotation, Scale, layerDepth);
        }

        public virtual void UpdateFrame(float elapsed)
        {
            spriteTexture.UpdateFrame(elapsed);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteTexture.DrawFrame(spriteBatch, position);
        }
        public virtual void ResetFrame()
        {
            spriteTexture.Reset();
        }
    }
}
