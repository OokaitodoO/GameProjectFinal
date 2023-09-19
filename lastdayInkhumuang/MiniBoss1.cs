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
        int stateBoss;
        bool readySkill;
        bool speared;
        float attakTiming;
        float delaySpear;
        Game1 game;
        public MiniBoss1(Game1 game, Vector2 position, int boundHeight, int boundWidth, int frames, int framesPerSec, int framesRow, float layerDepth) : base(game, position, Vector2.Zero, boundHeight, boundWidth, TILE_SIZE, TILE_SIZE, frames, framesPerSec, framesRow, layerDepth)
        {
            spriteTexture.Load(game.Content, "Boss/Dullahahitn", frames, framesRow, framesPerSec);
            speed = 2;
            alive = true;
            hp = 500;
            attack = false;
            outSide = false;
            flip = false;
            enable = true;
            alive = true;
            readySkill = false;
            stateBoss = 0; // 0: NormalAttack | 1: Skill
            this.game = game;
        }

        public override Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y + 120, boundWidth, boundHeight - 120);
        public Vector2 Position => position;
        
        public void Update(float elapsed, Player player) 
        {
            Console.WriteLine("BossHp: " + hp);
            if (alive)
            {
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
            if (position.X + 64 < player.GetPos().X) //Right
            {
                position.X += speed;
                spriteRow = 1;
                flip = false;
            }
            if (position.X + 64 > player.GetPos().X) //Left
            {
                position.X -= speed;
                spriteRow = 1;
                flip = true;
            }
            if (position.Y + 128 < player.GetPos().Y) //Down
            {
                position.Y += speed;
                spriteRow = 1;
            }
            if (position.Y + 128 > player.GetPos().Y) //Up
            {
                position.Y -= speed;
                spriteRow = 1;
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
            if (attack)
            {                
                attakTiming += elapsed;
                if (attakTiming >= 3)
                {
                    dealDamage = true;
                    attack = false;
                    
                }
            }
            else
            {
                dealDamage = false;
                attakTiming = 0;
            }

            //TestSwitchcase
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                stateBoss = 1;
            }
        }
        public void SpearAttack(Player player, float elapsed)
        {            
            if (!readySkill)
            {
                if (position.X > player.Position.X)
                {
                    flip = false;
                    position.X += speed*3;
                    direction = "Right";
                }
                else if (position.X < player.Position.X)
                {
                    flip = true;
                    position.X -= speed*3;
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
                    flip = true;
                    position -= new Vector2(speed*7, 0f);
                    if (position.X + boundWidth < 0)
                    {
                        speared = false;
                        readySkill = false;
                        stateBoss = 0;
                    }
                }
                if (direction == "Left" && !speared)
                {
                    position = new Vector2(0 - (boundWidth), player.GetPos().Y - 108);
                }
                else if (direction == "Left" && speared)
                {
                    flip = false;
                    position += new Vector2(speed*7, 0f);
                    if (position.X > game.GraphicsDevice.Viewport.Width)
                    {
                        speared = false;
                        readySkill = false;
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
                if (player.GetType().IsAssignableTo(typeof(Player)) && !speared)
                {
                    attack = true;
                }
                else if (player.GetType().IsAssignableTo(typeof(Player)) && speared)
                {
                    dealDamage = true;
                }
                else
                {
                    dealDamage = false;
                }
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
            
        }

        public bool GetSpear()
        {
            return speared;
        }
    }
}
