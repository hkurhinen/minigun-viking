using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace MinigunViking
{
    class EnemySpawner
    {
        public Rectangle position;
        public Rectangle exppos;
        public Rectangle enemyrec;
        public Texture2D housetex;
        public Texture2D destroyedtex;
        public Texture2D exptex;
        public Texture2D enemytex;

        public SoundEffect explosion;

        private bool activated = false;
        private bool burning = false;
        private bool destroyed = false;
        private bool spawning = false;
        private bool soundplaying = false;


        private float spawntimer = 5f;
        private int spawncounter = 0;

        private int spawncolor = 0;

        public void initialize(Rectangle pos)
        {
            this.position = pos;
            this.exppos = new Rectangle(pos.X + 600, 500, 27, 20);
            this.enemyrec = new Rectangle(pos.X + 600, 630, 100, 180);
        }
        public void LoadContent(ContentManager content)
        {
            housetex = content.Load<Texture2D>("Textures\\house");
            destroyedtex = content.Load<Texture2D>("Textures\\house_destroyed");
            exptex = content.Load<Texture2D>("Textures\\explosion");
            enemytex = content.Load<Texture2D>("Textures\\enemyFull");
            explosion = content.Load<SoundEffect>("Audio\\house_explosion");
        }
        public bool Update(int movespeed, GameTime time)
        {
            position.X -= movespeed;
            exppos.X -= movespeed;
            enemyrec.X -= movespeed;

            if (burning)
            {
                if (!soundplaying)
                {
                    explosion.Play();
                    soundplaying = true;
                }

                
                if (exppos.Width < 1200)
                {
                    float scalefactor = 1;
                    scalefactor += (float)(8 * time.ElapsedGameTime.TotalSeconds);

                    exppos.Width = (int)(exppos.Width * scalefactor);
                    exppos.Height = (int)(exppos.Height * scalefactor);

                    exppos.X = position.X + 600 - (exppos.Width / 2);
                    exppos.Y = 500 - (exppos.Height / 2);
                }
                else
                {
                    destroyed = true;
                    burning = false;
                }
            }

            if (spawning)
            {
                if (enemyrec.Width < 200)
                {
                    spawncolor += (int)(650 * time.ElapsedGameTime.TotalSeconds);
                    if (spawncolor > 255)
                    {
                        spawncolor = 255;
                    }

                    float scalefactor = 1;
                    scalefactor += (float)(2 * time.ElapsedGameTime.TotalSeconds);

                    enemyrec.Width = (int)(enemyrec.Width * scalefactor);
                    enemyrec.Height = (int)(enemyrec.Height * scalefactor);

                    enemyrec.X = position.X + 600 - (enemyrec.Width / 2);

                    if (300 + enemyrec.Height > 630)
                    {
                        enemyrec.Y = 300 + enemyrec.Height;
                    }
                    else
                    {
                        enemyrec.Y = 630;
                    }
                }
            }

            if (position.X < 1920)
            {
                activated = true;
            }

            if (activated && !destroyed)
            {
                spawntimer -= (float)time.ElapsedGameTime.TotalSeconds;

                if (spawntimer < 0)
                {
                    spawntimer = 5f;
                    spawncounter++;

                    if (spawncounter > 5)
                    {
                        burning = true;
                    }

                    spawning = false;
                    this.enemyrec = new Rectangle(position.X + 600, 630, 100, 180);
                    spawncolor = 0;
                    return true;
                }
                else if (spawntimer < 1)
                {
                    spawning = true;
                    return false;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public void Draw(SpriteBatch spritebatch)
        {
            if (!destroyed)
                spritebatch.Draw(housetex, position, Color.White);
            else
                spritebatch.Draw(destroyedtex, position, Color.White);

            if (spawning)
            {
                spritebatch.Draw(enemytex, enemyrec, new Color(spawncolor,spawncolor,spawncolor));
            }

            if (burning)
            {
                spritebatch.Draw(exptex, exppos, Color.White);
            }

        }


    }
}
