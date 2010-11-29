#region Using Statements
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Angels_Vs_Demons.GameObjects;
using Angels_Vs_Demons.GameObjects.Units;
using Angels_Vs_Demons.Players;
using Angels_Vs_Demons.Screens.ScreenManagers;
using Angels_Vs_Demons.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
#endregion

namespace Angels_Vs_Demons.BoardObjects
{
    class Board : ICloneable
    {
        /// <summary>
        /// Gets X size of the board.
        /// </summary>
        public int X_size
        {
            get { return x_size; }
        }
        private const int x_size = 10;

        /// <summary>
        /// Gets Y size of the board.
        /// </summary>
        public int Y_size
        {
            get { return y_size; }
        }
        private const int y_size = 9;

        /// <summary>
        /// Gets/sets the grid.
        /// </summary>
        public Tile[][] Grid
        {
            get { return grid; }
            set { grid = value; }
        }
        private Tile[][] grid;

        /// <summary>
        /// Creates a board for use for displaying the units.
        /// </summary>
        public Board(int tile_size, Texture2D TileTexture, int screen_x_center, int grid_x_center)
        {
            grid = new Tile[x_size][];
            for (int i = 0; i < x_size; i++)
            {
                grid[i] = new Tile[y_size];
                for (int j = 0; j < y_size; j++)
                {
                    grid[i][j] = new Tile(TileTexture);
                    grid[i][j].rect.X = (i * tile_size) + (screen_x_center - grid_x_center);
                    grid[i][j].rect.Y = j * tile_size;
                    grid[i][j].rect.Width = tile_size;
                    grid[i][j].rect.Height = tile_size;
                    grid[i][j].position.X = i;
                    grid[i][j].position.Y = j;
                }
            }
        }

        /// <summary>
        /// Creates a blank board to be use for the AI (and for cloning purposes).
        /// </summary>
        public Board()
        {
            grid = new Tile[x_size][];
            for (int i = 0; i < x_size; i++)
            {
                grid[i] = new Tile[y_size];
            }
        }

        /// <summary>
        /// Performs a deep clone of the board.
        /// </summary>
        /// <returns></returns>
        public Object Clone()
        {
            Board other = new Board();
            other.grid = new Tile[x_size][];
            for (int i = 0; i < x_size; i++)
            {
                other.grid[i] = new Tile[y_size];
            }

            for (int i = 0; i < other.Grid.Length; i++)
            {
                for (int j = 0; j < other.Grid[i].Length; j++)
                {
                    other.Grid[i][j] = this.Grid[i][j].Clone() as Tile;
                    if (this.Grid[i][j].IsOccupied)
                    {
                        other.Grid[i][j].Unit = this.Grid[i][j].Unit.Clone() as Unit;
                    }
                }
            }
            return other;
        }
    }
}
