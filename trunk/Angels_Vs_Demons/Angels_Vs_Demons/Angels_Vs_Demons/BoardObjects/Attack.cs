#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Angels_Vs_Demons.GameObjects;
#endregion

namespace Angels_Vs_Demons.BoardObjects
{
    class Attack
    {
        #region Fields

        private bool isExecutable;

        /// <summary>
        /// Stores whether this move can be executed or not
        /// </summary>
        public bool IsExecutable
        {
            get { return isExecutable; }
            set { isExecutable = value; }
        }

        private Tile attackerTile;

        /// <summary>
        /// Sets and gets the previousTile member
        /// </summary>
        public Tile AttackerTile
        {
            get { return attackerTile; }
            set { attackerTile = value; }
        }

        private Tile victimTile;

        /// <summary>
        /// Sets and gets the victimTile member
        /// </summary>
        public Tile VictimTile
        {
            get { return victimTile; }
            set { victimTile = value; }
        }
        #endregion

        /// <summary>
        /// Creates an attack based on an attacker and a  and attacker tile.
        /// </summary>
        /// <param name="victimTile">the tile containing the victim</param>
        /// <param name="attackerTile">the tile containing the attacker</param>
        public Attack(Tile victimTile, Tile attackerTile)
        {
            if (victimTile == null || attackerTile == null)
            {
                IsExecutable = false;
            }
            else
            {
                IsExecutable = true;
            }
            VictimTile = victimTile;
            AttackerTile = attackerTile;
        }

        /// <summary>
        /// Checks and if the attack is executable and returns the result.
        /// </summary>
        /// <returns>True if this attack is executable.</returns>
        public bool checkIsExecutable()
        {
            if (victimTile == null || attackerTile == null)
            {
                IsExecutable = false;
            }
            else if (victimTile.Unit == null)
            {
                isExecutable = false;
            }
            else
            {
                isExecutable = true;
            }

            return isExecutable;
        }
    }
}
