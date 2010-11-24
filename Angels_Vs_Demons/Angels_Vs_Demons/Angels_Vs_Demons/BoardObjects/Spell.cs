using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Angels_Vs_Demons.GameObjects;

namespace Angels_Vs_Demons.BoardObjects
{
    abstract class Spell : Attack
    {
        public int MpCost
        {
            get { return mpCost; }
            set { mpCost = value; }
        }

        private int mpCost;

        public Spell(Tile newVictimTile, Tile newAttackerTile)
            : base(newVictimTile, newAttackerTile)
        {
        }
    }
}
