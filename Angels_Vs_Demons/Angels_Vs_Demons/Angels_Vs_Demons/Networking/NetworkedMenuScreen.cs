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
    class NetworkedMenuScreen : MenuScreen
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

        public NetworkedMenuScreen()
            : base("Network Lobby")
        {
            // Create our menu entries.
            MenuEntry CreateSessionEntry = new MenuEntry("Create Session");
            MenuEntry JoinSessionEntry = new MenuEntry("Join Session");
            MenuEntry ExitMenuEntry = new MenuEntry("Back");

            // Hook up menu event handlers.
            CreateSessionEntry.Selected += CreateSessionSelected;
            JoinSessionEntry.Selected += JoinSessionSelected;
            ExitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(CreateSessionEntry);
            MenuEntries.Add(JoinSessionEntry);
            MenuEntries.Add(ExitMenuEntry);
        }
        #region Handle Input

        /// <summary>
        /// Event handler for when the menu options are selected
        /// </summary>

        void CreateSessionSelected(object sender, PlayerIndexEventArgs e)
        {
            //create the new session
            if (Gamer.SignedInGamers.Count == 0)
            {
                // If there are no profiles signed in, we cannot proceed.
                // Show the Guide so the user can sign in.
                Guide.ShowSignIn(maxLocalGamers, false);
            }
            ScreenManager.AddScreen(new NetworkedGameplayScreen(true), e.PlayerIndex);
        }

        void JoinSessionSelected(object sender, PlayerIndexEventArgs e)
        {
            //search for sessions and join one if found
            if (Gamer.SignedInGamers.Count == 0)
            {
                // If there are no profiles signed in, we cannot proceed.
                // Show the Guide so the user can sign in.
                Guide.ShowSignIn(maxLocalGamers, false);
            }
            ScreenManager.AddScreen(new NetworkedGameplayScreen(false), e.PlayerIndex);
        }

        void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }

        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }


        #endregion
    }
}