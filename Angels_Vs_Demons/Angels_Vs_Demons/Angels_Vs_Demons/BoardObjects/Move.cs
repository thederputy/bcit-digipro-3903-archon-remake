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
        private Tile previousTile;

        /// <summary>
        /// Sets and gets the previousTile member
        /// </summary>
        public Tile PreviousTile
        {
            get { return previousTile; }
            set { previousTile = value; }
        }

        private Tile newTile;

        /// <summary>
        /// Sets and gets the newTile member
        /// </summary>
        public Tile NewTile
        {
            get { return newTile; }
            set { newTile = value; }
        }
        #endregion

        /// <summary>
        /// Creates a move based on an new and old tile.
        /// </summary>
        /// <param name="newTile">The new grid position</param>
        /// <param name="oldTile">The old grid position</param>
        public Move(Tile newTile, Tile oldTile)
        {
            NewTile = newTile;
            PreviousTile = oldTile;
        }
    }
}
