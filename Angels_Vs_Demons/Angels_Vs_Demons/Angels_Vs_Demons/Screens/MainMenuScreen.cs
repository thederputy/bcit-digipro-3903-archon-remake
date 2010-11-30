#region Using Statements
using Angels_Vs_Demons.Networking;
using Angels_Vs_Demons.Screens.GameplayScreens;
using Angels_Vs_Demons.Screens.ScreenManagers;
using Microsoft.Xna.Framework;
#endregion

namespace Angels_Vs_Demons.Screens
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class MainMenuScreen : MenuScreen
    {
        #region Initialization


        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen()
            : base("Main Menu")
        {
            // Create our menu entries.
            MenuEntry HvHMenuEntry = new MenuEntry("Hotseat game");
            MenuEntry HvAMenuEntry = new MenuEntry("Vs Computer");
//#if DEBUG
            MenuEntry AvAMenuEntry = new MenuEntry("Ai Vs Ai");
//#endif
            MenuEntry networkedGameMenuEntry = new MenuEntry("Networked Game");
            MenuEntry optionsMenuEntry = new MenuEntry("Options");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

            // Hook up menu event handlers.
            HvHMenuEntry.Selected += HvHMenuEntrySelected;
            HvAMenuEntry.Selected += HvAMenuEntrySelected;
//#if DEBUG
            AvAMenuEntry.Selected += AvAMenuEntrySelected;
//#endif
            networkedGameMenuEntry.Selected += networkedGameMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(HvHMenuEntry);
            MenuEntries.Add(HvAMenuEntry);
//#if DEBUG
            MenuEntries.Add(AvAMenuEntry);
//#endif
            MenuEntries.Add(networkedGameMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }


        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler for when the menu options are selected
        /// </summary>

        void HvHMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new HvHGameplayScreen(), e.PlayerIndex);
        }

        void HvAMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new HvAGameplayScreen(), e.PlayerIndex);
        }

//#if DEBUG
        void AvAMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new AvAGameplayScreen(), e.PlayerIndex);
        }
//#endif

        void networkedGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new NetworkedMenuScreen(), e.PlayerIndex);
        }

        void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }


        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to exit this game?";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
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
