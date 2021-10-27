using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MinigunViking
{
    class Spider
    {

        private Texture2D texture;
        private Texture2D t;
        public Rectangle position;
        private int direction = 0;

        public bool attack;
        public bool active;

        public Rectangle activatepos;

        public void Initialize(Vector2 pos, GraphicsDevice g)
        {
            this.position = new Rectangle((int)(pos.X - 75),(int)pos.Y,150,72);
            this.attack = false;
            this.active = true;

            this.activatepos = new Rectangle((int)pos.X, (int)pos.Y, 2, 1080-(int)pos.Y);

            t = new Texture2D(g, 1, 1);
            t.SetData<Color>(new Color[] { Color.White });

        }

        public void LoadContent(ContentManager c)
        {
            texture = c.Load<Texture2D>("Textures\\spider2");
        }

        public void Update(GameTime time, int speedmodifier)
        {
            position.X -= speedmodifier;
            activatepos.X -= speedmodifier;

            if (attack)
            {
                //Update spider
                if (direction < 1)
                {
                    position.Y +=(int)(700 * time.ElapsedGameTime.TotalSeconds);

                    if (position.Y > 900)
                    {
                        direction = 1;
                    }
                    
                }
                else
                {
                    position.Y -= (int)(700 * time.ElapsedGameTime.TotalSeconds);

                    if (position.Y < activatepos.Y)
                    {
                        active = false;
                    }

                }
            }

        }
        public void Draw(SpriteBatch spritebatch)
        {
            if (attack)
            {
                spritebatch.Draw(texture, position, Color.White);
                DrawLine(spritebatch, new Vector2((float)activatepos.X, (float)activatepos.Y), new Vector2(position.X + 75, position.Y));
            }
        }


        void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end)
        {
            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);


            sb.Draw(t,
                new Rectangle(
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), 
                    1), 
                null,
                Color.White,
                angle, 
                new Vector2(0, 0),
                SpriteEffects.None,
                0);

        }

    }
}
