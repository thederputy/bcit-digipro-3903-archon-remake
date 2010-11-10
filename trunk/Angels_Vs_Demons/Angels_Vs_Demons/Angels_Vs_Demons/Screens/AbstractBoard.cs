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
        List<Move> getValidMoves();
    }
}
