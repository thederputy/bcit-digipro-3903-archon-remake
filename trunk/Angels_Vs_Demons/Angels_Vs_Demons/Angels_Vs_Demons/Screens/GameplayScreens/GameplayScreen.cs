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
using Angels_Vs_Demons.BoardObjects.Spells;
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

        protected AvDGame game;

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

            game = new AvDGame(content);
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
                    Turn turn = cp.getTurn(game);
                    game.applyTurn(turn);
                    Console.WriteLine("DONE CHANGING STUFF");
                }

                //check for end of turn
                if (!game.MovePhase && !game.AttackPhase)
                {
                    game.endTurn();    //end the current turn
                    
                    //switch the controlling players
                    Player tempPlayer = CurrentPlayer;
                    CurrentPlayer = NextPlayer;
                    NextPlayer = tempPlayer;

                    game.beginTurn();  //begin the next turn
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
                    game.moveCursor(-1, 0);
                }
                else if (keyboardState.IsKeyDown(Keys.Right) && !previousKeyboardState.IsKeyDown(Keys.Right))
                {
                    game.moveCursor(1, 0);
                }
                else if (keyboardState.IsKeyDown(Keys.Up) && !previousKeyboardState.IsKeyDown(Keys.Up))
                {
                    game.moveCursor(0, -1);
                }
                else if (keyboardState.IsKeyDown(Keys.Down) && !previousKeyboardState.IsKeyDown(Keys.Down))
                {
                    game.moveCursor(0, 1);
                }
                else if (keyboardState.IsKeyDown(Keys.Enter) && !previousKeyboardState.IsKeyDown(Keys.Enter))
                {
                    makeAction();
                }
                else if (keyboardState.IsKeyDown(Keys.E) && !previousKeyboardState.IsKeyDown(Keys.E))
                {

                    if (game.MovePhase == true)
                    {
                        game.endMovePhaseNoMove();
                    }
                    else if (game.AttackPhase == true)
                    {
                        game.endAttackPhase();
                    }
                }
                else if (keyboardState.IsKeyDown(Keys.U) && !previousKeyboardState.IsKeyDown(Keys.U))
                {
                    if (game.AttackPhase == true)
                    {
                        game.undoLastMove();
                    }
                }
                else if (keyboardState.IsKeyDown(Keys.D1) && !previousKeyboardState.IsKeyDown(Keys.D1))
                {
                    if (game.IsChampionAttack)
                    {
                        game.attackFinder.bitMaskAllTilesAsNotAttackable();
                        game.attackFinder.findAttacksForSpellType(game.selectedTile, SpellValues.spellTypes.BOLT);
                        game.SpellType = SpellValues.spellTypes.BOLT;
                        Debug.WriteLine("Finding BOLT attacks");
                    }
                }
                else if (keyboardState.IsKeyDown(Keys.D2) && !previousKeyboardState.IsKeyDown(Keys.D2))
                {
                    if (game.IsChampionAttack)
                    {
                        game.attackFinder.bitMaskAllTilesAsNotAttackable();
                        game.attackFinder.findAttacksForSpellType(game.selectedTile, SpellValues.spellTypes.STUN);
                        game.SpellType = SpellValues.spellTypes.STUN;
                        Debug.WriteLine("Finding STUN attacks");
                    }
                }
                else if (keyboardState.IsKeyDown(Keys.D3) && !previousKeyboardState.IsKeyDown(Keys.D3))
                {
                    if (game.IsChampionAttack)
                    {
                        game.attackFinder.bitMaskAllTilesAsNotAttackable();
                        game.attackFinder.findAttacksForSpellType(game.selectedTile, SpellValues.spellTypes.TELE);
                        game.SpellType = SpellValues.spellTypes.TELE;
                        Debug.WriteLine("Finding TELE attacks");
                    }
                }
                else if (keyboardState.IsKeyDown(Keys.D4) && !previousKeyboardState.IsKeyDown(Keys.D4))
                {
                    if (game.IsChampionAttack)
                    {
                        game.attackFinder.bitMaskAllTilesAsNotAttackable();
                        game.attackFinder.findAttacksForSpellType(game.selectedTile, SpellValues.spellTypes.BUFF);
                        game.SpellType = SpellValues.spellTypes.BUFF;
                        Debug.WriteLine("Finding BUFF attacks");
                    }
                }
                else if (keyboardState.IsKeyDown(Keys.D5) && !previousKeyboardState.IsKeyDown(Keys.D5))
                {
                    if (game.IsChampionAttack)
                    {
                        game.attackFinder.bitMaskAllTilesAsNotAttackable();
                        game.attackFinder.findAttacksForSpellType(game.selectedTile, SpellValues.spellTypes.HEAL);
                        game.SpellType = SpellValues.spellTypes.HEAL;
                        Debug.WriteLine("Finding HEAL attacks");
                    }
                }
                else if (keyboardState.IsKeyDown(Keys.D6) && !previousKeyboardState.IsKeyDown(Keys.D6))
                {
                    if (game.IsChampionAttack)
                    {
                        Attack rest = new Rest(game.selectedTile.position, game.selectedTile.position);
                        Debug.WriteLine("I'm just resting");
                        game.applyAttack(rest);
                        
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
            if (game.MovePhase)
            {
                processMovePhase();
            }
            if (game.AttackPhase)
            {
                if (game.selectedTile != null)
                {
                    if (game.selectedTile.Unit is Champion)
                    {
                        processChampionAttackPhase();
                    }
                    else
                    {
                        processAttackPhase();
                    }
                }
                else
                {
                    Unit championCheck = game.GetCurrentTile().Unit;
                    if (championCheck != null && championCheck is Champion)
                    {
                        processChampionAttackPhase();
                    }
                    else
                    {
                        processAttackPhase();
                    }
                }
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
            Tile currentTile = game.GetCurrentTile();
#if DEBUG
            Debug.WriteLine("currentTile.IsUsable: " + currentTile.IsUsable);
#endif
            if (currentTile.IsUsable)
            {
                //check that there is a tile selected
                if (game.selectedTile != null)
                {
                    //if we've selected the same tile again, deselect it
                    if (currentTile.position == game.selectedTile.position)
                    {
                        game.selectedTile = null;
#if DEBUG
                        Debug.WriteLine("selected tile = null");
#endif
                    }
                    else
                    {
#if DEBUG
                        Debug.WriteLine("updating selected tile");
#endif
                        game.selectedTile = currentTile;
                    }
                }
                else
                {
#if DEBUG
                    Debug.WriteLine("updating selected tile");
#endif
                    game.selectedTile = currentTile;
                }
            }
            else
            {
                //the currentTile is not occupied
                //if there is a selected tile, check to see if currentTile is within its move range
                if (game.selectedTile != null && (currentTile.MoveID & game.selectedTile.Unit.ID) != 0)
                {
                    //execute the move phase
                    executeMovePhase(currentTile, game.selectedTile);
                }
            }
        }

        /// <summary>
        /// Executes the move phase.
        /// This should be overridden in each individual gameplayscreen, though they will be similar implementations
        /// </summary>
        protected virtual void executeMovePhase(Tile currentTile, Tile boardSelectedTile)
        {
            game.applyMove(new Move(currentTile.position, boardSelectedTile.position));
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
            Tile currentTile = game.GetCurrentTile();
#if DEBUG
            Debug.WriteLine("currentTile.IsUsable: " + currentTile.IsUsable);
#endif
            if (currentTile.IsUsable)
            {
                //check that there is a tile selected
                if (game.selectedTile != null)
                {
                    //if we've selected the same tile again, attack phase is over 
                    //(not sure if this is the best implementation)
                    if (currentTile.position == game.selectedTile.position)
                    {
                        game.selectedTile = null;
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
                        if (currentTile.Unit is Champion)
                        {
                            processChampionAttackPhase();
                        }
                        else
                        {
                            game.selectedTile = currentTile;
                        }
                    }
                }
                else
                {
#if DEBUG
                    Debug.WriteLine("no tile selected, selecting current tile");
                    Debug.WriteLine("updating selected tile");
#endif
                    game.selectedTile = currentTile;
                }
            }
            else
            {
                //we've selected a tile that is not one of ours.
                //if there is a selected tile, check to see if the current tile is within our attack range
                if (game.selectedTile != null && (currentTile.AttackID & game.selectedTile.Unit.ID) != 0)
                {
                    //if the current tile has a unit on it
                    if (currentTile.IsOccupied)
                    {
#if DEBUG
                        Debug.WriteLine("executing attack phase");
#endif
                        //execute the attack phase
                        executeAttackPhase(currentTile, game.selectedTile);
                    }
                }
            }
        }

        private void processChampionAttackPhase()
        {
#if DEBUG
            //board.showAttackBitMasks();
#endif
            Tile currentTile = game.GetCurrentTile();
#if DEBUG
            Debug.WriteLine("currentTile.IsUsable: " + currentTile.IsUsable);
#endif
            if (currentTile.IsUsable)
            {
                //check that there is a tile selected
                if (game.selectedTile != null)
                {
                    //if we've selected the same tile again
                    if (currentTile.position == game.selectedTile.position)
                    {
                        game.selectedTile = null;
                        game.IsChampionAttack = false;
#if DEBUG
                        Debug.WriteLine("selected the same tile again");
                        Debug.WriteLine("selected tile = null");
#endif
                    }
                    else
                    {
                        //we've selected a tile that is not one of ours.
                        //if there is a selected tile, check to see if the current tile is within our attack range
                        if (game.selectedTile != null && (currentTile.AttackID & game.selectedTile.Unit.ID) != 0)
                        {
                            //if the current tile has a unit on it
                            if (currentTile.IsOccupied)
                            {
#if DEBUG
                                Debug.WriteLine("executing attack phase");
#endif
                                //execute the attack phase
                                executeChampionAttackPhase(currentTile, game.selectedTile);
                            }
                        }
//#if DEBUG
//                        Debug.WriteLine("selected a new tile");
//                        Debug.WriteLine("updating selected tile");
//#endif
//                        game.selectedTile = currentTile;
//                        game.attackFinder.findAttacksForTile(game.selectedTile);
//                        game.attackFinder.bitMaskAllTilesForChampionAsNotAttackable(game.selectedTile.Unit.ID);
//                        game.IsChampionAttack = true;
                    }
                }
                else
                {
                    //We're selecting another one of our usable units
#if DEBUG
                    Debug.WriteLine("no tile selected, selecting current tile");
                    Debug.WriteLine("updating selected tile");
#endif
                    game.selectedTile = currentTile;
                    game.attackFinder.findAttacksForTile(game.selectedTile);
                    game.attackFinder.bitMaskAllTilesForChampionAsNotAttackable(game.selectedTile.Unit.ID);
                    game.IsChampionAttack = true;
                }
            }
            else
            {
                //we've selected a tile that is not one of ours.
                //if there is a selected tile, check to see if the current tile is within our attack range
                if (game.selectedTile != null && (currentTile.AttackID & game.selectedTile.Unit.ID) != 0)
                {
                    //if the current tile has a unit on it
                    if (currentTile.IsOccupied)
                    {
#if DEBUG
                        Debug.WriteLine("executing attack phase");
#endif
                        //execute the attack phase
                        executeChampionAttackPhase(currentTile, game.selectedTile);
                    }
                }
            }
            Console.WriteLine("isChampionAttack: " + game.IsChampionAttack);
        }

        /// <summary>
        /// Executes the attack phase.
        /// This should be overridden in each individual gameplayscreen, though they will be similar implementations
        /// </summary>
        /// <param name="victimTile">the tile that is getting attacked</param>
        /// <param name="attackerTile">the tile that is attacking</param>
        protected virtual void executeAttackPhase(Tile victimTile, Tile attackerTile)
        {
            game.applyAttack(new Attack(victimTile.position, attackerTile.position));
        }

        /// <summary>
        /// Executes the champion's attack phase.
        /// This should be overridden in each individual gameplayscreen, though they will be similar implementations
        /// </summary>
        /// <param name="victimTile">the tile that is getting attacked</param>
        /// <param name="attackerTile">the tile that is attacking</param>
        protected virtual void executeChampionAttackPhase(Tile victimTile, Tile attackerTile)
        {
            Attack spell = null;
            switch (game.SpellType)
            {
                case SpellValues.spellTypes.BOLT:
                    spell = new Bolt(victimTile.position, attackerTile.position);
                    break;
                case SpellValues.spellTypes.BUFF:
                    spell = new Buff(victimTile.position, attackerTile.position);
                    break;
                case SpellValues.spellTypes.HEAL:
                    spell = new Heal(victimTile.position, attackerTile.position);
                    break;
                case SpellValues.spellTypes.REST:
                    spell = new Rest(victimTile.position, attackerTile.position);
                    break;
                case SpellValues.spellTypes.STUN:
                    spell = new Stun(victimTile.position, attackerTile.position);
                    break;
                case SpellValues.spellTypes.TELE:
                    spell = new Teleport(victimTile.position, attackerTile.position);
                    break;
            }
            game.applyAttack(spell);
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

            game.PaintGrid(spriteBatch);
            game.PaintUnits(spriteBatch);

            //Painting information text

            unitDisplayWindow.Draw(spriteBatch, game.GetCurrentTile().Unit);

            spriteBatch.Begin();

            if (game.ControllingFaction == Faction.ANGEL)
            {
                fontPosition.X = 20;
                fontPosition.Y = 20;
                spriteBatch.DrawString(gameFont, "Angel Turn", fontPosition, Color.Black);
                if (game.MovePhase)
                {
                    fontPosition.X = 35;
                    fontPosition.Y = 50;
                    spriteBatch.DrawString(mapFont, "Move Phase", fontPosition, Color.Black);
                }
                if (game.AttackPhase)
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
                if (game.MovePhase)
                {
                    fontPosition.X = ScreenManager.screenWidth - 185;
                    fontPosition.Y = 50;
                    spriteBatch.DrawString(mapFont, "Move Phase", fontPosition, Color.Black);
                }
                if (game.AttackPhase)
                {
                    fontPosition.X = ScreenManager.screenWidth - 185;
                    fontPosition.Y = 50;
                    spriteBatch.DrawString(mapFont, "Attack Phase", fontPosition, Color.Black);
                }
            }

            fontPosition.X = 5;
            fontPosition.Y = 120;

            Dictionary<int, Unit>.ValueCollection AngelValues = game.Angels.Values;

            foreach (Unit u in AngelValues)
            {
                spriteBatch.DrawString(mapFont, u.Name + "HP: " + u.CurrHP + "/" + u.TotalHP, fontPosition, Color.Black);
                fontPosition.Y += mapFont.LineSpacing + 2;
            }

            fontPosition.X = 635;
            fontPosition.Y = 120;

            Dictionary<int, Unit>.ValueCollection DemonValues = game.Demons.Values;

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
