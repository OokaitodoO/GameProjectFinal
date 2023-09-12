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

        public PlayerAttackEffect(Game1 game, Vector2 position, int frames, int framesPerSec, int framesRow, float layerDepth) : base(game, position, 0f, 0f, 0f, frames, framesPerSec, framesRow, layerDepth)
        {
            spriteTexture.Load(game.Content, "Player/Effect_Attack", frames, framesRow, framesPerSec);
        }
        public override Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y, 128, 128);
        public Vector2 AtkPosition => position;
        public void Update(float elapsed, Player player)
        {

            attack = player.attacked;            
            position = player.GetPos();
            
            if (!attack)
            {
                direction = player.GetDirection();
                spriteTexture.Reset();
            }
            else if (attack)
            {
                spriteTexture.Play();
                spriteTexture.UpdateFrame(elapsed);
                if (spriteTexture.GetFrame() == 8 && direction == "Right")
                {
                    spriteTexture.Pause(9, 1);
                }
                else if (spriteTexture.GetFrame() == 8 && direction == "Left")
                {
                    spriteTexture.Pause(9, 2);
                }
            }
        }
        public void CheckColiision(GameObject other)
        {
            if (other.Bounds.Intersects(this.Bounds))
            {
                if (other.GetType().IsAssignableTo(typeof(Melee_Enemy)) && attack)
                {
                    ((Melee_Enemy)other).GotDamage(10);
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (attack && direction == "Right")
            {
                spriteTexture.DrawFrame(spriteBatch, position, 1);
            }           
            else if (attack && direction == "Left")
            {
                spriteTexture.DrawFrame(spriteBatch, position, 2);
            }
        }
        public override bool IsAttack()
        {
            return base.IsAttack();
        }
    }
}
