#region Using Statements
using System;
using Angels_Vs_Demons.BoardObjects;
#endregion

namespace Angels_Vs_Demons.Players
{
    [Serializable]
    abstract class Player
    {
        #region Properties

        /// <summary>
        /// A player's associated faction.
        /// </summary>
        public Faction Faction
        {
            get { return faction; }
            set { faction = value; }
        }
        protected Faction faction;

        /// <summary>
        /// Represents a player's turn.
        /// </summary>
        public Turn Turn
        {
            get { return turn; }
            set { turn = value; }
        }
        protected Turn turn;

        #endregion

        protected Player(Faction faction)
        {
            Faction = faction;
            Turn = null;
        }
    }
}
