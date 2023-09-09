using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lastdayInkhumuang
{
    public class Player : AnimatedObject, IGameFunction
    {
        int SpriteRow;
        Vector2 startPos;
        Vector2 lastPos;
        int speed;
        public bool attacked;
        protected bool skilled;
        bool dash;
        const int WIDTH = 128;
        const int HEIGHT = 128;
        const int RANGE_DASH = 192;
        float dashCd;

        float hp;
        float stamina;
        float mana;
        protected bool dealDamage;


        public Player(Game1 game, Vector2 position, float hp, float stamina, float mana, int frames, int framesPerSec, int framesRow, float layerDepth) : base(game, position, Vector2.Zero, 128, 128, TILE_SIZE, TILE_SIZE, frames, framesPerSec, framesRow, layerDepth)
        {
            spriteTexture.Load(game.Content, "Player/Player_all_set", frames, framesRow, framesPerSec);
            SpriteRow = 7;
            speed = 8;
            this.hp = hp;
            this.stamina = stamina;
            this.mana = mana;
            startPos = position;
            direction = "Right";
            attacked = false;
            skilled = false;
            dash = false;
        }
        public override Rectangle Bounds => new Rectangle((int)position.X + 50, (int)position.Y + 64, 28, 64); //player collision position(not yet conplete)
                                                                                                               //
        public Vector2 Position => position;

        public void Update(KeyboardState ks, KeyboardState oldKs, MouseState ms, PlayerSkills skill, float elapsed)
        {
            lastPos = position;            
            CheckDirection(ms);

            

            //Attack (not yet complete)
            if (stamina > 6)
            {
                if (ms.LeftButton == ButtonState.Pressed && !attacked)
                {
                    spriteTexture.Reset();
                    attacked = true;
                    stamina -= 5;
                    if (direction == "Right")
                    {
                        SpriteRow = 3;
                    }
                    if (direction == "Left")
                    {
                        SpriteRow = 4;
                    }
                    //if (direction == "Up")
                    //{

                    //}
                    //if (direction == "Down")
                    //{

                    //}
                }                
            }
            if (attacked)
            {
                spriteTexture.UpdateFrame(elapsed);
            }

            //Skill
            if (ks.IsKeyDown(Keys.E) && !skilled && !skill.GetSkilled())
            {
                spriteTexture.Reset();
                skilled = true;                
                if (direction == "Right")
                {
                    SpriteRow = 3;
                }
                if (direction == "Left")
                {
                    SpriteRow = 4;
                }
            }
            if (skilled)
            {
                spriteTexture.UpdateFrame(elapsed);
            }

            //Movement
            if (!attacked && !skilled)
            {
                if (ks.IsKeyDown(Keys.A))
                {
                    position.X -= speed;
                    SpriteRow = 2;
                    UpdateFrame(elapsed);
                    //direction = "Left";
                }
                else if (ks.IsKeyDown(Keys.D))
                {
                    position.X += speed;
                    SpriteRow = 1;
                    UpdateFrame(elapsed);
                    //direction = "Right";
                }
                if (ks.IsKeyDown(Keys.W))
                {
                    position.Y -= speed;
                    SpriteRow = 5;
                    UpdateFrame(elapsed);
                }
                else if (ks.IsKeyDown(Keys.S))
                {
                    position.Y += speed;
                    SpriteRow = 6;
                    UpdateFrame(elapsed);
                }
                if (ks.IsKeyUp(Keys.W) && ks.IsKeyUp(Keys.A) && ks.IsKeyUp(Keys.S) && ks.IsKeyUp(Keys.D))
                {
                    if (direction == "Left" )
                    {
                        SpriteRow = 8;
                        UpdateFrame(elapsed);
                    }
                    else if (direction == "Right")
                    {
                        SpriteRow = 7;
                        UpdateFrame(elapsed);
                    }
                    if (SpriteRow == 2 || SpriteRow == 8 && (direction == "Up" || direction == "Down"))
                    {
                        SpriteRow = 8;
                        UpdateFrame(elapsed);
                    }
                    else if (SpriteRow == 1 || SpriteRow == 7 && (direction == "Up" || direction == "Down"))
                    {
                        SpriteRow = 7;
                        UpdateFrame(elapsed);
                    }
                }
            }

            //Dash
            if (ks.IsKeyDown(Keys.LeftShift) && oldKs.IsKeyUp(Keys.LeftShift) && !dash)
            {
                dash = true;
                if (!attacked && !skilled && stamina > 20)
                {
                    Dash(ms);
                    stamina -= 20;
                }                
            }
            if (!dash)
            {
                dashCd = 0;
            }
            else
            {
                dashCd += elapsed;
                if (dashCd >= 2)
                {
                    dash = false;
                }
            }

            //Test Hp            
            if (ks.IsKeyDown(Keys.Down))
            {
                hp -= 5;
                stamina -= 5;
            }
            if (ks.IsKeyDown(Keys.Up))
            {
                hp += 5;
            }

            if (hp >= 100)
            {
                hp = 100;
            }
            else if (hp <= 0)
            {
                hp = 0;
            }
            //Regen stamina
            if (stamina <= 0)
            {
                stamina = 1;                
            }
            else if (stamina < 100)
            {
                stamina += elapsed * 2;
            }



        }

        public void CheckColiision(GameObject other, bool dealDamage)
        {
            if (other.Bounds.Intersects(this.Bounds))
            {
                if (other.GetType().IsAssignableTo(typeof(Melee_Enemy)) && dealDamage)
                {
                    hp -= 5;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!attacked)
            {
                spriteTexture.DrawFrame(spriteBatch, position, SpriteRow);
            }
            else if (attacked)
            {
                if (spriteTexture.GetFrame() <= 8)
                {
                    spriteTexture.DrawFrame(spriteBatch, position, SpriteRow);                   
                }  
                else
                {
                    attacked = false;                    
                }
                    
            }
            if (skilled)
            {
                if (spriteTexture.GetFrame() <= 8)
                {
                    spriteTexture.DrawFrame(spriteBatch, position, SpriteRow);
                }
                else
                {
                    skilled = false;
                }
            }
        }

        public void Restart()
        {
            position = startPos;
            SpriteRow = 7;
        }

        float dx;
        float dy;
        public void CheckDirection(MouseState ms)
        {
            dx = ms.Position.X - (position.X + WIDTH/2);
            dy = ms.Position.Y - (position.Y + HEIGHT/2);
            if (dx >= 0)
            {
                if (dy >= 0)
                {
                    if (Math.Abs(dx) >= Math.Abs(dy))
                    {
                        direction = "Right";
                    }
                    else
                    {
                        direction = "Down";
                    }
                }
                else
                {
                    if (Math.Abs(dx) >= Math.Abs(dy))
                    {
                        direction = "Right";
                    }
                    else
                    {
                        direction = "Up";
                    }
                }
            }
            else
            {
                if (dy >= 0)
                {
                    if (Math.Abs(dx) >= Math.Abs(dy))
                    {
                        direction = "Left";
                    }
                    else
                    {
                        direction = "Down";
                    }
                }
                else
                {
                    if (Math.Abs(dx) >= Math.Abs(dy))
                    {
                        direction = "Left";
                    }
                    else
                    {
                        direction = "Up";
                    }
                }
            }
        }

        //public void Dash(MouseState ms)
        //{
        //    dx = ms.Position.X - (position.X + WIDTH/2);
        //    dy = ms.Position.Y - (position.Y + HEIGHT/2);

        //    if (Math.Abs(dy)/Math.Abs(dx) == 1)
        //    {
        //        if (Math.Abs(dx) <= 192)
        //        {
        //            if(dx > 0) { position.X += Math.Abs(dx); }
        //            if (dx < 0) { position.X -= Math.Abs(dx); }
        //        }
        //        else
        //        {
        //            if (dx > 0)
        //            {
        //                position.X += 192;
        //            }
        //            if (dx < 0)
        //            {
        //                position.X -= 192; 
        //            }
        //        }
        //        if (Math.Abs(dy) <= 192)
        //        {
        //            if (dy > 0) { position.Y += Math.Abs(dx); }
        //            if (dy   < 0) { position.Y -= Math.Abs(dy); }
        //        }
        //        else
        //        {
        //            if (dy > 0)
        //            {
        //                position.Y += 192;
        //            }
        //            if (dy < 0)
        //            {
        //                position.Y -= 192;
        //            }
        //        }
        //    }

        //    if (Math.Abs(dy)/Math.Abs(dx) > 1)
        //    {                
        //        if (Math.Abs(dy) <= 192)
        //        {
        //            if (dy > 0) { position.Y += Math.Abs(dy); }
        //            if (dy < 0) { position.Y -= Math.Abs(dy); }
        //            if (Math.Abs(dx) <= 192)
        //            {
        //                if (dx > 0) { position.X += Math.Abs(dx); }
        //                if (dx < 0) { position.X -= Math.Abs(dx); }
        //            }
        //            else
        //            {
        //                if (dx > 0)
        //                {
        //                    position.X += (Math.Abs(dy)) * (Math.Abs(dx) / Math.Abs(dy));
        //                }
        //                if (dx < 0)
        //                {
        //                    position.X -= (Math.Abs(dy)) * (Math.Abs(dx) / Math.Abs(dy));
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (dy > 0) { position.Y += 192; }
        //            if (dy < 0) { position.Y -= 192; }
        //            if (Math.Abs(dx) <= 192)
        //            {
        //                if (dx > 0) { position.X += Math.Abs(dx); }
        //                if (dx < 0) { position.X -= Math.Abs(dx); }
        //            }
        //            else
        //            {
        //                if (dx > 0)
        //                {
        //                    position.X += 192 * (Math.Abs(dx) / Math.Abs(dy));
        //                }
        //                if (dx < 0)
        //                {
        //                    position.X -= 192 * (Math.Abs(dx) / Math.Abs(dy));
        //                }
        //            }
        //        }
        //    }

        //    if (Math.Abs(dy)/Math.Abs(dx) < 1)
        //    {
        //        if (Math.Abs(dx) <= 192)
        //        {
        //            if (dx > 0) { position.X += Math.Abs(dx); }
        //            if (dx < 0) { position.X -= Math.Abs(dx); }
        //            if (Math.Abs(dy) <= 192)
        //            {
        //                if (dy > 0) { position.Y += Math.Abs(dy); }
        //                if (dy < 0) { position.Y -= Math.Abs(dy); }
        //            }
        //            else
        //            {
        //                if (dy > 0)
        //                {
        //                    position.Y += (Math.Abs(dx)) * (Math.Abs(dy) / Math.Abs(dx));
        //                }
        //                if (dy < 0)
        //                {
        //                    position.Y -= (Math.Abs(dx)) * (Math.Abs(dy) / Math.Abs(dx));
        //                }
        //            }
        //        }
        //        else
        //        {

        //            if (dx > 0) { position.X += 192; }
        //            if (dx < 0) { position.X -= 192; }
        //            if (Math.Abs(dy) <= 192)
        //            {
        //                if (dy > 0) { position.Y += Math.Abs(dy); }
        //                if (dy < 0) { position.Y -= Math.Abs(dy); }
        //            }
        //            else
        //            {
        //                if (dy > 0)
        //                {
        //                    position.Y += 192 * (Math.Abs(dy) / Math.Abs(dx));
        //                }
        //                if (dy < 0)
        //                {
        //                    position.Y -= 192 * (Math.Abs(dy) / Math.Abs(dx));
        //                }
        //            }
        //        }
                
        //    }
        //}

        public void Dash(MouseState ms)
        {
            dx = ms.Position.X - (position.X + WIDTH / 2);
            dy = ms.Position.Y - (position.Y + HEIGHT / 2);

            if (Math.Abs(dy) / Math.Abs(dx) == 1)
            {
                if (Math.Abs(dx) <= RANGE_DASH)
                {
                    if (dx > 0) { position.X += Math.Abs(dx); }
                    if (dx < 0) { position.X -= Math.Abs(dx); }
                }
                else
                {
                    if (dx > 0)
                    {
                        position.X += RANGE_DASH;
                    }
                    if (dx < 0)
                    {
                        position.X -= RANGE_DASH;
                    }
                }
                if (Math.Abs(dy) <= RANGE_DASH)
                {
                    if (dy > 0) { position.Y += Math.Abs(dx); }
                    if (dy < 0) { position.Y -= Math.Abs(dy); }
                }
                else
                {
                    if (dy > 0)
                    {
                        position.Y += RANGE_DASH;
                    }
                    if (dy < 0)
                    {
                        position.Y -= RANGE_DASH;
                    }
                }
            }

            if (Math.Abs(dy) / Math.Abs(dx) > 1)
            {
                if (Math.Abs(dy) <= RANGE_DASH)
                {
                    if (dy > 0) { position.Y += Math.Abs(dy); }
                    if (dy < 0) { position.Y -= Math.Abs(dy); }
                    if (Math.Abs(dx) <= RANGE_DASH)
                    {
                        if (dx > 0) { position.X += Math.Abs(dx); }
                        if (dx < 0) { position.X -= Math.Abs(dx); }
                    }
                    else
                    {
                        if (dx > 0)
                        {
                            position.X += (Math.Abs(dy)) * (Math.Abs(dx) / Math.Abs(dy));
                        }
                        if (dx < 0)
                        {
                            position.X -= (Math.Abs(dy)) * (Math.Abs(dx) / Math.Abs(dy));
                        }
                    }
                }
                else
                {
                    if (dy > 0) { position.Y += RANGE_DASH; }
                    if (dy < 0) { position.Y -= RANGE_DASH; }
                    if (Math.Abs(dx) <= RANGE_DASH)
                    {
                        if (dx > 0) { position.X += Math.Abs(dx); }
                        if (dx < 0) { position.X -= Math.Abs(dx); }
                    }
                    else
                    {
                        if (dx > 0)
                        {
                            position.X += RANGE_DASH * (Math.Abs(dx) / Math.Abs(dy));
                        }
                        if (dx < 0)
                        {
                            position.X -= RANGE_DASH * (Math.Abs(dx) / Math.Abs(dy));
                        }
                    }
                }
            }

            if (Math.Abs(dy) / Math.Abs(dx) < 1)
            {
                if (Math.Abs(dx) <= RANGE_DASH)
                {
                    if (dx > 0) { position.X += Math.Abs(dx); }
                    if (dx < 0) { position.X -= Math.Abs(dx); }
                    if (Math.Abs(dy) <= RANGE_DASH)
                    {
                        if (dy > 0) { position.Y += Math.Abs(dy); }
                        if (dy < 0) { position.Y -= Math.Abs(dy); }
                    }
                    else
                    {
                        if (dy > 0)
                        {
                            position.Y += (Math.Abs(dx)) * (Math.Abs(dy) / Math.Abs(dx));
                        }
                        if (dy < 0)
                        {
                            position.Y -= (Math.Abs(dx)) * (Math.Abs(dy) / Math.Abs(dx));
                        }
                    }
                }
                else
                {

                    if (dx > 0) { position.X += RANGE_DASH; }
                    if (dx < 0) { position.X -= RANGE_DASH; }
                    if (Math.Abs(dy) <= RANGE_DASH)
                    {
                        if (dy > 0) { position.Y += Math.Abs(dy); }
                        if (dy < 0) { position.Y -= Math.Abs(dy); }
                    }
                    else
                    {
                        if (dy > 0)
                        {
                            position.Y += RANGE_DASH * (Math.Abs(dy) / Math.Abs(dx));
                        }
                        if (dy < 0)
                        {
                            position.Y -= RANGE_DASH * (Math.Abs(dy) / Math.Abs(dx));
                        }
                    }
                }

            }
        }
        public Vector2 GetPos()
        {
            return position;
        }
        public string GetDirection()
        {
            return direction;
        }
        public float GetHp()
        {
            return hp;
        }
        public float GetStamina()
        {
            return stamina;
        }
        public float GetMana()
        {
            return mana;
        }
        public bool GetAttack()
        {
            return attacked;
        }
        public bool Skill()
        {
            return skilled;
        }
        public virtual bool IsAttack() 
        {
            return dealDamage;
        }
    }
}
