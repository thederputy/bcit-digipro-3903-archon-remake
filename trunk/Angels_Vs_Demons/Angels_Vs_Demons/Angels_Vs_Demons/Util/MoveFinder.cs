#region Using Statements
using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Angels_Vs_Demons.BoardObjects;
using Angels_Vs_Demons.GameObjects;
using Angels_Vs_Demons.GameObjects.Units;
using Angels_Vs_Demons.Players;
using Angels_Vs_Demons.Screens.ScreenManagers;
using Angels_Vs_Demons.Screens;
#endregion

namespace Angels_Vs_Demons.Util
{
    class MoveFinder
    {

//#if DEBUG
//        /// <summary>
//        /// Debugging method that writes the move bitmask data of all tiles to the debug console.
//        /// </summary>
//        public void showMoveBitMasks()
//        {
//            Debug.WriteLine("\nDEBUG: entering showMoveBitMasks()");
//            for (int i = 0; i < grid.Length; i++)
//            {
//                foreach (Tile tile in grid[i])
//                {
//                    showTileMoveBitMasks(tile);
//                }
//            }
//            Debug.WriteLine("DEBUG: leaving showMoveBitMasks()");
//        }

//        /// <summary>
//        /// Debugging method that writes the move bitmask data of a tile to the debug console.
//        /// </summary>
//        /// <param name="currentTile"> the current tile to get the bitmask of.</param>
//        private void showTileMoveBitMasks(Tile currentTile)
//        {
//            Debug.Write("DEBUG: x = ");
//            Debug.Write(currentTile.position.X);
//            Debug.Write(", y = ");
//            Debug.Write(currentTile.position.Y);
//            Debug.Write(": id = ");
//            Debug.WriteLine(GetTile(currentTile.position).MoveID);
//        }
//#endif

//        /// <summary>
//        /// Uses bit masking to mark the tiles as movable.
//        /// </summary>
//        public bool bitMaskGetMoves()
//        {
//            bool canMove = false;
//            int moveTotal = 0;
//            for (int i = 0; i < grid.Length; i++)
//            {
//                foreach (Tile tile in grid[i])
//                {
//                    if (tile.IsOccupied)
//                    {
//                        //if we're checking one of the controlling units
//                        if (tile.Unit.FactionType == controllingFaction && tile.IsUsable)
//                        {
//                            bool isFlying = false;
//                            for (int j = 0; j < tile.Unit.Special.Length; j++)
//                            {
//                                if (tile.Unit.Special[j] == specialType.FLYING)
//                                {
//                                    isFlying = true;
//                                }
//                            }
//                            int unitMoves = 0;
//                            unitMoves = bitMaskMoves(unitMoves, tile.Unit.Movement, isFlying, tile.position, tile, tile.Unit.ID);
//                            moveTotal += unitMoves;
//                        }
//                    }
//                }
//            }
//            Debug.WriteLine("moveTotal: " + moveTotal);
//            if (moveTotal > 0)
//            {
//                canMove = true;
//            }
//            return canMove;
//        }


//        /// <summary>
//        /// Uses bit masking to all tiles within a Units move range that are not occupied as movable.
//        /// </summary>
//        /// <param name="unitMoves">the number of moves this unit can make</param>
//        /// <param name="distance">The move range of a unit</param>
//        /// <param name="isFlying">true if the unit can fly</param>
//        /// <param name="startPosition">The starting position of this recursive call</param>
//        /// <param name="currentTile">The current tile we are checking</param>
//        /// <param name="id">the ID of the unit we're checking</param>
//        /// <returns>how many moves the unit can make</returns>
//        private int bitMaskMoves(int unitMoves, int distance, bool isFlying, Vector2 startPosition, Tile currentTile, int id)
//        {
//            distance--;
//            //as long as we're not checking the unit against itself
//            if (currentTile.position != startPosition)
//            {
//                //if we haven't moved to this tile before and it is not occupied
//                if (!(GetTile(currentTile.position).IsOccupied)
//                    && (GetTile(currentTile.position).MoveID & id) == 0)
//                {
//                    unitMoves++;
//                    GetTile(currentTile.position).MoveID |= id;//OR EQUALS
//                }
//            }
//            if (distance >= 0)
//            {
//                // are there tiles left and the tile is not occupied, go left
//                if (currentTile.position.X - 1 >= 0)
//                {
//                    if (!isFlying)
//                    {
//                        if (grid[(int)currentTile.position.X - 1][(int)currentTile.position.Y].IsOccupied == false)
//                        {
//                            currentTile.PathLeft = grid[(int)currentTile.position.X - 1][(int)currentTile.position.Y];
//                            unitMoves = bitMaskMoves(unitMoves, distance, isFlying, startPosition, currentTile.PathLeft, id);
//                        }
//                    }
//                    else
//                    {
//                        currentTile.PathLeft = grid[(int)currentTile.position.X - 1][(int)currentTile.position.Y];
//                        unitMoves = bitMaskMoves(unitMoves, distance, isFlying, startPosition, currentTile.PathLeft, id);
//                    }
//                }
//                // are there tiles right and the tile is not occupied, go right
//                if (currentTile.position.X + 1 < x_size)
//                {
//                    if (!isFlying)
//                    {
//                        if (grid[(int)currentTile.position.X + 1][(int)currentTile.position.Y].IsOccupied == false)
//                        {
//                            currentTile.PathRight = grid[(int)currentTile.position.X + 1][(int)currentTile.position.Y];
//                            unitMoves = bitMaskMoves(unitMoves, distance, isFlying, startPosition, currentTile.PathRight, id);
//                        }
//                    }
//                    else
//                    {
//                        currentTile.PathRight = grid[(int)currentTile.position.X + 1][(int)currentTile.position.Y];
//                        unitMoves = bitMaskMoves(unitMoves, distance, isFlying, startPosition, currentTile.PathRight, id);
//                    }
//                }
//                // are there tiles above and the tile is not occupied, go up
//                if (currentTile.position.Y - 1 >= 0)
//                {
//                    if (!isFlying)
//                    {
//                        if (grid[(int)currentTile.position.X][(int)currentTile.position.Y - 1].IsOccupied == false)
//                        {
//                            currentTile.PathTop = grid[(int)currentTile.position.X][(int)currentTile.position.Y - 1];
//                            unitMoves = bitMaskMoves(unitMoves, distance, isFlying, startPosition, currentTile.PathTop, id);
//                        }
//                    }
//                    else
//                    {
//                        currentTile.PathTop = grid[(int)currentTile.position.X][(int)currentTile.position.Y - 1];
//                        unitMoves = bitMaskMoves(unitMoves, distance, isFlying, startPosition, currentTile.PathTop, id);
//                    }
//                }
//                // are there tiles below and the tile is not occupied, go down
//                if (currentTile.position.Y + 1 < y_size)
//                {
//                    if (!isFlying)
//                    {
//                        if (grid[(int)currentTile.position.X][(int)currentTile.position.Y + 1].IsOccupied == false)
//                        {
//                            currentTile.PathBottom = grid[(int)currentTile.position.X][(int)currentTile.position.Y + 1];
//                            unitMoves = bitMaskMoves(unitMoves, distance, isFlying, startPosition, currentTile.PathBottom, id);
//                        }
//                    }
//                    else
//                    {
//                        currentTile.PathBottom = grid[(int)currentTile.position.X][(int)currentTile.position.Y + 1];
//                        unitMoves = bitMaskMoves(unitMoves, distance, isFlying, startPosition, currentTile.PathBottom, id);
//                    }
//                }
//            }
//            return unitMoves;
//        }

//        /// <summary>
//        /// Iterates through the grid and masks all tiles as not moveable.
//        /// </summary>
//        public void bitMaskAllTilesAsNotMovable()
//        {
//            for (int i = 0; i < grid.Length; i++)
//            {
//                foreach (Tile tile in grid[i])
//                {
//                    tile.MoveID = 0;
//                }
//            }
//        }
    }
}
