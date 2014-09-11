using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MinigunViking
{
    public class Bullet
    {

        public Texture2D Texture;
        public Texture2D muzzleBlast;
        public Rectangle Position;
        public bool Active;
        public int Damage;
        Viewport viewport;
        public Boolean shootRight;
        public double angle;

        public int Width
        {
            get { return Texture.Width; }
        }
        public int Height
        {
            get { return Texture.Height; }
        }

        float bulletMoveSpeed;

        public void Initialize(Viewport viewport, Texture2D texture, Rectangle position, Boolean sRight, Texture2D blast, double bulletangle)
        {
            shootRight = sRight;
            Texture = texture;
            muzzleBlast = blast;
            Position = position;
            angle = bulletangle;

            this.viewport = viewport;

            Active = true;

            Damage = 15;

            bulletMoveSpeed = 40f;
        }

        public void Update()
        {
            if (shootRight)
            {
                Position.X += (int)(Math.Cos(angle) * bulletMoveSpeed);
                Position.Y += (int)(Math.Sin(angle) * bulletMoveSpeed);
            }
            else
            {

                Position.X -= (int)(Math.Cos(angle) * bulletMoveSpeed);
                Position.Y -= (int)(Math.Sin(angle) * bulletMoveSpeed);
            }

            if (Position.X + Texture.Width / 2 > 1920)
                Active = false;

            if (Position.Y + Texture.Height / 2 > 1080)
                Active = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (shootRight)
            {
                //spriteBatch.Draw(Texture, Position, Color.White);
                spriteBatch.Draw(Texture, new Rectangle(Position.X, Position.Y, 23, 12), new Rectangle(0, 0, 23, 12), Color.White,
                    //roration      origin          effect    layer
                 (float)angle, new Vector2(0, -3), SpriteEffects.None, 0.0f);

            }

            else
                spriteBatch.Draw(Texture, new Rectangle(Position.X, Position.Y, 23, 12), new Rectangle(0, 0, 23, 12), Color.White,
                    //roration      origin          effect    layer
                (float)angle, new Vector2(0, -5), SpriteEffects.FlipHorizontally, 0.0f);

        }
    }
}
