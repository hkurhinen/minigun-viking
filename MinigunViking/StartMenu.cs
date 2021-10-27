using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace MinigunViking
{
    class StartMenu
    {
        public Texture2D pixel;
        public Rectangle screen;
        public Texture2D cursor;
        public Texture2D splashLogo;
        public Rectangle logopos;
        public Rectangle cursorpos = new Rectangle(0, 0, 50, 50); // cursorPos for CROSSHAIR
        public Rectangle interCursor = new Rectangle(0, 0, 5, 5); //cursorbox for checking intersects
        public Rectangle turhuus;
        public int width;
        public int height;
        public Texture2D menuTausta;
        public Texture2D menuBg;
        public Texture2D random;
        private Texture2D volBar;
        private Texture2D volBar2;
        private Texture2D volBar3;
        private Texture2D vidOp;
        private Texture2D audOp;

        private int index = 0;
        private int testIndex = 0;
        private int optionsIndex = 0;
        private string fullscreenText;
        private int optionsFullScreen;

        private Boolean logoEffect = false;

        //volume
        private float masterVolume = 0.5f;
        private float gunVolume = 0.5f;
        private float otherVolume = 0.5f;
        private float musicVolume = 0.5f;
        //volumes before being applied
        private int optionsMasterVolume;
        private int optionsGunVolume;
        private int optionsOtherVolume;
        private int optionsMusicVolume;
        //volume bars
        public Rectangle masterBar;
        public Rectangle gunBar;
        public Rectangle musicBar;
        public Rectangle otherBar;
        public Rectangle barTexture;
        public Rectangle barTexture2;
        public Rectangle barTexture3;
        public Rectangle vidOpRec;
        public Rectangle audOpRec;

        //menubuttonit
        public Rectangle startButton;
        public Rectangle optionsButton;
        public Rectangle backButton;
        public Rectangle exitButton;
        public Rectangle how2playButton;
        public Rectangle applyOptionsButton;

        //optionsbuttons
        public Rectangle increaseMasterVolume;
        public Rectangle decreaseMasterVolume;
        public Rectangle increaseMusicVolume;
        public Rectangle decreaseMusicVolume;
        public Rectangle increaseGunVolume;
        public Rectangle decreaseGunVolume;
        public Rectangle increaseResolution;
        public Rectangle decreaseResolution;
        public Rectangle increaseFullscreen;
        public Rectangle decreaseFullscreen;
        public Rectangle increaseOtherVolume;
        public Rectangle decreaseOtherVolume;

        //intersectbuttons
        public Rectangle startIntersect;
        public Rectangle optionsIntersect;
        public Rectangle exitIntersect;

        public Texture2D intersectTest;

        //ja niiden kuvat
        public Texture2D startButtonPic;
        public Texture2D startButtonPicHighlight;
        public Texture2D continueButtonPic;
        public Texture2D continueButtonPicHighlight;
        public Texture2D backButtonPic;
        public Texture2D backButtonPicHighlight;
        public Texture2D optionsButtonPic;
        public Texture2D optionsButtonPicHighlight;
        public Texture2D exitButtonPic;
        public Texture2D exitButtonPicHighlight;
        public Texture2D applyOptionsButtonPic;
        public Texture2D applyOptionsButtonPicHighlight; 

        //volume options buttons
        public Texture2D increaseVolumeButtonPic;
        public Texture2D increaseVolumeButtonPicHighlight;
        public Texture2D decreaseVolumeButtonPic;
        public Texture2D decreaseVolumeButtonPicHighlight;

        Boolean kikkeli = false;
        Boolean exitGame = false;
        SpriteFont fontti1;

        public int highlight = 1;
        public int menustate = 0;
        public int screenChanged = 0;
        public float changeTimer = 1f;

        private bool isPaused = false;
        private IO lukija = new IO();
        public float mouseMoveSpeed;

        private List<int> supportedScreenWidth = new List<int>();
        private List<int> supportedScreenHeight = new List<int>();

        public int widthToSet;
        public int heightToSet;
        public int IfFullscreen;

        public StartMenu()
        {
            //Constructor
        }

        public float getMasterVolume()
        {
            return masterVolume;
        }
        public float getGunVolume()
        {
            return gunVolume;
        }
        public float getMusicVolume()
        {
            return musicVolume;
        }
        public float getOtherVolume()
        {
            return otherVolume;
        }

        public void saveSettings()
        {

            List<String> settings = new List<String>();
            settings.Add("mastervolume:" + optionsMasterVolume);
            settings.Add("musicvolume:" + optionsMusicVolume);
            settings.Add("othervolume:" + optionsOtherVolume);
            settings.Add("gunvolume:" + optionsGunVolume);
            settings.Add("screenwidth:" + widthToSet);
            settings.Add("screenheight:" + heightToSet);
            settings.Add("fullscreen:" + IfFullscreen);
            //insert new settings here

            lukija.writeSettings(settings);
        }

        public void Initialize(GraphicsDevice g)
        {
            //TODO set fullhealth at startup
            //get supported display modes

            width = 1920;
            height = 1080;

            if (lukija.readSettings("screenwidth:") != "notfound" && lukija.readSettings("screenheight:") != "notfound" && lukija.readSettings("fullscreen:") != "notfound")
            {
                widthToSet = Convert.ToInt32(lukija.readSettings("screenwidth:"));
                heightToSet = Convert.ToInt32(lukija.readSettings("screenheight:"));
                IfFullscreen = Convert.ToInt32(lukija.readSettings("fullscreen:"));

                if (IfFullscreen > 0)
                {
                    Resolution.SetResolution(widthToSet, heightToSet, true);
                    fullscreenText = "YES";
                    optionsFullScreen = 1;
                }
                else
                {
                    Resolution.SetResolution(widthToSet, heightToSet, false);
                    fullscreenText = "NO";
                    optionsFullScreen = 0;
                }

            }
            else
            {
                widthToSet = 1920;
                heightToSet = 1080;
                IfFullscreen = 1;
                Resolution.SetResolution(1920, 1080, true);
            }

            foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                supportedScreenHeight.Add(mode.Height);
                supportedScreenWidth.Add(mode.Width);
                if (mode.Height == heightToSet && mode.Width == widthToSet)
                {
                    index = testIndex;
                    optionsIndex = testIndex;
                }

                testIndex++;
            }


            //load default volumes at startup if settings are not found, else load saved volumes
            if (lukija.readSettings("mastervolume:") != "notfound")
            {
                optionsMasterVolume = Convert.ToInt32(lukija.readSettings("mastervolume:"));
                masterVolume = optionsMasterVolume / 100.0f;

            }
            else
            {
                optionsMasterVolume = (int)(100 * masterVolume);
            }

            if (lukija.readSettings("musicvolume:") != "notfound")
            {
                optionsMusicVolume = Convert.ToInt32(lukija.readSettings("musicvolume:"));
                musicVolume = (float)(optionsMusicVolume / 100.0);

            }
            else
            {
                optionsMusicVolume = (int)(100 * musicVolume);
            }

            if (lukija.readSettings("othervolume:") != "notfound")
            {
                optionsOtherVolume = Convert.ToInt32(lukija.readSettings("othervolume:"));
                otherVolume = optionsOtherVolume / 100.0f;

            }
            else
            {
                optionsOtherVolume = (int)(100 * otherVolume);
            }

            if (lukija.readSettings("gunvolume:") != "notfound")
            {
                optionsGunVolume = Convert.ToInt32(lukija.readSettings("gunvolume:"));
                gunVolume = optionsGunVolume / 100.0f;

            }
            else
            {
                optionsGunVolume = (int)(100 * gunVolume);
            }

            //Create settings file
            saveSettings();

            logopos = new Rectangle(1920 / 2 - 400, 1080 / 2 - 400, 800, 400);

            turhuus = new Rectangle(0, height - 150, 766, 138);


            //PARTLY scalable options( for 16:9 )
            //MENU SCREEN 1
            startButton = new Rectangle(960 - 142 + 50, 1080 - 4 * (int)((1080 / 5.19230769231) * 0.7)+27, 284, (int)((1080 / 5.19230769231) * 0.7));
            optionsButton = new Rectangle(960 - 142 + 186, 1080 - 3 * (int)((1080 / 5.19230769231) * 0.7) + 27, 284, (int)((1080 / 5.19230769231) * 0.7)); //Huom           
            exitButton = new Rectangle(960 - 142 + 50, 1080 - 2 * (int)((1080 / 5.19230769231) * 0.7) + 27, 284, (int)((1080 / 5.19230769231) * 0.7));
            how2playButton = new Rectangle(892, 1080 / 2 + 15, 284, (int)((1080 / 5.19230769231) * 0.7));

            startIntersect = new Rectangle(startButton.X+20, startButton.Y+20, 160, 80);
            optionsIntersect = new Rectangle(optionsButton.X-100, optionsButton.Y+15, 160, 80);
            exitIntersect = new Rectangle(exitButton.X+20, exitButton.Y+20, 160, 80);
            
            //MENU SCREEN 2
            applyOptionsButton = new Rectangle(960 - 142 - 142, 1080 - 2 * (int)((1080 / 5.19230769231) * 0.7), 284, (int)((1080 / 5.19230769231) * 0.7));
            backButton = new Rectangle(960 - 142 + 142, 1080 - 2 * (int)((1080 / 5.19230769231) * 0.7), 284, (int)((1080 / 5.19230769231) * 0.7));
            
            //VOLUmeE CONTROLS
            increaseMasterVolume = new Rectangle(1260, 560, (int)((1920 / 24)), (int)((1080 / 13.5)));
            decreaseMasterVolume = new Rectangle(980, 560, (int)((1920 / 24)), (int)((1080 / 13.5)));
            increaseGunVolume = new Rectangle(1260, 620, (int)((1920 / 24)), (int)((1080 / 13.5)));
            decreaseGunVolume = new Rectangle(980, 620, (int)((1920 / 24)), (int)((1080 / 13.5)));
            increaseMusicVolume = new Rectangle(1260, 680, (int)((1920 / 24)), (int)((1080 / 13.5)));
            decreaseMusicVolume = new Rectangle(980, 680, (int)((1920 / 24)), (int)((1080 / 13.5)));

            //resolution controls
            increaseResolution = new Rectangle(900, 560, (int)((1920 / 24)), (int)((1080 / 13.5)));
            decreaseResolution = new Rectangle(620, 560, (int)((1920 / 24)), (int)((1080 / 13.5)));

            //fullscreen controls
            increaseFullscreen = new Rectangle(900, 620, (int)((1920 / 24)), (int)((1080 / 13.5)));
            decreaseFullscreen = new Rectangle(620, 620, (int)((1920 / 24)), (int)((1080 / 13.5)));

            //volumebar rectangles (not yet scaling)
            masterBar = new Rectangle(1035, 580, (int)(250 * (optionsMasterVolume / 100.0)), 40);
            gunBar = new Rectangle(1035, 640, (int)(250 * (optionsGunVolume / 100.0)), 40);
            musicBar = new Rectangle(1035, 700, (int)(250 * (optionsMusicVolume / 100.0)), 40);
            barTexture = new Rectangle(masterBar.X, masterBar.Y, 250, 40);
            barTexture2 = new Rectangle(musicBar.X, musicBar.Y, 250, 40);
            barTexture3 = new Rectangle(gunBar.X, gunBar.Y, 250, 40);

            //other rectangles
            vidOpRec = new Rectangle(675,500,250,80);
            audOpRec = new Rectangle(1035,500,250,80);

            //screen = new Rectangle(0, 0, 1920, 1080);
            screen = new Rectangle(0, 0, 1920, 1080);

            pixel = new Texture2D(g, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });

        }
        public void LoadContent(ContentManager content)
        {
            this.cursor = content.Load<Texture2D>("Textures\\crosshair2");
            this.splashLogo = content.Load<Texture2D>("Textures\\minigunviking");

            //menubutton pictures
            this.startButtonPic = content.Load<Texture2D>("Textures\\startButton");
            this.startButtonPicHighlight = content.Load<Texture2D>("Textures\\startButtonGlo");
            this.continueButtonPic = content.Load<Texture2D>("Textures\\continueButton");
            this.continueButtonPicHighlight = content.Load<Texture2D>("Textures\\continueButtonGlo");
            this.backButtonPic = content.Load<Texture2D>("Textures\\backButton");
            this.backButtonPicHighlight = content.Load<Texture2D>("Textures\\backButtonGlo");
            this.optionsButtonPic = content.Load<Texture2D>("Textures\\optionsButton");
            this.optionsButtonPicHighlight = content.Load<Texture2D>("Textures\\optionsButtonGlo");
            this.exitButtonPic = content.Load<Texture2D>("Textures\\exitButton");
            this.exitButtonPicHighlight = content.Load<Texture2D>("Textures\\exitButtonGlo");
            this.applyOptionsButtonPic = content.Load<Texture2D>("Textures\\applyButton");
            this.applyOptionsButtonPicHighlight = content.Load<Texture2D>("Textures\\applyButtonGlo");

            //volume control buttons
            this.increaseVolumeButtonPic = content.Load<Texture2D>("Textures\\increaseMasterVolumeButton");
            this.increaseVolumeButtonPicHighlight = content.Load<Texture2D>("Textures\\increaseMasterVolumeButtonGlo");
            this.decreaseVolumeButtonPic = content.Load<Texture2D>("Textures\\decreaseMasterVolumeButton");
            this.decreaseVolumeButtonPicHighlight = content.Load<Texture2D>("Textures\\decreaseMasterVolumeButtonGlo");
            
            //other
            this.volBar = content.Load<Texture2D>("Textures\\barTexture");
            this.volBar2 = content.Load<Texture2D>("Textures\\barTexture2");
            this.volBar3 = content.Load<Texture2D>("Textures\\barTexture3");
            this.menuTausta = content.Load<Texture2D>("Textures\\vuoria");
            this.intersectTest = content.Load<Texture2D>("Textures\\redbox");
            this.random = content.Load<Texture2D>("Textures\\random");
            this.menuBg = content.Load<Texture2D>("Textures\\menuBg");
            this.audOp = content.Load<Texture2D>("Textures\\audioOptions");
            this.vidOp = content.Load<Texture2D>("Textures\\videoOptions");
        }

        public int Update(MouseState mouse, Point mpos, KeyboardState keys, GameTime gameTime, bool gamePaused)
        {

            isPaused = gamePaused;

            //effect for logo
            if (logoEffect)
            {
                if (logopos.Width < 840)
                {
                    logopos.Width += (int)(gameTime.ElapsedGameTime.TotalMilliseconds/24.0 * 4.0);
                    logopos.Height += (int)(gameTime.ElapsedGameTime.TotalMilliseconds / 24.0 * 2.0);
                    logopos.X -= (int)(gameTime.ElapsedGameTime.TotalMilliseconds / 24.0 * 2.0);
                    logopos.Y -= (int)(gameTime.ElapsedGameTime.TotalMilliseconds / 24.0 * 1.0);
                }
                else
                    logoEffect = false;
            }
            else if (!logoEffect)
            {
                if (logopos.Width > 800)
                {
                    logopos.Width -= (int)(gameTime.ElapsedGameTime.TotalMilliseconds / 24.0 * 4.0);
                    logopos.Height -= (int)(gameTime.ElapsedGameTime.TotalMilliseconds / 24.0 * 2.0);
                    logopos.X += (int)(gameTime.ElapsedGameTime.TotalMilliseconds / 24.0 * 2.0);
                    logopos.Y += (int)(gameTime.ElapsedGameTime.TotalMilliseconds / 24.0 * 1.0);
                }
                else
                    logoEffect = true;
            }

                //update volumebars
                masterBar.Width = (int)((width / 7.68) * (optionsMasterVolume / 100.0));
            musicBar.Width = (int)((width / 7.68) * (optionsMusicVolume / 100.0));
            gunBar.Width = (int)((width / 7.68) * (optionsGunVolume / 100.0));

            //cursorposition relative to resolution
            cursorpos.X = mpos.X;
            cursorpos.Y = mpos.Y;
            interCursor.X = mpos.X+22;
            interCursor.Y = mpos.Y+22;

            //CONTROLLING MENUS WITH ARROW KEYS, NEED SOME WORK
            if (keys.IsKeyDown(Keys.Up))
            {
                if (!kikkeli)
                    highlight--;
                if (highlight < 1 && !kikkeli && menustate == 0)
                    highlight = 3;
                if (highlight < 1 && !kikkeli && menustate == 1)
                    highlight = 10;
                kikkeli = true;
            }
            if (keys.IsKeyDown(Keys.Down))
            {
                if (!kikkeli)
                    highlight++;
                if (((highlight > 3 && menustate == 0) || (highlight > 10 && menustate == 1)) && !kikkeli)
                    highlight = 1;
                kikkeli = true;
            }

            if (keys.IsKeyUp(Keys.Up) && keys.IsKeyUp(Keys.Down))
                kikkeli = false;

            //TIMER SO ONLY ONE ACTION LAUNCHS PER CLICK / PER ENTER
            if (screenChanged == 1)
            {
                changeTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (changeTimer < 0)
                {
                    screenChanged = 0;
                }
            }

            

            //HIGHLIGHITNG SELECTED BUTTON
            if (menustate == 0)
            {
                if (interCursor.Intersects(startIntersect))
                {
                    highlight = 1; //highlight
                }
                else if (interCursor.Intersects(optionsIntersect))
                {
                    highlight = 2; //highlight
                }
                else if (interCursor.Intersects(exitIntersect))
                {
                    highlight = 3; //highlight
                }
            }
            if (menustate == 1)
            {
                if (interCursor.Intersects(backButton) && menustate == 1)
                {
                    highlight = 5; //highlight
                }
                else if (interCursor.Intersects(applyOptionsButton) && menustate == 1)
                {
                    highlight = 1; //highlight
                }
                else if (interCursor.Intersects(decreaseMusicVolume) && menustate == 1)
                {
                    highlight = 8; //highlight
                }
                else if (interCursor.Intersects(decreaseGunVolume) && menustate == 1)
                {
                    highlight = 7; //highlight
                }
                else if (interCursor.Intersects(decreaseMasterVolume) && menustate == 1)
                {
                    highlight = 6; //highlight
                }
                else if (interCursor.Intersects(increaseMasterVolume) && menustate == 1)
                {
                    highlight = 2; //highlight
                }
                else if (interCursor.Intersects(increaseGunVolume) && menustate == 1)
                {
                    highlight = 3; //highlight
                }
                else if (interCursor.Intersects(increaseMusicVolume) && menustate == 1)
                {
                    highlight = 4; //highlight
                }
                else if (interCursor.Intersects(increaseResolution) && menustate == 1)
                {
                    highlight = 9; //highlight
                }
                else if (interCursor.Intersects(decreaseResolution) && menustate == 1)
                {
                    highlight = 10; //highlight
                }
                else if (interCursor.Intersects(increaseFullscreen) && menustate == 1)
                {
                    highlight = 11; //highlight
                }
                else if (interCursor.Intersects(decreaseFullscreen) && menustate == 1)
                {
                    highlight = 12; //highlight
                }
            }


            //CONTROLLING OPTIONS MENU, SPECIFICIALLY AUDIO
            if ((menustate == 1 && highlight == 5 && keys.IsKeyDown(Keys.Enter) && screenChanged == 0) || (interCursor.Intersects(backButton) && mouse.LeftButton == ButtonState.Pressed && menustate == 1 && screenChanged == 0))
            {
                screenChanged = 1;
                menustate = 0;
                changeTimer = 0.2f;

                //DISCARD CHANGES
                index = optionsIndex;
                optionsFullScreen = IfFullscreen;
                if (optionsFullScreen > 0)                
                    fullscreenText = "YES";                
                else
                    fullscreenText = "NO";

                optionsMasterVolume = (int)(100 * masterVolume);
                optionsMusicVolume = (int)(100 * musicVolume);
                optionsOtherVolume = (int)(100 * otherVolume);
                optionsGunVolume = (int)(100 * gunVolume);
            }
            if ((menustate == 1 && highlight == 1 && keys.IsKeyDown(Keys.Enter) && screenChanged == 0) || (interCursor.Intersects(applyOptionsButton) && mouse.LeftButton == ButtonState.Pressed && menustate == 1 && screenChanged == 0))
            {
                screenChanged = 1;
                menustate = 0;
                changeTimer = 0.2f;

                //APPLY VIDEO SETTINGS
                heightToSet = supportedScreenHeight[index];
                widthToSet = supportedScreenWidth[index];

                if (optionsFullScreen > 0)
                {
                    Resolution.SetResolution(widthToSet, heightToSet, true);
                    fullscreenText = "YES";
                    IfFullscreen = 1;
                    optionsFullScreen = 1;
                }
                else
                {
                    Resolution.SetResolution(widthToSet, heightToSet, false);
                    fullscreenText = "NO";
                    IfFullscreen = 0;
                    optionsFullScreen = 0;
                }

                optionsIndex = index;

                //APPLY VOLUME SETTINGS
                saveSettings();
                masterVolume = (float)(optionsMasterVolume / 100.0);
                musicVolume = (float)(optionsMusicVolume / 100.0);
                otherVolume = (float)(optionsOtherVolume / 100.0);
                gunVolume = (float)(optionsGunVolume / 100.0);
            }
            if ((menustate == 1 && highlight == 6 && keys.IsKeyDown(Keys.Enter) && screenChanged == 0) || (interCursor.Intersects(decreaseMasterVolume) && mouse.LeftButton == ButtonState.Pressed && menustate == 1 && screenChanged == 0))
            {
                screenChanged = 1;
                if (optionsMasterVolume > 1)                
                    optionsMasterVolume -= 2;                
                else
                    optionsMasterVolume = 0;
                changeTimer = 0.2f;
            }
            if ((menustate == 1 && highlight == 7 && keys.IsKeyDown(Keys.Enter) && screenChanged == 0) || (interCursor.Intersects(decreaseGunVolume) && mouse.LeftButton == ButtonState.Pressed && menustate == 1 && screenChanged == 0))
            {
                screenChanged = 1;
                if (optionsGunVolume > 1)
                    optionsGunVolume -= 2;
                else
                    optionsGunVolume = 0;
                changeTimer = 0.2f;
            }
            if ((menustate == 1 && highlight == 8 && keys.IsKeyDown(Keys.Enter) && screenChanged == 0) || (interCursor.Intersects(decreaseMusicVolume) && mouse.LeftButton == ButtonState.Pressed && menustate == 1 && screenChanged == 0))
            {
                screenChanged = 1;
                if (optionsMusicVolume > 1)
                    optionsMusicVolume -= 2;
                else
                    optionsMusicVolume = 0;
                changeTimer = 0.2f;
            }
            if ((menustate == 1 && highlight == 2 && keys.IsKeyDown(Keys.Enter) && screenChanged == 0) || (interCursor.Intersects(increaseMasterVolume) && mouse.LeftButton == ButtonState.Pressed && menustate == 1 && screenChanged == 0))
            {
                screenChanged = 1;
                if (optionsMasterVolume < 99)
                    optionsMasterVolume += 2;
                else
                    optionsMasterVolume = 100;
                changeTimer = 0.2f;
            }

            if ((menustate == 1 && highlight == 3 && keys.IsKeyDown(Keys.Enter) && screenChanged == 0) || (interCursor.Intersects(increaseGunVolume) && mouse.LeftButton == ButtonState.Pressed && menustate == 1 && screenChanged == 0))
            {
                screenChanged = 1;
                if (optionsGunVolume < 99)
                    optionsGunVolume += 2;
                else
                    optionsGunVolume = 100;
                changeTimer = 0.2f;
            }
            if ((menustate == 1 && highlight == 4 && keys.IsKeyDown(Keys.Enter) && screenChanged == 0) || (interCursor.Intersects(increaseMusicVolume) && mouse.LeftButton == ButtonState.Pressed && menustate == 1 && screenChanged == 0))
            {
                screenChanged = 1;
                if (optionsMusicVolume < 99)
                    optionsMusicVolume += 2;
                else
                    optionsMusicVolume = 100;
                changeTimer = 0.2f;
            }

            if ((menustate == 1 && highlight == 9 && keys.IsKeyDown(Keys.Enter) && screenChanged == 0) || (interCursor.Intersects(increaseResolution) && mouse.LeftButton == ButtonState.Pressed && menustate == 1 && screenChanged == 0))
            {
                screenChanged = 1;
                if (index < supportedScreenHeight.Count-1)
                    index++;
                else
                    index = 0;
                changeTimer = 0.2f;
            }

            if ((menustate == 1 && highlight == 10 && keys.IsKeyDown(Keys.Enter) && screenChanged == 0) || (interCursor.Intersects(decreaseResolution) && mouse.LeftButton == ButtonState.Pressed && menustate == 1 && screenChanged == 0))
            {
                screenChanged = 1;
                if (index > 0)
                    index--;
                else
                    index = supportedScreenHeight.Count-1;
                changeTimer = 0.2f;
            }

            if ((menustate == 1 && highlight == 11 && keys.IsKeyDown(Keys.Enter) && screenChanged == 0) || (interCursor.Intersects(increaseFullscreen) && mouse.LeftButton == ButtonState.Pressed && menustate == 1 && screenChanged == 0))
            {
                screenChanged = 1;
                if (optionsFullScreen > 0)
                {
                    optionsFullScreen = 0;
                    fullscreenText = "NO";
                }
                else
                {
                    optionsFullScreen = 1;
                    fullscreenText = "YES";
                }
                changeTimer = 0.2f;
            }

            if ((menustate == 1 && highlight == 12 && keys.IsKeyDown(Keys.Enter) && screenChanged == 0) || (interCursor.Intersects(decreaseFullscreen) && mouse.LeftButton == ButtonState.Pressed && menustate == 1 && screenChanged == 0))
            {
                screenChanged = 1;
                if (optionsFullScreen > 0)
                {
                    optionsFullScreen = 0;
                    fullscreenText = "NO";
                }
                else
                {
                    optionsFullScreen = 1;
                    fullscreenText = "YES";
                }
                changeTimer = 0.2f;
            }

            //ALKUMENUN OHJAUS, vois yhistää samaan iffiin klikkauksen ja entterin painalluksen joskus
            if (interCursor.Intersects(optionsIntersect) && mouse.LeftButton == ButtonState.Pressed && menustate == 0 && screenChanged == 0)
            {
                screenChanged = 1;
                highlight = 1;
                menustate = 1;
                changeTimer = 0.2f;
            }
            if (menustate == 0 && highlight == 2 && keys.IsKeyDown(Keys.Enter) && screenChanged == 0)
            {
                screenChanged = 1;
                menustate = 1;
                changeTimer = 0.2f;
            }
            if (interCursor.Intersects(exitIntersect) && mouse.LeftButton == ButtonState.Pressed && menustate == 0 && screenChanged == 0)
            {
                exitGame = true;
            }

            if (interCursor.Intersects(startIntersect) && mouse.LeftButton == ButtonState.Pressed && menustate == 0 && screenChanged == 0)
            {
                return 1;
            }
            else if (menustate == 0 && highlight == 1 && keys.IsKeyDown(Keys.Enter) && screenChanged == 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public Boolean menuExit()
        {
            return exitGame;           
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont fontti)
        {
            //TODO draw health bar


            this.fontti1 = fontti;
            

            spriteBatch.Draw(menuTausta, screen, new Color(150, 255, 255));
            spriteBatch.Draw(menuBg, new Rectangle(310, 0, 1300, 1080), Color.White);

            if (menustate == 0)
            {

                if (highlight == 1)
                {
                    if (isPaused)
                    {
                        //spriteBatch.Draw(continueButtonPicHighlight, startButton, Color.Firebrick);
                        spriteBatch.Draw(continueButtonPicHighlight, startButton, null, Color.Firebrick, MathHelper.ToRadians(5), new Vector2(50,27), SpriteEffects.None, 0.0f);                      
                    }
                    else
                    {
                        //spriteBatch.Draw(startButtonPicHighlight, startButton, Color.Firebrick);
                        spriteBatch.Draw(startButtonPicHighlight, startButton, null, Color.Firebrick, MathHelper.ToRadians(5), new Vector2(50, 27), SpriteEffects.None, 0.0f);
                    }
                }
                else
                {
                    if (isPaused)
                    {
                        //spriteBatch.Draw(continueButtonPic, startButton, Color.White);
                        spriteBatch.Draw(continueButtonPic, startButton, null, Color.White, MathHelper.ToRadians(5), new Vector2(50, 27), SpriteEffects.None, 0.0f);
                    }
                    else
                    {
                        //spriteBatch.Draw(startButtonPic, startButton, Color.White);
                        spriteBatch.Draw(startButtonPic, startButton, null, Color.White, MathHelper.ToRadians(5), new Vector2(50, 27), SpriteEffects.None, 0.0f);
                    }
                }

                if (highlight == 2)
                {
                    //spriteBatch.Draw(optionsButtonPicHighlight, optionsButton, Color.Firebrick);
                    spriteBatch.Draw(optionsButtonPicHighlight, optionsButton, null, Color.Firebrick, MathHelper.ToRadians(355), new Vector2(235,27), SpriteEffects.None, 0.0f);                    
                }
                else
                {
                    //spriteBatch.Draw(optionsButtonPic, optionsButton, Color.White);
                    spriteBatch.Draw(optionsButtonPic, optionsButton, null, Color.White, MathHelper.ToRadians(355), new Vector2(235,27), SpriteEffects.None, 0.0f);
                }

                if (highlight == 3)
                {
                    //spriteBatch.Draw(exitButtonPicHighlight, exitButton, Color.Firebrick);
                    spriteBatch.Draw(exitButtonPicHighlight, exitButton, null, Color.Firebrick, MathHelper.ToRadians(5), new Vector2(50, 27), SpriteEffects.None, 0.0f);
                }
                else
                {
                    //spriteBatch.Draw(exitButtonPic, exitButton, Color.White);
                    spriteBatch.Draw(exitButtonPic, exitButton, null, Color.White, MathHelper.ToRadians(5), new Vector2(50, 27), SpriteEffects.None, 0.0f);
                }

            }
            else if (menustate == 1)
            {
                //draw volumebars
                spriteBatch.Draw(pixel, barTexture, Color.Black);
                spriteBatch.Draw(pixel, masterBar, Color.Red);
                spriteBatch.Draw(volBar, barTexture, Color.White);

                spriteBatch.Draw(pixel, barTexture3, Color.Black);
                spriteBatch.Draw(pixel, gunBar, Color.Red);
                spriteBatch.Draw(volBar3, barTexture3, Color.White);

                spriteBatch.Draw(pixel, barTexture2, Color.Black);
                spriteBatch.Draw(pixel, musicBar, Color.Red);
                spriteBatch.Draw(volBar2, barTexture2, Color.White);

                spriteBatch.Draw(pixel, new Rectangle(675, 580, 250, 40), Color.Black);
                spriteBatch.DrawString(fontti1, supportedScreenWidth[index]+" x "+supportedScreenHeight[index], new Vector2(710, 580), Color.DarkRed);

                spriteBatch.Draw(pixel, new Rectangle(675, 640, 250, 40), Color.Black);
                spriteBatch.DrawString(fontti1, fullscreenText, new Vector2(710, 640), Color.DarkRed);

                spriteBatch.Draw(vidOp,vidOpRec,Color.White);
                spriteBatch.Draw(audOp,audOpRec,Color.White);

                if (highlight == 1)
                {
                    spriteBatch.Draw(applyOptionsButtonPicHighlight, applyOptionsButton, Color.Firebrick);
                }
                else
                {
                    spriteBatch.Draw(applyOptionsButtonPic, applyOptionsButton, Color.White);
                }

                if (highlight == 5)
                {
                    spriteBatch.Draw(backButtonPicHighlight, backButton, Color.Firebrick);
                }
                else
                {
                    spriteBatch.Draw(backButtonPic, backButton, Color.White);
                }

                if (highlight == 2)
                {
                    spriteBatch.Draw(increaseVolumeButtonPicHighlight, increaseMasterVolume, Color.Firebrick);
                }
                else
                {
                    spriteBatch.Draw(increaseVolumeButtonPic, increaseMasterVolume, Color.White);
                }

                if (highlight == 6)
                {
                    spriteBatch.Draw(decreaseVolumeButtonPicHighlight, decreaseMasterVolume, Color.Firebrick);
                }
                else
                {
                    spriteBatch.Draw(decreaseVolumeButtonPic, decreaseMasterVolume, Color.White);
                }

                if (highlight == 3)
                {
                    spriteBatch.Draw(increaseVolumeButtonPicHighlight, increaseGunVolume, Color.Firebrick);
                }
                else
                {
                    spriteBatch.Draw(increaseVolumeButtonPic, increaseGunVolume, Color.White);
                }

                if (highlight == 7)
                {
                    spriteBatch.Draw(decreaseVolumeButtonPicHighlight, decreaseGunVolume, Color.Firebrick);
                }
                else
                {
                    spriteBatch.Draw(decreaseVolumeButtonPic, decreaseGunVolume, Color.White);
                }

                if (highlight == 4)
                {
                    spriteBatch.Draw(increaseVolumeButtonPicHighlight, increaseMusicVolume, Color.Firebrick);
                }
                else
                {
                    spriteBatch.Draw(increaseVolumeButtonPic, increaseMusicVolume, Color.White);
                }

                if (highlight == 8)
                {
                    spriteBatch.Draw(decreaseVolumeButtonPicHighlight, decreaseMusicVolume, Color.Firebrick);
                }
                else
                {
                    spriteBatch.Draw(decreaseVolumeButtonPic, decreaseMusicVolume, Color.White);
                }

                if (highlight == 9)
                {
                    spriteBatch.Draw(increaseVolumeButtonPicHighlight, increaseResolution, Color.Firebrick);
                }
                else
                {
                    spriteBatch.Draw(increaseVolumeButtonPic, increaseResolution, Color.White);
                }

                if (highlight == 10)
                {
                    spriteBatch.Draw(decreaseVolumeButtonPicHighlight, decreaseResolution, Color.Firebrick);
                }
                else
                {
                    spriteBatch.Draw(decreaseVolumeButtonPic, decreaseResolution, Color.White);
                }

                if (highlight == 11)
                {
                    spriteBatch.Draw(increaseVolumeButtonPicHighlight, increaseFullscreen, Color.Firebrick);
                }
                else
                {
                    spriteBatch.Draw(increaseVolumeButtonPic, increaseFullscreen, Color.White);
                }

                if (highlight == 12)
                {
                    spriteBatch.Draw(decreaseVolumeButtonPicHighlight, decreaseFullscreen, Color.Firebrick);
                }
                else
                {
                    spriteBatch.Draw(decreaseVolumeButtonPic, decreaseFullscreen, Color.White);
                }
            }

            spriteBatch.Draw(splashLogo, logopos, Color.White);
            spriteBatch.Draw(cursor, cursorpos, Color.White);


            for (int i = 310; i < 1300; i += 650)
            {
                spriteBatch.Draw(random, new Rectangle(0+i, turhuus.Y, 650, turhuus.Height), Color.Black);
            }

            for (int i = 310; i < 1300; i += 650)
            {
                spriteBatch.Draw(random, new Rectangle(0+i, turhuus.Y - 900, 650, turhuus.Height), Color.Black);
            }
            spriteBatch.DrawString(fontti1, "MINIGUNVIKING BETA v0.01", new Vector2(760, 1040), Color.Gray);
        }
    }
}
