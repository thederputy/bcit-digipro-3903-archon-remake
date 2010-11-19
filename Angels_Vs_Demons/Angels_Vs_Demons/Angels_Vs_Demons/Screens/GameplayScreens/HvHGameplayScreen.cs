﻿#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Angels_Vs_Demons.GameObjects;
#endregion

namespace Angels_Vs_Demons.Screens.GameplayScreens
{
    class HvHGameplayScreen : GameplayScreen
    {
        /// <summary>
        /// Handles input for a Human versus Human (local hotseat) game.
        /// </summary>
        protected override void makeAction()
        {
            if (board.movePhase)
            {
#if DEBUG
                //showBitMasks();
#endif

                Tile currentTile = board.GetCurrentTile();

#if DEBUG
                Console.WriteLine("currentTile.IsUsable: " + currentTile.IsUsable);
#endif
                if (currentTile.IsUsable)
                {
                    //check that there is a tile selected
                    if (board.selectedTile != null)
                    {
                        //if we've selected the same tile again
                        if (currentTile.position == board.selectedTile.position)
                        {
                            //we're de-selecting this tile, no tiles are selected anymore
                            //board.markAllTilesAsNotMovable();
                            board.selectedTile = null;
#if DEBUG
                            Console.WriteLine("selected tile = null");
#endif
                        }
                        else
                        {
                            //board.makeMove(currentTile);
#if DEBUG
                            Console.WriteLine("updating seleted tile");
#endif
                            board.selectedTile = currentTile;
                        }
                    }
                    else
                    {
                        //board.makeMove(currentTile);
#if DEBUG
                        Console.WriteLine("updating seleted tile");
#endif
                        board.selectedTile = currentTile;
                    }
                }
                else
                {
                    //tile is not occupied, check to see if we can move to it
                    //if (currentTile.IsMovable)
                    if (board.selectedTile != null && (currentTile.UnitIDs & board.selectedTile.Unit.ID) != 0)
                    {
                        //move to this tile
                        //board.swapTiles(currentTile);
#if DEBUG
                        Console.WriteLine("bit mask swapping");
#endif
                        board.bitMaskSwapTile(currentTile);
                        board.selectedTile = null;
                        board.movePhase = false;
                        board.attackPhase = true;
                    }
                }
            }
            if (board.attackPhase)
            {
                board.endTurn();
            }
        }
    }
}
