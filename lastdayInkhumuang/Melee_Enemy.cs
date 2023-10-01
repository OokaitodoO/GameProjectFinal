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
    internal class Melee_Enemy : Enemy, IGameFunction
    {
        Vector2 lastPos;
        const int KNOCKBACK = 100;
        const float REGEN_HP = 0.2f;
        //const int SIZE_HEIGHT = 180;
        //const int SIZE_WIDTH = 150;
        public Melee_Enemy(Game1 game, Vector2 position, Vector2 tileLocation, int boundHeight, int boundWidth, int frames, int framesPerSec, int framesRow, float layerDepth) : base(game, position, tileLocation, boundHeight, boundWidth, TILE_SIZE, TILE_SIZE, frames, framesPerSec, framesRow, layerDepth)
        {
            spriteTexture.Load(game.Content, "Enemy/Monster_Sword_all", frames, framesRow, framesPerSec);
            this.boundHeight= boundHeight;
            this.boundWidth= boundWidth;
            hp = 100;
            speed = 2;
            speedToOriginPos = 2;
            originPos = position;
            this.position = originPos - new Vector2(0 , boundHeight/2);
            attack = false;
            outSide = true;
            flip = false;
            alive = true;

            rangePos = new Vector2(originPos.X - (RANGE_WIDTH / 2) + this.boundWidth / 2, originPos.Y - (RANGE_HEIGHT / 2) + this.boundHeight / 2);
            //rangePos = originPos;
        }

        public override Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y+90, 150, 90); //player collision position(not yet conplete)

        public Vector2 Position => position;

        public void Update(Player player, float elapsed)
        {

            //Check Alive
            if (alive)
            {
                spriteTexture.Play();

            }
            if (hp <= 0 && alive)
            {                
                spriteTexture.Reset();
                alive = false;
                Game1.monsterCount--;
                Hitted = false;
            }
            if (!alive)
            {                
                spriteRow = 4;
                if (spriteTexture.GetFrame() == 3)
                {
                    spriteTexture.Pause(4, 4);
                }
            }

            if (alive)
            {
                lastPos = position;
                CheckDirection(player);
                //Check player
                if (player.Bounds.X + 28 > rangePos.X && player.Bounds.X < rangePos.X + RANGE_WIDTH + boundWidth/2 && player.Bounds.Y > rangePos.Y && player.Bounds.Y < rangePos.Y + RANGE_HEIGHT - boundHeight)
                {
                    outSide = false;
                }
                //CheckRange
                if (position.X > rangePos.X + RANGE_WIDTH - boundWidth) //Right
                {
                    position.X -= speed;
                    outSide = true;
                }
                else if (position.X < rangePos.X) //Left
                {
                    position.X += speed;
                    outSide = true;
                }
                if (position.Y > rangePos.Y + RANGE_HEIGHT - boundHeight) //Down
                {
                    position.Y -= speed;
                    outSide = true;
                }
                else if (position.Y + 55 < rangePos.Y) //Up
                {
                    position.Y += speed;
                    outSide = true;
                }

                //Hitted (KnockBack)               
                if (Hitted && delayHitted == 0)
                {                    
                    spriteRow = 5;
                    Hitted = true;
                    spriteTexture.Reset();
                    hp -= damage;
                    if (direction == "Left")
                    {
                        position.X += KNOCKBACK;
                    }
                    if (direction == "Right")
                    {
                        position.X -= KNOCKBACK;
                    }
                    if (direction == "Down")
                    {
                        position.Y -= KNOCKBACK;
                    }
                    if (direction == "Up")
                    {
                        position.Y += KNOCKBACK;
                    }
                    //if (player.GetPos().X + 64 < position.X + 90)
                    //{
                    //    position.X += KNOCKBACK;
                    //}
                    //if (player.GetPos().X + 64 > position.X + 90)
                    //{
                    //    position.X -= KNOCKBACK;
                    //}
                    //else if (player.GetPos().Y + 64 > position.Y + 120)
                    //{
                    //    position.Y -= KNOCKBACK;
                    //}
                    //else if (player.GetPos().Y + 64 < position.Y + 120)
                    //{
                    //    position.Y += KNOCKBACK;
                    //}
                }
               

                //Follow player
                if (!attack && !Hitted)
                {
                    if (!outSide)
                    {
                        if (position.X < player.GetPos().X) //Right
                        {
                            position.X += speed;
                            spriteRow = 1;
                            flip = false;
                        }
                        if (position.X > player.GetPos().X) //Left
                        {
                            position.X -= speed;
                            spriteRow = 1;
                            flip = true;
                        }
                        if (position.Y + 55 < player.GetPos().Y) //Down
                        {
                            position.Y += speed;
                            spriteRow = 1;
                        }
                        if (position.Y + 55 > player.GetPos().Y) //Up
                        {
                            position.Y -= speed;
                            spriteRow = 1;
                        }
                    }
                }

                //Back to OriginPos
                if (outSide)
                {
                    speed = 0;
                    speedToOriginPos = 2;
                    if (position.X < originPos.X) //right
                    {
                        position.X += speedToOriginPos;
                        flip = false;
                    }
                    if (position.X > originPos.X) // left
                    {
                        position.X -= speedToOriginPos;
                        flip = true;
                    }
                    if (position.Y + boundHeight / 2< originPos.Y) //down
                    {
                        position.Y += speedToOriginPos;
                    }
                    if (position.Y + boundHeight / 2 > originPos.Y) //up
                    {
                        position.Y -= speedToOriginPos;
                    }
                    if (position + new Vector2(0 , boundHeight / 2) == originPos)
                    {
                        spriteRow = 2;
                        if (hp < 100)
                        {
                            hp += REGEN_HP;
                        }
                    }
                }
                if (!outSide)
                {
                    speed = 2;
                    speedToOriginPos = 0;
                }

                //hitcooldown
                if (Hitted)
                {
                    attack = false;
                    delayHitted += elapsed;
                    if (delayHitted >= 0.3)
                    {
                        Hitted = false;
                    }
                }
                else
                {
                    delayHitted = 0;
                }                               


                //attack
                if (attack && spriteTexture.GetFrame() == 4)
                {
                    dealDamage = true;
                    attack = false;
                }
                else
                {
                    dealDamage = false;
                }
            }                        
        }

        float dx;
        float dy;
        public void CheckDirection(Player player)
        {
            dx = (player.Position.X) - (position.X + Bounds.Width / 2);
            dy = (player.Position.Y) - (position.Y + Bounds.Height / 2);
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

        public override void CheckColiision(GameObject player)
        {           
            if (player.Bounds.Intersects(this.Bounds))
            {
                if (player.GetType().IsAssignableTo(typeof(Player)) && !attack && alive)
                {                    
                        spriteRow = 3;
                        spriteTexture.Reset();
                        attack = true;                                        
                }                    
            }
        }
        float boundTimer;
        public void CheckMapColiision(BoundsCheck other, float elapsed)
        {
            if (other.Bounds.Intersects(this.Bounds))
            {
                position = lastPos;
                if (!outSide)
                {
                    boundTimer += elapsed;
                    if (boundTimer >= 2)
                    {
                        speed = 0;
                        speedToOriginPos = 2;
                        outSide = true;
                    }
                }
                else
                {
                    boundTimer = 0;
                }
            }
        }

        public override void GotDamage(int damage)
        {
            if (alive)
            {
                Hitted = true;
                this.damage = damage;
            }            
        }
        public bool GetAlive()
        {
            return alive;
        }
        public override void UpdateFrame(float elapsed)
        {
            if (!Hitted && alive)
            {
                spriteTexture.UpdateFrame(elapsed);
            }
            if (!alive && !Hitted)
            {                
                spriteTexture.UpdateFrame(elapsed);
            }            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {            
            spriteTexture.DrawFrame(spriteBatch, position, spriteRow, flip);                   
        }
         

        public void Restart()
        {
            alive = true;
            hp = 100;
            position = originPos - new Vector2(0, boundHeight / 2);
            attack = false;
            outSide = true;
            flip = false;
        }

        public override bool DealDamage()
        {
            return base.DealDamage();
        }
    }
}
