#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Angels_Vs_Demons.BoardObjects;
using Angels_Vs_Demons.GameObjects;
using Angels_Vs_Demons.Players;
using Angels_Vs_Demons.Screens.ScreenManagers;
using Angels_Vs_Demons.Screens;
#endregion

namespace Angels_Vs_Demons.Screens.GameplayScreens
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

            //check for end of turn
            if (!board.movePhase && !board.attackPhase)
            {
                board.beginTurn();
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
                    makeAction();
                }
            }
            previousKeyboardState = keyboardState;
            previousGamePadState = gamePadState;
        }

        /// <summary>
        /// Gets called when you press enter on the board.
        /// This is the main loop that processes the gamestate.
        /// </summary>
        protected virtual void makeAction()
        {
            if (board.movePhase)
            {
                processMovePhase();
            }
            if (board.attackPhase)
            {
                processAttackPhase();
            }
        }

        /// <summary>
        /// Processes the move phase.
        /// This should be the same for all types of gameplay screen.
        /// </summary>
        protected virtual void processMovePhase()
        {
#if DEBUG
            board.showBitMasks();
#endif
            Tile currentTile = board.GetCurrentTile();
#if DEBUG
            Console.WriteLine("currentTile.IsUsable: " + currentTile.IsUsable);
#endif
            if (currentTile.IsUsable)
            {
                //check that there is a tile selected
                if (board.selectedTile != null)
                {
                    //if we've selected the same tile again
                    if (currentTile.position == board.selectedTile.position)
                    {
                        board.selectedTile = null;
#if DEBUG
                        Console.WriteLine("selected tile = null");
#endif
                    }
                    else
                    {
#if DEBUG
                        Console.WriteLine("updating seleted tile");
#endif
                        board.selectedTile = currentTile;
                    }
                }
                else
                {
#if DEBUG
                    Console.WriteLine("updating seleted tile");
#endif
                    board.selectedTile = currentTile;
                }
            }
            else
            {
                //the tile is not occupied, check to see if we can move to it
                if (board.selectedTile != null && (currentTile.MoveID & board.selectedTile.Unit.ID) != 0)
                {
                    //execute the move phase
                    executeMovePhase(currentTile, board.selectedTile);
                }
            }
        }

        /// <summary>
        /// Executes the move phase.
        /// This should be overridden in each individual gameplayscreen, though they will be similar implementations
        /// </summary>
        protected abstract void executeMovePhase(Tile currentTile, Tile boardSelectedTile);

        /// <summary>
        /// Processes the attack phase.
        /// This should be overridden in each individual gameplayscreen, though they will be similar implementations
        /// </summary>
        protected virtual void processAttackPhase()
        {
            ///this attack is just default for now, until we have attacking working
            Attack attack = new Attack(null, null);
            board.applyAttack(attack);
            board.endTurn();
        }

        /// <summary>
        /// Executes the attack phase.
        /// This should be overridden in each individual gameplayscreen, though they will be similar implementations
        /// </summary>
        protected abstract void executeAttackPhase(Tile currentTile, Tile boardSelectedTile);

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

            
            //unitDisplayWindow.Draw(gameTime);

            //debugging information
            Vector2 debugLocation = new Vector2(10,10);
            Vector2 debugLocation2 = new Vector2(10, 50);

            //if (board.isAngelTurn)
            //    spriteBatch.DrawString(board.gameFont, "Angel Turn", debugLocation, Color.Black);
            //else
            //    spriteBatch.DrawString(board.gameFont, "Demon Turn", debugLocation, Color.Black);

            //spriteBatch.DrawString(board.debugFont, "Angel: " + board.grid[(int)board.Cursor.position.Y][(int)board.Cursor.position.X].IsAngel.ToString() + '\n'
            //                                + "Attackable: " + board.grid[(int)board.Cursor.position.Y][(int)board.Cursor.position.X].IsAttackable.ToString() + '\n'
            //                                + "CurrentTile: " + board.grid[(int)board.Cursor.position.Y][(int)board.Cursor.position.X].IsCurrentTile.ToString() + '\n'
            //                                + "Movable: " + board.grid[(int)board.Cursor.position.Y][(int)board.Cursor.position.X].IsMovable.ToString() + '\n'
            //                                + "Occupied: " + board.grid[(int)board.Cursor.position.Y][(int)board.Cursor.position.X].IsOccupied.ToString() + '\n'
            //                                + "Selected: " + board.grid[(int)board.Cursor.position.Y][(int)board.Cursor.position.X].IsSelected.ToString() + '\n'
            //                                + "Usable: " + board.grid[(int)board.Cursor.position.Y][(int)board.Cursor.position.X].IsUsable.ToString() + '\n'
            //                                + "Move Phase: " + board.movePhase.ToString() + '\n'
            //                                + "Attack Phase: " + board.attackPhase.ToString() + '\n'
            //    , debugLocation2, Color.Black);
            
            
            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0)
                ScreenManager.FadeBackBufferToBlack(255 - TransitionAlpha);
        }


        #endregion
    }
}
