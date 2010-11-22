#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Angels_Vs_Demons.BoardObjects;
using Angels_Vs_Demons.GameObjects;
using Angels_Vs_Demons.Players;
using Angels_Vs_Demons.Screens;
using Angels_Vs_Demons.Screens.GameplayScreens;
#endregion


namespace Angels_Vs_Demons.Networking
{
    class NetworkedGameplayScreen : GameplayScreen
    {
        const int maxGamers = 2;
        const int maxLocalGamers = 2;

        Vector2 previousCursorPosition;

        KeyboardState currentKeyboardState;
        GamePadState currentGamePadState;

        NetworkSession networkSession;
        NetworkSessionType sessionType;

        PacketWriter packetWriter = new PacketWriter();
        PacketReader packetReader = new PacketReader();

        Move localMove;
        Attack localAttack;
        Turn localTurn;

        Gamer localGamer;

        string errorMessage;

        public NetworkedGameplayScreen(bool isHost, NetworkSessionType type)
            : base()
        {
            sessionType = type;
            previousCursorPosition = new Vector2(0, 0);
            player1 = new HumanPlayer(Faction.ANGEL);
            player2 = new HumanPlayer(Faction.DEMON);

            localMove = null;
            localAttack = null;
            localTurn = null;

            if (isHost)
            {
                CreateSession();
            }
            else
            {
                JoinSession();
            }
        }

        /// <summary>
        /// Checks if the specified button is pressed on either keyboard or gamepad.
        /// </summary>
        bool IsPressed(Keys key, Buttons button)
        {
            return (currentKeyboardState.IsKeyDown(key) ||
                    currentGamePadState.IsButtonDown(button));
        }

        #region Networking

        /// <summary>
        /// Starts hosting a new network session.
        /// </summary>
        void CreateSession()
        {
            //DrawMessage("Creating session...");

            try
            {
                networkSession = NetworkSession.Create(sessionType, maxLocalGamers, maxGamers);

                HookSessionEvents();
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }
        }


        /// <summary>
        /// Joins an existing network session.
        /// </summary>
        void JoinSession()
        {
            //DrawMessage("Joining session...");
            //SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            try
            {
                // Search for sessions.
                using (AvailableNetworkSessionCollection availableSessions =
                            NetworkSession.Find(sessionType, maxLocalGamers, null))
                {
                    if (availableSessions.Count == 0)
                    {
                        errorMessage = "No network sessions found.";
                        //spriteBatch.DrawString(board.debugFont, errorMessage, new Vector2(100, 400), Color.Black);
                        Console.WriteLine(errorMessage);
                        return;
                    }

                    // Join the first session we found.
                    networkSession = NetworkSession.Join(availableSessions[0]);

                    HookSessionEvents();
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }
        }

        // <summary>
        /// After creating or joining a network session, we must subscribe to
        /// some events so we will be notified when the session changes state.
        /// </summary>
        void HookSessionEvents()
        {
            networkSession.GamerJoined += GamerJoinedEventHandler;
            networkSession.SessionEnded += SessionEndedEventHandler;
        }


        /// <summary>
        /// This event handler will be called whenever a new gamer joins the session.
        /// We use it to allocate a Tank object, and associate it with the new gamer.
        /// </summary>
        void GamerJoinedEventHandler(object sender, GamerJoinedEventArgs e)
        {
            //the creator will have index of 0, so make them player1, the ANGEL player
            int gamerIndex = networkSession.AllGamers.IndexOf(e.Gamer);

            if (gamerIndex == 0)
            {
                e.Gamer.Tag = player1;
            }
            else
            {
                e.Gamer.Tag = player2;
            }

        }


        /// <summary>
        /// Event handler notifies us when the network session has ended.
        /// </summary>
        void SessionEndedEventHandler(object sender, NetworkSessionEndedEventArgs e)
        {
            errorMessage = e.EndReason.ToString();

            networkSession.Dispose();
            networkSession = null;
        }


        /// <summary>
        /// Updates the state of the network session, moving the tanks
        /// around and synchronizing their state over the network.
        /// </summary>
        void UpdateNetworkSession()
        {
            // Confirm our move, send the move data to the other player.
            foreach (LocalNetworkGamer gamer in networkSession.LocalGamers)
            {
                UpdateLocalGamer(gamer);
            }

            // Pump the underlying session object.
            networkSession.Update();

            // Make sure the session has not ended.
            if (networkSession == null)
                return;

            // Read any packets telling us that they made a move.
            foreach (LocalNetworkGamer gamer in networkSession.LocalGamers)
            {
                ReadIncomingPackets(gamer);
            }
        }


        /// <summary>
        /// Helper for updating a locally controlled gamer.
        /// </summary>
        void UpdateLocalGamer(LocalNetworkGamer gamer)
        {
            // Look up the data associated with this local player.
            HumanPlayer hPlayer = gamer.Tag as HumanPlayer;
            localGamer = gamer;

            /*CURSOR VERSION OF NETWORKING
            // Update the cursor.
            if (hPlayer.Position != null)
                ReadPlayerInput(hPlayer, gamer.SignedInGamer.PlayerIndex);

            if (hPlayer.Position != previousCursorPosition)
            {
                // Write the unit state into a network packet.
                packetWriter.Write(hPlayer.Position);

                // Send the data to everyone in the session.
                gamer.SendData(packetWriter, SendDataOptions.InOrder);

                previousCursorPosition = hPlayer.Position;
            }
            */

            //if we haven't stored our turn yet
            if (hPlayer.Turn == null)
            {
                //ReadPlayerInput(hPlayer, gamer.SignedInGamer.PlayerIndex);
                hPlayer.Turn = localTurn;
            }

            //if our turn was stored properly, and we've made one
            if (hPlayer.Turn != null)
            {
                // Write the our turn into a network packet.
                packetWriter.Write(hPlayer.Turn.Move.IsExecutable);
                if (hPlayer.Turn.Move.IsExecutable)
                {
                    //send the Move data
                    packetWriter.Write(hPlayer.Turn.Move.NewTile.position);
                    packetWriter.Write(hPlayer.Turn.Move.PreviousTile.position);
                }

                packetWriter.Write(hPlayer.Turn.Attack.IsExecutable);
                if (hPlayer.Turn.Attack.IsExecutable)
                {
                    //send the Attack data
                    packetWriter.Write(hPlayer.Turn.Attack.VictimTile.position);
                    packetWriter.Write(hPlayer.Turn.Attack.AttackerTile.position);
                }

                // Send the data to everyone in the session.
                gamer.SendData(packetWriter, SendDataOptions.InOrder);
#if DEBUG
                Console.WriteLine("DEBUG: SENDING TURN DATA");
#endif

                //make our local turn null, so we don't send it again
                localMove = null;
                localAttack = null;
                localTurn = null;
                hPlayer.Turn = null;
            }
        }

        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.isActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            HandleInput();

            if (networkSession == null)
            {
                // If we are not in a network session, update the
                // menu screen that will let us create or join one.
                ExitScreen();
            }
            else
            {
                // If we are in a network session, update it.
                UpdateNetworkSession();
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        /// <summary>
        /// Helper for reading incoming network packets.
        /// </summary>
        void ReadIncomingPackets(LocalNetworkGamer gamer)
        {
            // Keep reading as long as incoming packets are available.
            while (gamer.IsDataAvailable)
            {
                NetworkGamer sender;

                // Read a single packet from the network.
                gamer.ReceiveData(packetReader, out sender);

                // Discard packets sent by local gamers: we already know their state!
                if (sender.IsLocal)
                    continue;

                // Look up the position of the cursor with whoever sent this packet.
                HumanPlayer hPlayer = sender.Tag as HumanPlayer;

                /*CURSOR VERSION OF NETWORKING
                // Read the state of this current tile from the network packet.
                hPlayer.Position = packetReader.ReadVector2();

                //move our cursor to match what the remote player did
                board.setCursor((int)hPlayer.Position.X, (int)hPlayer.Position.Y);
#if DEBUG
                Console.WriteLine("Moved remote player's cursor from " + board.GetCurrentTile().position.X
                    + "," + board.GetCurrentTile().position.Y + " to "
                    + hPlayer.Position.X + "," + hPlayer.Position.Y);
#endif
                 *END CURSOR VERSION 
                 */

                bool moveIsExecutable = false;
                bool attackIsExecutable = false;
                Vector2 newTilePosition = Vector2.Zero;
                Vector2 previousTilePosition = Vector2.Zero;
                Vector2 victimTilePosition = Vector2.Zero;
                Vector2 attackerTilePosition = Vector2.Zero;

                Move remoteMove;
                Attack remoteAttack;
                Turn remoteTurn;
                //process remote player's move
                moveIsExecutable = packetReader.ReadBoolean(); //Turn.Move.IsExecutable
                if (moveIsExecutable)
                {
                    newTilePosition = packetReader.ReadVector2();       //Turn.Move.NewTile.position
                    previousTilePosition = packetReader.ReadVector2();  //Turn.Move.PreviousTile.position
                    remoteMove = new Move(board.GetTile(newTilePosition), board.GetTile(previousTilePosition));
                }
                else
                {
                    remoteMove = new Move(null, null);
                }

                //process remote player's attack
                attackIsExecutable = packetReader.ReadBoolean(); //Turn.Attack.IsExecutable
                if (attackIsExecutable)
                {
                    victimTilePosition = packetReader.ReadVector2();    //Turn.Attack.VictimTile.position
                    attackerTilePosition = packetReader.ReadVector2();  //Turn.Attack.AttackerTile.position
                    remoteAttack = new Attack(board.GetTile(victimTilePosition), board.GetTile(attackerTilePosition));
                }
                else
                {
                    remoteAttack = new Attack(null, null);
                }

                remoteTurn = new Turn(remoteMove, remoteAttack);

                if (remoteTurn != null)
                {
                    board.applyTurn(remoteTurn);
                }

                remoteTurn = null;
            }
        }

        #endregion


        #region Input Handling

        /// <summary>
        /// Handles input specific to the NetworkedGameplayScreen.
        /// For basic game mechanics input, see HandleInput in GameplayScreen.
        /// </summary>
        private void HandleInput()
        {
            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            // Check for exit.
            if (IsActive && IsPressed(Keys.Escape, Buttons.Back))
            {
                //exit the program
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
        }

        /// <summary>
        /// Actual gameplay movement of the cursor is handled in Gamplay screen.
        /// This update methods gets the new position of the cursor and assigns it 
        /// to the position of the player that we passed in.
        /// </summary>
        /// <param name="hPlayer">The player we're going to update</param>
        /// <param name="playerIndex">The index, if local player</param>
        void ReadPlayerInput(HumanPlayer hPlayer, PlayerIndex playerIndex)
        {
            hPlayer.Position = board.GetCurrentTile().position;
            //check to see if it is our turn
            if (hPlayer.Faction == board.ControllingFaction)
            {
                hPlayer.Turn = localTurn;
            }
        }

        /// <summary>
        /// Gets called when you press enter on the board.
        /// This is the main loop that processes the gamestate.
        /// </summary>
        protected override void makeAction()
        {
            HumanPlayer hPlayer = localGamer.Tag as HumanPlayer;
            if (hPlayer.Faction == board.ControllingFaction)
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
#if DEBUG
            else
            {
                Console.WriteLine("NOT YOUR TURN, DUMMY!");
            }
#endif
        }

        /// <summary>
        /// Executes the move phase for a networked game.
        /// </summary>
        /// <param name="currentTile">the tile that the cursor is now on.</param>
        /// <param name="boardSelectedTile">the tile that was selected</param>
        protected override void executeMovePhase(Tile currentTile, Tile boardSelectedTile)
        {
            localMove = new Move(currentTile, board.selectedTile);
            board.applyMove(localMove);
            localTurn = new Turn(localMove, new Attack(null, null));
        }

        /// <summary>
        /// Executes the attack phase for a networked game.
        /// </summary>
        /// <param name="victimTile">the tile that is getting attacked</param>
        /// <param name="attackerTile">the tile that is attacking</param>
        protected override void executeAttackPhase(Tile victimTile, Tile attackerTile)
        {

        }

        #endregion
    }
}
