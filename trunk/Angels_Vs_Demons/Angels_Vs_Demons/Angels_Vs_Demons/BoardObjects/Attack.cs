#region Using Statements
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Angels_Vs_Demons.GameObjects;
using Angels_Vs_Demons.Util;
#endregion

namespace Angels_Vs_Demons.BoardObjects
{
    class Attack : ICloneable
    {
        #region Fields

        /// <summary>
        /// Stores whether this move can be executed or not
        /// </summary>
        public bool IsExecutable
        {
            get
            {
                updateIsExecutable();
                return isExecutable;
            }
        }
        protected bool isExecutable;

        /// <summary>
        /// Sets and gets the previousTile member
        /// </summary>
        public Vector2 AttackerPos
        {
            get { return attackerPos; }
            set
            {
                attackerPos = value;
                updateIsExecutable();
            }
        }
        protected Vector2 attackerPos;

        /// <summary>
        /// Sets and gets the victimPos member
        /// </summary>
        public Vector2 VictimPos
        {
            get { return victimPos; }
            set
            {
                victimPos = value;
                updateIsExecutable();
            }
        }
        protected Vector2 victimPos;

        #endregion

        /// <summary>
        /// Creates an attack based on an attacker and an attacker tile.
        /// </summary>
        /// <param name="newVictimPos">the grid position containing the victim</param>
        /// <param name="newAttackerPos">the grid position containing the attacker</param>
        public Attack(Vector2 newVictimPos, Vector2 newAttackerPos)
        {
            VictimPos = newVictimPos;
            AttackerPos = newAttackerPos;
        }

        /// <summary>
        /// Updates the isExecutable value.
        /// </summary>
        protected virtual void updateIsExecutable()
        {
            if (victimPos == null || attackerPos == null)
            {
                isExecutable = false;
            }
            else if (victimPos.Equals(Position.nil) || attackerPos.Equals(Position.nil))
            {
                isExecutable = false;
            }
            else
            {
                isExecutable = true;
            }
        }

        /// <summary>
        /// Performs a deep clone of the Attack.
        /// </summary>
        /// <returns>A new Attack instance populated with the same data as this Attack.</returns>
        public Object Clone()
        {
            Attack other = this.MemberwiseClone() as Attack;
            return other;
        }
    }
}
