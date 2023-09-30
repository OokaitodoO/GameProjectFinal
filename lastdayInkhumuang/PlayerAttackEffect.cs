using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lastdayInkhumuang
{
    public class PlayerAttackEffect : Player
    {
        bool attack;
        bool flip;
        int spriteRow;

        AnimatedTexture verticalAttack = new AnimatedTexture(Vector2.Zero, 0f, 1f, 0f);

        public PlayerAttackEffect(Game1 game, Vector2 position, int frames, int framesPerSec, int framesRow, float layerDepth) : base(game, position, 0f, 0f, 0f, frames, framesPerSec, framesRow, layerDepth)
        {
            spriteTexture.Load(game.Content, "Player/Effect-_attack", frames, framesRow, framesPerSec);
            verticalAttack.Load(game.Content, "Player/skill_player_Attack", frames, framesRow, framesPerSec);
        }
        public override Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y, 128, 128);
        public Vector2 AtkPosition => position;
        public void Update(float elapsed, Player player)
        {
            Console.WriteLine("AttackPos: " + position);
            attack = player.attacked;
            position = player.GetPos();
            if (!attack)
            {
                direction = player.GetDirection();
                spriteTexture.Reset();
                verticalAttack.Reset();                
            }
            else if (attack)
            {
                spriteTexture.Play();
                verticalAttack.Play();
                spriteTexture.UpdateFrame(elapsed);
                verticalAttack.UpdateFrame(elapsed);
                if (direction == "Right")
                {
                    flip = false;
                    //position = player.Position + new Vector2(-64, 0);
                    switch (player.comboCount) 
                    {
                        case 0:
                            spriteRow = 1;
                            if (spriteTexture.GetFrame() == 3)
                            {
                                spriteTexture.Pause(4, spriteRow);
                            }                            
                            break;
                        case 1:
                            spriteRow = 2;
                            if (spriteTexture.GetFrame() == 3)
                            {
                                spriteTexture.Pause(4, spriteRow);
                            }
                            break;
                        case 2:
                            spriteRow = 3;
                            if (spriteTexture.GetFrame() == 3)
                            {
                                spriteTexture.Pause(4, spriteRow);
                            }
                            break;
                    }                                        
                    
                }
                else if (direction == "Left")
                {
                    flip = true;
                    //position = player.Position + new Vector2(-64, 0);
                    switch (player.comboCount)
                    {
                        case 0:
                            spriteRow = 1;
                            if (spriteTexture.GetFrame() == 3)
                            {
                                spriteTexture.Pause(4, spriteRow);
                            }
                            break;
                        case 1:
                            spriteRow = 2;
                            if (spriteTexture.GetFrame() == 3)
                            {
                                spriteTexture.Pause(4, spriteRow);
                            }
                            break;
                        case 2:
                            spriteRow = 3;
                            if (spriteTexture.GetFrame() == 3)
                            {
                                spriteTexture.Pause(4, spriteRow);
                            }
                            break;
                    }
                }
                else if (direction == "Up")
                {
                    //position = player.Position + new Vector2(0, -64);
                    switch (player.comboCount)
                    {
                        case 0:
                            spriteRow = 1;
                            if (verticalAttack.GetFrame() == 3)
                            {
                                verticalAttack.Pause(4, spriteRow);
                            }
                            break;
                        case 1:
                            spriteRow = 1;
                            if (verticalAttack.GetFrame() == 3)
                            {
                                verticalAttack.Pause(4, spriteRow);
                            }
                            break;
                        case 2:
                            spriteRow = 3;
                            if (verticalAttack.GetFrame() == 3)
                            {
                                verticalAttack.Pause(4, spriteRow);
                            }
                            break;
                    }
                }
                else if (direction == "Down")
                {
                    //position = player.Position + new Vector2(0, -64);
                    switch (player.comboCount)
                    {
                        case 0:
                            spriteRow = 2;
                            if (verticalAttack.GetFrame() == 3)
                            {
                                verticalAttack.Pause(4, spriteRow);
                            }
                            break;
                        case 1:
                            spriteRow = 2;
                            if (verticalAttack.GetFrame() == 3)
                            {
                                verticalAttack.Pause(4, spriteRow);
                            }
                            break;
                        case 2:
                            spriteRow = 3;
                            if (verticalAttack.GetFrame() == 3)
                            {
                                verticalAttack.Pause(4, spriteRow);
                            }
                            break;
                    }
                }
            }
        }
        public void CheckColiision(GameObject other)
        {
            if (other.Bounds.Intersects(this.Bounds))
            {
                if (other.GetType().IsAssignableTo(typeof(Melee_Enemy)) && attack)
                {
                    ((Melee_Enemy)other).GotDamage(20);
                }
                if (other.GetType().IsAssignableTo(typeof(Range_Enemy)) && attack)
                {
                    ((Range_Enemy)other).GotDamage(20);
                }
                if (other.GetType().IsAssignableTo(typeof(MiniBoss1)) && attack)
                {
                    ((MiniBoss1)other).GotDamage(15);
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (attack && direction == "Right")
            {
                spriteTexture.DrawFrame(spriteBatch, position + new Vector2(-64, 0), spriteRow, flip);
            }           
            else if (attack && direction == "Left")
            {
                spriteTexture.DrawFrame(spriteBatch, position + new Vector2(-64, 0), spriteRow, flip);
            }
            else if (attack && direction == "Up")
            {
                verticalAttack.DrawFrame(spriteBatch, position + new Vector2(0, -64), spriteRow);
            }
            else if (attack && direction == "Down")
            {
                verticalAttack.DrawFrame(spriteBatch, position + new Vector2(0, -64), spriteRow);
            }
        }
        public override bool IsAttack()
        {
            return base.IsAttack();
        }
    }
}
