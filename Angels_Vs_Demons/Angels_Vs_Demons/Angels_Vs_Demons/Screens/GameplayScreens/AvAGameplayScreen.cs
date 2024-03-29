﻿#region Using Statements
using Angels_Vs_Demons.Players;
using Microsoft.Xna.Framework;
using Angels_Vs_Demons.BoardObjects;
using System;
#endregion

namespace Angels_Vs_Demons.Screens.GameplayScreens
{
    /// <summary>
    /// Represents an AI vs AI game
    /// </summary>
    class AvAGameplayScreen : GameplayScreen
    {
        #region Initialization

        /// <summary>
        /// Constructs a gameplay screen with two AI players.
        /// </summary>
        public AvAGameplayScreen()
            :base()
        {
            Player1 = new ComputerPlayer(Faction.ANGEL, 1);
            Player2 = new ComputerPlayer(Faction.DEMON, 1);
        }

        #endregion

        #region Update
        #endregion
    }
}
