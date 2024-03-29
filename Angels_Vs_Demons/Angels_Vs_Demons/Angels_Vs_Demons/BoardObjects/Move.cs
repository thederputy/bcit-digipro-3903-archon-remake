﻿#region Using Statements
using System;
using System.Diagnostics;
using Angels_Vs_Demons.Util;
using Microsoft.Xna.Framework;
#endregion

namespace Angels_Vs_Demons.BoardObjects
{
    [Serializable]
    class Move : ICloneable
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
        public Vector2 PreviousTile
        {
            get { return previousTile; }
            set
            {
                previousTile = value;
                updateIsExecutable();
            }
        }
        private Vector2 previousTile;

        /// <summary>
        /// Sets and gets the newTile member
        /// </summary>
        public Vector2 NewTile
        {
            get { return newTile; }
            set
            {
                newTile = value;
                updateIsExecutable();
            }
        }
        private Vector2 newTile;

        #endregion

        /// <summary>
        /// Creates a move based on an new and old tile. If null is passed in for one or both of the parameters,
        /// then this move is not executable. If you want to instantiate a move object that is not executable,
        /// pass in <code>Position.nil</code> for both arguments or call the 0 argument constructor.
        /// </summary>
        /// <param name="newNewTile">The new grid position</param>
        /// <param name="newPreviousTile">The previous grid position</param>
        public Move(Vector2 newNewTile, Vector2 newPreviousTile)
        {
            NewTile = newNewTile;
            PreviousTile = newPreviousTile;
        }

        /// <summary>
        /// Creates a move that is initially not executable.
        /// </summary>
        public Move()
        {
            NewTile = Position.nil;
            PreviousTile = Position.nil;
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
            else if (newTile.Equals(Position.nil) || previousTile.Equals(Position.nil))
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

        /// <summary>
        /// Performs a deep clone of the Move.
        /// </summary>
        /// <returns>A new Move instance populated with the same data as this Move.</returns>
        public Object Clone()
        {
            Move other = this.MemberwiseClone() as Move;
            //other.NewTile = this.NewTile.Clone() as Tile;
            //other.PreviousTile = this.PreviousTile.Clone() as Tile;
            return other;
        }
    }
}
