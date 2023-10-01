using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lastdayInkhumuang
{
    public class Player : AnimatedObject, IGameFunction
    {
        PlayerDash dashEffect;
        public Game game;
        int SpriteRow;
        Vector2 startPos;
        Vector2 lastPos;
        protected Vector2 origin;
        int SPEED = 8;
        public bool attacked;
        float timerAtk;
        public int comboCount;
        public bool chainCombo;
        protected bool skilled;
        public bool dash;
        const int WIDTH = 128;
        const int HEIGHT = 128;
        const int RANGE_DASH = 128;
        public float dashCd;

        const int MOVE_ATK_FORWARD = 64;

        float hp;
        float stamina;
        float mana;
        protected bool dealDamage;

        bool flip;
        bool boundMap;
        bool move;

        bool alive;

        public Player(Game1 game, Vector2 position, float hp, float stamina, float mana, int frames, int framesPerSec, int framesRow, float layerDepth) : base(game, position, Vector2.Zero, 128, 128, TILE_SIZE, TILE_SIZE, frames, framesPerSec, framesRow, layerDepth)
        {
            this.game = game;
            dashEffect = new PlayerDash(game, Vector2.Zero, 5, 16, 1, 0f);
            spriteTexture.Load(game.Content, "Player/player_all_set_", frames, framesRow, framesPerSec);
            SpriteRow = 13;
            this.hp = hp;
            this.stamina = stamina;
            this.mana = mana;
            startPos = position;
            direction = "Right";
            attacked = false;
            skilled = false;
            dash = false;
            alive = true;
        }
        public Player(Game1 game, Vector2 position, Vector2 origin, int frames, int framesPerSec, int framesRow, float layerDepth) : base(game, position, origin, Vector2.Zero, 128, 128, TILE_SIZE, TILE_SIZE, frames, framesPerSec, framesRow, layerDepth)
        {
            
        }
        
        public override Rectangle Bounds => new Rectangle((int)position.X + 50, (int)position.Y + 64, 28, 64); //player collision position(not yet conplete)
        public Vector2 Position => position;
        
        public void Update(Game1 game,KeyboardState ks, KeyboardState oldKs, MouseState ms, PlayerSkills skill, float elapsed)
        {
            //Console.WriteLine("Attack: " + attacked);
            //Console.WriteLine("Stamina: " + stamina);

            lastPos = position;
            CheckDirection(ms);
            //Attack (not yet complete)
            ComboAttack(ms, elapsed);           
            //Skill
            if (ks.IsKeyDown(Keys.E) && !skilled && !skill.GetSkilled())
            {
                spriteTexture.Reset();
                skilled = true;
                if (direction == "Right")
                {
                    SpriteRow = 7;
                    flip = false;
                }
                if (direction == "Left")
                {
                    SpriteRow = 7;
                    flip = true;
                }
                if (direction == "Up")
                {
                    SpriteRow = 1;
                }
                if (direction == "Down")
                {
                    SpriteRow = 4;
                }
            }
            if (skilled)
            {
                spriteTexture.UpdateFrame(elapsed);
                if (spriteTexture.GetFrame() == 4)
                {
                    skilled = false;
                }
            }

            //Right
            if (Bounds.X - game.GetCameraPosX() >= 600 && game.GetCameraPosX() < Game1.MAP_WIDTH - (Game1.MAP_WIDTH / 3)) 
            {
                game.UpdateCamera(new Vector2(SPEED, 0f));
            }
            //Left
            if (Bounds.X - game.GetCameraPosX() <= 450 && game.GetCameraPosX() > 0)
            {
                game.UpdateCamera(new Vector2(-SPEED, 0f));
            }
            //Up
            if (Bounds.Y - game.GetCameraPosY() <= 250 && game.GetCameraPosY() > 0)
            {
                game.UpdateCamera(new Vector2(0f, -SPEED));
            }
            //Down
            if (Bounds.Y - game.GetCameraPosY() >= 450 && game.GetCameraPosY() < Game1.MAP_HEIGHT - (Game1.MAP_HEIGHT / 3))
            {
                game.UpdateCamera(new Vector2(0f, SPEED));
            }


            //Movement
            /*if (!attacked && !skilled)
            {
                if (ks.IsKeyDown(Keys.A) && Bounds.X > 0)//Left
                {
                    move = true;
                    position.X -= 8;
                    SpriteRow = 12;
                    flip = true;
                    UpdateFrame(elapsed);
                }
                else if (ks.IsKeyDown(Keys.D) && Bounds.X + 64 < Game1.MAP_WIDTH && ks.IsKeyDown(Keys.D) && Bounds.X + 64 < game.GraphicsDevice.Viewport.Width + Game1._cameraPosition.X)
                {
                    move = true;
                    position.X += 8;
                    SpriteRow = 12;
                    flip = false;
                    UpdateFrame(elapsed);
                }
                if (ks.IsKeyDown(Keys.W) && Bounds.Y > 0)
                {
                    move = true;
                    position.Y -= 8;
                    SpriteRow = 10;
                    flip = false;
                    UpdateFrame(elapsed);
                }
                else if (ks.IsKeyDown(Keys.S) && Bounds.Y + 64 < Game1.MAP_HEIGHT && ks.IsKeyDown(Keys.S) && Bounds.Y + 64 < game.GraphicsDevice.Viewport.Height + Game1._cameraPosition.Y)
                {
                    move = true;
                    position.Y += 8;
                    SpriteRow = 11;
                    flip = false;
                    UpdateFrame(elapsed);
                }
            }*/
            if (!attacked && !skilled)
            {
                if (ks.IsKeyDown(Keys.A))//Left
                {
                    move = true;
                    position.X -= 8;
                    SpriteRow = 12;
                    flip = true;
                    UpdateFrame(elapsed);
                }
                else if (ks.IsKeyDown(Keys.D))
                {
                    move = true;
                    position.X += 8;
                    SpriteRow = 12;
                    flip = false;
                    UpdateFrame(elapsed);
                }
                if (ks.IsKeyDown(Keys.W))
                {
                    move = true;
                    position.Y -= 8;
                    SpriteRow = 10;
                    flip = false;
                    UpdateFrame(elapsed);
                }
                else if (ks.IsKeyDown(Keys.S))
                {
                    move = true;
                    position.Y += 8;
                    SpriteRow = 11;
                    flip = false;
                    UpdateFrame(elapsed);
                }
                if (ks.IsKeyUp(Keys.W) && ks.IsKeyUp(Keys.A) && ks.IsKeyUp(Keys.S) && ks.IsKeyUp(Keys.D))
                {
                    move = false;
                    if (direction == "Left")
                    {
                        SpriteRow = 13;
                        flip = true;
                        UpdateFrame(elapsed);
                    }
                    else if (direction == "Right")
                    {
                        SpriteRow = 13;
                        flip = false;
                        UpdateFrame(elapsed);
                    }
                    if ((SpriteRow == 12 && flip) || (SpriteRow == 13 && flip) && (direction == "Up" || direction == "Down")) //Left
                    {
                        SpriteRow = 13;
                        flip = true;
                        UpdateFrame(elapsed);
                    }
                    else if ((SpriteRow == 12 && !flip) || (SpriteRow == 13 && !flip) && (direction == "Up" || direction == "Down")) //Right
                    {
                        SpriteRow = 13;
                        flip = false;
                        UpdateFrame(elapsed);
                    }
                    if (SpriteRow == 10 || SpriteRow == 11)
                    {
                        SpriteRow = 13;
                        UpdateFrame(elapsed);
                    }
                    if ((SpriteRow == 1) || (SpriteRow == 2) || SpriteRow == 3)
                    {
                        SpriteRow = 13;
                        UpdateFrame(elapsed);
                    }
                    else if ((SpriteRow == 4) || (SpriteRow == 5) || SpriteRow == 6)
                    {
                        SpriteRow = 13;
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
            dashEffect.Update(elapsed, this);

            //Test Hp            
            //if (ks.IsKeyDown(Keys.Down))
            //{
            //    hp -= 5;
            //    stamina -= 5;
            //}
            //if (ks.IsKeyDown(Keys.Up))
            //{
            //    hp += 5;
            //    stamina+= 5;
            //}

            if (hp >= 100)
            {
                hp = 100;
                alive = true;
            }
            else if (hp <= 0)
            {
                hp = 0;
                alive = false;
                Game1.GAME_STATE = 2;
            }
            //Regen stamina
            if (stamina <= 0)
            {
                stamina = 1;
            }
            else if (stamina < 100)
            {
                stamina += elapsed * 4;
            }
        }

        public void ComboAttack(MouseState ms, float elapsed)
        {
            if (!chainCombo)
            {
                timerAtk += elapsed;
                if (timerAtk > 2)
                {
                    comboCount = 0;
                }
            }
            else
            {
                timerAtk = 0;
            }
            if (stamina > 6)
            {
                if (ms.LeftButton == ButtonState.Pressed && !chainCombo)
                {
                    chainCombo = true;
                    spriteTexture.Reset();
                    attacked = true;
                    stamina -= 5;
                    if (direction == "Right" /*&& timerAtk < 2*/)
                    {
                        switch (comboCount)
                        {
                            case 0:
                                SpriteRow = 7;
                                flip = false;
                                comboCount++;
                                break;
                            case 1:
                                SpriteRow = 8;
                                flip = false;
                                comboCount++;
                                break;
                            case 2:
                                SpriteRow = 9;
                                flip = false;
                                comboCount = 0;
                                break;
                            default:
                                comboCount = 0;
                                break;
                        }
                        position.X += MOVE_ATK_FORWARD;
                    }
                    if (direction == "Left")
                    {
                        switch (comboCount)
                        {
                            case 0:
                                SpriteRow = 7;
                                flip = true;
                                comboCount++;
                                break;
                            case 1:
                                SpriteRow = 8;
                                flip = true;
                                comboCount++;
                                break;
                            case 2:
                                SpriteRow = 9;
                                flip = true;
                                comboCount = 0;
                                break;
                            default:
                                comboCount = 0;
                                break;
                        }
                        position.X -= MOVE_ATK_FORWARD;
                    }
                    if (direction == "Up")
                    {
                        switch (comboCount)
                        {
                            case 0:
                                SpriteRow = 1;
                                comboCount++;
                                break;
                            case 1:
                                SpriteRow = 2;
                                comboCount++;
                                break;
                            case 2:
                                SpriteRow = 3;
                                comboCount = 0;
                                break;
                            default:
                                comboCount = 0;
                                break;
                        }
                        position.Y -= MOVE_ATK_FORWARD;
                    }
                    if (direction == "Down")
                    {
                        switch (comboCount)
                        {
                            case 0:
                                SpriteRow = 4;
                                comboCount++;
                                break;
                            case 1:
                                SpriteRow = 5;
                                comboCount++;
                                break;
                            case 2:
                                SpriteRow = 6;
                                comboCount = 0;
                                break;
                            default:
                                comboCount = 0;
                                break;
                        }
                        position.Y += MOVE_ATK_FORWARD;
                    }
                    
                }
            }
            if (attacked)
            {
                spriteTexture.UpdateFrame(elapsed);
                if (spriteTexture.GetFrame() == 4)
                {
                    attacked = false;
                    chainCombo = false;
                }
            }
        }

        public void SetPos(Vector2 pos)
        {
            position = pos;
        }
        public void CheckColiision(GameObject other, bool dealDamage)
        {
            if (other.GetType().IsAssignableTo(typeof(Range_Enemy)))
            {
                if (((Range_Enemy)other).Rasengan().Intersects(this.Bounds) && dealDamage)
                {
                    hp -= 5;
                }
            }
            if (other.Bounds.Intersects(this.Bounds))
            {
                if (other.GetType().IsAssignableTo(typeof(Melee_Enemy)) && dealDamage)
                {
                    hp -= 5;
                }                
                if (other.GetType().IsAssignableTo(typeof(MiniBoss1)) && dealDamage)
                {
                    if (((MiniBoss1)other).GetSpear())
                    {
                        hp -= 4;
                    }
                    else
                    {
                        hp -= 15;
                    }
                }
            }
        }
        public void CheckMapColiision(BoundsCheck other)
        {
            if (other.Bounds.Intersects(this.Bounds))
            {
                position = lastPos;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!attacked)
            {
                spriteTexture.DrawFrame(spriteBatch, position, SpriteRow, flip);
            }
            else if (attacked)
            {
                if (spriteTexture.GetFrame() <= 8)
                {
                    spriteTexture.DrawFrame(spriteBatch, position, SpriteRow, flip);                   
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
                    spriteTexture.DrawFrame(spriteBatch, position, SpriteRow, flip);
                }
                else
                {
                    skilled = false;
                }
            }
            dashEffect.Draw(spriteBatch, this);
        }

        public void Restart()
        {
            PlayerSkills.Restart();
            PlayerDash.SetIsDash(false);
            hp = 100;
            stamina = 100;
            position = startPos;
            attacked = false;
            chainCombo = false;
            skilled = false;
            dash = false;
            spriteTexture.Reset();
        }

        float dx;
        float dy;
        public void CheckDirection(MouseState ms)
        {           
            dx = (ms.Position.X + Game1._cameraPosition.X) - (position.X + WIDTH/2);
            dy = (ms.Position.Y + Game1._cameraPosition.Y) - (position.Y + HEIGHT/2);
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
            dx = (ms.Position.X + Game1._cameraPosition.X) - (position.X + WIDTH / 2);
            dy = (ms.Position.Y + Game1._cameraPosition.Y) - (position.Y + HEIGHT / 2);

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
        public Vector2 GetMiddle()
        {
            return new Vector2(position.X/2, position.Y/2);
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
        public bool GetAlive()
        {
            return alive;
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
