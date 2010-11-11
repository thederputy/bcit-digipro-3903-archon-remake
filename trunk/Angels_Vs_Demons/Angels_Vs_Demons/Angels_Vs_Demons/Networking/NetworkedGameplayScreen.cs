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
using Angels_Vs_Demons.Players;
#endregion


namespace Angels_Vs_Demons.Networking
{
    class NetworkedGameplayScreen : GameplayScreen
    {
        const int maxGamers = 2;
        const int maxLocalGamers = 2;

        KeyboardState currentKeyboardState;
        GamePadState currentGamePadState;

        NetworkSession networkSession;

        PacketWriter packetWriter = new PacketWriter();
        PacketReader packetReader = new PacketReader();

        string errorMessage;

        public NetworkedGameplayScreen(bool isHost)
            : base()
        {
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
        /// Menu screen provides options to create or join network sessions.
        /// </summary>
        void UpdateMenuScreen()
        {
            if (IsActive)
            {
                if (IsPressed(Keys.A, Buttons.A))
                {
                    // Create a new session?
                    CreateSession();
                }
                else if (IsPressed(Keys.B, Buttons.B))
                {
                    // Join an existing session?
                    JoinSession();
                }
            }
        }

        /// <summary>
        /// Starts hosting a new network session.
        /// </summary>
        void CreateSession()
        {
            //DrawMessage("Creating session...");

            try
            {
                networkSession = NetworkSession.Create(NetworkSessionType.SystemLink,
                                                       maxLocalGamers, maxGamers);

                HookSessionEvents();
                
                //TODO: add code for being the angel player, going first, etc.
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
                            NetworkSession.Find(NetworkSessionType.SystemLink,
                                                maxLocalGamers, null))
                {
                    if (availableSessions.Count == 0)
                    {
                        errorMessage = "No network sessions found.";
                        //spriteBatch.DrawString(board.debugFont, errorMessage, new Vector2(100, 400), Color.Black);
                        return;
                    }

                    // Join the first session we found.
                    networkSession = NetworkSession.Join(availableSessions[0]);

                    HookSessionEvents();
                    
                    //TODO: add code for being the demon player, going second etc.
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
            //not quite sure yet how this code will be used by our game, as each player maintains
            // a copy of the board
            int gamerIndex = networkSession.AllGamers.IndexOf(e.Gamer);
            if (gamerIndex == 0)
            {
                e.Gamer.Tag = new HumanPlayer(Faction.ANGEL);
            }
            else
            {
                e.Gamer.Tag = new HumanPlayer(Faction.DEMON);
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
            // Look up the cursor associated with this local player.
            //Tile currentTile = gamer.Tag as Tile;
            GameObject cursor = gamer.Tag as GameObject;
            
            // Update the cursor.
            if(cursor != null)
                ReadTileInput(cursor, gamer.SignedInGamer.PlayerIndex);


            // Write the unit state into a network packet.
            packetWriter.Write(board.GetCurrentTile().position);

            // Send the data to everyone in the session.
            gamer.SendData(packetWriter, SendDataOptions.InOrder);
        }

        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.isActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
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
        /// Handles input.
        /// </summary>
        private void HandleInput()
        {
            currentKeyboardState    = Keyboard.GetState();
            currentGamePadState     = GamePad.GetState(PlayerIndex.One);

            // Check for exit.
            if (IsActive && IsPressed(Keys.Escape, Buttons.Back))
            {
                //exit the program
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
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

                // Look up the cursor with whoever sent this packet.
                GameObject remoteCursor = sender.Tag as GameObject;

                // Read the state of this current tile from the network packet.
                remoteCursor.position = packetReader.ReadVector2();

                //move our cursor to match what the remote player did
                board.moveCursor((int)remoteCursor.position.X, (int)remoteCursor.position.Y);
            }
        }

        #endregion


        #region Input Handling

        /// <summary>
        /// Reads input data from keyboard and gamepad, and stores
        /// it into the cursor
        /// </summary>
        void ReadTileInput(GameObject cursor, PlayerIndex playerIndex)
        {
            // Read the gamepad.
            GamePadState gamePadState = GamePad.GetState(playerIndex);

            //Vector2 unitInput = gamePad.ThumbSticks.Left;

            // Read the keyboard.
            KeyboardState keyboardState = Keyboard.GetState(playerIndex);

            if (keyboardState.IsKeyDown(Keys.Left) && !previousKeyboardState.IsKeyDown(Keys.Left))
            {
                board.moveCursor(-1, 0);
                //update cursor position to be sent over network
                cursor.position = board.GetCurrentTile().position;
            }

            if (keyboardState.IsKeyDown(Keys.Right) && !previousKeyboardState.IsKeyDown(Keys.Right))
            {
                board.moveCursor(1, 0);
                //update cursor position to be sent over network
                cursor.position = board.GetCurrentTile().position;
            }

            if (keyboardState.IsKeyDown(Keys.Up) && !previousKeyboardState.IsKeyDown(Keys.Up))
            {
                board.moveCursor(0, -1);
                //update cursor position to be sent over network
                cursor.position = board.GetCurrentTile().position;
            }

            if (keyboardState.IsKeyDown(Keys.Down) && !previousKeyboardState.IsKeyDown(Keys.Down))
            {
                board.moveCursor(0, 1);
                //update cursor position to be sent over network
                cursor.position = board.GetCurrentTile().position;
            }
        }   

        #endregion
    }
}
