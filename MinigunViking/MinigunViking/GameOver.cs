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
    class GameOver
    {
        public Texture2D gameOverPic;
        public Rectangle gameOver;
        public int width;
        public int height;
        private float waittimer2 = 5f;
        private float timerstuff2 = 0;

        public GameOver()
        {
            //Constructor
        }

        public void Initialize(GraphicsDevice g)
        {

            width = 1920;
            height = 1080;

            gameOver = new Rectangle(width / 2 - ((int)(width / 1.92) / 2), height / 2 - ((int)(height / 1.88153310105) / 2), (int)(width / 1.92), (int)(height / 1.88153310105));
        }

        public void LoadContent(ContentManager content)
        {
            this.gameOverPic = content.Load<Texture2D>("Textures\\gameOverPic");
        }

        public int Update(GameTime time)
        {
            //odota ja siirry menuun

            timerstuff2 += (float)time.ElapsedGameTime.TotalSeconds;

            if (timerstuff2 > waittimer2)
            {
                timerstuff2 = 0;
                return 0;
            }
            else
            {
                return 2;
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont fontti)
        {
            spriteBatch.Draw(gameOverPic, gameOver, Color.White);
        }
    }
}
