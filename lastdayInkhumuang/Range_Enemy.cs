using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lastdayInkhumuang
{
    internal class Range_Enemy : Enemy, IGameFunction
    {

        const int KNOCKBACK = 100;
        const float REGEN_HP = 0.2f;
        Vector2 detectArea;
        Vector2 lastPos;
        //Vector2 attackRange;

        Vector2 rasenganPos;
        Texture2D rasengan;
        Rectangle rasenganRec;

        bool hitPlayer;
        bool detect;
        bool readyAttack;
        bool releaseBullet;
        bool rasenganVisible;
        float delayAttack;
        public Range_Enemy(Game1 game, Vector2 position, Vector2 tileLocation, int boundHeight, int boundWidth, int frames, int framesPerSec, int framesRow, float layerDepth) : base(game, position, Vector2.Zero, boundHeight, boundWidth, TILE_SIZE, TILE_SIZE, frames, framesPerSec, framesRow, layerDepth)
        {
            spriteTexture.Load(game.Content, "Enemy/Monster_Mage_Sprite", frames, framesRow, framesPerSec);
            rasengan = game.Content.Load<Texture2D>("ball_mage");            
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
            enable = true;
            alive = true;
            detect = false;
            releaseBullet = false;
            readyAttack = true;
            spriteRow = 1;

            rangePos = new Vector2(originPos.X - (RANGE_WIDTH / 2) + this.boundWidth / 2, originPos.Y - (RANGE_HEIGHT / 2) + this.boundHeight / 2);
        }

        public override Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y + 90, 150, 90); //player collision position(not yet conplete)

        public Vector2 Position => position;
        public void Update(Player player, float elapsed)
        {
            detectArea = new Vector2(Bounds.X + (boundWidth/2), Bounds.Y) - new Vector2((RANGE_WIDTH / 2) + 100, RANGE_HEIGHT / 2);
            //attackRange = new Vector2(detectArea.X + 100, detectArea.Y + 100);

            rasenganRec = new Rectangle((int)rasenganPos.X, (int)rasenganPos.Y, 24, 24);
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
                spriteRow = 3;
                if (spriteTexture.GetFrame() == 3)
                {
                    spriteTexture.Pause(4, 4);
                }
            }

            if (alive)
            {
                lastPos = position;
                CheckDirection(player);
                //Check detect
                if (!Hitted && readyAttack && !attack && player.Bounds.X + 28 > detectArea.X && player.Bounds.X < detectArea.X + RANGE_WIDTH + 200 && player.Bounds.Y + 64 > detectArea.Y && player.Bounds.Y < detectArea.Y + RANGE_HEIGHT)
                {
                    detect = true;
                    attack = true;
                    spriteRow = 2;
                }
                //Attack
                //if (attack)
                //{
                //    spriteRow = 3;
                //}
                //else if (attack)
                //{
                //    spriteRow = 1;
                //}

                //CheckRange
                if (position.X > rangePos.X + RANGE_WIDTH - boundWidth) //Right
                {
                    position.X -= speed;
                    outSide = true;
                    detect = false;
                }
                else if (position.X < rangePos.X) //Left
                {
                    position.X += speed;
                    outSide = true;
                    detect = false;
                }
                if (position.Y > rangePos.Y + RANGE_HEIGHT - boundHeight) //Down
                {
                    position.Y -= speed;
                    outSide = true;
                    detect = false;
                }
                else if (position.Y + 55 < rangePos.Y) //Up
                {
                    position.Y += speed;
                    outSide = true;
                    detect = false;
                }

                //Hitted                
                if (Hitted && delayHitted == 0)
                {
                    spriteRow = 4;
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
                if (!attack && !Hitted && detect)
                {
                    if (!outSide)
                    {
                        if (position.X < player.GetPos().X) //Right
                        {
                            position.X += speed;
                            spriteRow = 1;
                            flip = true;
                        }
                        if (position.X > player.GetPos().X) //Left
                        {
                            position.X -= speed;
                            spriteRow = 1;
                            flip = false;
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
                if (outSide && !attack)
                {
                    if (detect)
                    {
                        outSide = false;
                    }
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
                    if (position.Y + boundHeight / 2 < originPos.Y) //down
                    {
                        position.Y += speedToOriginPos;
                    }
                    if (position.Y + boundHeight / 2 > originPos.Y) //up
                    {
                        position.Y -= speedToOriginPos;
                    }
                    if (position + new Vector2(0, boundHeight / 2) == originPos)
                    {
                        outSide = false;
                        detect = false;
                        spriteRow = 1;
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
                    spriteTexture.Pause(1, 4);
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
                    releaseBullet = true;                    
                }                
                if (releaseBullet)
                {
                    if (!Hitted)
                    {
                        spriteTexture.Pause(1, 2);
                    }
                    else
                    {
                        spriteTexture.Pause(0, 4);
                    }
                    
                    rasenganPos += new Vector2(SPEED_BULLET * (float)Math.Cos(rasenganRo), SPEED_BULLET * (float)Math.Sin(rasenganRo));
                    delayAttack += elapsed;
                    if (delayAttack >= 3)
                    {
                        hitPlayer = false;
                        releaseBullet = false;
                        attack = false;
                        readyAttack = true;
                        spriteTexture.Reset();
                        Console.WriteLine("ResetFrame");
                        spriteTexture.Play();
                    }
                }
                else
                {
                    delayAttack = 0;
                    rasenganPos = position + new Vector2(boundWidth/2, boundHeight/2);
                    CheckAngle(player);
                }
                if (hitPlayer)
                {
                    rasenganPos = position + new Vector2(boundWidth / 2, boundHeight / 2);
                    dealDamage = false;
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

        const int SPEED_BULLET = 5;        
        float dx;
        float dy;
        float attackVectorLine;
        float rasenganRo;
        public void CheckAngle(Player player)
        {
            //dx = ms.X - (player.GetPos().X + 64);
            //dy = ms.Y - (player.GetPos().Y + 64);
            dx = (position.X + 75) - (player.Bounds.X + 50);
            dy = (position.Y + 75) - (player.Bounds.Y + 64);
            attackVectorLine = (float)Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
            if (dx > 0)
            {
                if (dy > 0)
                {
                    rasenganRo = (float)((3 * Math.PI / 2) - Math.Acos(Math.Abs(dy) / attackVectorLine));
                }
                if (dy < 0)
                {
                    rasenganRo = (float)((3 * Math.PI / 2) - Math.Acos(-1 * Math.Abs(dy) / attackVectorLine));
                }
            }
            if (dx < 0)
            {
                if (dy > 0)
                {
                    rasenganRo = (float)((Math.PI / 2) - Math.Acos(-1 * Math.Abs(dy) / attackVectorLine));
                }
                if (dy < 0)
                {
                    rasenganRo = (float)((Math.PI / 2) - Math.Acos(Math.Abs(dy) / attackVectorLine));
                }
            }
            if (dx == 0)
            {
                if (dy > 0)
                {
                    rasenganRo = (float)((Math.PI * 2) - Math.Acos(-1 * Math.Abs(dx) / attackVectorLine));

                }
                if (dy < 0)
                {
                    rasenganRo = (float)((Math.PI * 2) - Math.Asin(-1 * Math.Abs(dy) / attackVectorLine));
                }
            }
            if (dy == 0)
            {
                if (dx > 0)
                {
                    rasenganRo = (float)((Math.PI) - Math.Acos(Math.Abs(dx) / attackVectorLine));
                }
                if (dx < 0)
                {
                    rasenganRo = (float)((Math.PI * 2) - Math.Asin(Math.Abs(dy) / attackVectorLine));
                }
            }
        }
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

        public Vector2 GetPos()
        {
            return position;
        }
        public override bool DealDamage()
        {
            return base.DealDamage();
        }
        public override void GotDamage(int damage)
        {
            if (alive)
            {
                Hitted = true;
                this.damage = damage;
            }
        }
        public override void UpdateFrame(float elapsed)
        {
            spriteTexture.UpdateFrame(elapsed);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            rasenganVisible = false;
            if (!Hitted)
            {
                spriteTexture.DrawFrame(spriteBatch, position, spriteRow, flip);
            }
            else
            {
                spriteTexture.DrawFrame(spriteBatch, position, 4, flip);
            }
            
            if (releaseBullet && !hitPlayer && alive)
            {
                rasenganVisible = true;
                spriteBatch.Draw(rasengan, rasenganPos, new Rectangle(0,0, 24, 24),  Color.White);
            }
        }                

        public Rectangle Rasengan()
        {
            return rasenganRec;
        }

        public override void CheckColiision(GameObject other)
        {
            if(!rasenganRec.Intersects(((Player)other).Bounds))
            {
                dealDamage = false;                                
            }
            else if (rasenganRec.Intersects(((Player)other).Bounds) && rasenganVisible)
            {
                hitPlayer = true;
                dealDamage = true;
            }

        }

        public void Restart()
        {
            alive = true;
            hp = 100;
            position = originPos - new Vector2(0, boundHeight / 2);
            attack = false;
            outSide = true;
            flip = false;
            detect = false;
            releaseBullet = false;
            readyAttack = true;
        }
    }
}
