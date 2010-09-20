#region Using Statements
using Microsoft.Xna.Framework;
#endregion

namespace Angels_Vs_Demons
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields

        MenuEntry numMenuEntry;

        static int num = 23;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base("Options")
        {
            // Create our menu entries.
            numMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();

            MenuEntry backMenuEntry = new MenuEntry("Back");

            // Hook up menu event handlers.
            numMenuEntry.Selected += numMenuEntrySelected;
            backMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(numMenuEntry);
            MenuEntries.Add(backMenuEntry);
        }


        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            numMenuEntry.Text = "number: " + num;
        }


        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler for when the num menu entry is selected.
        /// </summary>
        void numMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            num++;

            SetMenuEntryText();
        }


        #endregion
    }
}
