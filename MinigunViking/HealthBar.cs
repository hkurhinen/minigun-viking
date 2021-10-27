using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MinigunViking
{
    class HealthBar
    {
        public Texture2D pixel;
        public Rectangle fullHealth;
        public Rectangle currentHealth;
        public Rectangle healthBarLocationRectangle;
        public Rectangle testiColour;
        public Rectangle testiColour2;

        public Texture2D healthBarStartOutline;
        public Texture2D healthBarStartColour;
        public Texture2D healthBarPieceOutline;
        public Texture2D healthBarPieceColour;
        public Texture2D healthBarEndOutline;
        public Texture2D healthBarEndColour;

        public int cLevel;
        public int health;

        public int fullhealth;

        public HealthBar()
        {
            //Constructor
        }

        public void Initialize(GraphicsDevice g, Viking viking)
        {
            //TODO set fullhealth at startup
            fullHealth = new Rectangle(10, 10, 200, 20);
            currentHealth = new Rectangle(10, 10, 200, 20);

            healthBarLocationRectangle = new Rectangle(20, 40, 342, 138);
            testiColour = new Rectangle(0, 0, 62, 138);
            testiColour2 = new Rectangle(122, 40, 62, 138);

            pixel = new Texture2D(g, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
            health = viking.getHealth();
        }

        public void LoadContent(ContentManager content)
        {
            this.healthBarStartOutline = content.Load<Texture2D>("healthBarTextures\\healthBarStartOutline");
            this.healthBarStartColour = content.Load<Texture2D>("healthBarTextures\\healthBarStartColour");
            this.healthBarPieceOutline = content.Load<Texture2D>("healthBarTextures\\healthBarPieceOutline");
            this.healthBarPieceColour = content.Load<Texture2D>("healthBarTextures\\healthBarPieceColour");
            this.healthBarEndOutline = content.Load<Texture2D>("healthBarTextures\\healthBarEndOutline");
            this.healthBarEndColour = content.Load<Texture2D>("healthBarTextures\\healthBarEndColour");
        }

        public void Update(Viking viking)
        {
            //TODO resize current health box
            fullHealth.Width = 2 * viking.getFullHealth();
            currentHealth.Width = 2 * viking.getHealth();
            fullhealth = viking.getFullHealth();

            //testiColour.Width -= 62/25;
            //testiColour2.Width -= 62/25;

            cLevel = viking.getLevel();
            if(health > viking.getHealth())
                health -= 1;
            if (health < viking.getHealth())
                health += 1;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            //TODO draw health bar

            for (int i = 0; i < ((health)/25); i++)
            {
                if (i < (health / 25) - 1)
                    spriteBatch.Draw(healthBarPieceColour, new Rectangle(123 + i * 61, 40, 61, 138), new Rectangle(0, 0, 61, 138), Color.White);
                else
                {
                    if(health == fullhealth)
                        spriteBatch.Draw(healthBarEndColour, new Rectangle(123 + i * 61, 40, 61, 138), new Rectangle(0, 0, 61, 138), Color.White);
                    else
                        spriteBatch.Draw(healthBarPieceColour, new Rectangle(123 + i * 61, 40, 61, 138), new Rectangle(0, 0, 61, 138), Color.White);
                }
            }
            if (!((health - 50) % 25 == 0))
            {
                if(health > fullhealth-25)
                    spriteBatch.Draw(healthBarEndColour, new Rectangle((123 + (((health) / 25) * 61)), 40, (int)((61.0f / 25.0f) * ((health) % 25.0f)), 138), new Rectangle(0, 0, (int)((61.0f / 25.0f) * ((health) % 25.0f)), 138), Color.White);
                else
                    spriteBatch.Draw(healthBarPieceColour, new Rectangle((123 + (((health) / 25)*61)), 40, (int)((61.0f / 25.0f) * ((health) % 25.0f)), 138), new Rectangle(0, 0, (int)((61.0f / 25.0f) * ((health) % 25.0f)), 138), Color.White);
            }

            for (int i = 0; i < ((fullhealth) / 25); i++)
            {
                if (i == 0)
                    spriteBatch.Draw(healthBarStartOutline, healthBarLocationRectangle, Color.White);
                else if(i+1 < fullhealth/25)
                    spriteBatch.Draw(healthBarPieceOutline, new Rectangle(123 + i * 61, 40, 61, 138), new Rectangle(0, 0, 61, 138), Color.White);
                else
                    spriteBatch.Draw(healthBarEndOutline, new Rectangle(123 + i * 61, 40, 61, 138), new Rectangle(0, 0, 61, 138), Color.White);
            }            

        }


    }
}
