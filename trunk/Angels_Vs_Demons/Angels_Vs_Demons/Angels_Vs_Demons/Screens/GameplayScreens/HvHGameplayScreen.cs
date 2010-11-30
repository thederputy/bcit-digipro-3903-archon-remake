#region Using Statements
using Angels_Vs_Demons.Players;
#endregion

namespace Angels_Vs_Demons.Screens.GameplayScreens
{
    /// <summary>
    /// Represents a Human vs Human (hotseat) game.
    /// </summary>
    class HvHGameplayScreen : GameplayScreen
    {
        #region Initialization

        /// <summary>
        /// Constructs a gameplay screen with two human players.
        /// </summary>
        public HvHGameplayScreen()
            :base()
        {
            Player1 = new HumanPlayer(Faction.ANGEL);
            Player2 = new HumanPlayer(Faction.DEMON);
        }

        #endregion
    }
}
