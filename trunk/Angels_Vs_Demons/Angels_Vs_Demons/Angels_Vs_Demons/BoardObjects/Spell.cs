﻿#region Using Statements
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