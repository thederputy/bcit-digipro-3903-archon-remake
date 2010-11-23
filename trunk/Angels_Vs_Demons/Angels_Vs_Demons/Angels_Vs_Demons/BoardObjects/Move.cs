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
    class Move
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
        private bool isExecutable;

        /// <summary>
        /// Sets and gets the previousTile member
        /// </summary>
        public Tile PreviousTile
        {
            get { return previousTile; }
            set
            {
                previousTile = value;
                updateIsExecutable();
            }
        }
        private Tile previousTile;

        /// <summary>
        /// Sets and gets the newTile member
        /// </summary>
        public Tile NewTile
        {
            get { return newTile; }
            set
            {
                newTile = value;
                updateIsExecutable();
            }
        }
        private Tile newTile;

        #endregion

        /// <summary>
        /// Creates a move based on an new and old tile. If null is passed in for one or both of the parameters,
        /// then this move is not executable.
        /// </summary>
        /// <param name="newNewTile">The new grid position</param>
        /// <param name="newPreviousTile">The previous grid position</param>
        public Move(Tile newNewTile, Tile newPreviousTile)
        {
            NewTile = newNewTile;
            PreviousTile = newPreviousTile;
        }

        /// <summary>
        /// Checks and if the move is executable and returns the result.
        /// </summary>
        public void updateIsExecutable()
        {
#if DEBUG
            Debug.WriteLine("DEBUG: updating isExecutable");
#endif
            if (newTile == null || previousTile == null)
            {
                isExecutable = false;
            }
            else if (previousTile.Unit == null)
            {
                isExecutable = false;
            }
            else
            {
                isExecutable = true;
            }
#if DEBUG
            Debug.WriteLine("DEBUG: isExecutable is now: " + isExecutable);
#endif
        }
    }
}
