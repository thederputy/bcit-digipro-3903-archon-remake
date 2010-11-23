#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace Angels_Vs_Demons.BoardObjects
{
    class Turn
    {
        #region Fields

        /// <summary>
        /// The move associated with this turn.
        /// </summary>
        public Move Move
        {
            get { return move; }
            set { move = value; }
        }
        private Move move;

        /// <summary>
        /// The attack associated with this turn.
        /// </summary>
        public Attack Attack
        {
            get { return attack; }
            set { attack = value; }
        }
        private Attack attack;

        #endregion

        #region Initialization
        public Turn(Move move, Attack attack)
        {
            this.move   = move;
            this.attack = attack;
        }
        #endregion
    }
}
