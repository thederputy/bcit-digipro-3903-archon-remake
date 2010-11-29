﻿#region Using Statements
using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Angels_Vs_Demons.BoardObjects;
using Angels_Vs_Demons.GameObjects;
using Angels_Vs_Demons.GameObjects.Units;
using Angels_Vs_Demons.Players;
using Angels_Vs_Demons.Screens.ScreenManagers;
using Angels_Vs_Demons.Screens;
using Angels_Vs_Demons.Util;
#endregion

namespace Angels_Vs_Demons.Screens.GameplayScreens
{
    /// <summary>
    /// This screen implements the actual game logic
    /// </summary>
    abstract class GameplayScreen : GameScreen
    {
        #region Fields

        private Player player1;

        public Player Player1
        {
            get { return player1; }
            set { player1 = value; }
        }
        private Player player2;

        public Player Player2
        {
            get { return player2; }
            set { player2 = value; }
        }

        /// <summary>
        /// The player who is currently moving.  null if the game is over.
        /// </summary>
        private Player currentPlayer;

        public Player CurrentPlayer
        {
            get { return currentPlayer; }
            set { currentPlayer = value; }
        }

        /// <summary>
        /// The player who will go after this turn is completed.  null if the game is over.
        /// </summary>
        private Player nextPlayer;

        public Player NextPlayer
        {
            get { return nextPlayer; }
            set { nextPlayer = value; }
        }

        /// <summary>
        /// The player that won the game.  null if there is no winner yet or if the game is tied.
        /// </summary>
        private Player winnerPlayer;

        public Player WinnerPlayer
        {
            get { return winnerPlayer; }
            set { winnerPlayer = value; }
        }

        public ContentManager content;

        protected KeyboardState previousKeyboardState;
        protected GamePadState previousGamePadState;

        protected AvDGame board;

        private SpriteFont mapFont, gameFont;
        private Vector2 fontPosition;

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
        /// Copy constructor. The tictactoe game has one, so I thought I'd add it.
        /// </summary>
        /// <param name="game"></param>
        public GameplayScreen(GameplayScreen game)
        {
            content = game.content;
            //previousKeyboardState = game.previousKeyboardState;
            //previousGamePadState = game.previousGamePadState;
            //player1 = game.player1;
            //player2 = game.player2;
            //currentPlayer = game.currentPlayer;
            //nextPlayer = game.nextPlayer;
            //winnerPlayer = game.winnerPlayer;
            //board = game.board;
            //unitDisplayWindow = game.unitDisplayWindow;
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            board = new AvDGame(content);
            //create new unit display window
            unitDisplayWindow = new UnitDisplayWindow(content);

            mapFont = content.Load<SpriteFont>("MapFont");
            gameFont = content.Load<SpriteFont>("MenuFont");

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

            if (WinnerPlayer == null)
            {
                //will only get called at the beginning of the game
                if (CurrentPlayer == null)
                {
                    CurrentPlayer = Player1;
                    NextPlayer = Player2;
                }

                if (CurrentPlayer is ComputerPlayer)
                {
                    //do the computer player stuff
                    ComputerPlayer cp = CurrentPlayer as ComputerPlayer;
                    Turn turn = cp.getTurn(board);
                    board.applyTurn(turn);
                    Console.WriteLine("DONE CHANGING STUFF");
                }

                //check for end of turn
                if (!board.MovePhase && !board.AttackPhase)
                {
                    board.endTurn();    //end the current turn
                    
                    //switch the controlling players
                    Player tempPlayer = CurrentPlayer;
                    CurrentPlayer = NextPlayer;
                    NextPlayer = tempPlayer;

                    board.beginTurn();  //begin the next turn
                }
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
                else if (keyboardState.IsKeyDown(Keys.Right) && !previousKeyboardState.IsKeyDown(Keys.Right))
                {
                    board.moveCursor(1, 0);
                }
                else if (keyboardState.IsKeyDown(Keys.Up) && !previousKeyboardState.IsKeyDown(Keys.Up))
                {
                    board.moveCursor(0, -1);
                }
                else if (keyboardState.IsKeyDown(Keys.Down) && !previousKeyboardState.IsKeyDown(Keys.Down))
                {
                    board.moveCursor(0, 1);
                }
                else if (keyboardState.IsKeyDown(Keys.Enter) && !previousKeyboardState.IsKeyDown(Keys.Enter))
                {
                    makeAction();
                }
                else if (keyboardState.IsKeyDown(Keys.E) && !previousKeyboardState.IsKeyDown(Keys.E))
                {
                    
                    if (board.MovePhase == true)
                    {
                        board.endMovePhaseNoMove();
                    }
                    else if (board.AttackPhase == true)
                    {
                        board.endAttackPhase();
                    }
                }
                else if (keyboardState.IsKeyDown(Keys.U) && !previousKeyboardState.IsKeyDown(Keys.U))
                {
                    if (board.AttackPhase == true)
                    {
                        board.undoLastMove();
                    }
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
            if (board.MovePhase)
            {
                processMovePhase();
            }
            if (board.AttackPhase)
            {
                processAttackPhase();
            }
        }

        /// <summary>
        /// Processes the move phase.
        /// This should be the same for all types of gameplay screen.
        /// </summary>
        public void processMovePhase()
        {
#if DEBUG
            //board.showMoveBitMasks();
#endif
            Tile currentTile = board.GetCurrentTile();
#if DEBUG
            Debug.WriteLine("currentTile.IsUsable: " + currentTile.IsUsable);
#endif
            if (currentTile.IsUsable)
            {
                //check that there is a tile selected
                if (board.selectedTile != null)
                {
                    //if we've selected the same tile again, deselect it
                    if (currentTile.position == board.selectedTile.position)
                    {
                        board.selectedTile = null;
#if DEBUG
                        Debug.WriteLine("selected tile = null");
#endif
                    }
                    else
                    {
#if DEBUG
                        Debug.WriteLine("updating selected tile");
#endif
                        board.selectedTile = currentTile;
                    }
                }
                else
                {
#if DEBUG
                    Debug.WriteLine("updating selected tile");
#endif
                    board.selectedTile = currentTile;
                }
            }
            else
            {
                //the currentTile is not occupied
                //if there is a selected tile, check to see if currentTile is within its move range
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
        protected virtual void executeMovePhase(Tile currentTile, Tile boardSelectedTile)
        {
            board.applyMove(new Move(currentTile.position, boardSelectedTile.position));
        }

        /// <summary>
        /// Processes the attack phase.
        /// This should be the same for all types of gameplay screen.
        /// </summary>
        public void processAttackPhase()
        {
#if DEBUG
            //board.showAttackBitMasks();
#endif
            Tile currentTile = board.GetCurrentTile();
#if DEBUG
            Debug.WriteLine("currentTile.IsUsable: " + currentTile.IsUsable);
#endif
            if (currentTile.IsUsable)
            {
                //check that there is a tile selected
                if (board.selectedTile != null)
                {
                    //if we've selected the same tile again, attack phase is over 
                    //(not sure if this is the best implementation)
                    if (currentTile.position == board.selectedTile.position)
                    {
                        board.selectedTile = null;
#if DEBUG
                        Debug.WriteLine("selected the same tile again");
                        Debug.WriteLine("selected tile = null");
#endif
                    }
                    else
                    {
#if DEBUG
                        Debug.WriteLine("selected a new tile");
                        Debug.WriteLine("updating selected tile");
#endif
                        board.selectedTile = currentTile;
                    }
                }
                else
                {
#if DEBUG
                    Debug.WriteLine("no tile selected, selecting current tile");
                    Debug.WriteLine("updating selected tile");
#endif
                    board.selectedTile = currentTile;
                }
            }
            else
            {
                //we've selected a tile that is not one of ours.
                //if there is a selected tile, check to see if the current tile is within our attack range
                if (board.selectedTile != null && (currentTile.AttackID & board.selectedTile.Unit.ID) != 0)
                {
                    //if the current tile has a unit on it
                    if (currentTile.IsOccupied)
                    {
#if DEBUG
                        Debug.WriteLine("executing attack phase");
#endif
                        //execute the attack phase
                        executeAttackPhase(currentTile, board.selectedTile);
                    }
                }
            }
        }

        /// <summary>
        /// Executes the attack phase.
        /// This should be overridden in each individual gameplayscreen, though they will be similar implementations
        /// </summary>
        /// <param name="victimTile">the tile that is getting attacked</param>
        /// <param name="attackerTile">the tile that is attacking</param>
        protected virtual void executeAttackPhase(Tile victimTile, Tile attackerTile)
        {
            board.applyAttack(new Attack(victimTile.position, attackerTile.position));
        }

        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Azure, 0, 0);
            
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            //Painting the grid

            board.PaintGrid(spriteBatch);
            board.PaintUnits(spriteBatch);

            //Painting information text

            unitDisplayWindow.Draw(spriteBatch, board.GetCurrentTile().Unit);

            spriteBatch.Begin();

            if (board.ControllingFaction == Faction.ANGEL)
            {
                fontPosition.X = 20;
                fontPosition.Y = 20;
                spriteBatch.DrawString(gameFont, "Angel Turn", fontPosition, Color.Black);
                if (board.MovePhase)
                {
                    fontPosition.X = 35;
                    fontPosition.Y = 50;
                    spriteBatch.DrawString(mapFont, "Move Phase", fontPosition, Color.Black);
                }
                if (board.AttackPhase)
                {
                    fontPosition.X = 35;
                    fontPosition.Y = 50;
                    spriteBatch.DrawString(mapFont, "Attack Phase", fontPosition, Color.Black);
                }
            }
            else
            {
                fontPosition.X = ScreenManager.screenWidth - 200;
                fontPosition.Y = 20;
                spriteBatch.DrawString(gameFont, "Demon Turn", fontPosition, Color.Black);
                if (board.MovePhase)
                {
                    fontPosition.X = ScreenManager.screenWidth - 185;
                    fontPosition.Y = 50;
                    spriteBatch.DrawString(mapFont, "Move Phase", fontPosition, Color.Black);
                }
                if (board.AttackPhase)
                {
                    fontPosition.X = ScreenManager.screenWidth - 185;
                    fontPosition.Y = 50;
                    spriteBatch.DrawString(mapFont, "Attack Phase", fontPosition, Color.Black);
                }
            }

            fontPosition.X = 5;
            fontPosition.Y = 120;

            Dictionary<int, Unit>.ValueCollection AngelValues = board.Angels.Values;

            foreach (Unit u in AngelValues)
            {
                spriteBatch.DrawString(mapFont, u.Name + "HP: " + u.CurrHP + "/" + u.TotalHP, fontPosition, Color.Black);
                fontPosition.Y += mapFont.LineSpacing + 2;
            }

            fontPosition.X = 635;
            fontPosition.Y = 120;

            Dictionary<int, Unit>.ValueCollection DemonValues = board.Demons.Values;

            foreach (Unit u in DemonValues)
            {
                spriteBatch.DrawString(mapFont, u.Name + "HP: " + u.CurrHP + "/" + u.TotalHP, fontPosition, Color.Black);
                fontPosition.Y += mapFont.LineSpacing + 2;
            }

            spriteBatch.End();
            
            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0)
                ScreenManager.FadeBackBufferToBlack(255 - TransitionAlpha);
        }

        #endregion
    }
}
