#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Angels_Vs_Demons.GameObjects;
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
        public Tile AttackerTile
        {
            get { return attackerTile; }
            set
            {
                attackerTile = value;
                updateIsExecutable();
            }
        }
        protected Tile attackerTile;

        /// <summary>
        /// Sets and gets the victimTile member
        /// </summary>
        public Tile VictimTile
        {
            get { return victimTile; }
            set
            {
                victimTile = value;
                updateIsExecutable();
            }
        }
        protected Tile victimTile;

        #endregion

        /// <summary>
        /// Creates an attack based on an attacker and an attacker tile.
        /// </summary>
        /// <param name="newVictimTile">the tile containing the victim</param>
        /// <param name="newAttackerTile">the tile containing the attacker</param>
        public Attack(Tile newVictimTile, Tile newAttackerTile)
        {
            VictimTile = newVictimTile;
            AttackerTile = newAttackerTile;
        }

        /// <summary>
        /// Updates the isExecutable value.
        /// </summary>
        protected virtual void updateIsExecutable()
        {
            if (victimTile == null || attackerTile == null)
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

        /// <summary>
        /// Performs a deep clone of the Attack.
        /// </summary>
        /// <returns>A new Attack instance populated with the same data as this Attack.</returns>
        public Object Clone()
        {
            Attack other = this.MemberwiseClone() as Attack;
            other.VictimTile = this.VictimTile.Clone() as Tile;
            other.AttackerTile = this.AttackerTile.Clone() as Tile;
            return other;
        }
    }
}
