using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MinigunViking
{
    class Enemy
    {
        //Textures to build the enemy
        public Texture2D enemyBody;
        public Texture2D enemyLeftArm;
        public Texture2D enemyRightArm;
        private Texture2D enemyRightLeg;
        private Texture2D enemyLeftLeg;
        //NOT YET IMPLEMENTED
        //private Texture2D enemyHead;
        //private Texture2D enemyLeftFoot;
        //private Texture2D enemyRightFoot;

        private Texture2D shadow;
        public Texture2D Bloodtext;
        public Texture2D pixel;
        private Texture2D groundblood;

        //Position rectangles for enemy bodyparts
        public Rectangle bodyPos;
        public Rectangle leftArmPos;
        public Rectangle rightArmPos;
        public Rectangle leftLegPos;
        public Rectangle rightLegPos;

        //Rotation for arms
        public float Rotation = 0;
        public float Rotation2 = 0;
        public float attackRotation = 0;
        Boolean rot1;
        Boolean rot2;

        public Rectangle fullhealth;
        public Rectangle currenthealth;

        public bool Active;
        public bool Dead;
        public int Damage;
        public int health;
        public int drawblood = 0;
        public float bloodtimer = 0.5f;
        public List<Bloodspatter> bloodspatters;
        public double angle;
        public bool rewarded = false;
        Viewport viewport;
        Random random = new Random();

        private List<Bullet> bullets;

        private bool hit = false;
        private float hittimer = 50f;

        private bool jumping = false;
        private bool falling = false;
        private int groundLevel = 0;
        private bool attacking = false;
        private bool hitting = false;

        private Texture2D bloodDrop;

        public Boolean enemyDirection;



        public int Width
        {
            get { return enemyBody.Width; }
        }
        public int Height
        {
            get { return enemyBody.Height; }
        }

        float enemyMoveSpeed;


        private void addSpatter(Rectangle initialPosition)
        {

            for (int i = 0; i < 150; i++)
            {
                int kulma = random.Next(0, 360);
                double nopeus = random.Next(0, 250);
                Color color = new Color(random.Next(70, 255), 0, 0);
                Bloodspatter spat = new Bloodspatter();
                int stopheight = random.Next(940, 1040);
                //spat.Initialize(pixel, initialPosition, kulma, nopeus, color);
                spat.Initialize(bloodDrop, initialPosition, kulma, nopeus, color, groundblood, stopheight);
                bloodspatters.Add(spat);
            }
        }


        public void Initialize(Texture2D blood, Rectangle position, GraphicsDevice g)
        {
            Bloodtext = blood;
            bodyPos = position;

            groundLevel = position.Y;

            bloodspatters = new List<Bloodspatter>();

            fullhealth = new Rectangle(position.X + 60, position.Y - 5, 50, 3);
            currenthealth = new Rectangle(position.X + 60, position.Y - 5, 50, 3);

            this.viewport = g.Viewport;

            Active = true;
            Dead = false;
            rewarded = false;

            Damage = 2;
            health = 100;

            enemyMoveSpeed = 300;

            pixel = new Texture2D(g, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });

        }

        public void LoadContent(ContentManager content)
        {
            this.bloodDrop = content.Load<Texture2D>("Textures\\veritesti");
            this.enemyBody = content.Load<Texture2D>("Enemy\\enemyBody");
            this.enemyLeftArm = content.Load<Texture2D>("Enemy\\enemyLeftArm");
            this.enemyRightArm = content.Load<Texture2D>("Enemy\\enemyRightArm");
            this.enemyLeftLeg = content.Load<Texture2D>("Enemy\\enemyLeftLeg");
            this.enemyRightLeg = content.Load<Texture2D>("Enemy\\enemyRightLeg");
            this.groundblood = content.Load<Texture2D>("Textures\\groundBlood");

            this.shadow = content.Load<Texture2D>("Textures\\shadow");
        }

        public void Update(Viking viking, GameTime time, int modifySpeed)
        {
            if (bloodspatters.Count > 0)
            {
                drawblood = 1;
                for (int b = 0; b < bloodspatters.Count; b++)
                {
                    bloodspatters[b].Update(time, modifySpeed);
                    if (!bloodspatters[b].Active)
                    {
                        bloodspatters.RemoveAt(b);
                    }
                }
            }
            else
            {
                drawblood = 0;
            }

            if (!Dead)
            {
                currenthealth.Width = (int)(0.5 * health);

                if (hit)
                {
                    hittimer -= (float)time.ElapsedGameTime.TotalMilliseconds;

                    if (hittimer < 0)
                    {
                        this.hit = false;
                        this.hittimer = 50f;
                    }
                }

                if (attacking)
                {
                    if (hitting)
                    {
                        bodyPos.X -= modifySpeed;
                       attackRotation += MathHelper.ToRadians(25);

                       if (!(bodyPos.X - viking.Position.X < 150 && bodyPos.X - viking.Position.X > -120))
                       {
                           this.hitting = false;
                           attackRotation = 0;
                       }

                        
                    }
                    else
                    {
                        if (bodyPos.X - viking.Position.X < 150 && bodyPos.X - viking.Position.X > -120)
                        {
                            bodyPos.X -= modifySpeed;
                            this.hitting = true;
                        }
                        else
                        {
                            if (bodyPos.X > viking.Position.X)
                            {
                                bodyPos.X -= (int)(enemyMoveSpeed * time.ElapsedGameTime.TotalSeconds) + modifySpeed;
                                enemyDirection = false;
                            }
                            else
                            {
                                bodyPos.X += (int)(enemyMoveSpeed * time.ElapsedGameTime.TotalSeconds) - modifySpeed;
                                enemyDirection = true;
                            }
                        }
                    }
                }
                else
                {
                    bodyPos.X -= modifySpeed;
                    if (bodyPos.X < 2000)
                    {
                        attacking = true;
                    }
                }
                currenthealth.X = bodyPos.X + 60;
                fullhealth.X = bodyPos.X + 60;


                bullets = viking.getBullets();

                for (int i = 0; i < bullets.Count; i++)
                {
                    if (bullets[i].Position.Intersects(bodyPos) || bullets[i].Position.Intersects(leftLegPos) || bullets[i].Position.Intersects(rightLegPos))
                    {

                        if (bullets[i].Position.Intersects(leftLegPos) || bullets[i].Position.Intersects(rightLegPos))
                        {
                            if (!this.jumping)
                            {
                                this.falling = false;
                                this.jumping = true;
                            }
                        }

                        this.health -= bullets[i].Damage;

                        angle = bullets[i].angle;

                        addSpatter(bullets[i].Position);

                        bullets[i].Active = false;

                        if (!hit)
                        {
                            hit = true;
                        }

                        if (enemyDirection)
                        {
                            bodyPos.X -= 5;
                        }
                        else
                        {
                            bodyPos.X += 5;
                        }
                    }
                }
            }

            if (this.health <= 0)
            {
                this.Dead = true;
            }
            if (Dead && drawblood == 0)
            {
                this.Active = false;
            }

            //Positions for other bodyparts
            leftArmPos = new Rectangle(bodyPos.X + 136, bodyPos.Y + 80, 59, 148);
            rightArmPos = new Rectangle(bodyPos.X + 34, bodyPos.Y + 80, 51, 150);
            leftLegPos = new Rectangle(bodyPos.X + 115, bodyPos.Y + 160, 104, 157);
            rightLegPos = new Rectangle(bodyPos.X + 55, bodyPos.Y + 170, 116, 158);


            //Arm rotation
            if (!rot1)
            {
                Rotation = Rotation + MathHelper.ToRadians(4);
                if (MathHelper.ToDegrees(Rotation) >= 45)
                    rot1 = true;
            }
            else if (rot1)
            {
                Rotation = Rotation - MathHelper.ToRadians(4);
                if (MathHelper.ToDegrees(Rotation) <= -45)
                    rot1 = false;
            }

            if (rot2)
            {
                Rotation2 = Rotation2 + MathHelper.ToRadians(4);
                if (MathHelper.ToDegrees(Rotation2) >= 45)
                    rot2 = false;
            }
            else if (!rot2)
            {
                Rotation2 = Rotation2 - MathHelper.ToRadians(4);
                if (MathHelper.ToDegrees(Rotation2) <= -45)
                    rot2 = true;
            }


            if (jumping && bodyPos.Y > 517 && !falling)
            {
                if (bodyPos.Y > groundLevel - 150)
                {
                    bodyPos.Y -= 10;
                }
                else if (bodyPos.Y > groundLevel - 200 && bodyPos.Y <= groundLevel - 150)
                {
                    bodyPos.Y -= 6;
                }
            }
            else if (bodyPos.Y < groundLevel)
            {
                falling = true;
                if (bodyPos.Y < groundLevel - 100)
                {
                    bodyPos.Y += 6;
                }
                else if (bodyPos.Y > groundLevel - 100 && bodyPos.Y < groundLevel)
                {
                    bodyPos.Y += 9;
                }
            }
            else
            {
                jumping = false;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            {
                if (!Dead)
                {
                    spriteBatch.Draw(shadow, new Rectangle(bodyPos.X - 20, 985, 250, 50), Color.White);

                    if (!enemyDirection)
                    {

                        if (!hit)
                        {
                            spriteBatch.Draw(enemyRightArm, rightArmPos, null, Color.White, Rotation2, new Vector2(27, 21), SpriteEffects.None, 0.0f);
                            spriteBatch.Draw(enemyRightLeg, rightLegPos, null, Color.White, Rotation, new Vector2(77, 32), SpriteEffects.None, 0.0f);
                            spriteBatch.Draw(enemyLeftLeg, leftLegPos, null, Color.White, Rotation2, new Vector2(59, 22), SpriteEffects.None, 0.0f);
                            spriteBatch.Draw(enemyBody, bodyPos, Color.White);
                            spriteBatch.Draw(enemyLeftArm, leftArmPos, null, Color.White, Rotation + attackRotation, new Vector2(29, 20), SpriteEffects.None, 0.0f);
                        }
                        else
                        {
                            spriteBatch.Draw(enemyRightArm, rightArmPos, null, Color.Brown, Rotation2, new Vector2(27, 21), SpriteEffects.None, 0.0f);
                            spriteBatch.Draw(enemyRightLeg, rightLegPos, null, Color.Brown, Rotation, new Vector2(77, 32), SpriteEffects.None, 0.0f);
                            spriteBatch.Draw(enemyLeftLeg, leftLegPos, null, Color.Brown, Rotation2, new Vector2(59, 22), SpriteEffects.None, 0.0f);
                            spriteBatch.Draw(enemyBody, bodyPos, Color.Brown);
                            spriteBatch.Draw(enemyLeftArm, leftArmPos, null, Color.Brown, Rotation + attackRotation, new Vector2(29, 20), SpriteEffects.None, 0.0f);

                        }
                    }
                    else
                    {
                        if (!hit)
                        {
                            spriteBatch.Draw(enemyRightArm, new Rectangle(rightArmPos.X+100,rightArmPos.Y,rightArmPos.Width,rightArmPos.Height), null, Color.White, Rotation2, new Vector2(23, 21), SpriteEffects.FlipHorizontally, 0.0f);
                            spriteBatch.Draw(enemyRightLeg, new Rectangle(rightLegPos.X+58,rightLegPos.Y,rightLegPos.Width,rightLegPos.Height), null, Color.White, Rotation, new Vector2(38, 32), SpriteEffects.FlipHorizontally, 0.0f);
                            spriteBatch.Draw(enemyLeftLeg, new Rectangle(leftLegPos.X-62,leftLegPos.Y,leftLegPos.Width,leftLegPos.Height), null, Color.White, Rotation2, new Vector2(45, 22), SpriteEffects.FlipHorizontally, 0.0f);
                            spriteBatch.Draw(enemyBody, bodyPos, null, Color.White, 0, new Vector2(0,0), SpriteEffects.FlipHorizontally, 0.0f);
                            spriteBatch.Draw(enemyLeftArm, new Rectangle(leftArmPos.X-104,leftArmPos.Y,leftArmPos.Width,leftArmPos.Height), null, Color.White, Rotation + attackRotation, new Vector2(29, 20), SpriteEffects.FlipHorizontally, 0.0f);
                        }
                        else
                        {
                            spriteBatch.Draw(enemyRightArm, new Rectangle(rightArmPos.X+100, rightArmPos.Y, rightArmPos.Width, rightArmPos.Height), null, Color.Brown, Rotation2, new Vector2(23, 21), SpriteEffects.FlipHorizontally, 0.0f);
                            spriteBatch.Draw(enemyRightLeg, new Rectangle(rightLegPos.X+58, rightLegPos.Y, rightLegPos.Width, rightLegPos.Height), null, Color.Brown, Rotation, new Vector2(38, 32), SpriteEffects.FlipHorizontally, 0.0f);
                            spriteBatch.Draw(enemyLeftLeg, new Rectangle(leftLegPos.X-62, leftLegPos.Y, leftLegPos.Width, leftLegPos.Height), null, Color.Brown, Rotation2, new Vector2(45, 22), SpriteEffects.FlipHorizontally, 0.0f);
                            spriteBatch.Draw(enemyBody, bodyPos, null, Color.Brown, 0, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0.0f);
                            spriteBatch.Draw(enemyLeftArm, new Rectangle(leftArmPos.X-104, leftArmPos.Y, leftArmPos.Width, leftArmPos.Height), null, Color.Brown, Rotation + attackRotation, new Vector2(29, 20), SpriteEffects.FlipHorizontally, 0.0f);

                        }
                    }

                    spriteBatch.Draw(pixel, fullhealth, Color.Brown);
                    spriteBatch.Draw(pixel, currenthealth, Color.Green);
                }
                if (drawblood == 1)
                {
                    for (int x1 = 0; x1 < bloodspatters.Count; x1++)
                    {
                        bloodspatters[x1].Draw(spriteBatch);
                    }
                }
            }

        }
    }
}
