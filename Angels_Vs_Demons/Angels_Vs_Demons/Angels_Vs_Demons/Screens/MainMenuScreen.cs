#region Using Statements
using Microsoft.Xna.Framework;
using Angels_Vs_Demons.Networking;
#endregion

namespace Angels_Vs_Demons
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
            MenuEntry HvHMenuEntry = new MenuEntry("Human Vs Human");
            MenuEntry HvAMenuEntry = new MenuEntry("Human Vs Ai");
            MenuEntry AvAMenuEntry = new MenuEntry("Ai Vs Ai");
            MenuEntry networkedGameMenuEntry = new MenuEntry("Networked Game");
            MenuEntry optionsMenuEntry = new MenuEntry("Options");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

            // Hook up menu event handlers.
            HvHMenuEntry.Selected += HvHMenuEntrySelected;
            HvAMenuEntry.Selected += HvAMenuEntrySelected;
            AvAMenuEntry.Selected += AvAMenuEntrySelected;
            networkedGameMenuEntry.Selected += networkedGameMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(HvHMenuEntry);
            MenuEntries.Add(HvAMenuEntry);
            MenuEntries.Add(AvAMenuEntry);
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
            ScreenManager.AddScreen(new HvHMenuScreen(), e.PlayerIndex);
        }

        void HvAMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new HvAMenuScreen(), e.PlayerIndex);
        }

        void AvAMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new AvAMenuScreen(), e.PlayerIndex);
        }

        void networkedGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new NetworkedGameScreen(), e.PlayerIndex);
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
