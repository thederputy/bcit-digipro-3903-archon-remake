using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Angels_Vs_Demons.Players
{
    class Player
    {
        #region Properties

        private Faction faction;

        public Faction Faction
        {
            get { return faction; }
            set { faction = value; }
        }
        #endregion

        protected Player(Faction faction)
        {
            Faction = faction;
        }
    }
}
