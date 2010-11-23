#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Angels_Vs_Demons.GameObjects;
#endregion

namespace Angels_Vs_Demons.BoardObjects
{
    class Move
    {
        #region Fields

        private bool isExecutable;

        /// <summary>
        /// Stores whether this move can be executed or not
        /// </summary>
        public bool IsExecutable
        {
            get { return isExecutable; }
        }

        private Tile previousTile;

        /// <summary>
        /// Sets and gets the previousTile member
        /// </summary>
        public Tile PreviousTile
        {
            get { return previousTile; }
        }

        private Tile newTile;

        /// <summary>
        /// Sets and gets the newTile member
        /// </summary>
        public Tile NewTile
        {
            get { return newTile; }
        }
        #endregion

        /// <summary>
        /// Creates a move based on an new and old tile. If null is passed in for one or both of the parameters,
        /// then this move is not executable.
        /// </summary>
        /// <param name="newTile">The new grid position</param>
        /// <param name="oldTile">The old grid position</param>
        public Move(Tile newTile, Tile oldTile)
        {
            if (newTile == null || oldTile == null)
            {
                isExecutable = false;
            }
            else
            {
                isExecutable = true;
            }

            this.newTile = newTile;
            this.previousTile = oldTile;
        }
    }
}
