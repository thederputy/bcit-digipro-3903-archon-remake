using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Angels_Vs_Demons.Players
{
    class Player
    {
        /// <summary>
        /// Rpresents a faction, either ANGEL or DEMON
        /// </summary>
        public enum factionType
        {
            ANGEL,
            DEMON
        }

        #region Properties

        private factionType faction;

        public factionType Faction
        {
            get { return faction; }
            set { faction = value; }
        }
        #endregion

        protected Player(factionType faction)
        {
            Faction = faction;
        }
    }
}
