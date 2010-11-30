#region Using Statements
using Angels_Vs_Demons.Screens;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Net;
#endregion

namespace Angels_Vs_Demons.Networking
{
    class NetworkedMenuScreen : MenuScreen
    {
        #region Fields

        const int screenWidth = 1067;
        const int screenHeight = 600;

        const int maxGamers = 16;
        const int maxLocalGamers = 4;

        bool isHost;

        PacketWriter packetWriter = new PacketWriter();
        PacketReader packetReader = new PacketReader();

        #endregion

        public NetworkedMenuScreen()
            : base("Network Lobby")
        {
            if (Gamer.SignedInGamers.Count == 0)
            {
                // If there are no profiles signed in, we cannot proceed.
                // Show the Guide so the user can sign in.
                Guide.ShowSignIn(maxLocalGamers, false);
            }
            // Create our menu entries.
            MenuEntry HostLocalSessionEntry = new MenuEntry("Host LAN Game");
            MenuEntry JoinLocalSessionEntry = new MenuEntry("Join LAN Game");
            MenuEntry HostOnlineSessionEntry = new MenuEntry("Host Online Game");
            MenuEntry JoinOnlineSessionEntry = new MenuEntry("Join Online Game");
            MenuEntry ExitMenuEntry = new MenuEntry("Back");

            // Hook up menu event handlers.
            HostLocalSessionEntry.Selected += HostLocalSessionSelected;
            JoinLocalSessionEntry.Selected += JoinLocalSessionSelected;
            HostOnlineSessionEntry.Selected += HostOnlineSessionSelected;
            JoinOnlineSessionEntry.Selected += JoinOnlineSessionSelected;
            ExitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(HostLocalSessionEntry);
            MenuEntries.Add(JoinLocalSessionEntry);
            MenuEntries.Add(HostOnlineSessionEntry);
            MenuEntries.Add(JoinOnlineSessionEntry);
            MenuEntries.Add(ExitMenuEntry);
        }

        #region Handle Input
        // Event handlers for when the menu options are selected

        /// <summary>
        /// Hosts a system link game with this player as the Angel faction.
        /// </summary>
        /// <param name="sender">Not sure what this parameter is.</param>
        /// <param name="e">This one either</param>
        void HostLocalSessionSelected(object sender, PlayerIndexEventArgs e)
        {
            //create the new session
            isHost = true;
            ScreenManager.AddScreen(new NetworkedGameplayScreen(isHost, NetworkSessionType.SystemLink), e.PlayerIndex);
        }

        /// <summary>
        /// Joins a system link game with this player as the Demon faction.
        /// </summary>
        /// <param name="sender">Not sure what this parameter is.</param>
        /// <param name="e">This one either</param>
        void JoinLocalSessionSelected(object sender, PlayerIndexEventArgs e)
        {
            //search for sessions and join one if found
            isHost = false;
            ScreenManager.AddScreen(new NetworkedGameplayScreen(isHost, NetworkSessionType.SystemLink), e.PlayerIndex);
        }

        /// <summary>
        /// Hosts an online game using XBOX LIVE servers with this player as the Angel faction.
        /// </summary>
        /// <param name="sender">Not sure what this parameter is.</param>
        /// <param name="e">This one either</param>
        void HostOnlineSessionSelected(object sender, PlayerIndexEventArgs e)
        {
            //create the new session
            isHost = true;
            ScreenManager.AddScreen(new NetworkedGameplayScreen(isHost, NetworkSessionType.PlayerMatch), e.PlayerIndex);
        }

        /// <summary>
        /// Joins an online game using XBOX LIVE servers with this player as the Demon faction.
        /// </summary>
        /// <param name="sender">Not sure what this parameter is.</param>
        /// <param name="e">This one either</param>
        void JoinOnlineSessionSelected(object sender, PlayerIndexEventArgs e)
        {
            //search for sessions and join one if found
            isHost = false;
            ScreenManager.AddScreen(new NetworkedGameplayScreen(isHost, NetworkSessionType.PlayerMatch), e.PlayerIndex);
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

        #region Session Initialization
        // I will be moving all the session initialization here.
        // That way I will just pass the network session to the Networked Gameplayscreen.
        #endregion
    }
}