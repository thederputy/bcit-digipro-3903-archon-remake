#region Using Statements
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Angels_Vs_Demons.GameObjects;
#endregion

namespace Angels_Vs_Demons.BoardObjects
{
    abstract class Spell : Attack
    {
        /// <summary>
        /// Enumerates the range for the spells
        /// </summary>
        public static enum spellRange
        {
            BOLT = 3,
            BUFF = 3,
            HEAL = 6,
            REST = 0,
            STUN = 4,
            TELE = 5
        }

        /// <summary>
        /// Enumerates the mana cost for spells
        /// </summary>
        public static enum spellCost
        {
            BOLT = 20,
            BUFF = 30,
            HEAL = 50,
            REST = 0,
            STUN = 20,
            TELE = 30
        }

        public int MpCost
        {
            get { return mpCost; }
            set { mpCost = value; }
        }

        private int mpCost;

        public Spell(Vector2 newVictimPos, Vector2 newAttackerPos)
            : base(newVictimPos, newAttackerPos)
        {
        }

        /// <summary>
        /// Updates the isExecutable value.
        /// </summary>
        protected override void updateIsExecutable()
        {
            if (victimPos == null || attackerPos == null)
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
