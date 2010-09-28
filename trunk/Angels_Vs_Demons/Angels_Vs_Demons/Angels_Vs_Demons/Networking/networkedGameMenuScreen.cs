using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Angels_Vs_Demons.Networking;

namespace Angels_Vs_Demons
{
    class networkedGameMenuScreen : GameplayScreen
    {
        #region Fields

        const int screenWidth = 1067;
        const int screenHeight = 600;

        const int maxGamers = 16;
        const int maxLocalGamers = 4;

        KeyboardState currentKeyboardState;
        GamePadState currentGamePadState;

        NetworkSession networkSession;

        PacketWriter packetWriter = new PacketWriter();
        PacketReader packetReader = new PacketReader();

        string errorMessage;

        #endregion

        public networkedGameMenuScreen()
        {

        }

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
            int gamerIndex = networkSession.AllGamers.IndexOf(e.Gamer);
            e.Gamer.Tag = new networkUnit(content.Load<Texture2D>("blank"), gamerIndex, content);
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
            // Update our locally controlled tanks, and send their
            // latest position data to everyone in the session.
            foreach (LocalNetworkGamer gamer in networkSession.LocalGamers)
            {
                UpdateLocalGamer(gamer);
            }

            // Pump the underlying session object.
            networkSession.Update();

            // Make sure the session has not ended.
            if (networkSession == null)
                return;

            // Read any packets telling us the positions of remotely controlled tanks.
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
            // Look up what tank is associated with this local player.
            networkUnit localUnit = gamer.Tag as networkUnit;

            // Update the tank.
            ReadUnitInputs(localUnit, gamer.SignedInGamer.PlayerIndex);

            localUnit.Update();

            // Write the unit state into a network packet.
            packetWriter.Write(localUnit.position);

            // Send the data to everyone in the session.
            gamer.SendData(packetWriter, SendDataOptions.InOrder);
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

                // Look up the unit associated with whoever sent this packet.
                networkUnit remoteUnit = sender.Tag as networkUnit;

                // Read the state of this tank from the network packet.
                remoteUnit.position = packetReader.ReadVector2();
            }
        }

        #region Handle Input


        /*/// <summary>
        /// Handles input.
        /// </summary>
        private void HandleInput()
        {
            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            // Check for exit.
            if (IsActive && IsPressed(Keys.Escape, Buttons.Back))
            {
                Exit();
            }
        }*/


        /// <summary>
        /// Checks if the specified button is pressed on either keyboard or gamepad.
        /// </summary>
        bool IsPressed(Keys key, Buttons button)
        {
            return (currentKeyboardState.IsKeyDown(key) ||
                    currentGamePadState.IsButtonDown(button));
        }


        /// <summary>
        /// Reads input data from keyboard and gamepad, and stores
        /// it into the specified tank object.
        /// </summary>
        void ReadUnitInputs(networkUnit unit, PlayerIndex playerIndex)
        {
            // Read the gamepad.
            GamePadState gamePad = GamePad.GetState(playerIndex);

            Vector2 unitInput = gamePad.ThumbSticks.Left;

            // Read the keyboard.
            KeyboardState keyboard = Keyboard.GetState(playerIndex);

            if (keyboard.IsKeyDown(Keys.Left))
                unitInput.X = -1;
            else if (keyboard.IsKeyDown(Keys.Right))
                unitInput.X = 1;

            if (keyboard.IsKeyDown(Keys.Up))
                unitInput.Y = 1;
            else if (keyboard.IsKeyDown(Keys.Down))
                unitInput.Y = -1;

            /*if (keyboard.IsKeyDown(Keys.A))
                turretInput.X = -1;
            else if (keyboard.IsKeyDown(Keys.D))
                turretInput.X = 1;

            if (keyboard.IsKeyDown(Keys.W))
                turretInput.Y = 1;
            else if (keyboard.IsKeyDown(Keys.S))
                turretInput.Y = -1;*/

            // Normalize the input vectors.
            if (unitInput.Length() > 1)
                unitInput.Normalize();

            /*if (turretInput.Length() > 1)
                turretInput.Normalize();*/

            // Store these input values into the unit object.
            unit.unitInput = unitInput;
            //unit.TurretInput = turretInput;
        }


            #endregion
        
    }
}
