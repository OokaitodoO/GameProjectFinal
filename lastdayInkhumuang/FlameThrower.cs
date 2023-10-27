using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lastdayInkhumuang
{
    public class FlameThrower : MiniBoss2
    {
        Game1 game;
        Texture2D fireBall;
        Texture2D warningZone;
        Random rand = new Random();
        private int Width;
        private int Height;
        private bool done;
        private Vector2 fallPos;
        public Rectangle fallRec;
        private int count;
        public FlameThrower(Game1 game, int boundHeight, int boundWidth) : base(game, Vector2.Zero, boundHeight, boundWidth, 0, 0, 0, 1f)
        {
            this.game = game;
            fireBall = game.Content.Load<Texture2D>("ball_mage");
            warningZone = game.Content.Load<Texture2D>("Scenes/Bg1");
            Width = boundWidth;
            Height= boundHeight;
            fallPos.X = rand.Next(0,Game1.MAP_WIDTH/3 - Width);
            fallPos.Y = rand.Next((int)Game1._cameraPosition.Y, (int)Game1._cameraPosition.Y + Game1.TILE_SIZE * 5 - Height);
            fallRec = new Rectangle((int)fallPos.X, (int)fallPos.Y, Width, Height);
            position = new Vector2(fallPos.X, (int)Game1._cameraPosition.Y);
            speed = 6;
            done = false;
        }
        public override Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y, Width, Height);

        public void Update(Player player)
        {
            if (Bounds.Intersects(player.Bounds) && fallRec.Intersects(player.Bounds))
            {
                RandomPos();
                count++;
            }
            else
            {
                if (position.Y < fallPos.Y)
                {
                    position.Y += speed;
                }
                else
                {
                    RandomPos();
                    count++;
                }
            }

            if (count >= 4)
            {
                done = true;
            }            
        }
        public void RandomPos()
        {
            fallPos.X = rand.Next(0, Game1.MAP_WIDTH / 3 - Width);
            fallPos.Y = rand.Next((int)Game1._cameraPosition.Y, (int)Game1._cameraPosition.Y + Game1.TILE_SIZE * 5 - Height);
            fallRec = new Rectangle((int)fallPos.X, (int)fallPos.Y, Width, Height);
            position = new Vector2(fallPos.X, (int)Game1._cameraPosition.Y);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(fireBall, position, Color.White);
            //spriteBatch.Draw(fireBall, fallPos, Color.Red * 0.5f);
            spriteBatch.Draw(warningZone, position, new Rectangle(0,0 , 128, 128), Color.White);
            spriteBatch.Draw(warningZone, fallPos, new Rectangle(0, 0, 128, 128), Color.Red * 0.5f);
        }
        public bool Done()
        {
            return done;
        }
        public override bool DealDamage()
        {
            return base.DealDamage();
        }
    }
}
