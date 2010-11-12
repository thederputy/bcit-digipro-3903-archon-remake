using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Angels_Vs_Demons.BoardObjects
{
    class Move
    {
        #region Fields
        private Vector2 previousPosition;

        /// <summary>
        /// Sets and gets the previousPosition member
        /// </summary>
        public Vector2 PreviousPosition
        {
            get { return previousPosition; }
            set { previousPosition = value; }
        }

        private Vector2 newPosition;

        /// <summary>
        /// Sets and gets the newPosition member
        /// </summary>
        public Vector2 NewPosition
        {
            get { return newPosition; }
            set { newPosition = value; }
        }
        #endregion

        /// <summary>
        /// Creates a move based on an old and new position
        /// </summary>
        /// <param name="oldPos">The old grid position</param>
        /// <param name="newPos">The new grid position</param>
        public Move(Vector2 oldPos, Vector2 newPos)
        {
            PreviousPosition = oldPos;
            NewPosition = newPos;
        }
    }
}
