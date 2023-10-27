using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lastdayInkhumuang
{
    public class MiniBoss2 : Enemy, IGameFunction
    {
        Game1 game;
        int stateBoss;
        int attackCount;
        int phase;
        bool readySkill;
        bool speared;
        bool spearHit;
        float delaySpear;

        public static float hp = 500;
        private const float MaxHp = 500;

        public List<FlameThrower> flameThrower = new List<FlameThrower>();
        public MiniBoss2(Game1 game, Vector2 position, int boundHeight, int boundWidth, int frames, int framesPerSec, int framesRow, float layerDepth) : base(game, position, Vector2.Zero, boundHeight, boundWidth, 0, 0, frames, framesPerSec, framesRow, layerDepth)
        {
            spriteTexture.Load(game.Content, "Boss/Dullaha/Dullahahitn_All_Set", frames, framesRow, framesPerSec);
            this.game = game;
            originPos = position;
            speed = 6;
            phase = 1;
            alive = true;
            attack = false;
            flip = false;
            alive = true;
            speared = false;
            readySkill = false;
            spearHit = false;
            stateBoss = 0;
            this.game = game;
            spriteRow = 3;
        }

        public override Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y + 120, boundWidth, boundHeight - 120);

        public void Update(float elapsed, Player player)
        {
            Console.WriteLine(hp);
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
                    case 2:
                        FlameThrowers(player, elapsed);
                        break;
                }

            }
        }

        public void NormalAttack(Player player, float elapsed)
        {
            //FollowPlayer
            readySkill = false;
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

            //Attack
            if (attack && spriteTexture.GetFrame() == 3)
            {
                attackCount++;
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
                Hitted = true;
                hp -= damage;
            }
            if (Hitted)
            {
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

            //Alive
            if (hp <= 0 && alive)
            {
                alive = false;
                spriteTexture.Reset();
                spriteTexture.SetFramePerSec(3);
                spriteRow = 4;
                Game1.monsterCount--;
            }

            Random rand = new Random();
            //TestSwitchcase
            if (attackCount >= 6)
            {
                //stateBoss = rand.Next(0, 3);
                stateBoss = rand.Next(1, 3);
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
                    if (hp < MaxHp)
                    {
                        hp += 0.25f;
                    }
                    spriteRow = 1;
                    flip = true;
                    position -= new Vector2(speed * 3, 0f);
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
                    if (hp < MaxHp)
                    {
                        hp += 0.25f;
                    }
                    spriteRow = 1;
                    flip = false;
                    position += new Vector2(speed * 3, 0f);
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
        float timer;
        public void FlameThrowers(Player player, float elapsed)
        {            
            if (!readySkill)
            {
                timer = 0;
                if (position.Y + Bounds.Y > Game1._cameraPosition.Y)
                {
                    position += new Vector2(0, -speed);
                }
                else
                {
                    readySkill = true;
                    while (flameThrower.Count < 3)
                    {
                        flameThrower.Add(new FlameThrower(game, 24, 24));
                    }
                }
            }
            else
            {
                if (hp < MaxHp)
                {
                    hp += 0.25f;
                }                
                dealDamage = true;
                foreach (FlameThrower flames in flameThrower)
                {
                    flames.Update(player);                    
                }
                for (int i=0; i<flameThrower.Count; i++)
                {
                    if (flameThrower[i].Done())
                    {
                        flameThrower.RemoveAt(i);
                    }                    
                }

                if (flameThrower.Count == 0)
                {
                    stateBoss = 0;
                    attackCount = 0;
                }
                //timer += elapsed;
                //if (timer >= 3)
                //{                    
                //    stateBoss = 0;
                //    attackCount = 0;
                //}
            }
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
                    Sfx.PlaySfx(16);
                }
                else
                {
                    dealDamage = false;
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
            //return base.DealDamage();
            return dealDamage;
        }

        public Rectangle GetFlameThrower(int index)
        {
            return flameThrower[index].Bounds;
        }

        public Rectangle GetFallRec(int index)
        {
            return flameThrower[index].fallRec;
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
            if (stateBoss == 2)
            {
                foreach (FlameThrower flames in flameThrower)
                {
                    flames.Draw(spriteBatch);
                }
            }
        }



        public void Restart()
        {
            Console.WriteLine("Restart");
            flameThrower.Clear();
            position = originPos;
            speed = 6;
            alive = true;
            hp = 500;
            attack = false;
            flip = false;
            alive = true;
            speared = false;
            readySkill = false;
            spearHit = false;
            stateBoss = 0;
            spriteRow = 3;
        }

        public bool GetSpear()
        {
            return speared;
        }
    }
}
