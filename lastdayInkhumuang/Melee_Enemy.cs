using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lastdayInkhumuang
{
    internal class Melee_Enemy : Enemy, IGameFunction
    {                                               

        const int KNOCKBACK = 100;
        const float REGEN_HP = 0.2f;
        //const int SIZE_HEIGHT = 180;
        //const int SIZE_WIDTH = 150;
        public Melee_Enemy(Game1 game, Vector2 position, Vector2 tileLocation, string element, int boundHeight, int boundWidth, int frames, int framesPerSec, int framesRow, float layerDepth) : base(game, position, tileLocation, boundHeight, boundWidth, TILE_SIZE, TILE_SIZE, frames, framesPerSec, framesRow, layerDepth)
        {
            spriteTexture.Load(game.Content, "Enemy/Monster_Sword_all", frames, framesRow, framesPerSec);
            this.boundHeight= boundHeight;
            this.boundWidth= boundWidth;
            hp = 100;
            speed = 2;
            speedToOriginPos = 2;
            this.element = element;
            originPos = position;
            this.position = originPos;
            attack = false;
            outSide = false;
            flip = false;
            enable = true;
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
                //Check player
                if (player.Bounds.X > rangePos.X && player.Bounds.X < rangePos.X + RANGE_WIDTH - boundWidth && player.Bounds.Y > rangePos.Y && player.Bounds.Y < rangePos.Y + RANGE_HEIGHT - boundHeight)
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

                //Hitted                
                if (Hitted && delayHitted == 0)
                {                    
                    spriteRow = 5;
                    Hitted = true;
                    spriteTexture.Reset();
                    hp -= damage;
                    if (player.GetPos().X + 64 < position.X + 90)
                    {
                        position.X += KNOCKBACK;
                    }
                    if (player.GetPos().X + 64 > position.X + 90)
                    {
                        position.X -= KNOCKBACK;
                    }
                    else if (player.GetPos().Y + 64 > position.Y + 120)
                    {
                        position.Y -= KNOCKBACK;
                    }
                    else if (player.GetPos().Y + 64 < position.Y + 120)
                    {
                        position.Y += KNOCKBACK;
                    }
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
                    if (position.Y < originPos.Y) //down
                    {
                        position.Y += speedToOriginPos;
                    }
                    if (position.Y > originPos.Y) //up
                    {
                        position.Y -= speedToOriginPos;
                    }
                    if (position == originPos)
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
                    if (delayHitted >= 1)
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
            
        }

        public override bool DealDamage()
        {
            return base.DealDamage();
        }
    }
}
