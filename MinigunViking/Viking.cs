using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MinigunViking
{

    public class Viking
    {
        private SpriteEffects myEffect = SpriteEffects.FlipHorizontally;

        public Rectangle Position;
        public Rectangle Position2;

        public Boolean rot1;
        public Boolean rot2;
        public float legRot;
        public float legRot2;

        public Texture2D web { get; set; }

        public Boolean immobilized;
        public float immobilizedTimer = 3.00f;

        public Rectangle collisionRectangle;

        public Rectangle spinnaus;

        public Rectangle crosshairbox = new Rectangle(0, 0, 50, 150);

        public int groundLevelPos = 500;

        public float Rotation;
        public int degrees;
        public float elapsed;
        public int barrelRoll;
        public int bulletGoesViuhhhh = 0;

        public int GroundLevel;

        public int degrees2;
        public int degrees3;
        public int degrees4;
        private int degrees5;
        private int degrees6;

        public int feetDegrees;
        public int feetDegrees2;

        public int naamaVatkaus;

        public int gunShake = 0;
        Boolean suunta = false;

        public double asd;
        public double asd2;
        public double asd3;
        public double asd4;
        private double asd5;
        private double asd6;

        public double xasd;
        public double xasd2;
        public double xasd3;
        public double xasd4;
        private double xasd5;
        private double xasd6;

        //volumes for audio
        private float masterVolume;
        private float gunVolume;
        private float otherVolume;
        private float musicVolume;


        public float rotateCrossHair;
        public float scaleCrossHairX = 50;
        public float scaleCrosshairY = 150;
        public Boolean scaleHair = false;

        public double aimrotation = 0;

        private Texture2D rLeg;
        private Texture2D lLeg;
        private Texture2D rArm;
        private Texture2D lArm;
        private Texture2D mouth;
        private Texture2D face;
        private Texture2D belly;
        private Texture2D mGun;
        private Texture2D mGunBarrel;
        private Texture2D bulletTexture;
        private Texture2D rFoot;
        private Texture2D lFoot;
        private Texture2D mBlast;
        private Texture2D beard;
        private Texture2D shadow;
        private Texture2D hat;

        private Texture2D crosshair;
        public Texture2D t;

        //sounds
        private SoundEffect gunfire;
        private SoundEffect gunslow;
        private SoundEffect gunspin;
        private SoundEffect gunspin2;

        private bool songplaying = false;
        private Song bgmusic;

        public Boolean walkRight;
        public Boolean scrollRight;
        public Boolean scrollLeft;

        Boolean shooting;
        Boolean jumping;
        Boolean falling;
        Boolean spinning;
        Boolean pum;
        Boolean walking;
        public Boolean lookright;
        public Boolean isDead = false;

        Boolean test;

        private List<Bullet> bullets;

        //heikin muuttuja
        private int shootstart = 0;
        private int shootstop = 0;
        private int shootcontinue = 0;

        int valiy;
        int valix;

        private float maxidledist;
        private TimeSpan idlespeed;
        private TimeSpan lastidle;
        private float idledist = 5;
        private float direction = 0;
        private float oldX = 0;

        Vector2 aimorigin;
        Vector2 aimtarget;
        Vector2 rotationorigin = new Vector2(100, 100);

        Vector2 bulletorigin;
        Vector2 flippedorigin;

        private GraphicsDevice graphicsDevice;
        private bool damaged = false;
        private float safetimer = 0.5f;

        private int fullHealth;
        private int health;
        private float exp = 0;
        private float exptolevel = 100;
        private int clevel = 1;
        float elapsedTime;
        float timer;

        private bool finishIsVisible = false;
        
        public Viking()
        {

        }

        public void Initialize(GraphicsDevice g)
        {
            // TODO: Add your initialization logic here

            graphicsDevice = g;

            MediaPlayer.Volume = musicVolume * (float)(masterVolume / 100.0);

            GroundLevel = (int)(1080 * 0.65);

            Position = new Rectangle((int)(1920 * 0.1), GroundLevel, 622, 350);
            Position2 = new Rectangle((int)(1920 * 0.1), GroundLevel, 622, 350);

            collisionRectangle = new Rectangle(Position.X, Position.Y, 242, Position.Height);

            spinnaus = new Rectangle(50, 50, 100, 100);

            bullets = new List<Bullet>();

            walkRight = true;
            shooting = false;

            test = true;
            damaged = false;
            safetimer = 0.5f;
            isDead = false;
            fullHealth = 250;
            health = fullHealth;
        }

        public void setVolume(float master, float gun, float music, float other)
        {
            //Lineaarinen säätö volyyymille
            SoundEffect.MasterVolume = (float)(1 - Math.Sqrt(1 - (master * master)));

            masterVolume = (float)(1 - Math.Sqrt(1 - (master * master)));
            gunVolume = (float)(1 - Math.Sqrt(1 - (gun * gun)));
            music = music * master;
            musicVolume = (float)(1 - Math.Sqrt(1 - (music * music)));
            otherVolume = (float)(1 - Math.Sqrt(1 - (other * other)));
        }

        public void setFinish(bool state)
        {
            finishIsVisible = state;
        }

        public bool isDamaged()
        {
            return damaged;
        }
        public void isDamaged(bool damaged)
        {
            this.damaged = damaged;
        }

        public List<Bullet> getBullets()
        {
            return bullets;
        }

        public int getLevel()
        {
            return clevel;
        }

        public int getFullHealth()
        {
            return fullHealth;
        }

        public int getHealth()
        {
            return health;
        }

        public bool collidesWith(Rectangle collider)
        {
            if (collisionRectangle.Intersects(collider))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void levelUp()
        {
            this.clevel++;
            this.fullHealth += 25;
            this.health = fullHealth;

            this.exp = 0;
            this.exptolevel = exptolevel * 2;

        }

        public bool dead()
        {
            return this.isDead;
        }

        public float getExp()
        {
            return exp;
        }

        public float getExpToLevel()
        {
            return exptolevel;
        }

        public void giveExp(int amount)
        {
            this.exp += amount;
        }

        public void heal()
        {
            this.health = fullHealth;
        }

        public void damage()
        {
            this.health -= 25;

        }


        private void AddBullet(Rectangle position, Boolean sRight, double bulletangle)
        {
            Bullet bullet = new Bullet();

            if (sRight)
            {
                position.X = position.X - (int)(115 * Math.Cos(bulletangle));
                position.Y = position.Y - (int)(115 * Math.Sin(bulletangle));
            }
            else
            {
                position.X = position.X + (int)(115 * Math.Cos(bulletangle));
                position.Y = position.Y + (int)(115 * Math.Sin(bulletangle));
            }

            bullet.Initialize(graphicsDevice.Viewport, bulletTexture, position, sRight, mBlast, bulletangle);
            bullets.Add(bullet);
        }


        private double calculateAngle(double dx, double dy)
        {

            double angle = Math.Atan2(dy, dx);
            if (angle < 0)
            {
                angle = Math.PI * 2 + angle;
            }


            return angle;
        }

        public void updateAimOrigin(double angle)
        {

            if (health < 0)
                isDead = true;

            double cy = Position.Y + 100;
            double cx = Position.X + 100;

        
            double radius = 1;

            this.aimorigin.X = (float)(cx + radius * Math.Cos(angle));
            this.aimorigin.Y = (float)(cy + radius * Math.Sin(angle));

            double radius2 = 10;
            double radius3 = 30;

            valix = (int)((cy + radius2 * Math.Sin(angle)) - (cy + radius3 * Math.Sin(angle)));
            valiy = (int)((cy + radius2 * Math.Cos(angle)) - (cy + radius3 * Math.Cos(angle)));

            if (valiy < 0)
            {
                valiy = -valiy;

            }

            double gunradius = 350;
            double flippedradius = 350;

            this.bulletorigin.X = (float)(cx + gunradius * Math.Cos(angle + MathHelper.ToRadians((float)(19))));
            this.bulletorigin.Y = (float)(cy + gunradius * Math.Sin(angle + MathHelper.ToRadians((float)(19))));

            this.flippedorigin.Y = (float)(cy + flippedradius * Math.Sin(angle + MathHelper.ToRadians((float)(180 - 19))));
            this.flippedorigin.X = (float)(cx + flippedradius * Math.Cos(angle + MathHelper.ToRadians((float)(180 - 19))));

        }

        public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {

            t = new Texture2D(graphicsDevice, 1, 1);
            t.SetData<Color>(
                new Color[] { Color.White });// fill the texture with white

            // Ladataan kuvia yms
            this.rArm = content.Load<Texture2D>("Viking\\rArmV2");
            this.lArm = content.Load<Texture2D>("Viking\\lArmV2");
            this.rLeg = content.Load<Texture2D>("Viking\\rightLeg");
            this.lLeg = content.Load<Texture2D>("Viking\\leftLeg");
            this.belly = content.Load<Texture2D>("Viking\\bellyV2");
            this.face = content.Load<Texture2D>("Viking\\face3");
            this.mouth = content.Load<Texture2D>("Viking\\mouth2");
            this.mGun = content.Load<Texture2D>("Viking\\miniGun3");
            this.mGunBarrel = content.Load<Texture2D>("Viking\\minigunBarrel");
            this.rFoot = content.Load<Texture2D>("Viking\\rightFoot");
            this.lFoot = content.Load<Texture2D>("Viking\\leftFoot");
            this.beard = content.Load<Texture2D>("Viking\\beard2");
            this.web = content.Load<Texture2D>("Viking\\webbedViking");
            this.hat = content.Load<Texture2D>("Viking\\hat");

            this.shadow = content.Load<Texture2D>("Textures\\shadow");

            this.crosshair = content.Load<Texture2D>("Textures\\gameCrosshair");

            this.mBlast = content.Load<Texture2D>("Viking\\gunFlame");
            this.bulletTexture = content.Load<Texture2D>("Textures\\bullet");

            this.gunfire = content.Load<SoundEffect>("Audio\\gun3");
            this.gunslow = content.Load<SoundEffect>("Audio\\minigun_slow");
            this.gunspin = content.Load<SoundEffect>("Audio\\minigun_spin");            
            this.gunspin2 = content.Load<SoundEffect>("Audio\\minigun_spin2");

            this.bgmusic = content.Load<Song>("Audio\\bgmusic");

            MediaPlayer.IsRepeating = true;
        }

        public void Update(GameTime gameTime, float e, KeyboardState kstate, MouseState mstate, Point mPos)
        {
            MediaPlayer.Volume = musicVolume;

            if (immobilized)
            {
                immobilizedTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (immobilizedTimer <= 0)
                {
                    immobilizedTimer = 3.0f;
                    immobilized = false;
                }
            }

            if (!songplaying)
            {
                MediaPlayer.Play(bgmusic);
                songplaying = true;
            }

            if (damaged)
            {
                safetimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (safetimer < 0)
                {
                    damaged = false;
                    safetimer = 0.5f;
                }
            }

            collisionRectangle.X = Position.X;
            collisionRectangle.Y = Position.Y;

            elapsed = e;
            elapsedTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (mstate.LeftButton == ButtonState.Pressed)
                shooting = true;
            if (mstate.LeftButton == ButtonState.Released)
                shooting = false;

            if (mstate.RightButton == ButtonState.Pressed)
                spinning = true;
            if (mstate.RightButton == ButtonState.Released)
                spinning = false;

            crosshairbox.X = mPos.X;
            crosshairbox.Y = mPos.Y;

            if (kstate.IsKeyDown(Keys.D) && !immobilized)
            {
                if (finishIsVisible)
                {
                    if (Position.X < (1920 - collisionRectangle.Width))
                    {
                        Position.X += (int)(500 * elapsed);
                        Position2.X += (int)(500 * elapsed);
                    }
                    scrollRight = false;
                    scrollLeft = false;

                }
                else
                {
                    if (Position.X < (int)(1920 * 0.33))
                    {
                        Position.X += (int)(500 * elapsed);
                        Position2.X += (int)(500 * elapsed);
                        scrollRight = false;
                        scrollLeft = false;
                    }
                    else
                    {
                        scrollLeft = false;
                        scrollRight = true;
                    }
                    walkRight = true;
                    walking = true;
                }
            }
            else
            {
                scrollRight = false;
                if (!kstate.IsKeyDown(Keys.A))
                    walking = false;
            }


            if (kstate.IsKeyDown(Keys.Space) && !jumping && !immobilized)
            {
                falling = false;
                jumping = true;
            }
            else if (!jumping)
            {
                falling = true;
            }

            if (kstate.IsKeyDown(Keys.A) && !immobilized)
            {
                if (Position.X > (int)(1920 * 0.14))
                {
                    Position.X -= (int)(500 * elapsed);
                    Position2.X -= (int)(500 * elapsed);
                    scrollRight = false;
                    scrollLeft = false;
                }
                else
                {
                    scrollRight = false;
                    scrollLeft = true;
                }
                walkRight = false;
                walking = true;
            }
            else
            {
                scrollLeft = false;
                if (!kstate.IsKeyDown(Keys.D))
                    walking = false;
            }

            //Heikin koodi
            if (oldX != Position.X)
            {
                idlespeed = TimeSpan.FromMilliseconds(20);
                maxidledist = 5;
            }
            else
            {
                idlespeed = TimeSpan.FromMilliseconds(50);
                maxidledist = 5;
            }

            oldX = Position.X;

            if (lastidle + idlespeed < gameTime.TotalGameTime)
            {

                lastidle = gameTime.TotalGameTime;
                idledist--;

                if (idledist == 0)
                {

                    idledist = maxidledist;

                    if (direction == 0)
                    {
                        direction = 1;
                    }
                    else
                    {
                        direction = 0;
                    }
                }

                if (direction == 1)
                {
                    naamaVatkaus += 1;
                    Position.Y += 1;
                }

                if (direction == 0)
                {
                    naamaVatkaus -= 1;
                    Position.Y -= 1;
                }

            }

            if (spinning || shooting && barrelRoll > 200)
            {
                timer -= elapsedTime;

                if (timer <= 0.0f && shootcontinue == 1)
                {
                    gunspin2.Play(gunVolume, 0.0f, 0.0f);
                    timer = (float)gunspin2.Duration.TotalMilliseconds;
                } 
            }

            if ((spinning || shooting) && barrelRoll < 1600)
            {
                barrelRoll += (int)(1500 * elapsed);

                shootcontinue = 1;

                if (shootstart == 0 && barrelRoll > 200)
                {                    
                    shootstart = 1;
                    shootstop = 0;
                    gunspin.Play(gunVolume, 0.0f, 0.0f);                    
                    
                    timer = (float)gunspin.Duration.TotalMilliseconds;
                }
            }

            if (!spinning && !shooting && barrelRoll > 0)
            {
                barrelRoll -= (int)(500 * elapsed);
                if (shootstop == 0)
                {
                    shootcontinue = 0;
                    shootstop = 1;
                    shootstart = 0;
                    gunslow.Play(gunVolume, 0.0f, 0.0f);
                }
            }

            if (shooting && barrelRoll >= 1600)
            {
                if (!suunta)
                {
                    gunShake -= (int)(200 * elapsed);
                    pum = false;
                }
                if (suunta)
                {
                    gunShake += (int)(150 * elapsed);
                    pum = true;
                }
                if (gunShake >= 0)
                {
                    suunta = false;

                    gunfire.Play(gunVolume, 0.0f, 0.0f);

                    if (lookright)
                        AddBullet(new Rectangle((int)bulletorigin.X, (int)bulletorigin.Y, 23, 12), true, aimrotation);
                    else
                        AddBullet(new Rectangle((int)flippedorigin.X, (int)flippedorigin.Y, 23, 12), false, aimrotation);
                }
                if (gunShake <= -3)
                    suunta = true;
            }
            else
            {
                gunShake = 0;
                pum = false;
            }



            if (degrees < 360)
                degrees += (int)(barrelRoll * elapsed);
            else
                degrees = 1;

            if (degrees <= 300)
                degrees2 = 60 + degrees;
            else
                degrees2 = 60 + degrees - 360;

            if (degrees <= 240)
                degrees3 = 120 + degrees;
            else
                degrees3 = 120 + degrees - 360;

            if (degrees <= 180)
                degrees4 = 180 + degrees;
            else
                degrees4 = 180 + degrees - 360;

            if (degrees <= 120)
                degrees5 = 240 + degrees;
            else
                degrees5 = 240 + degrees - 360;

            if (degrees <= 60)
                degrees6 = 300 + degrees;
            else
                degrees6 = 300 + degrees - 360;

            UpdateBullets();

            Rotation = MathHelper.ToRadians(degrees);

            asd = Math.Cos(MathHelper.ToRadians(degrees)) * valiy; // LASKETAAN VÄLIT PYSSYN PIIPUILLE ERI ASENNOISSA
            asd2 = Math.Cos(MathHelper.ToRadians(degrees + 60)) * valiy;
            asd3 = Math.Cos(MathHelper.ToRadians(degrees + 120)) * valiy;
            asd4 = Math.Cos(MathHelper.ToRadians(degrees + 180)) * valiy;
            asd5 = Math.Cos(MathHelper.ToRadians(degrees + 240)) * valiy;
            asd6 = Math.Cos(MathHelper.ToRadians(degrees + 300)) * valiy;

            xasd = Math.Cos(MathHelper.ToRadians(degrees)) * valix;
            xasd2 = Math.Cos(MathHelper.ToRadians(degrees + 60)) * valix;
            xasd3 = Math.Cos(MathHelper.ToRadians(degrees + 120)) * valix;
            xasd4 = Math.Cos(MathHelper.ToRadians(degrees + 180)) * valix;
            xasd5 = Math.Cos(MathHelper.ToRadians(degrees + 240)) * valix;
            xasd6 = Math.Cos(MathHelper.ToRadians(degrees + 300)) * valix;

            if (jumping && Position.Y > GroundLevel - 200 && !falling)
            {
                if (Position.Y > GroundLevel - 150)
                {
                    Position = new Rectangle(Position.X, Position.Y - 10, Position.Width, Position.Height);
                    Position2 = new Rectangle(Position2.X, Position2.Y - 10, Position2.Width, Position2.Height);
                }
                else if (Position.Y > GroundLevel - 200 && Position.Y <= GroundLevel - 150)
                {
                    Position = new Rectangle(Position.X, Position.Y - 6, Position.Width, Position.Height);
                    Position2 = new Rectangle(Position2.X, Position2.Y - 6, Position2.Width, Position2.Height);
                }
            }
            else if (Position.Y < GroundLevel)
            {
                falling = true;
                if (Position.Y < GroundLevel - 100)
                {
                    Position = new Rectangle(Position.X, Position.Y + 6, Position.Width, Position.Height);
                    Position2 = new Rectangle(Position2.X, Position2.Y + 6, Position2.Width, Position2.Height);
                }
                else if (Position.Y > GroundLevel - 100 && Position.Y < GroundLevel)
                {
                    Position = new Rectangle(Position.X, Position.Y + 9, Position.Width, Position.Height);
                    Position2 = new Rectangle(Position2.X, Position2.Y + 9, Position2.Width, Position2.Height);
                }
            }
            else
                jumping = false;

            if (feetDegrees <= 70 && test && !falling && jumping)
            {
                feetDegrees += (int)(350 * elapsed);

                if (feetDegrees >= 70)
                {
                    test = false;
                }
            }
            else if (falling)
            {
                if (feetDegrees > 0)
                    feetDegrees -= (int)(200 * elapsed);
                else
                {
                    feetDegrees = 0;
                    test = true;
                }
            }
            feetDegrees2 = -feetDegrees;

            aimtarget.X = crosshairbox.X + 10;
            aimtarget.Y = crosshairbox.Y + 10;      




            updateAimOrigin(aimrotation);

            if (MathHelper.ToDegrees((float)(calculateAngle(aimtarget.X - aimorigin.X, aimtarget.Y - aimorigin.Y))) > 330 || MathHelper.ToDegrees((float)(calculateAngle(aimtarget.X - aimorigin.X, aimtarget.Y - aimorigin.Y))) < 15)
            {
                aimrotation = calculateAngle(aimtarget.X - aimorigin.X, aimtarget.Y - aimorigin.Y);
                lookright = true;
            }

            if (MathHelper.ToDegrees((float)(calculateAngle(aimtarget.X - aimorigin.X, aimtarget.Y - aimorigin.Y))) > 165 && MathHelper.ToDegrees((float)(calculateAngle(aimtarget.X - aimorigin.X, aimtarget.Y - aimorigin.Y))) < 210)
            {
                aimrotation = calculateAngle(aimorigin.X - aimtarget.X, aimorigin.Y - aimtarget.Y);
                lookright = false;
            }

            if (MathHelper.ToDegrees(rotateCrossHair) >= 360)
            {
                rotateCrossHair = MathHelper.ToRadians(0);
            }
            else
            {
                rotateCrossHair += MathHelper.ToRadians(3);
            }

            if (!scaleHair)
            {
                scaleCrossHairX += 2 * 0.33333333333f;
                scaleCrosshairY += 2;
                if (scaleCrosshairY >= 200)
                    scaleHair = true;
            }
            else if (scaleHair)
            {
                scaleCrossHairX -= 2 * 0.33333333333f;
                scaleCrosshairY -= 2;
                if (scaleCrosshairY <= 150)
                    scaleHair = false;
            }

            if (!rot1)
            {
                legRot = legRot + MathHelper.ToRadians(4);
                if (MathHelper.ToDegrees(legRot) >= 45)
                    rot1 = true;
            }
            else if (rot1)
            {
                legRot = legRot - MathHelper.ToRadians(4);
                if (MathHelper.ToDegrees(legRot) <= -45)
                    rot1 = false;
            }

            if (rot2)
            {
                legRot2 = legRot2 + MathHelper.ToRadians(4);
                if (MathHelper.ToDegrees(legRot2) >= 45)
                    rot2 = false;
            }
            else if (!rot2)
            {
                legRot2 = legRot2 - MathHelper.ToRadians(4);
                if (MathHelper.ToDegrees(legRot2) <= -45)
                    rot2 = true;
            }

        }

        private void UpdateBullets()
        {
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                bullets[i].Update();

                if (bullets[i].Active == false)
                {
                    bullets.RemoveAt(i);
                }
            }
        }

        public void pauseMusic()
        {
            MediaPlayer.Pause();
        }

        public void resumeMusic()
        {
            MediaPlayer.Resume();
        }

        public Boolean musicIsPaused()
        {
            if (MediaPlayer.State == MediaState.Paused)
            {
                return true;
            }
            else
                return false;
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            // TODO: Add your drawing code here


            if (lookright)
            {
                spriteBatch.Draw(shadow, new Rectangle(collisionRectangle.X+5, 985, 250, 50), Color.White);
                if (!jumping)
                {
                    if (walking)
                    {
                        spriteBatch.Draw(rFoot, new Rectangle(Position2.X + 136, Position2.Y + 191, Position2.Width, Position2.Height), null, Color.Gray, legRot2, new Vector2(136, 191), SpriteEffects.None, 0.0f);
                        spriteBatch.Draw(rLeg, new Rectangle(Position2.X+136, Position2.Y+191, Position2.Width, Position2.Height), null, Color.Gray, legRot2, new Vector2(136, 191), SpriteEffects.None, 0.0f);
                        spriteBatch.Draw(lFoot, new Rectangle(Position2.X + 95, Position2.Y + 212, Position2.Width, Position2.Height), null, Color.White, legRot, new Vector2(95, 212), SpriteEffects.None, 0.0f);
                        spriteBatch.Draw(lLeg, new Rectangle(Position2.X + 95, Position2.Y + 212, Position2.Width, Position2.Height), null, Color.White, legRot, new Vector2(95, 212), SpriteEffects.None, 0.0f);
                    }
                    else
                    {
                        spriteBatch.Draw(rFoot, Position2, Color.Gray);
                        spriteBatch.Draw(rLeg, Position2, Color.Gray);
                        spriteBatch.Draw(lFoot, Position2, Color.LightGray);
                        spriteBatch.Draw(lLeg, Position2, Color.LightGray);
                    }
                }
                else
                {
                    spriteBatch.Draw(rFoot, new Vector2(Position2.X + 130, Position2.Y + 280), null, Color.Gray, MathHelper.ToRadians(feetDegrees), new Vector2(130, 280), 1.0f, SpriteEffects.None, 1);
                    spriteBatch.Draw(rLeg, Position2, Color.Gray);
                    spriteBatch.Draw(lFoot, new Vector2(Position2.X + 90, Position2.Y + 280), null, Color.LightGray, MathHelper.ToRadians(feetDegrees), new Vector2(90, 280), 1.0f, SpriteEffects.None, 1);
                    spriteBatch.Draw(lLeg, Position2, Color.LightGray);
                }                
            }
            else
            {
                spriteBatch.Draw(shadow, new Rectangle(collisionRectangle.X - 20, 985, 250, 50), Color.White);
                if (!jumping)
                {
                    if (walking)
                    {
                        spriteBatch.Draw(rFoot, new Rectangle(Position2.X + 86, Position2.Y + 191, Position2.Width, Position2.Height), null, Color.Gray, legRot2, new Vector2(486, 191), myEffect, 0.0f);
                        spriteBatch.Draw(rLeg, new Rectangle(Position2.X + 86, Position2.Y + 191, Position2.Width, Position2.Height), null, Color.Gray, legRot2, new Vector2(486, 191), myEffect, 0.0f);
                        spriteBatch.Draw(lFoot, new Rectangle(Position2.X + 129, Position2.Y + 212, Position2.Width, Position2.Height), null, Color.White, legRot, new Vector2(529, 212), myEffect, 0.0f);
                        spriteBatch.Draw(lLeg, new Rectangle(Position2.X + 129, Position2.Y + 212, Position2.Width, Position2.Height), null, Color.White, legRot, new Vector2(529, 212), myEffect, 0.0f);

                    }
                    else
                    {
                        spriteBatch.Draw(rFoot, new Vector2(Position2.X + 90, Position2.Y + 280), null, Color.Gray, MathHelper.ToRadians(feetDegrees2), new Vector2(490, 280), 1.0f, myEffect, 1);
                        spriteBatch.Draw(rLeg, new Rectangle(Position2.X - 400, Position2.Y, 622, 350), new Rectangle(0, 0, 622, 350), Color.Gray,
                            //roration      origin          effect    layer
                        0.0f, new Vector2(0, 0), myEffect, 0.0f);
                                                spriteBatch.Draw(lFoot, new Vector2(Position2.X + 130, Position2.Y + 280), null, Color.LightGray, MathHelper.ToRadians(feetDegrees2), new Vector2(530, 280), 1.0f, myEffect, 1);
                        spriteBatch.Draw(lLeg, new Rectangle(Position2.X - 400, Position2.Y, 622, 350), new Rectangle(0, 0, 622, 350), Color.LightGray,
                            //roration      origin          effect    layer
                        0.0f, new Vector2(0, 0), myEffect, 0.0f);
                    }
                }
                else
                {
                    spriteBatch.Draw(rFoot, new Vector2(Position2.X + 90, Position2.Y + 280), null, Color.Gray, MathHelper.ToRadians(feetDegrees2), new Vector2(490, 280), 1.0f, myEffect, 1);

                    spriteBatch.Draw(rLeg, new Rectangle(Position2.X - 400, Position2.Y, 622, 350), new Rectangle(0, 0, 622, 350), Color.Gray,
                        //roration      origin          effect    layer
                    0.0f, new Vector2(0, 0), myEffect, 0.0f);

                    spriteBatch.Draw(lFoot, new Vector2(Position2.X + 130, Position2.Y + 280), null, Color.LightGray, MathHelper.ToRadians(feetDegrees2), new Vector2(530, 280), 1.0f, myEffect, 1);

                    spriteBatch.Draw(lLeg, new Rectangle(Position2.X - 400, Position2.Y, 622, 350), new Rectangle(0, 0, 622, 350), Color.LightGray,
                        //roration      origin          effect    layer
                    0.0f, new Vector2(0, 0), myEffect, 0.0f);
                }


            }

            for (int i = 0; i < bullets.Count; i++)
                bullets[i].Draw(spriteBatch);

            if (lookright)
            {
                if (pum)
                {
                    //spriteBatch.Draw(mBlast, new Rectangle((int)(bulletorigin.X) + jynkky, (int)bulletorigin.Y, 49, 25), Color.White);
                    spriteBatch.Draw(mBlast, new Rectangle((int)(bulletorigin.X) + gunShake, (int)bulletorigin.Y, 49, 25), new Rectangle(0, 0, 49, 25), Color.White,
                        //roration      origin          effect    layer
                    (float)aimrotation, new Vector2(50, 0), SpriteEffects.None, 0.0f);
                }

                spriteBatch.Draw(rArm, new Rectangle(Position.X + gunShake + 100, Position.Y + 100, Position.Width, Position.Height), new Rectangle(0, 0, Position.Width, Position.Height), Color.Gray,
                    //roration      origin          effect    layer
                (float)aimrotation, rotationorigin, SpriteEffects.None, 0.0f);


                spriteBatch.Draw(belly, Position, Color.LightGray);

                spriteBatch.Draw(beard, Position, Color.White);
                spriteBatch.Draw(mouth, new Rectangle(Position.X, Position.Y - 2 + naamaVatkaus, Position.Width, Position.Height), Color.White);
                spriteBatch.Draw(lArm, new Rectangle(Position.X + gunShake + 100, Position.Y + 100, Position.Width, Position.Height), new Rectangle(0, 0, Position.Width, Position.Height), Color.White,
                    //roration      origin          effect    layer
                (float)aimrotation, rotationorigin, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(face, Position, Color.White);


                if(immobilized)
                    spriteBatch.Draw(web, Position2, Color.White);

                if (degrees4 > 181)
                    spriteBatch.Draw(mGunBarrel, new Rectangle(Position.X + gunShake + 100 + (valix - 2) + (int)xasd4, Position.Y + (int)asd4 + (valiy - 2) + 100, 622, 350), new Rectangle(0, 0, 622, 350), Color.LightGray,
                        //roration      origin          effect    layer
                    (float)aimrotation, rotationorigin, SpriteEffects.None, 0.0f);
                if (degrees5 > 181)
                    spriteBatch.Draw(mGunBarrel, new Rectangle(Position.X + gunShake + 100 + (valix - 2) + (int)xasd5, Position.Y + (int)asd5 + (valiy - 2) + 100, 622, 350), new Rectangle(0, 0, 622, 350), Color.LightGray,
                        //roration      origin          effect    layer
                    (float)aimrotation, rotationorigin, SpriteEffects.None, 0.0f);
                if (degrees6 > 181)
                    spriteBatch.Draw(mGunBarrel, new Rectangle(Position.X + gunShake + 100 + (valix - 2) + (int)xasd6, Position.Y + (int)asd6 + (valiy - 2) + 100, 622, 350), new Rectangle(0, 0, 622, 350), Color.LightGray,
                        //roration      origin          effect    layer
                    (float)aimrotation, rotationorigin, SpriteEffects.None, 0.0f);
                if (degrees > 181)
                    spriteBatch.Draw(mGunBarrel, new Rectangle(Position.X + gunShake + 100 + (valix - 2) + (int)xasd, Position.Y + (int)asd + (valiy - 2) + 100, 622, 350), new Rectangle(0, 0, 622, 350), Color.LightGray,
                        //roration      origin          effect    layer
                    (float)aimrotation, rotationorigin, SpriteEffects.None, 0.0f);
                if (degrees2 > 181)
                    spriteBatch.Draw(mGunBarrel, new Rectangle(Position.X + gunShake + 100 + (valix - 2) + (int)xasd2, Position.Y + (int)asd2 + (valiy - 2) + 100, 622, 350), new Rectangle(0, 0, 622, 350), Color.LightGray,
                        //roration      origin          effect    layer
                    (float)aimrotation, rotationorigin, SpriteEffects.None, 0.0f);
                if (degrees3 > 181)
                    spriteBatch.Draw(mGunBarrel, new Rectangle(Position.X + gunShake + 100 + (valix - 2) + (int)xasd3, Position.Y + (int)asd3 + (valiy - 2) + 100, 622, 350), new Rectangle(0, 0, 622, 350), Color.LightGray,
                        //roration      origin          effect    layer
                    (float)aimrotation, rotationorigin, SpriteEffects.None, 0.0f);


                if (degrees4 <= 181)
                    spriteBatch.Draw(mGunBarrel, new Rectangle(Position.X + gunShake + 100 + (valix - 2) + (int)xasd4, Position.Y + (int)asd4 + (valiy - 2) + 100, 622, 350), new Rectangle(0, 0, 622, 350), Color.White,
                        //roration      origin          effect    layer
                    (float)aimrotation, rotationorigin, SpriteEffects.None, 0.0f);
                if (degrees5 <= 181)
                    spriteBatch.Draw(mGunBarrel, new Rectangle(Position.X + gunShake + 100 + (valix - 2) + (int)xasd5, Position.Y + (int)asd5 + (valiy - 2) + 100, 622, 350), new Rectangle(0, 0, 622, 350), Color.White,
                        //roration      origin          effect    layer
                    (float)aimrotation, rotationorigin, SpriteEffects.None, 0.0f);
                if (degrees6 <= 181)
                    spriteBatch.Draw(mGunBarrel, new Rectangle(Position.X + gunShake + 100 + (valix - 2) + (int)xasd6, Position.Y + (int)asd6 + (valiy - 2) + 100, 622, 350), new Rectangle(0, 0, 622, 350), Color.White,
                        //roration      origin          effect    layer
                    (float)aimrotation, rotationorigin, SpriteEffects.None, 0.0f);
                if (degrees <= 181)
                    spriteBatch.Draw(mGunBarrel, new Rectangle(Position.X + gunShake + 100 + (valix - 2) + (int)xasd, Position.Y + (int)asd + (valiy - 2) + 100, 622, 350), new Rectangle(0, 0, 622, 350), Color.White,
                        //roration      origin          effect    layer
                   (float)aimrotation, rotationorigin, SpriteEffects.None, 0.0f);
                if (degrees2 <= 181)
                    spriteBatch.Draw(mGunBarrel, new Rectangle(Position.X + gunShake + 100 + (valix - 2) + (int)xasd2, Position.Y + (int)asd2 + (valiy - 2) + 100, 622, 350), new Rectangle(0, 0, 622, 350), Color.White,
                        //roration      origin          effect    layer
                    (float)aimrotation, rotationorigin, SpriteEffects.None, 0.0f);
                if (degrees3 <= 181)
                    spriteBatch.Draw(mGunBarrel, new Rectangle(Position.X + gunShake + 100 + (valix - 2) + (int)xasd3, Position.Y + (int)asd3 + (valiy - 2) + 100, 622, 350), new Rectangle(0, 0, 622, 350), Color.White,
                        //roration      origin          effect    layer
                    (float)aimrotation, rotationorigin, SpriteEffects.None, 0.0f);

                /******************************************************/

                spriteBatch.Draw(mGun, new Rectangle(Position.X + gunShake + 100, Position.Y + 100, Position.Width, Position.Height), new Rectangle(0, 0, Position.Width, Position.Height), Color.White,
                    //roration      origin          effect    layer
                (float)aimrotation, rotationorigin, SpriteEffects.None, 0.0f);

                // spriteBatch.Draw(mGun, new Rectangle(Position.X + jynkky, Position.Y, Position.Width, Position.Height), Color.White);
            }
            else
            {
                if (pum)
                {
                    //spriteBatch.Draw(mBlast, new Rectangle((int)(bulletorigin.X) + jynkky, (int)bulletorigin.Y, 49, 25), Color.White);
                    spriteBatch.Draw(mBlast, new Rectangle((int)(flippedorigin.X) + gunShake, (int)flippedorigin.Y, 49, 25), new Rectangle(0, 0, 49, 25), Color.White,
                        //roration      origin          effect    layer
                    (float)aimrotation, new Vector2(-20, 0), myEffect, 0.0f);
                }

                spriteBatch.Draw(rArm, new Rectangle(Position.X + gunShake + 122, Position.Y + 100, Position.Width, Position.Height), new Rectangle(0, 0, Position.Width, Position.Height), Color.Gray,
                    //roration      origin          effect    layer
                (float)aimrotation, new Vector2(522, 100), myEffect, 0.0f);
                spriteBatch.Draw(belly, new Rectangle(Position.X - 400, Position.Y, 622, 350), new Rectangle(0, 0, 622, 350), Color.LightGray,
                    //roration      origin          effect    layer
                0.0f, new Vector2(0, 0), myEffect, 0.0f);
                spriteBatch.Draw(beard, new Rectangle(Position.X - 400, Position.Y, 622, 350), new Rectangle(0, 0, 622, 350), Color.White,
                    //roration      origin          effect    layer
                0.0f, new Vector2(0, 0), myEffect, 0.0f);
                spriteBatch.Draw(mouth, new Rectangle(Position.X - 400, Position.Y - 2 + naamaVatkaus, 622, 350), new Rectangle(0, 0, 622, 350), Color.White,
                    //roration      origin          effect    layer
                0.0f, new Vector2(0, 0), myEffect, 0.0f);
                spriteBatch.Draw(lArm, new Rectangle(Position.X + gunShake + 122, Position.Y + 100, Position.Width, Position.Height), new Rectangle(0, 0, Position.Width, Position.Height), Color.White,
                    //roration      origin          effect    layer
                (float)aimrotation, new Vector2(522, 100), myEffect, 0.0f);
                spriteBatch.Draw(face, new Rectangle(Position.X - 400, Position.Y, 622, 350), new Rectangle(0, 0, 622, 350), Color.White,
                    //roration      origin          effect    layer
                0.0f, new Vector2(0, 0), myEffect, 0.0f);


                if (immobilized)
                    spriteBatch.Draw(web, new Rectangle(Position2.X - 400, Position2.Y, Position2.Width, Position2.Height), null, Color.White, 0.0f, Vector2.Zero, myEffect, 0.0f); 

                if (degrees4 > 181)
                    spriteBatch.Draw(mGunBarrel, new Rectangle(Position.X + gunShake + 122 + (valix - 2) + (int)xasd4, Position.Y + (int)asd4 + (valiy - 2) + 100, 622, 350), new Rectangle(0, 0, 622, 350), Color.LightGray,
                        //roration      origin          effect    layer
                    (float)aimrotation, new Vector2(522, 100), myEffect, 0.0f);
                if (degrees5 > 181)
                    spriteBatch.Draw(mGunBarrel, new Rectangle(Position.X + gunShake + 122 + (valix - 2) + (int)xasd5, Position.Y + (int)asd5 + (valiy - 2) + 100, 622, 350), new Rectangle(0, 0, 622, 350), Color.LightGray,
                        //roration      origin          effect    layer
                    (float)aimrotation, new Vector2(522, 100), myEffect, 0.0f);
                if (degrees6 > 181)
                    spriteBatch.Draw(mGunBarrel, new Rectangle(Position.X + gunShake + 122 + (valix - 2) + (int)xasd6, Position.Y + (int)asd6 + (valiy - 2) + 100, 622, 350), new Rectangle(0, 0, 622, 350), Color.LightGray,
                        //roration      origin          effect    layer
                    (float)aimrotation, new Vector2(522, 100), myEffect, 0.0f);
                if (degrees > 181)
                    spriteBatch.Draw(mGunBarrel, new Rectangle(Position.X + gunShake + 122 + (valix - 2) + (int)xasd, Position.Y + (int)asd + (valiy - 2) + 100, 622, 350), new Rectangle(0, 0, 622, 350), Color.LightGray,
                        //roration      origin          effect    layer
                    (float)aimrotation, new Vector2(522, 100), myEffect, 0.0f);
                if (degrees2 > 181)
                    spriteBatch.Draw(mGunBarrel, new Rectangle(Position.X + gunShake + 122 + (valix - 2) + (int)xasd2, Position.Y + (int)asd2 + (valiy - 2) + 100, 622, 350), new Rectangle(0, 0, 622, 350), Color.LightGray,
                        //roration      origin          effect    layer
                    (float)aimrotation, new Vector2(522, 100), myEffect, 0.0f);
                if (degrees3 > 181)
                    spriteBatch.Draw(mGunBarrel, new Rectangle(Position.X + gunShake + 122 + (valix - 2) + (int)xasd3, Position.Y + (int)asd3 + (valiy - 2) + 100, 622, 350), new Rectangle(0, 0, 622, 350), Color.LightGray,
                        //roration      origin          effect    layer
                    (float)aimrotation, new Vector2(522, 100), myEffect, 0.0f);

                if (degrees4 <= 181)
                    spriteBatch.Draw(mGunBarrel, new Rectangle(Position.X + gunShake + 122 + (valix - 2) + (int)xasd4, Position.Y + (int)asd4 + (valiy - 2) + 100, 622, 350), new Rectangle(0, 0, 622, 350), Color.White,
                        //roration      origin          effect    layer
                    (float)aimrotation, new Vector2(522, 100), myEffect, 0.0f);
                if (degrees5 <= 181)
                    spriteBatch.Draw(mGunBarrel, new Rectangle(Position.X + gunShake + 122 + (valix - 2) + (int)xasd5, Position.Y + (int)asd5 + (valiy - 2) + 100, 622, 350), new Rectangle(0, 0, 622, 350), Color.White,
                        //roration      origin          effect    layer
                    (float)aimrotation, new Vector2(522, 100), myEffect, 0.0f);
                if (degrees6 <= 181)
                    spriteBatch.Draw(mGunBarrel, new Rectangle(Position.X + gunShake + 122 + (valix - 2) + (int)xasd6, Position.Y + (int)asd6 + (valiy - 2) + 100, 622, 350), new Rectangle(0, 0, 622, 350), Color.White,
                        //roration      origin          effect    layer
                    (float)aimrotation, new Vector2(522, 100), myEffect, 0.0f);
                if (degrees <= 181)
                    spriteBatch.Draw(mGunBarrel, new Rectangle(Position.X + gunShake + 122 + (valix - 2) + (int)xasd, Position.Y + (int)asd + (valiy - 2) + 100, 622, 350), new Rectangle(0, 0, 622, 350), Color.White,
                        //roration      origin          effect    layer
                   (float)aimrotation, new Vector2(522, 100), myEffect, 0.0f);
                if (degrees2 <= 181)
                    spriteBatch.Draw(mGunBarrel, new Rectangle(Position.X + gunShake + 122 + (valix - 2) + (int)xasd2, Position.Y + (int)asd2 + (valiy - 2) + 100, 622, 350), new Rectangle(0, 0, 622, 350), Color.White,
                        //roration      origin          effect    layer
                    (float)aimrotation, new Vector2(522, 100), myEffect, 0.0f);
                if (degrees3 <= 181)
                    spriteBatch.Draw(mGunBarrel, new Rectangle(Position.X + gunShake + 122 + (valix - 2) + (int)xasd3, Position.Y + (int)asd3 + (valiy - 2) + 100, 622, 350), new Rectangle(0, 0, 622, 350), Color.White,
                        //roration      origin          effect    layer
                    (float)aimrotation, new Vector2(522, 100), myEffect, 0.0f);

                spriteBatch.Draw(mGun, new Rectangle(Position.X + gunShake + 122, Position.Y + 100, Position.Width, Position.Height), new Rectangle(0, 0, Position.Width, Position.Height), Color.White,
                    //roration      origin          effect    layer
                (float)aimrotation, new Vector2(522, 100), myEffect, 0.0f);
            }

            //spriteBatch.Draw(crosshair, crosshairbox, Color.White);
            spriteBatch.Draw(crosshair, new Rectangle(crosshairbox.X + 24, crosshairbox.Y + 125,(int)scaleCrossHairX,(int)scaleCrosshairY) , null, Color.White, rotateCrossHair, new Vector2(24, 125), SpriteEffects.None, 1.0f);
        }
    }
}

