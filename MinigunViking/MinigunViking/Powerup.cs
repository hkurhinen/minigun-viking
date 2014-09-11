using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MinigunViking
{
    class Powerup
    {
        Texture2D texture;
        public Rectangle position;
        private float activetimer;
        private float eselapsed = 0f;
        public bool active;
        private double gravity = -1000f;


        private double initialangle;
        private double initialforce;
        private int initialLength;
        private int initialHeight;

        public void Initialize(Rectangle pos, Texture2D tex, int angle, double force)
        {
            this.texture = tex;
            this.position = pos;
            activetimer = 8f;
            active = true;
            this.initialangle = MathHelper.ToRadians(angle);
            this.initialforce = force;
            this.initialLength = pos.X;
            this.initialHeight = pos.Y;

        }

        public void Update(int movespeed, GameTime time)
        {
            activetimer -= (float)time.ElapsedGameTime.TotalSeconds;

            if (activetimer < 0)
                active = false;

            position.X -= movespeed;

            if (position.Y < 900)
            {
                eselapsed += (float)time.ElapsedGameTime.TotalSeconds;
            }

            initialLength -= movespeed;

            double vx = initialforce * Math.Cos(initialangle);
            double vy = initialforce * Math.Sin(initialangle);

            position.X = initialLength + (int)(vx * eselapsed);
            position.Y = initialHeight - (int)(vy * eselapsed + 0.5 * gravity * (eselapsed * eselapsed));

            if (position.Y >= 900)
            {
                position.Y = 900;
            }


        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, position, Color.White);
        }


    }
}
