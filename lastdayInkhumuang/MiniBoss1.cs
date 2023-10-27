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
    public class MiniBoss1 : Enemy, IGameFunction
    {
        bool IsOnPos = false;
        int attackCount;
        int stateBoss;
        bool readySkill;
        bool speared;
        bool spearHit;
        float attakTiming;
        float delaySpear;
        Game1 game;
        SpriteFont BossInfo;

        Texture2D font;

        public static float hp;
        public MiniBoss1(Game1 game, Vector2 position, int boundHeight, int boundWidth, int frames, int framesPerSec, int framesRow, float layerDepth) : base(game, position, Vector2.Zero, boundHeight, boundWidth, TILE_SIZE, TILE_SIZE, frames, framesPerSec, framesRow, layerDepth)
        {
            spriteTexture.Load(game.Content, "Boss/Dullaha/Dullahahitn_All_Set", frames, framesRow, framesPerSec);
            this.position = position;
            originPos = position;
            speed = 6;
            alive = true;
            hp = 500;
            attack = false;
            flip = false;
            alive = true;
            speared = false;
            readySkill = false;
            spearHit = false;
            stateBoss = 2; // 0: NormalAttack | 1: Skill | 2: cutscenes
            this.game = game;
            spriteRow = 3;

            BossInfo = game.Content.Load<SpriteFont>("Ui/File");
            font = game.Content.Load<Texture2D>("Ui/Text/spritesheetFont");
        }

        public override Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y + 120, boundWidth, boundHeight - 120);
        public Vector2 Position => position;
        
        public void Update(float elapsed, Player player) 
        {
            if (alive)
            {
                if (!Game1.IsCutscenes)
                {
                    spriteTexture.SetFramePerSec(7);                    
                }
                spriteTexture.Play();
                switch (stateBoss) 
                {
                    case 0:
                        NormalAttack(player, elapsed);
                        break;
                    case 1:
                        SpearAttack(player, elapsed);
                        break;
                    case 2:
                        CutScene(elapsed);
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
                Sfx.InsPlaySfx(6);
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
            else
            {
                Sfx.InsStopSfx(6);
            }
           

            //Hitted
            if (Hitted && delayHitted == 0)
            {
                Hitted = true;
                hp -= damage;
                Sfx.PlaySfx(4);
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
                Sfx.PlaySfx(25);
                alive = false;
                spriteTexture.Reset();
                spriteTexture.SetFramePerSec(3);
                spriteRow = 4;
                Game1.monsterCount--;
                Sfx.InsStopSfx(6);                
            }

            //Attack
            if (attack && spriteTexture.GetFrame() == 3)
            {
                attackCount++;
                dealDamage = true;
                attack = false;
                Sfx.PlaySfx(9);
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
                    Sfx.PlaySfx(5);
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
                    if (delaySpear >= 2.5)
                    {
                        Sfx.InsPlaySfx(22);
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
                    Sfx.InsPlaySfx(6);                    
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
                        Sfx.InsStopSfx(6);
                    }                    
                }
                if (direction == "Left" && !speared)
                {
                    position = new Vector2(0 - (boundWidth), player.GetPos().Y - 108);
                }
                else if (direction == "Left" && speared)
                {
                    Sfx.InsPlaySfx(6);
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
                        Sfx.InsStopSfx(6);
                    }                   
                }
            }

            
        }

        float timer;
        public void CutScene(float elapsed)
        {                        
            if (position.X > Game1.MAP_WIDTH/3 - 450 && !IsOnPos)
            {
                Sfx.InsPlaySfx(6);
                spriteRow = 3;
                flip = true;
                position.X -= 3;
                spriteTexture.SetFramePerSec(7);
            }
            else if(position.X <= Game1.MAP_WIDTH / 3 - 450 && !IsOnPos)
            {
                Sfx.InsStopSfx(6);
                Sfx.InsPlaySfx(5);
                spriteRow = 2;
                flip = true;
                IsOnPos = true;
                spriteTexture.Reset();
            }
            if (IsOnPos)
            {
                timer += elapsed;
                if (timer >= 2)
                {
                    stateBoss = 0;
                    Sfx.InsStopSfx(6);
                    Game1.IsCutscenes = false;
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
            else
            {                
                if (spriteTexture.GetFrame() <= 4)
                {
                    spriteTexture.DrawFrame(spriteBatch, position, spriteRow, flip);
                    if (spriteTexture.GetFrame() == 4)
                    {
                        spriteTexture.Pause();
                    }
                    
                }
            }
            if (Game1.IsCutscenes)
            {
                spriteBatch.Draw(font, new Vector2((Game1.MAP_WIDTH / 3) / 2 - 100, 50), new Rectangle(0, 200, 420, 150), Color.White);
                //spriteBatch.DrawString(BossInfo, "-Dullahan-", new Vector2((Game1.MAP_WIDTH/3)/2 - 100, 50), Color.Lavender);
                //spriteBatch.DrawString(BossInfo, "-Quan kha mhar-", new Vector2((Game1.MAP_WIDTH / 3) / 2 - 180, 150), Color.Lavender);
            }
        }
        public void Restart()
        {
            timer = 0;
            position = originPos;
            IsOnPos = false;
            Game1.IsCutscenes = true;
            spriteTexture.SetFramePerSec(7);
            attackCount = 0;
            alive = true;
            hp = 500;
            attack = false;
            flip = false;
            alive = true;
            readySkill = false;
            speared = false;
            stateBoss = 2; // 0: NormalAttack | 1: Skill | 2: CutScene
        }

        public bool GetSpear()
        {
            return speared;
        }

        public float GetHp()
        {
            return hp;
        }
    }
}
