#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
#endregion

namespace Angels_Vs_Demons.Players
{
    class Player
    {
        #region Properties

        protected Faction faction;

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
