#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Angels_Vs_Demons.Util;
#endregion

namespace Angels_Vs_Demons.BoardObjects
{
    interface AbstractBoard
    {
        /// <sumary>
        ///   Finds all valid turns for the current player in this board.
        /// </sumary>    
        /// <value>
        ///  A list with all the valid turns.
        /// </value>
        List getValidTurns();

        /// <summary>
        /// Performs a game turn. This consists of a movement and/or attack.
        /// </summary>
        /// <param name="turn">Turn object that contains details of a turn.</param>
        void applyTurn(Turn turn);

        /// <summary>
        /// Performs a game move.
        /// </summary>
        /// <param name="move">Move object that contains details of a move.</param>
        void applyMove(Move move);

        /// <summary>
        /// Performs a game attack.
        /// </summary>
        /// <param name="attack">Attack object that contains details of an attack.</param>
        void applyAttack(Attack attack);
    }
}
