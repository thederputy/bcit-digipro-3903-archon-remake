#region Using Statements
using System.Diagnostics;
using Angels_Vs_Demons.BoardObjects;
using Angels_Vs_Demons.GameObjects;
using Angels_Vs_Demons.GameObjects.Units;
using Angels_Vs_Demons.Players;
using Microsoft.Xna.Framework;

#endregion

namespace Angels_Vs_Demons.Util
{
    class MoveFinder
    {
        private Board board;

        public Faction ControllingFaction
        {
            get { return controllingFaction; }
            set { controllingFaction = value; }
        }
        private Faction controllingFaction;

        /// <summary>
        /// Creates a move finder.
        /// </summary>
        /// <param name="newBoard">the board.</param>
        /// <param name="newControllingFaction">the controlling faction</param>
        public MoveFinder(Board newBoard, Faction newControllingFaction)
        {
            board = newBoard;
        }

#if DEBUG
        /// <summary>
        /// Debugging method that writes the move bitmask data of all tiles to the debug console.
        /// </summary>
        public void showMoveBitMasks()
        {
            Debug.WriteLine("\nDEBUG: entering showMoveBitMasks()");
            for (int i = 0; i < board.Grid.Length; i++)
            {
                foreach (Tile tile in board.Grid[i])
                {
                    showTileMoveBitMasks(tile);
                }
            }
            Debug.WriteLine("DEBUG: leaving showMoveBitMasks()");
        }

        /// <summary>
        /// Debugging method that writes the move bitmask data of a tile to the debug console.
        /// </summary>
        /// <param name="currentTile"> the current tile to get the bitmask of.</param>
        private void showTileMoveBitMasks(Tile currentTile)
        {
            Debug.Write("DEBUG: x = ");
            Debug.Write(currentTile.position.X);
            Debug.Write(", y = ");
            Debug.Write(currentTile.position.Y);
            Debug.Write(": attackID = ");
            Debug.WriteLine(currentTile.MoveID);
        }
#endif

        /// <summary>
        /// Uses bit masking to mark the tiles as movable.
        /// </summary>
        public bool findMoves()
        {
            bool canMove = false;
            int moveTotal = 0;
            for (int i = 0; i < board.Grid.Length; i++)
            {
                foreach (Tile tile in board.Grid[i])
                {
                    if (tile.IsOccupied)
                    {
                        //if we're checking one of the controlling units
                        if (tile.Unit.FactionType == controllingFaction && tile.IsUsable)
                        {
                            bool isFlying = false;
                            for (int j = 0; j < tile.Unit.Special.Length; j++)
                            {
                                if (tile.Unit.Special[j] == specialType.FLYING)
                                {
                                    isFlying = true;
                                }
                            }
                            int unitMoves = 0;
                            unitMoves = bitMaskMoves(unitMoves, tile.Unit.Movement, isFlying, tile.position, tile, tile.Unit.ID);
                            moveTotal += unitMoves;
                        }
                    }
                }
            }
            Debug.WriteLine("moveTotal: " + moveTotal);
            if (moveTotal > 0)
            {
                canMove = true;
            }
            return canMove;
        }

        /// <summary>
        /// Uses bit masking to all tiles within a Units move range that are not occupied as movable.
        /// </summary>
        /// <param name="unitMoves">the number of moves this unit can make</param>
        /// <param name="distance">The move range of a unit</param>
        /// <param name="isFlying">true if the unit can fly</param>
        /// <param name="startPosition">The starting position of this recursive call</param>
        /// <param name="currentTile">The current tile we are checking</param>
        /// <param name="attackID">the ID of the unit we're checking</param>
        /// <returns>how many moves the unit can make</returns>
        private int bitMaskMoves(int unitMoves, int distance, bool isFlying, Vector2 startPosition, Tile currentTile, int attackID)
        {
            distance--;
            //as long as we're not checking the unit against itself
            if (currentTile.position != startPosition)
            {
                //if we haven't moved to this tile before and it is not occupied
                if (!currentTile.IsOccupied && (currentTile.MoveID & attackID) == 0)
                {
                    unitMoves++;
                    currentTile.MoveID |= attackID;//OR EQUALS
                }
            }
            if (distance >= 0)
            {
                // are there tiles left and the tile is not occupied, go left
                if (currentTile.position.X - 1 >= 0)
                {
                    if (!isFlying)
                    {
                        if (board.Grid[(int)currentTile.position.X - 1][(int)currentTile.position.Y].IsOccupied == false)
                        {
                            currentTile.PathLeft = board.Grid[(int)currentTile.position.X - 1][(int)currentTile.position.Y];
                            unitMoves = bitMaskMoves(unitMoves, distance, isFlying, startPosition, currentTile.PathLeft, attackID);
                        }
                    }
                    else
                    {
                        currentTile.PathLeft = board.Grid[(int)currentTile.position.X - 1][(int)currentTile.position.Y];
                        unitMoves = bitMaskMoves(unitMoves, distance, isFlying, startPosition, currentTile.PathLeft, attackID);
                    }
                }
                // are there tiles right and the tile is not occupied, go right
                if (currentTile.position.X + 1 < board.X_size)
                {
                    if (!isFlying)
                    {
                        if (board.Grid[(int)currentTile.position.X + 1][(int)currentTile.position.Y].IsOccupied == false)
                        {
                            currentTile.PathRight = board.Grid[(int)currentTile.position.X + 1][(int)currentTile.position.Y];
                            unitMoves = bitMaskMoves(unitMoves, distance, isFlying, startPosition, currentTile.PathRight, attackID);
                        }
                    }
                    else
                    {
                        currentTile.PathRight = board.Grid[(int)currentTile.position.X + 1][(int)currentTile.position.Y];
                        unitMoves = bitMaskMoves(unitMoves, distance, isFlying, startPosition, currentTile.PathRight, attackID);
                    }
                }
                // are there tiles above and the tile is not occupied, go up
                if (currentTile.position.Y - 1 >= 0)
                {
                    if (!isFlying)
                    {
                        if (board.Grid[(int)currentTile.position.X][(int)currentTile.position.Y - 1].IsOccupied == false)
                        {
                            currentTile.PathTop = board.Grid[(int)currentTile.position.X][(int)currentTile.position.Y - 1];
                            unitMoves = bitMaskMoves(unitMoves, distance, isFlying, startPosition, currentTile.PathTop, attackID);
                        }
                    }
                    else
                    {
                        currentTile.PathTop = board.Grid[(int)currentTile.position.X][(int)currentTile.position.Y - 1];
                        unitMoves = bitMaskMoves(unitMoves, distance, isFlying, startPosition, currentTile.PathTop, attackID);
                    }
                }
                // are there tiles below and the tile is not occupied, go down
                if (currentTile.position.Y + 1 < board.Y_size)
                {
                    if (!isFlying)
                    {
                        if (board.Grid[(int)currentTile.position.X][(int)currentTile.position.Y + 1].IsOccupied == false)
                        {
                            currentTile.PathBottom = board.Grid[(int)currentTile.position.X][(int)currentTile.position.Y + 1];
                            unitMoves = bitMaskMoves(unitMoves, distance, isFlying, startPosition, currentTile.PathBottom, attackID);
                        }
                    }
                    else
                    {
                        currentTile.PathBottom = board.Grid[(int)currentTile.position.X][(int)currentTile.position.Y + 1];
                        unitMoves = bitMaskMoves(unitMoves, distance, isFlying, startPosition, currentTile.PathBottom, attackID);
                    }
                }
            }
            return unitMoves;
        }

        /// <summary>
        /// Iterates through the grid and masks all tiles as not moveable.
        /// </summary>
        public void bitMaskAllTilesAsNotMovable()
        {
            for (int i = 0; i < board.Grid.Length; i++)
            {
                foreach (Tile tile in board.Grid[i])
                {
                    tile.MoveID = 0;
                }
            }
        }
    }
}
