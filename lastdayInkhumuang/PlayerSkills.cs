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

        public PlayerSkills(Game1 game, Vector2 position, int frames, int framesPerSec, int framesRow, float layerDepth) : base(game, position, 0f, 0f, 0f, frames, framesPerSec, framesRow, layerDepth)
        {
            spriteTexture.Load(game.Content, "Player/set-Skill-E", frames, framesRow, framesPerSec);
            this.position = position;
            skilled = false;
            released = false;
        }

        public override Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y, 128, 128); //player collision position(not yet conplete)

        public Vector2 SkillPosition => position;

        public void Update(float elapsed, Player player, KeyboardState ks)
        {
            if (ks.IsKeyDown(Keys.E) && !skilled)
            {
                skilled = true;
            }
            if (skilled)
            {
                if (!released)
                {
                    spriteTexture.Play();
                }                
                spriteTexture.UpdateFrame(elapsed);
                if (spriteTexture.GetFrame() == 2)
                {
                    spriteTexture.Pause(3, spriteRow);
                    released = true;
                }

                if (direction == "Right")
                {                    
                    spriteRow = 1;
                    if (released)
                    {
                        position += new Vector2(SPEED, 0f);
                    }
                }
                else if (direction == "Left")
                {                    
                    spriteRow = 2;
                    if (released)
                    {
                        position -= new Vector2(SPEED, 0f);
                    }
                }
                else if (direction == "Up" && released)
                {                    
                    spriteRow = 3;
                    if(released) 
                    {
                        position -= new Vector2(0f, SPEED);
                    }
                }
                else if (direction == "Down" && released)
                {                    
                    spriteRow = 4;
                    if (released)
                    {
                        position += new Vector2(0f, SPEED);
                    }
                }
            }
            else if (!skilled && !released)
            {
                position = player.GetPos();
                direction = player.GetDirection();
                spriteTexture.Reset();
            }

            if (!released)
            {
                this.elapsed = 0;
            }
            else
            {
                this.elapsed += elapsed;
                if (this.elapsed >= 3)
                {
                    skilled = false;
                    released = false;
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
            }
        }

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
