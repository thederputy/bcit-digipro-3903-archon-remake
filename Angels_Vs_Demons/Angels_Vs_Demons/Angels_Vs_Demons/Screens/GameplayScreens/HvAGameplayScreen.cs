#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Angels_Vs_Demons.BoardObjects;
using Angels_Vs_Demons.GameObjects;
using Angels_Vs_Demons.Players;
#endregion

namespace Angels_Vs_Demons.Screens.GameplayScreens
{
    /// <summary>
    /// Represents a Human vs AI game.
    /// </summary>
    class HvAGameplayScreen : GameplayScreen
    {
        #region Initialization

        /// <summary>
        /// Constructs a gameplay screen with one human player and one AI player.
        /// </summary>
        public HvAGameplayScreen()
            :base()
        {
            Player1 = new HumanPlayer(Faction.ANGEL);
            Player2 = new ComputerPlayer(Faction.DEMON);
        }

        #endregion
    }
}
