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
            player1 = new ComputerPlayer(Faction.ANGEL);
            player2 = new ComputerPlayer(Faction.DEMON);
        }

        #endregion

        #region Move/Attack Phases

        /// <summary>
        /// Executes the move phase for an AI vs AI game.
        /// </summary>
        /// <param name="currentTile">the tile that the cursor is now on.</param>
        /// <param name="boardSelectedTile">the tile that was selected</param>
        protected override void executeMovePhase(Tile currentTile, Tile boardSelectedTile)
        {
            board.applyMove(new Move(currentTile, boardSelectedTile));
        }


        /// <summary>
        /// Executes the attack phase for an AI vs AI game.
        /// </summary>
        /// <param name="victimTile">the tile that is getting attacked</param>
        /// <param name="attackerTile">the tile that is attacking</param>
        protected override void executeAttackPhase(Tile victimTile, Tile attackerTile)
        {
            board.applyAttack(new Attack(victimTile, attackerTile));
        }

        #endregion
    }
}
