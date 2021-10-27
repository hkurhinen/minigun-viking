using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MinigunViking
{
    class Bloodspatter
    {
        private Texture2D pixel;
        private Texture2D groundblood;
        private Rectangle position;
        private Rectangle groundposition;
        private double initialAngle;
        public bool Active;
        private float spatterElapsed = 0f;

        private double gravity = -170f;
        private double initialvelocity = 40f;
        private int initialHeight;
        private int initialLength;
        private Color color;
        private bool spatteridle = false;
        private float idletimer = 5f;
        private int lowlevel;
        Random ramdom = new Random();

        public Bloodspatter()
        {
            //Constructor
        }

        public void Initialize(Texture2D texture, Rectangle pos, int angle, double speed, Color color, Texture2D groundtext, int stopheigtht)
        {
            this.color = color;
            Active = true;
            this.initialAngle = MathHelper.ToRadians(angle);
            pixel = texture;
            initialvelocity += speed;
            position = new Rectangle(pos.X,pos.Y,3,3);
            spatterElapsed = 0f;
            initialHeight = pos.Y;
            initialLength = pos.X + 30;
            spatteridle = false;
            idletimer = 3f;
            lowlevel = stopheigtht;
            groundposition = new Rectangle(pos.X - 8, lowlevel, 16, 14);
            this.groundblood = groundtext;
        }


        
        public void Update(GameTime time, int modifyspeed)
        {

            

            if (position.Y < lowlevel)
            {
                spatterElapsed += (float)time.ElapsedGameTime.TotalSeconds;
            }

            initialLength -= modifyspeed;

            double vx = initialvelocity * Math.Cos(initialAngle);
            double vy = initialvelocity * Math.Sin(initialAngle);

            position.X = initialLength + (int)(vx * spatterElapsed);
            position.Y = initialHeight - (int)(vy * spatterElapsed + 0.5 * gravity * (spatterElapsed * spatterElapsed));

            if (position.Y >= lowlevel)
            {
                position.Y = lowlevel;
                spatteridle = true;
            }

            if (spatteridle)
            {
                idletimer -= (float)time.ElapsedGameTime.TotalSeconds;
                if (idletimer < 0)
                {
                    this.Active = false;
                }
            }

            groundposition.X = position.X - 8;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!spatteridle)
            {
                spriteBatch.Draw(pixel, position, color);
            }
            else
            {
                spriteBatch.Draw(groundblood, groundposition, color);
            }
        }


    }
}
