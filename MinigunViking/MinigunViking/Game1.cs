using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading;

namespace MinigunViking
{

    /// This is the main type for your game
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Viking viking = new Viking();
        HealthBar health = new HealthBar();
        ExperienceBar experience = new ExperienceBar();
        StartMenu menu = new StartMenu();
        GameOver theend = new GameOver();
        LevelReader levelReader = new LevelReader();
        Goal goal = new Goal();

        public int gamestate = 0;

        //0 = menu
        //1 = running
        //2 = gameover
        //3 = finished

        public int levelLength;
        public int levelLoc;

        public Rectangle Position;
        public Rectangle Position2;

        public Rectangle talo;
        Random random = new Random();

        public Rectangle spinnaus;

        public Rectangle tausta1;
        public Rectangle tausta2;
        public Rectangle tausta3;

        public Rectangle groundBlock;
        public Rectangle treeRec;       

        public float elapsed;

        public bool paused = false;

        private float waittimer = 1f;
        private float timerstuff = 0;

        public int GroundLevel;

        //Pictures for background etc
        private Texture2D taustaKuva;
        private Texture2D walkSurface;
        private Texture2D start1;
        private Texture2D start2;
        private Texture2D start3;

        private Texture2D tree;
        private Texture2D house;
        private Texture2D blood;
        private Texture2D estexture;

        private string[] leveldet;

        public Point mousePos;

        private Texture2D filter;
        private SpriteFont fontti;
        private SpriteFont fontti2;
        private int killcount = 0;

        public int speedModifier = 0;
        public int spiderSpeedModifier = 0;

        List<Enemy> enemies;
        List<Powerup> powerups;
        List<Spider> spiders;
        List<EnemySpawner> spawners;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Resolution.Init(ref graphics);

            Resolution.SetVirtualResolution(1920, 1080);
            Resolution.SetResolution(800, 600, false);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //this.IsMouseVisible = true;            

            talo = new Rectangle(200, 150, 1331, 789);
            spinnaus = new Rectangle(50, 50, 100, 100);

            //taustat textuurin kokosiks
            tausta1 = new Rectangle(0, 0, 1920, 1080);
            tausta2 = new Rectangle(1920, 0, 1920, 1080);
            tausta3 = new Rectangle(-1920, 0, 1920, 1080);

            enemies = new List<Enemy>();
            powerups = new List<Powerup>();
            spiders = new List<Spider>();
            spawners = new List<EnemySpawner>();

            groundBlock = new Rectangle(0, 1080 - 268, 50, 268);
            treeRec = new Rectangle(0, 0, 1858, 1080);

            viking.Initialize(GraphicsDevice);
            health.Initialize(GraphicsDevice, viking);
            experience.Initialize(GraphicsDevice, GraphicsDevice.Viewport);
            menu.Initialize(GraphicsDevice);
            theend.Initialize(GraphicsDevice);

            
            leveldet = levelReader.readLevel("level1");

            base.Initialize();

            addEnemy();
            createSpawners();
            createGoal();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Ladataan kuvia yms
            this.taustaKuva = Content.Load<Texture2D>("Textures\\vuoria");

            this.walkSurface = Content.Load<Texture2D>("Textures\\weed");
            this.start1 = Content.Load<Texture2D>("Textures\\start_01");
            this.start2 = Content.Load<Texture2D>("Textures\\start_02");
            this.start3 = Content.Load<Texture2D>("Textures\\start_03");

            this.house = Content.Load<Texture2D>("Textures\\house");
            this.tree = Content.Load<Texture2D>("Textures\\ebinTree2");

            this.fontti = Content.Load<SpriteFont>("Fonts\\fontti1");
            this.fontti2 = Content.Load<SpriteFont>("Fonts\\fontti2");

            //Loading content for enemy?

            //Red "filter" for game-over screen
            this.filter = Content.Load<Texture2D>("Textures\\filterTest");
            this.estexture = Content.Load<Texture2D>("Textures\\es");

            //Load Content for main Character
            viking.LoadContent(Content, GraphicsDevice);
            //Load Content for menus
            menu.LoadContent(Content);
            //Load Content for dying/game over
            theend.LoadContent(Content);
            //Load healthbar textures
            health.LoadContent(Content);

            goal.LoadContent(Content);
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        private void createGoal()
        {
            for (int i = 0; i < leveldet[4].Length; i++)
            {
                if (leveldet[5][i].Equals('F'))
                {
                    goal.Initialize(new Rectangle((i*50) - 10,1080-600,450,600));
                }
            }
        }

        private void addSpider(Vector2 spiderlocation)
        {
            Spider spider = new Spider();
            spider.Initialize(spiderlocation, GraphicsDevice);
            spider.LoadContent(Content);
            spiders.Add(spider);
        }

        private void updateSpiders(GameTime time)
        {
            for (int i = 0; i < spiders.Count; i++)
            {
                spiders[i].Update(time, spiderSpeedModifier);
                if (viking.collidesWith(spiders[i].activatepos))
                {
                    spiders[i].attack = true;
                }
                if (spiders[i].attack && viking.collidesWith(spiders[i].position) && !viking.isDamaged())
                {
                    viking.damage();
                    viking.immobilized = true;
                    viking.isDamaged(true);
                }
                if (!spiders[i].active)
                {
                    spiders.RemoveAt(i);
                }

            }
        }

        public void createSpawners()
        {
            for (int i = 0; i < leveldet[4].Length; i++)
            {
                if (leveldet[4][i].Equals('S'))
                {
                    EnemySpawner spawner = new EnemySpawner();
                    spawner.initialize(new Rectangle(i*50,120,1211,904));
                    spawner.LoadContent(Content);
                    spawners.Add(spawner);
                }
            }
        }
        public void updateSpawners(GameTime time)
        {
            for (int i = 0; i < spawners.Count; i++)
            {
                if (spawners[i].Update(speedModifier, time))
                {
                    spawnEnemy(new Rectangle(spawners[i].position.X+470, 717, 169, 195));
                }
            }
        }


        public void spawnEnemy(Rectangle enemypos) //This is used to dynamically add more enemies during the gameplay ie. from spawners
        {
            Enemy enemy = new Enemy();
            enemy.Initialize(blood, enemypos, GraphicsDevice);
            enemy.LoadContent(Content);
            enemies.Add(enemy);
        }

        private void addEnemy() //This is used to add preconfigured enemies in the start of the game
        {
            //int randomSpawn = random.Next(1920, 2500);

            for (   int i = 0; i < leveldet[0].Length; i++)
            {
                if (leveldet[0][i].Equals(':'))
                {
                    //Do nothing
                }
                else if (leveldet[0][i].Equals('E'))
                {
                    Enemy enemy = new Enemy();
                    Rectangle position = new Rectangle(i * 50, 717, 169, 195);

                    enemy.Initialize(blood, position, GraphicsDevice);
                    enemy.LoadContent(Content);
                    enemies.Add(enemy);
                }

            }



            for (int n = 0; n < leveldet[1].Length; n++)
            {
                if (leveldet[1][n].Equals('P') || leveldet[2][n].Equals('P') || leveldet[3][n].Equals('P'))
                {
                    int spiderposmod = random.Next(300, 1000);

                    int spawnSpider = random.Next(0, 10);
                    if (spawnSpider < 3)
                    {
                        addSpider(new Vector2(n * 50 + spiderposmod, 150));
                    }
                }
            }



        }

        private void updateEnemies(GameTime time)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Update(viking, time, speedModifier);
                if (!enemies[i].Dead && viking.collidesWith(enemies[i].bodyPos) && !viking.isDamaged())
                {
                    viking.damage();
                    viking.isDamaged(true);
                }
                if (enemies[i].Dead && !enemies[i].rewarded)
                {
                    int givepower = random.Next(0, 10);
                    if (givepower < 3)
                    {
                        Powerup powerup = new Powerup();
                        Rectangle powerpos = new Rectangle(enemies[i].bodyPos.X + 15, 850 - 50, 85, 117);
                        int eskulma = random.Next(75, 115);
                        double esforce = random.Next(200, 400);
                        powerup.Initialize(powerpos, estexture, eskulma, esforce);
                        powerups.Add(powerup);
                    }
                    enemies[i].rewarded = true;
                    killcount++;
                    viking.giveExp(10);
                }
                if (enemies[i].Active == false)
                {

                    enemies.RemoveAt(i);
                }
            }
        }

        private void updatePowerups(GameTime time)
        {
            for (int i = 0; i < powerups.Count; i++)
            {
                powerups[i].Update(speedModifier, time);
                if (viking.collidesWith(powerups[i].position))
                {
                    viking.heal();
                    powerups[i].active = false;
                }

                if (!powerups[i].active)
                {
                    powerups.RemoveAt(i);
                }
            }
        }



        protected override void Update(GameTime gameTime)
        {

            KeyboardState keystate = Keyboard.GetState();
            MouseState mousestate = Mouse.GetState();

            viking.setFinish(goal.FinishVisible);

            mousePos = Resolution.MouseHelper.CurrentMousePosition;
            if (gamestate == 0)
            {
                //update menu screen
                gamestate = menu.Update(mousestate, mousePos, keystate, gameTime, paused);
                if (!paused)
                {
                    viking.Initialize(GraphicsDevice);
                    killcount = 0;
                }
                viking.setVolume(menu.getMasterVolume(), menu.getGunVolume(), menu.getMusicVolume(), menu.getOtherVolume());

            }
            else if (gamestate == 1)
            {                
                paused = false;

                if (viking.musicIsPaused())
                    viking.resumeMusic();

                if (viking.dead())
                {
                    gamestate = 2;
                }

                updateEnemies(gameTime);
                updatePowerups(gameTime);
                updateSpiders(gameTime);
                updateSpawners(gameTime);

                goal.Update(gameTime, viking, speedModifier);

                timerstuff += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (timerstuff > waittimer)
                {
                    //viking.giveExp(50); // uncomment to give constant exp
                    //viking.damage(); // uncomment to give constant damage
                    timerstuff = 0;
                }


                if (viking.scrollLeft && groundBlock.X < 0)
                {
                    tausta1.X += (int)(250 * elapsed);
                    tausta2.X += (int)(250 * elapsed);
                    tausta3.X += (int)(250 * elapsed);

                    groundBlock.X += (int)(500 * elapsed);

                    treeRec.X += (int)(400 * elapsed);

                    speedModifier = -(int)(500 * elapsed);
                    spiderSpeedModifier = -(int)(400 * elapsed);

                    levelLoc -= (int)(500 * elapsed);

                }
                else if (!viking.scrollRight)
                {
                    speedModifier = 0;
                    spiderSpeedModifier = 0;
                }

                if (viking.scrollRight && groundBlock.X > -levelLength)
                {
                    tausta1.X -= (int)(250 * elapsed);
                    tausta2.X -= (int)(250 * elapsed);
                    tausta3.X -= (int)(250 * elapsed);

                    groundBlock.X -= (int)(500 * elapsed);

                    treeRec.X -= (int)(400 * elapsed);

                    speedModifier = (int)(500 * elapsed);
                    spiderSpeedModifier = (int)(400 * elapsed);

                    levelLoc += (int)(500 * elapsed);
                }
                else if (!viking.scrollLeft)
                {
                    speedModifier = 0;
                    spiderSpeedModifier = 0;
                }

                //Tausta1 lähtee 0, tausta 2 lähtee 1920, tausta 3 lähtee -1920
                if (tausta1.X < -1920 && viking.walkRight)
                    tausta3.X = tausta2.X + 1920;

                if (tausta2.X < -1920 && viking.walkRight)
                    tausta1.X = tausta3.X + 1920;

                if (tausta3.X < -1920 && viking.walkRight)
                    tausta2.X = tausta1.X + 1920;

                //toisee suuntaa
                if (tausta1.X > 1920 && !viking.walkRight)
                    tausta2.X = tausta3.X - 1920;

                if (tausta2.X > 1920 && !viking.walkRight)
                    tausta3.X = tausta1.X - 1920;

                if (tausta3.X > 1920 && !viking.walkRight)
                    tausta1.X = tausta2.X - 1920;

                elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;


                viking.Update(gameTime, elapsed, keystate, mousestate, mousePos);
                health.Update(viking);
                experience.Update(viking);

            }
            else if (gamestate == 2)
            {
                gamestate = theend.Update(gameTime);
            }

            if (keystate.IsKeyDown(Keys.Escape))
            {
                if (gamestate == 1)
                {
                    gamestate = 0;
                    paused = true;
                    viking.pauseMusic();
                }

            }
            if (menu.menuExit())
                this.Exit();

            base.Update(gameTime);
        }


        public void floor()
        {


            for (int n = 0; n < leveldet[1].Length; n++)
            {
                if (leveldet[1][n].Equals('P'))
                {
                    spriteBatch.Draw(tree, new Rectangle(treeRec.X + n * 50, treeRec.Y - 75, treeRec.Width, treeRec.Height), Color.White);

                }
            }

            for (int n = 0; n < leveldet[2].Length; n++)
            {
                if (leveldet[2][n].Equals('P'))
                {
                    spriteBatch.Draw(tree, new Rectangle(treeRec.X + n * 50, treeRec.Y - 50, treeRec.Width, treeRec.Height), Color.White);
                }
            }

            for (int n = 0; n < leveldet[3].Length; n++)
            {  
                if (leveldet[3][n].Equals('P'))
                {
                    spriteBatch.Draw(tree, new Rectangle(treeRec.X + n * 50, treeRec.Y, treeRec.Width, treeRec.Height), Color.White);
                }
            }


            for (int i = levelLoc / 50; i < leveldet[5].Length; i++)
            {
                if (groundBlock.X + i * 50 < 2000)
                {
                    if (leveldet[5][i].Equals('T'))
                        spriteBatch.Draw(walkSurface, new Rectangle(groundBlock.X + i * 50, groundBlock.Y, groundBlock.Width, groundBlock.Height), Color.White);
                    else if (leveldet[5][i].Equals('1'))
                        spriteBatch.Draw(start1, new Rectangle(groundBlock.X + i * 50, 680, groundBlock.Width, 400), Color.White);
                    else if (leveldet[5][i].Equals('2'))
                        spriteBatch.Draw(start2, new Rectangle(groundBlock.X + i * 50, 680, groundBlock.Width, 400), Color.White);
                    else if (leveldet[5][i].Equals('3'))
                        spriteBatch.Draw(start3, new Rectangle(groundBlock.X + i * 50, 680, groundBlock.Width, 400), Color.White);


                    levelLength = leveldet[5].Length * 50;
                }
            }




        }



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            Resolution.BeginDraw();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());
            //spriteBatch.Begin();

            // TODO: Add your drawing code here
            if (gamestate == 0)
            {
                //draw menu screen
                menu.Draw(spriteBatch, fontti);
            }
            else if (gamestate == 1)
            {
                //Omaa säätöä
                //spriteBatch.Draw(taustaKuva, tausta1, Color.White);
                //spriteBatch.Draw(taustaKuva, tausta2, Color.White);
                spriteBatch.Draw(taustaKuva, new Rectangle((int)(tausta1.X), 0, 1920, 1080), Color.Gray);
                spriteBatch.Draw(taustaKuva, new Rectangle((int)(tausta2.X), 0, 1920, 1080), Color.Gray);
                spriteBatch.Draw(taustaKuva, new Rectangle((int)(tausta3.X), 0, 1920, 1080), Color.Gray);
                /* spriteBatch.Draw(tree, new Rectangle(tausta1.X + 0, 0, 1858, 1080), Color.White);
                 spriteBatch.Draw(tree, new Rectangle(tausta1.X + 600, -75, 1858, 1080), Color.LightGray);
                 spriteBatch.Draw(tree, new Rectangle(tausta1.X + 1800, -50, 1858, 1080), Color.White);
                 spriteBatch.Draw(tree, new Rectangle(tausta2.X + 600, 0, 1858, 1080), Color.LightGray);
                 spriteBatch.Draw(tree, new Rectangle(tausta2.X + 1800, -25, 1858, 1080), Color.White);
                 spriteBatch.Draw(tree, new Rectangle(tausta3.X + 600, 0, 1858, 1080), Color.White);*/
                
                this.floor();
                spriteBatch.DrawString(fontti2, "" + viking.getLevel(), new Vector2(380, 155), Color.Red);
                spriteBatch.DrawString(fontti, "Character level: ", new Vector2(55, 170), Color.Black);
                int stringlengt = (int)fontti.MeasureString("kills:____").X;
                spriteBatch.DrawString(fontti, "kills: " + killcount, new Vector2(1920 - stringlengt - 10, 30), Color.White);
                //spriteBatch.Draw(house, new Rectangle(tausta1.X + talo.X, talo.Y, talo.Width, talo.Height), Color.White);

                goal.Draw(spriteBatch);
                health.Draw(spriteBatch);
                experience.Draw(spriteBatch);

                for (int i = 0; i < spawners.Count; i++)
                {
                    spawners[i].Draw(spriteBatch);
                }

                for (int i = 0; i < enemies.Count; i++)
                    enemies[i].Draw(spriteBatch);

                for (int i = 0; i < spiders.Count; i++)
                {
                    spiders[i].Draw(spriteBatch);
                }


                for (int n = 0; n < powerups.Count; n++)
                {
                    powerups[n].Draw(spriteBatch);
                }



                    viking.Draw(gameTime, spriteBatch);
                    
            }
            else if (gamestate == 2)
            {

                //Omaa säätöä
                //spriteBatch.Draw(taustaKuva, tausta1, Color.White);
                //spriteBatch.Draw(taustaKuva, tausta2, Color.White);
                spriteBatch.Draw(taustaKuva, new Rectangle((int)(tausta1.X), 0, 1920, 1080), Color.White);
                spriteBatch.Draw(taustaKuva, new Rectangle((int)(tausta2.X), 0, 1920, 1080), Color.White);
                spriteBatch.Draw(taustaKuva, new Rectangle((int)(tausta3.X), 0, 1920, 1080), Color.White);
                //spriteBatch.Draw(tree, new Rectangle(tausta1.X + 0, 220, 742, 619), Color.White);
                //spriteBatch.Draw(tree, new Rectangle(tausta1.X+200, 220, 742, 619), Color.LightGray);
                //spriteBatch.Draw(tree, new Rectangle(tausta1.X + 700, 220, 742, 619), Color.White);
                //spriteBatch.Draw(tree, new Rectangle(tausta1.X + 400, 220, 742, 619), Color.LightGray);
                //spriteBatch.Draw(tree, new Rectangle(tausta1.X + 1150, 220, 742, 619), Color.White);
                //spriteBatch.Draw(tree, new Rectangle(tausta1.X + 900, 220, 742, 619), Color.White);


                this.floor();
                spriteBatch.DrawString(fontti, "Character level: " + viking.getLevel(), new Vector2(10, 30), Color.Black);
                int stringlengt = (int)fontti.MeasureString("kills:____").X;
                spriteBatch.DrawString(fontti, "kills: " + killcount, new Vector2(1920 - stringlengt - 10, 30), Color.White);
                //spriteBatch.Draw(house, new Rectangle(tausta1.X + talo.X, talo.Y, talo.Width, talo.Height), Color.White);

               


                health.Draw(spriteBatch);
                experience.Draw(spriteBatch);
                for (int i = 0; i < enemies.Count; i++)
                    enemies[i].Draw(spriteBatch);

                viking.Draw(gameTime, spriteBatch);

                for (int x = 0; x < 1920; x += 50)
                {
                    for (int y = 0; y < 1080; y += 50)
                    {

                        spriteBatch.Draw(filter, new Rectangle(x, y, 50, 50), Color.White);
                    }
                }

                theend.Draw(spriteBatch, fontti);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

