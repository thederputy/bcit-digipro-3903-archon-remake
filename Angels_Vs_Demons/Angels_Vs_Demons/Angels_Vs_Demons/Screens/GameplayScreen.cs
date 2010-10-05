﻿#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace Angels_Vs_Demons
{
    /// <summary>
    /// This screen implements the actual game logic
    /// </summary>
    abstract class GameplayScreen : GameScreen
    {
        #region Fields

        public ContentManager content;
        SpriteFont gameFont;
        GameObject Cursor;
        Tile[][] grid;

        Texture2D Cursor_Texture;
        Texture2D TileTexture;
        Texture2D Arch_Demon_Texture;
        Texture2D Nightmare_Texture;
        Texture2D Demon_Lord_Texture;
        Texture2D Skeleton_Archer_Texture;
        Texture2D Imp_Texture;
        Texture2D Blood_Guard_Texture;

        KeyboardState previousKeyboardState;
        GamePadState previousGamePadState;

        int x_size;
        int y_size;
        int tile_size;

        UnitDisplayWindow unitDisplayWindow;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor. Do not initialize game objects in this constructor, use the load content function
        /// </summary>
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
            
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            // Loads the textures

            gameFont = content.Load<SpriteFont>("MenuFont");
            Cursor_Texture = content.Load<Texture2D>("Cursor");
            TileTexture = content.Load<Texture2D>("gridNormal");
            Arch_Demon_Texture = content.Load<Texture2D>("Arch_Demon");
            Nightmare_Texture = content.Load<Texture2D>("Nightmare");
            Demon_Lord_Texture = content.Load<Texture2D>("Demon_Lord");
            Skeleton_Archer_Texture = content.Load<Texture2D>("Skeleton_Archer");
            Imp_Texture = content.Load<Texture2D>("Imp");
            Blood_Guard_Texture = content.Load<Texture2D>("Blood_Guard");

            /// Initializes the screen with an empty grid of tiles

            x_size = 10;
            y_size = 9;
            tile_size = 40;
            int grid_totalwidth = tile_size * x_size;
            int grid_x_center = grid_totalwidth / 2;
            int screen_x_center = (ScreenManager.screenWidth / 2);

            grid = new Tile[y_size][];
            for (int i = 0; i < y_size; i++)
            {
                grid[i] = new Tile[x_size];
                for (int j = 0; j < x_size; j++)
                {
                    grid[i][j] = new Tile(TileTexture);
                    grid[i][j].rect.X = (j * tile_size) + (screen_x_center - grid_x_center);
                    grid[i][j].rect.Y = i * tile_size;
                    grid[i][j].rect.Width = tile_size;
                    grid[i][j].rect.Height = tile_size;
                }
            }

            // Initializes the cursor

            Cursor = new GameObject(Cursor_Texture);
            Cursor.rect.X = grid[0][0].rect.X;
            Cursor.rect.Y = grid[0][0].rect.Y;
            Cursor.rect.Width = tile_size;
            Cursor.rect.Height = tile_size;

            // Initialize Demon Army

            Champion ArchDemon = new Champion(Arch_Demon_Texture);
            Knight Nightmare = new Knight(Nightmare_Texture);
            Mage DemonLord = new Mage(Demon_Lord_Texture);
            Archer SkeletonArcher1 = new Archer(Skeleton_Archer_Texture);
            Archer SkeletonArcher2 = new Archer(Skeleton_Archer_Texture);
            Peon Imp1 = new Peon(Imp_Texture);
            Peon Imp2 = new Peon(Imp_Texture);
            Peon Imp3 = new Peon(Imp_Texture);
            Guard BloodGuard1 = new Guard(Blood_Guard_Texture);
            Guard BloodGuard2 = new Guard(Blood_Guard_Texture);

            // Place Demon army on grid

            grid[4][0].setUnit(ArchDemon);
            grid[5][0].setUnit(Nightmare);
            grid[3][0].setUnit(DemonLord);
            grid[2][0].setUnit(SkeletonArcher1);
            grid[6][0].setUnit(SkeletonArcher2);
            grid[2][1].setUnit(Imp1);
            grid[4][1].setUnit(Imp2);
            grid[6][1].setUnit(Imp3);
            grid[3][1].setUnit(BloodGuard1);
            grid[5][1].setUnit(BloodGuard2);

            // Initialize Angel Army

            Champion ArchAngel = new Champion(Arch_Demon_Texture);
            Knight Pegasus = new Knight(Nightmare_Texture);
            Mage HighAngel = new Mage(TileTexture);
            Archer ChosenOne1 = new Archer(TileTexture);
            Archer ChosenOne2 = new Archer(TileTexture);
            Peon Soldier1 = new Peon(TileTexture);
            Peon Soldier2 = new Peon(TileTexture);
            Peon Soldier3 = new Peon(TileTexture);
            Guard AngelicGuard1 = new Guard(TileTexture);
            Guard AngelicGuard2 = new Guard(TileTexture);

            // Place Angel army on grid

            grid[4][9].setUnit(ArchAngel);
            grid[5][9].setUnit(Pegasus);
            grid[3][9].setUnit(HighAngel);
            grid[2][9].setUnit(ChosenOne1);
            grid[6][9].setUnit(ChosenOne2);
            grid[2][8].setUnit(Soldier1);
            grid[4][8].setUnit(Soldier2);
            grid[6][8].setUnit(Soldier3);
            grid[3][8].setUnit(AngelicGuard1);
            grid[5][8].setUnit(AngelicGuard2);


            //create new unit display window
            unitDisplayWindow = new UnitDisplayWindow();

            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            Thread.Sleep(1);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (IsActive)
            {
                // Put the variable updates here
            }
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {

                grid[(int)Cursor.position.Y][(int)Cursor.position.X].isSelected = false;

                if (keyboardState.IsKeyDown(Keys.Left) && !previousKeyboardState.IsKeyDown(Keys.Left))
                {
                    if (Cursor.position.X > 0)
                    {
                        Cursor.position.X--;
                    }
                }


                if (keyboardState.IsKeyDown(Keys.Right) && !previousKeyboardState.IsKeyDown(Keys.Right))
                {
                    if (Cursor.position.X < (x_size - 1))
                    {
                        Cursor.position.X++;
                    }
                }

                if (keyboardState.IsKeyDown(Keys.Up) && !previousKeyboardState.IsKeyDown(Keys.Up))
                {
                    if (Cursor.position.Y > 0)
                    {
                        Cursor.position.Y--;
                    }
                }

                if (keyboardState.IsKeyDown(Keys.Down) && !previousKeyboardState.IsKeyDown(Keys.Down))
                {
                    if (Cursor.position.Y < (y_size - 1))
                    {
                        Cursor.position.Y++;
                    }
                }

                grid[(int)Cursor.position.Y][(int)Cursor.position.X].isSelected = true;

                if (grid[(int)Cursor.position.Y][(int)Cursor.position.X].isOccupied)
                {
                    Unit displayUnit = grid[(int)Cursor.position.Y][(int)Cursor.position.X].getUnit();

                    unitDisplayWindow.DisplayedUnit = displayUnit;
                }
                else
                {
                    unitDisplayWindow.DisplayedUnit = null;
                }
            }
            previousKeyboardState = keyboardState;
            previousGamePadState = gamePadState;
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.Azure, 0, 0);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            // Draw the grid on the screen

            for (int i = 0; i < y_size; i++)
            {
                for (int j = 0; j < x_size; j++)
                {
                    spriteBatch.Draw(TileTexture, grid[i][j].rect, Color.White);
                }
            }

            // Draw the units on the screen

            for (int i = 0; i < y_size; i++)
            {
                for (int j = 0; j < x_size; j++)
                {
                    if (grid[i][j].isOccupied)
                    {
                        Unit Tempunit = grid[i][j].getUnit();
                        Texture2D TempTexture = Tempunit.sprite;
                        spriteBatch.Draw(TempTexture, grid[i][j].rect, Color.White);
                    }
                    if (grid[i][j].isSelected)
                    {
                        spriteBatch.Draw(Cursor_Texture, grid[i][j].rect, Color.Silver);
                    }
                }
            }

            Vector2 unitType = new Vector2(10, 360);
            if (unitDisplayWindow.DisplayedUnit != null)
            {
                String unitName = unitDisplayWindow.DisplayedUnit.ToString();
                spriteBatch.DrawString(gameFont, unitName, unitType, Color.Black);
            }
            else
            {
                String unitName = "empty tile";
                spriteBatch.DrawString(gameFont, unitName, unitType, Color.Black);
            }
            //unitDisplayWindow.Draw(gameTime);

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0)
                ScreenManager.FadeBackBufferToBlack(255 - TransitionAlpha);
        }


        #endregion
    }
}
