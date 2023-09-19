using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lastdayInkhumuang
{
    public class Enemy : AnimatedObject
    {
        protected Vector2 originPos;
        protected Vector2 rangePos;
        protected int speed;
        protected int speedToOriginPos;
        protected bool attack;
        protected bool outSide;
        protected bool dealDamage;
        protected float hp;
        protected bool alive;
        protected int spriteRow;
        protected bool Hitted;
        protected bool flip;
        protected int damage;
        protected float delayHitted;

        public const int RANGE_WIDTH = 600;
        public const int RANGE_HEIGHT = 600;


        public Enemy(Game1 game, Vector2 position, Vector2 tileLocation, int boundHeight, int boundWidth, int DrawWidth, int drawHeight, int frames, int framesPerSec, int framesRow, float layerDepth) : base(game, position, tileLocation, boundHeight, boundWidth, DrawWidth, drawHeight, frames, framesPerSec, framesRow, layerDepth)
        {
                      
        }

        public virtual void GotDamage(int damage) { }

        public virtual void CheckColiision(GameObject other) { }
        public virtual bool DealDamage() { return dealDamage; }
    }
}
