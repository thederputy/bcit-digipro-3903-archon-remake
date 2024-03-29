﻿#region Using Statements
using Angels_Vs_Demons.Players;
using System.Diagnostics;
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
            Player2 = new ComputerPlayer(Faction.DEMON, 2);
        }

        /// <summary>
        /// Sets the Finish message
        /// </summary>
        /// <param name="message">the message we will be modifying.</param>
        /// <returns>the modified message</returns>
        protected override string FinishMessage(string message)
        {
            if (WinnerPlayer.Faction.Equals(Player1.Faction))
            {
                message = "YOU BEAT THE AI!!!!!!!!!\nCONGRATULATIONS";
            }
            else
            {
                message = "You lost against the AI? You must really suck!\nBetter luck next time.";
            }
            return message;
        }
        #endregion

        /// <summary>
        /// Gets called when you press enter on the board.
        /// This is the main loop that processes the gamestate.
        /// </summary>
        protected override void makeAction()
        {
            if (Player1.Faction == game.ControllingFaction)
            {
                base.makeAction();
            }
#if DEBUG
            else
            {
                Debug.WriteLine("NOT YOUR TURN, DUMMY!");
            }
#endif
        }
    }
}
