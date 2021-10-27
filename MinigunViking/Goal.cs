using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace MinigunViking
{
    class Goal
    {
        public Texture2D Goaltex;
        public Rectangle gatePos;
        public SpriteFont font;

        public bool FinishVisible = false;

        public bool finished = false;

        public Goal()
        {
            //Constructor
        }

        public void Initialize(Rectangle position)
        {

            gatePos = position;
        }

        public void LoadContent(ContentManager content)
        {
            this.Goaltex = content.Load<Texture2D>("Textures\\end");
            font = content.Load<SpriteFont>("Fonts\\fontti1");
        }

        public void Update(GameTime time, Viking viking, int speedmod)
        {
            gatePos.X -= speedmod;


            if (gatePos.X < (1920 - 450))
            {
                FinishVisible = true;
            }
            else
            {
                FinishVisible = false;
            }

            if (viking.collidesWith(gatePos))
            {
                finished = true;
            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Goaltex, gatePos, Color.White);

            if (finished)
            {
                spriteBatch.DrawString(font, "THANK YOU FOR TRYING OUT MINIGUN VIKING DEMO", new Vector2(1000, 500), Color.White);
                spriteBatch.DrawString(font, "MORE CONTENT COMING SOON!", new Vector2(1000, 530), Color.White);
            }
        }
    }
}
