#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace Angels_Vs_Demons
{
    interface AbstractBoard
    {
        /// <summary>
        /// create a 'deep' clone of this board object.
        /// </summary>
        /// <returns>deep clone of this board</returns>
        Object clone();

        /// <sumary>
        ///   Finds all valid moves for the current player in this board.
        /// </sumary>    
        /// <value>
        ///  A list with all the valid moves.
        /// </value>
        List getValidMoves();

        /// <summary>
        /// Performs a game move. This consists of a movement/attack.
        /// </summary>
        /// <param name="move">Move object that contains details of a move.</param>
        void move(Move move);
    }
}
