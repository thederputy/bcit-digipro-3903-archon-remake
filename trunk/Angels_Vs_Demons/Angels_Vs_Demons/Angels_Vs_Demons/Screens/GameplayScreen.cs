#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Angels_Vs_Demons.Players;
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

        protected KeyboardState previousKeyboardState;
        protected GamePadState previousGamePadState;

        protected Player player1;
        protected Player player2;

        protected Board board;

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

            board = new Board(content);
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
        /// Updates the state of the game. This method checks the GameScreen.isActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);


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
                
                
                if (keyboardState.IsKeyDown(Keys.Left) && !previousKeyboardState.IsKeyDown(Keys.Left))
                {
                    board.moveCursor(-1, 0);
                }

                if (keyboardState.IsKeyDown(Keys.Right) && !previousKeyboardState.IsKeyDown(Keys.Right))
                {
                    board.moveCursor(1, 0);
                }

                if (keyboardState.IsKeyDown(Keys.Up) && !previousKeyboardState.IsKeyDown(Keys.Up))
                {
                    board.moveCursor(0, -1);
                }

                if (keyboardState.IsKeyDown(Keys.Down) && !previousKeyboardState.IsKeyDown(Keys.Down))
                {
                    board.moveCursor(0, 1);
                }

                if (keyboardState.IsKeyDown(Keys.Enter) && !previousKeyboardState.IsKeyDown(Keys.Enter))
                {
                    board.makeAction();
                    /*
                    if (movePhase)
                    {// if the unit has not been moved yet
                        if (currentTile.isOccupied && currentTile.isUsable)
                        {
                            if (currentTile.isSelected == true)
                            {//if you're selecting the one that is currently active, deactivate it
                                currentTile.isSelected = false;

                                //mark the old movable tiles to not moveable
                                for (int i = 0; i < y_size; i++)
                                {
                                    for (int j = 0; j < x_size; j++)
                                    {
                                        if (grid[i][j].isMovable)
                                        {
                                            grid[i][j].isMovable = false;
                                        }
                                    }
                                }
                            }
                            else
                            {//select a new unit
                                //deselect the curently selected unit
                                if (selectedTile != null)
                                    selectedTile.isSelected = false;

                                //mark the old movable tiles to not moveable
                                for (int i = 0; i < y_size; i++)
                                {
                                    for (int j = 0; j < x_size; j++)
                                    {
                                        if (grid[i][j].isMovable)
                                        {
                                            grid[i][j].isMovable = false;
                                        }
                                    }
                                }

                                //select the new tile and assign it to the gameplayscreen's selectedTile
                                currentTile.isSelected = true;
                                selectedTile = currentTile;
                            }//end if selected == true
                        }
                        else if (currentTile.isMovable)
                        {
                            //call swap fuction
                            currentTile.setUnit(selectedTile.getUnit());
                            currentTile.isOccupied = true;
                            currentTile.isAngel = selectedTile.isAngel;
                            selectedTile.setUnit(null);
                            selectedTile.isSelected = false;
                            selectedTile.isOccupied = false;
                            selectedTile.isAngel = false;
                            //mark the old movable tiles to not moveable
                            for (int i = 0; i < y_size; i++)
                            {
                                for (int j = 0; j < x_size; j++)
                                {
                                    if (grid[i][j].isMovable)
                                    {
                                        grid[i][j].isMovable = false;
                                    }
                                }
                            }
                            //end the move phase and enter attack phase
                            movePhase = false;
                            attackPhase = true;
                            //make the current unit the only active one
                            for (int i = 0; i < y_size; i++)
                            {
                                for (int j = 0; j < x_size; j++)
                                {
                                    if (grid[i][j].isCurrentTile == false)
                                    {
                                        grid[i][j].isUsable = false;
                                    }
                                }
                            }
                            currentTile.isUsable = true;
                        }
                    }
                    if (attackPhase)
                    {
                        if (currentTile.isOccupied && currentTile.isUsable)
                        {
                            if (currentTile.isSelected == true)
                            {//if you're selecting the one that is currently active, deactivate it
                                currentTile.isSelected = false;
                                currentTile.isUsable = false;

                                //swap turns and initiates units for the next turn

                                attackPhase = false;
                                movePhase = true;

                                if (isAngelTurn)
                                {// if the angels turn is finishing, swap to demons turn
                                    isAngelTurn = false;
                                    for (int i = 0; i < y_size; i++)
                                    {
                                        for (int j = 0; j < x_size; j++)
                                        {
                                            if (grid[i][j].isOccupied)
                                            {
                                                if (grid[i][j].isAngel == false)
                                                {
                                                    grid[i][j].isUsable = true;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {// if the demons turn is finishing, swap to angels turn
                                    isAngelTurn = true;
                                    for (int i = 0; i < y_size; i++)
                                    {
                                        for (int j = 0; j < x_size; j++)
                                        {
                                            if (grid[i][j].isOccupied)
                                            {
                                                if (grid[i][j].isAngel)
                                                {
                                                    grid[i][j].isUsable = true;
                                                }
                                            }
                                        }
                                    }
                                }


                                //mark the old attackable tiles to not attackable
                                for (int i = 0; i < y_size; i++)
                                {
                                    for (int j = 0; j < x_size; j++)
                                    {
                                        if (grid[i][j].isAttackable)
                                        {
                                            grid[i][j].isAttackable = false;
                                        }
                                    }
                                }
                            }
                            else
                            {//select a new unit
                                //select the new tile and assign it to the gameplayscreen's selectedTile
                                currentTile.isSelected = true;
                                selectedTile = currentTile;
                            }//end if selected == true
                        }
                        else if (currentTile.isAttackable)
                        {
                            //call attack fuction

                            
                            if (currentTile.isSelected)
                            {//skips turn if you select a unit to attack itself
                                selectedTile.isSelected = false;

                                //end the move phase and enter attack phase
                                movePhase = false;
                                attackPhase = true;
                                //make the current unit the only active one
                                for (int i = 0; i < y_size; i++)
                                {
                                    for (int j = 0; j < x_size; j++)
                                    {
                                        if (grid[i][j].isCurrentTile == false)
                                        {
                                            grid[i][j].isUsable = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                grid[(int)Cursor.position.Y][(int)Cursor.position.X].isCurrentTile = true;

                if (currentTile.isOccupied)
                {
                    Unit displayUnit = currentTile.getUnit();

                    unitDisplayWindow.DisplayedUnit = displayUnit;
                }
                else
                {
                    unitDisplayWindow.DisplayedUnit = null;
                }

                for (int grid_y = 0; grid_y < y_size; grid_y++)
                {
                    for (int grid_x = 0; grid_x < x_size; grid_x++)
                    {
                        if (grid[grid_y][grid_x].isSelected)
                        {// finds if any tiles are selected by the player
                            if (movePhase)
                            {// finds if there are any movable tiles that the player can move to
                                
                                for (int k = 0; k < grid[grid_y][grid_x].getUnit().Movement; k++)
                                {
                                    for (int l = 0; l < (grid[grid_y][grid_x].getUnit().Movement - k); l++)
                                    {
                                        if ((grid_x + l) < x_size && (grid_y + k) < y_size && grid[grid_y + k][grid_x + l].isOccupied == false)
                                        {//BR
                                            grid[grid_y + k][grid_x + l].isMovable = true;
                                        }
                                        if ((grid_x - l) > -1 && (grid_y - k) > -1 && grid[grid_y - k][grid_x - l].isOccupied == false)
                                        {//TL
                                            grid[grid_y - k][grid_x - l].isMovable = true;
                                        }
                                        if ((grid_x + l) < x_size && (grid_y - k) > -1 && grid[grid_y - k][grid_x + l].isOccupied == false)
                                        {//TR
                                            grid[grid_y - k][grid_x + l].isMovable = true;
                                        }
                                        if ((grid_x - l) > -1 && (grid_y + k) < y_size && grid[grid_y + k][grid_x - l].isOccupied == false)
                                        {//BL
                                            grid[grid_y + k][grid_x - l].isMovable = true;
                                        }
                                    }
                                }
                            }
                            if (attackPhase)
                            {// finds if there are any attackable tiles that the player can move to

                                for (int k = 0; k < grid[grid_y][grid_x].getUnit().Movement; k++)
                                {
                                    for (int l = 0; l < (grid[grid_y][grid_x].getUnit().Movement - k); l++)
                                    {
                                        if ((grid_x + l) < x_size && (grid_y + k) < y_size)
                                        {//BR
                                            grid[grid_y + k][grid_x + l].isAttackable = true;
                                        }
                                        if ((grid_x - l) > -1 && (grid_y - k) > -1)
                                        {//TL
                                            grid[grid_y - k][grid_x - l].isAttackable = true;
                                        }
                                        if ((grid_x + l) < x_size && (grid_y - k) > -1)
                                        {//TR
                                            grid[grid_y - k][grid_x + l].isAttackable = true;
                                        }
                                        if ((grid_x - l) > -1 && (grid_y + k) < y_size)
                                        {//BL
                                            grid[grid_y + k][grid_x - l].isAttackable = true;
                                        }
                                    }
                                }
                            }
                        }
                    }//end inner for loop*/
                }//end outer for loop
                
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


            board.PaintGrid(spriteBatch);

            board.PaintUnits(spriteBatch);

            /*
            //unitDisplayWindow.Draw(gameTime);

            //debugging information
            Vector2 debugLocation = new Vector2(10,10);
            Vector2 debugLocation2 = new Vector2(10, 50);

            if (isAngelTurn)
                spriteBatch.DrawString(gameFont, "Angel Turn", debugLocation, Color.Black);
            else
                spriteBatch.DrawString(gameFont, "Demon Turn", debugLocation, Color.Black);

            spriteBatch.DrawString(debugFont, "Angel: " + grid[(int)Cursor.position.Y][(int)Cursor.position.X].isAngel.ToString() + '\n'
                                            + "Attackable: " + grid[(int)Cursor.position.Y][(int)Cursor.position.X].isAttackable.ToString() + '\n'
                                            + "CurrentTile: " + grid[(int)Cursor.position.Y][(int)Cursor.position.X].isCurrentTile.ToString() + '\n'
                                            + "Movable: " + grid[(int)Cursor.position.Y][(int)Cursor.position.X].isMovable.ToString() + '\n'
                                            + "Occupied: " + grid[(int)Cursor.position.Y][(int)Cursor.position.X].isOccupied.ToString() + '\n'
                                            + "Selected: " + grid[(int)Cursor.position.Y][(int)Cursor.position.X].isSelected.ToString() + '\n'
                                            + "Usable: " + grid[(int)Cursor.position.Y][(int)Cursor.position.X].isUsable.ToString() + '\n'
                                            + "Move Phase: " + movePhase.ToString() + '\n'
                                            + "Attack Phase: " + attackPhase.ToString() + '\n'
                , debugLocation2, Color.Black);
            */
            
            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0)
                ScreenManager.FadeBackBufferToBlack(255 - TransitionAlpha);
        }


        #endregion
    }
}
