#region Using Statements

#endregion

namespace Angels_Vs_Demons.Screens
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
        MenuEntry AIMenuEntry;

        static int num = 23;

        static int AIdifficulty = 0;

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
            AIMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();

            MenuEntry backMenuEntry = new MenuEntry("Back");

            // Hook up menu event handlers.
            numMenuEntry.Selected += numMenuEntrySelected;
            AIMenuEntry.Selected += AIMenuEntrySelected;
            backMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(numMenuEntry);
            MenuEntries.Add(AIMenuEntry);
            MenuEntries.Add(backMenuEntry);
        }


        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            numMenuEntry.Text = "number: " + num;
            switch (AIdifficulty)
            {
                case 0:
                    AIMenuEntry.Text = "AI difficulty: EASY";
                    break;
                case 1:
                    AIMenuEntry.Text = "AI difficulty: MEDIUM";
                    break;
                case 2:
                    AIMenuEntry.Text = "AI difficulty: HARD";
                    break;
            }
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

        /// <summary>
        /// Event handler for when the AI menu entry is selected.
        /// </summary>
        void AIMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (AIdifficulty < 2)
            {
                AIdifficulty++;
            }
            else
            {
                AIdifficulty = 0;
            }

            SetMenuEntryText();
        }



        #endregion
    }
}
