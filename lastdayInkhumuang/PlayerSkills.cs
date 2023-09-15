using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lastdayInkhumuang
{
    public class PlayerSkills : Player
    {
        const int SPEED = 10;
        int spriteRow;
        bool released;
        float elapsed;
        Vector2 origin;

        public PlayerSkills(Game1 game, Vector2 position, Vector2 origin, int frames, int framesPerSec, int framesRow, float layerDepth) : base(game, position, origin, frames, framesPerSec, framesRow, layerDepth)
        {
            this.position = position;            
            spriteTexture.Load(game.Content, "Player/set-Skill-E", frames, framesRow, framesPerSec);            
            skilled = false;
            released = false;
            spriteRow = 1;
            this.origin = origin;
        }
        
        public override Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y - 64, 128, 128); //player collision position(not yet conplete)

        public Vector2 SkillPosition => position;

        public void Update(float elapsed, Player player, KeyboardState ks, MouseState ms)
        {            
            if (ks.IsKeyDown(Keys.E) && !skilled)
            {
                skilled = true;
            }
            //if (skilled)
            //{
            //    if (!released)
            //    {
            //        spriteTexture.Play();
            //    }
            //    spriteTexture.UpdateFrame(elapsed);
            //    if (spriteTexture.GetFrame() == 2)
            //    {
            //        spriteTexture.Pause(3, spriteRow);
            //        released = true;
            //    }

            //    if (direction == "Right")
            //    {
            //        spriteRow = 1;
            //        if (released)
            //        {
            //            position += new Vector2(SPEED, 0f);
            //        }
            //    }
            //    else if (direction == "Left")
            //    {
            //        spriteRow = 2;
            //        if (released)
            //        {
            //            position -= new Vector2(SPEED, 0f);
            //        }
            //    }
            //    else if (direction == "Up" && released)
            //    {
            //        spriteRow = 3;
            //        if (released)
            //        {
            //            position -= new Vector2(0f, SPEED);
            //        }
            //    }
            //    else if (direction == "Down" && released)
            //    {
            //        spriteRow = 4;
            //        if (released)
            //        {
            //            position += new Vector2(0f, SPEED);
            //        }
            //    }
            //}

            if (!released)
            {
                this.elapsed = 0;
            }
            else if (released)
            {
                this.elapsed += elapsed;
                if (this.elapsed >= 3)
                {
                    spriteTexture.Reset();
                    skilled = false;
                    released = false;
                }
            }            
            if (!skilled && !released)
            {
                CheckAngle(player, ms);
                position = player.GetPos() + new Vector2(64, 64);
                direction = player.GetDirection();
                spriteTexture.Reset();
               
            }
            else if (skilled)
            {
                if (!released)
                {
                    spriteTexture.Play();                   
                    if (spriteTexture.GetFrame() == 2)
                    {
                        spriteTexture.Pause(3, spriteRow);
                        released = true;
                    }
                    spriteTexture.UpdateFrame(elapsed);
                }                
                else if (released)
                {
                    position += new Vector2(SPEED * (float)Math.Cos(spriteTexture.Rotation), SPEED * (float)Math.Sin(spriteTexture.Rotation));
                }
            }                           
        }
        public void CheckColiision(GameObject other)
        {
            if (other.Bounds.Intersects(this.Bounds))
            {               
                if (other.GetType().IsAssignableTo(typeof(Melee_Enemy)) && released)
                {
                    ((Melee_Enemy)other).GotDamage(65);
                }
                if (other.GetType().IsAssignableTo(typeof(MiniBoss1)) && released)
                {
                    ((MiniBoss1)other).GotDamage(65);
                }
            }

        }

        float dx;
        float dy;
        float attackVectorLine;
        public void CheckAngle(Player player, MouseState ms)
        {
            //dx = ms.X - (player.GetPos().X + 64);
            //dy = ms.Y - (player.GetPos().Y + 64);
            dx =  (player.GetPos().X + 64) - (ms.X + Game1._cameraPosition.X);
            dy =  (player.GetPos().Y + 64) - (ms.Y + Game1._cameraPosition.Y);
            attackVectorLine = (float)Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
            if (dx > 0)
            {
                if (dy > 0)
                {
                    spriteTexture.Rotation = (float)((3 * Math.PI / 2) - Math.Acos(Math.Abs(dy) / attackVectorLine));
                }
                if (dy < 0)
                {
                    spriteTexture.Rotation = (float)((3*Math.PI/2) - Math.Acos(-1*Math.Abs(dy)/attackVectorLine));
                }
            }
            if (dx < 0)
            {
                if (dy > 0)
                {
                    spriteTexture.Rotation = (float)((Math.PI / 2) - Math.Acos(-1*Math.Abs(dy)/attackVectorLine));
                }
                if (dy < 0)
                {
                    spriteTexture.Rotation = (float)((Math.PI / 2) - Math.Acos(Math.Abs(dy) / attackVectorLine));                    
                }
            }
            if (dx == 0)
            {
                if (dy > 0)
                {
                    spriteTexture.Rotation = (float)((Math.PI * 2) - Math.Acos(-1 * Math.Abs(dx) / attackVectorLine));
                    
                }
                if (dy < 0)
                {
                    spriteTexture.Rotation = (float)((Math.PI * 2) - Math.Asin(-1*Math.Abs(dy)/attackVectorLine));
                }
            }
            if (dy == 0)
            {
                if (dx > 0)
                {
                    spriteTexture.Rotation = (float)((Math.PI ) - Math.Acos(Math.Abs(dx)/attackVectorLine));
                }
                if (dx < 0)
                {
                    spriteTexture.Rotation = (float)((Math.PI * 2) - Math.Asin(Math.Abs(dy) / attackVectorLine));
                }
            }
        }

        /*public void CheckAngle(Player player, MouseState ms)
        {
            dx = ms.X - (player.GetPos().X + 64);
            dy = ms.Y - (player.GetPos().Y + 64);
            attackVectorLine = (float)Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
            if (dy == 0)
            {
                if (dx >= 0) { spriteTexture.Rotation = 0; }
                if (dx < 0) { spriteTexture.Rotation = 180; }
            }
            if (dy > 0)
            {
                if (dx > 0)
                {
                    spriteTexture.Rotation = (float)Math.Acos((-1 * (Math.Abs(dx))) / attackVectorLine);
                }
                if (dx < 0)
                {
                    spriteTexture.Rotation = (float)Math.Acos(Math.Abs(dx) / attackVectorLine);
                }
                if (dx == 0)
                {
                    spriteTexture.Rotation = 90;
                }
                spriteTexture.Rotation += 180;
            }
            if (dy < 0)
            {
                if (dx > 0)
                {
                    spriteTexture.Rotation = (float)Math.Acos(Math.Abs(dx) / attackVectorLine);
                }
                if (dx < 0)
                {
                    spriteTexture.Rotation = (float)Math.Acos((-1*(Math.Abs(dx))) / attackVectorLine);
                }
                if (dx == 0)
                {
                    spriteTexture.Rotation = 90 ;
                }
            }
        }*/
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (skilled)
            {
                spriteTexture.DrawFrame(spriteBatch, position, spriteRow);
            }            
        }

        public bool GetSkilled()
        {
            return skilled;
        }
        public override bool IsAttack()
        {
            return base.IsAttack();
        }
    }
}
