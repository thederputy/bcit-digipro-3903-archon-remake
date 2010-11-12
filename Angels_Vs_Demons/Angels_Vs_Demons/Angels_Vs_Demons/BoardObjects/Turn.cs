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
        private Move move;

        public Move Move
        {
            get { return move; }
        }

        private Attack attack;

        public Attack Attack
        {
            get { return attack; }
        }

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
