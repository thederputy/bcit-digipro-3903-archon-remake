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
            player1 = new HumanPlayer(Faction.ANGEL);
            player2 = new ComputerPlayer(Faction.DEMON);
        }

        #endregion

        #region Move/Attack Phases

        /// <summary>
        /// Executes the move phase for a human vs AI game.
        /// </summary>
        /// <param name="currentTile">the tile that the cursor is now on.</param>
        /// <param name="boardSelectedTile">the tile that was selected</param>
        protected override void executeMovePhase(Tile currentTile, Tile boardSelectedTile)
        {
            board.applyMove(new Move(currentTile, boardSelectedTile));
        }


        /// <summary>
        /// Executes the attack phase for a Human vs AI game.
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
