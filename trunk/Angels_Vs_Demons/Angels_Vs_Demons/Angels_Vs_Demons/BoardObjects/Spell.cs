using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Angels_Vs_Demons.GameObjects;
using Angels_Vs_Demons.GameObjects.Units;

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

        /// <summary>
        /// Updates the isExecutable value.
        /// </summary>
        protected override void updateIsExecutable()
        {
            if (victimTile == null || attackerTile == null)
            {
                isExecutable = false;
            }
            else if (!(attackerTile.Unit is Champion))
            {
                isExecutable = false;
            }
            else if (victimTile.Unit == null)
            {
                isExecutable = false;
            }
            else
            {
                isExecutable = true;
            }
        }
    }
}
