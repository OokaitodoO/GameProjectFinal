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
        int spriteRow;
        bool flip;
        float hp;
        int damage;
        bool Hitted;
        bool alive;
        float delayHitted;
        //const int SIZE_HEIGHT = 180;
        //const int SIZE_WIDTH = 150;
        public Melee_Enemy(Game1 game, Vector2 position, Vector2 tileLocation, string element, int boundHeight, int boundWidth, int frames, int framesPerSec, int framesRow, float layerDepth) : base(game, position, tileLocation, boundHeight, boundWidth, TILE_SIZE, TILE_SIZE, frames, framesPerSec, framesRow, layerDepth)
        {
            spriteTexture.Load(game.Content, "Enemy/Monster_Sword", frames, framesRow, framesPerSec);
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

        public override Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y, 150, 180); //player collision position(not yet conplete)

        public Vector2 Position => position;

        public void Update(Player player, float elapsed)
        {

            Console.WriteLine(alive);
            Console.WriteLine("IsEnable : " + IsEnable);
            //Check Alive
            if (alive)
            {
                spriteTexture.Play();

            }
            if (hp <= 0 && alive)
            {                
                spriteTexture.Reset();
                alive = false;
            }
            if (!alive)
            {
                spriteRow = 3;
                if (spriteTexture.GetFrame() == 3)
                {
                    spriteTexture.Pause(4, spriteRow);
                }
            }

            if (alive)
            {
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

                //Follow player
                if (!attack)
                {
                    if (!outSide)
                    {
                        if (position.X < player.GetPos().X) //Right
                        {
                            position.X += speed;
                            spriteRow = 2;
                            flip = false;
                        }
                        if (position.X > player.GetPos().X) //Left
                        {
                            position.X -= speed;
                            spriteRow = 2;
                            flip = true;
                        }
                        if (position.Y + 55 < player.GetPos().Y) //Down
                        {
                            position.Y += speed;
                        }
                        if (position.Y + 55 > player.GetPos().Y) //Up
                        {
                            position.Y -= speed;
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
                }
                if (!outSide)
                {
                    speed = 2;
                    speedToOriginPos = 0;
                }

                //Check player
                if (player.GetPos().X > rangePos.X && player.GetPos().X < rangePos.X + RANGE_WIDTH - boundWidth && player.GetPos().Y > rangePos.Y && player.GetPos().Y < rangePos.Y + RANGE_HEIGHT - boundHeight)
                {
                    outSide = false;
                }

                //Tets
                enable = true;


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

                //Hitted
                if (Hitted && delayHitted == 0)
                {
                    hp -= damage;
                }
                if (Hitted)
                {
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
            }
            
            
        }

        public void CheckColiision(GameObject player, GameObject playerAtk, GameObject playerSkill, PlayerAttackEffect gotAtk, PlayerSkills gotSkills)
        {           
            if (player.Bounds.Intersects(this.Bounds))
            {
                if (player.GetType().IsAssignableTo(typeof(Player)) && !attack && alive)
                {
                    spriteRow = 1;
                    spriteTexture.Reset();
                    attack = true;
                }                
            }
            else if (!player.Bounds.Intersects(this.Bounds) && alive)
            {
                spriteRow = 2;
            }
        }

        public void GotDamage(int damage)
        {
            Hitted = true;
            this.damage = damage;
        }

        public override void UpdateFrame(float elapsed)
        {
            spriteTexture.UpdateFrame(elapsed);         
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
