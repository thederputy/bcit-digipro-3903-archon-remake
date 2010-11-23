#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Angels_Vs_Demons.BoardObjects;
using Angels_Vs_Demons.GameObjects;
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
            player1 = new HumanPlayer(Faction.ANGEL);
            player2 = new HumanPlayer(Faction.DEMON);
        }

        #endregion
    }
}
