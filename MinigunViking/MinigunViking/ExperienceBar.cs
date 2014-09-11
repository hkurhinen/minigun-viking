using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MinigunViking
{
    class ExperienceBar
    {
        public Texture2D pixel;
        public Rectangle currentExp;
        public Rectangle expToLevel;
        public int width;

        public float exp = 0;
        public float toLevel = 0;

        public ExperienceBar()
        {
            //Constructor
        }

        public void Initialize(GraphicsDevice g, Viewport viewport)
        {
            

            this.width = 1920 - 200;

            expToLevel = new Rectangle((1920 / 2) - (width / 2), 1080 - 15, width, 5);
            currentExp = new Rectangle((1920 / 2) - (width / 2), 1080 - 15, 1, 5);

            pixel = new Texture2D(g, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
        }

        public void Update(Viking viking)
        {
            

            this.exp = viking.getExp();
            this.toLevel = viking.getExpToLevel();

            if (exp > toLevel)
            {
                viking.levelUp();
            }

            currentExp.Width = (int)((exp / toLevel) * width);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
           

            spriteBatch.Draw(pixel, expToLevel, Color.Black);
            spriteBatch.Draw(pixel, currentExp, Color.Yellow);

        }

    }
}
