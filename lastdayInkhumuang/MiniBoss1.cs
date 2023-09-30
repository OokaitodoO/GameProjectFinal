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
    internal class MiniBoss1 : Enemy, IGameFunction
    {
        int attackCount;
        int stateBoss;
        bool readySkill;
        bool speared;
        bool spearHit;
        float attakTiming;
        float delaySpear;
        Game1 game;
        public MiniBoss1(Game1 game, Vector2 position, int boundHeight, int boundWidth, int frames, int framesPerSec, int framesRow, float layerDepth) : base(game, position, Vector2.Zero, boundHeight, boundWidth, TILE_SIZE, TILE_SIZE, frames, framesPerSec, framesRow, layerDepth)
        {
            spriteTexture.Load(game.Content, "Boss/Dullaha/Dullahahitn_All_Set", frames, framesRow, framesPerSec);
            speed = 6;
            alive = true;
            hp = 500;
            attack = false;
            flip = false;
            alive = true;
            speared = false;
            readySkill = false;
            spearHit = false;
            stateBoss = 0; // 0: NormalAttack | 1: Skill
            this.game = game;
            spriteRow = 3;
        }

        public override Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y + 120, boundWidth, boundHeight - 120);
        public Vector2 Position => position;
        
        public void Update(float elapsed, Player player) 
        {
            if (alive)
            {
                Console.WriteLine("Hp: " + hp);
                switch (stateBoss) 
                {
                    case 0:
                        NormalAttack(player, elapsed);
                        break;
                    case 1:
                        SpearAttack(player, elapsed);
                        break;
                    default: 
                        break;
                }
            }

        }

        public void NormalAttack(Player player, float elapsed)
        {
            if (!attack)
            {
                if (position.X + 64 < player.GetPos().X) //Right
                {
                    position.X += speed;
                    spriteRow = 3;
                    flip = false;
                }
                if (position.X + 64 > player.GetPos().X) //Left
                {
                    position.X -= speed;
                    spriteRow = 3;
                    flip = true;
                }
                if (position.Y + 128 < player.GetPos().Y) //Down
                {
                    position.Y += speed;
                    spriteRow = 3;
                }
                if (position.Y + 128 > player.GetPos().Y) //Up
                {
                    position.Y -= speed;
                    spriteRow = 3;
                }
            }
           

            //Hitted
            if (Hitted && delayHitted == 0)
            {
                Hitted = true;
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

            //Alive
            if (hp <= 0)
            {
                alive = false;
                Game1.monsterCount--;
            }

            //Attack
            if (attack && spriteTexture.GetFrame() == 4)
            {
                attackCount++;
                dealDamage = true;
                attack = false;
            }
            else
            {
                dealDamage = false;
            }

            //TestSwitchcase
            if (attackCount >= 6)
            {
                stateBoss = 1;
            }
        }

        public void SpearAttack(Player player, float elapsed)
        {
            attack = false;
            if (!readySkill)
            {
                dealDamage = false;
                if (position.X > player.Position.X)
                {
                    flip = false;
                    spriteRow = 3;
                    position.X += speed;
                    direction = "Right";
                }
                else if (position.X < player.Position.X)
                {
                    flip = true;
                    spriteRow = 3;
                    position.X -= speed;
                    direction = "Left";
                }
                if (position.X + boundWidth < 0 || position.X > game.GraphicsDevice.Viewport.Width + Game1._cameraPosition.X)
                {
                    readySkill = true;
                }
            }
            else
            {
                if (!speared)
                {
                    delaySpear += elapsed;
                    if (delaySpear >= 3)
                    {
                        speared = true;
                    }
                }
                else
                {
                    delaySpear = 0;
                }

                //Spear
                if (direction == "Right" && !speared)
                {                    
                    position = new Vector2(game.GraphicsDevice.Viewport.Width, player.GetPos().Y - 108);
                }
                else if (direction == "Right" && speared)
                {
                    spriteRow = 1;
                    flip = true;
                    position -= new Vector2(speed*3, 0f);
                    if (position.X + boundWidth < 0)
                    {
                        speared = false;
                        readySkill = false;
                        spearHit = false;
                        attackCount = 0;
                        stateBoss = 0;
                    }                    
                }
                if (direction == "Left" && !speared)
                {
                    position = new Vector2(0 - (boundWidth), player.GetPos().Y - 108);
                }
                else if (direction == "Left" && speared)
                {
                    spriteRow = 1;
                    flip = false;
                    position += new Vector2(speed*3, 0f);
                    if (position.X > game.GraphicsDevice.Viewport.Width)
                    {
                        speared = false;
                        readySkill = false;
                        spearHit = false;
                        attackCount = 0;
                        stateBoss = 0;
                    }                   
                }
            }

            
        }

        public override void GotDamage(int damage)
        {
            if (alive && stateBoss == 0)
            {
                Hitted = true;
                this.damage = damage;                
            }
            
        }

        public override bool DealDamage()
        {
            return base.DealDamage();
        }

        public override void CheckColiision(GameObject player)
        {
            if (player.Bounds.Intersects(this.Bounds))
            {
                if (player.GetType().IsAssignableTo(typeof(Player)) && !speared && !attack && alive && stateBoss == 0)
                {
                    spriteRow = 2;
                    spriteTexture.Reset();
                    attack = true;
                }
                else if (player.GetType().IsAssignableTo(typeof(Player)) && speared && !dealDamage)
                {
                    dealDamage = true;
                }
                else
                {
                    dealDamage = false;
                }
                //if (player.GetType().IsAssignableTo(typeof(Player)) && speared && dealDamage)
                //{
                //    dealDamage = false;
                //}
            }
           
        }

        public override void UpdateFrame(float elapsed)
        {
            spriteTexture.UpdateFrame(elapsed);            
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                spriteTexture.DrawFrame(spriteBatch, position, spriteRow, flip);
            }
            
        }
        public void Restart()
        {
            attackCount = 0;
            alive = true;
            hp = 500;
            attack = false;
            flip = false;
            alive = true;
            readySkill = false;
            speared = false;
            stateBoss = 0; // 0: NormalAttack | 1: Skill
        }

        public bool GetSpear()
        {
            return speared;
        }
    }
}
