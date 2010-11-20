#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Angels_Vs_Demons.BoardObjects;
#endregion

namespace Angels_Vs_Demons.Players
{
    class Player
    {
        #region Properties

        protected Faction faction;

        /// <summary>
        /// A player's associated faction.
        /// </summary>
        public Faction Faction
        {
            get { return faction; }
            set { faction = value; }
        }

        protected Turn turn;

        /// <summary>
        /// Represents a player's turn.
        /// </summary>
        public Turn Turn
        {
            get { return turn; }
            set { turn = value; }
        }

        #endregion

        protected Player(Faction faction)
        {
            Faction = faction;
            Turn = null;
        }
    }
}
