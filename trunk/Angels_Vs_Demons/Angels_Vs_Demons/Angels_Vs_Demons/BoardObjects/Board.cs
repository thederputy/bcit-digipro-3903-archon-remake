#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Angels_Vs_Demons.GameObjects;
using Angels_Vs_Demons.GameObjects.Units;
using Angels_Vs_Demons.Players;
using Angels_Vs_Demons.Screens.ScreenManagers;
using Angels_Vs_Demons.Screens;
using Angels_Vs_Demons.Util;
#endregion

namespace Angels_Vs_Demons.BoardObjects
{
    class Board : AbstractBoard
    {
        public ContentManager content;
        public SpriteFont gameFont;
        public SpriteFont debugFont;
        public GameObject Cursor;
        public Tile[][] grid;

        public Boolean isAngelTurn;

        private Faction controllingFaction;

        public Faction ControllingFaction
        {
            get { return controllingFaction; }
            set { controllingFaction = value; }
        }

        public Boolean movePhase;
        public Boolean attackPhase;

        public Tile selectedTile;

        private Texture2D Cursor_Texture;
        private Texture2D TileTexture;

        private Texture2D Arch_Demon_Texture;
        private Texture2D Nightmare_Texture;
        private Texture2D Demon_Lord_Texture;
        private Texture2D Skeleton_Archer_Texture;
        private Texture2D Imp_Texture;
        private Texture2D Blood_Guard_Texture;

        private Texture2D Arch_Angel_Texture;
        private Texture2D Pegasus_Texture;
        private Texture2D High_Angel_Texture;
        private Texture2D Chosen_One_Texture;
        private Texture2D Soldier_Texture;
        private Texture2D Angelic_Guard_Texture;

        private int x_size;
        private int y_size;
        private int tile_size;

        public Move recentMove;

        public Board(ContentManager CONTENT)
        {
            content = CONTENT;
            isAngelTurn = true;
            movePhase = true;
            attackPhase = false;
            selectedTile = null;

            //creates a blank recent move for the class

            //recentMove = new Move();

            // Loads the textures

            gameFont = content.Load<SpriteFont>("MenuFont");
            debugFont = content.Load<SpriteFont>("debugFont");


            Cursor_Texture = content.Load<Texture2D>("Cursor");
            TileTexture = content.Load<Texture2D>("gridNormal");

            Arch_Demon_Texture = content.Load<Texture2D>("Arch_Demon");
            Nightmare_Texture = content.Load<Texture2D>("Nightmare");
            Demon_Lord_Texture = content.Load<Texture2D>("Demon_Lord");
            Skeleton_Archer_Texture = content.Load<Texture2D>("Skeleton_Archer");
            Imp_Texture = content.Load<Texture2D>("Imp");
            Blood_Guard_Texture = content.Load<Texture2D>("Blood_Guard");

            Arch_Angel_Texture = content.Load<Texture2D>("Arch_Angel");
            Pegasus_Texture = content.Load<Texture2D>("Pegasus");
            High_Angel_Texture = content.Load<Texture2D>("High_Angel");
            Chosen_One_Texture = content.Load<Texture2D>("Chosen_One");
            Soldier_Texture = content.Load<Texture2D>("Soldier");
            Angelic_Guard_Texture = content.Load<Texture2D>("Angelic_Guard");


            /// Initializes the screen with an empty grid of tiles

            x_size = 10;
            y_size = 9;
            tile_size = 40;
            int grid_totalwidth = tile_size * x_size;
            int grid_x_center = grid_totalwidth / 2;
            int screen_x_center = (ScreenManager.screenWidth / 2);

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

            // Initializes the cursor

            Cursor = new GameObject(Cursor_Texture);
            Cursor.rect.X = grid[0][0].rect.X;
            Cursor.rect.Y = grid[0][0].rect.Y;
            Cursor.rect.Width = tile_size;
            Cursor.rect.Height = tile_size;
            grid[(int)Cursor.position.X][(int)Cursor.position.Y].IsCurrentTile = true;

            // Initialize Demon Army
            Faction demon = Faction.DEMON;

            Champion ArchDemon = new Champion(Arch_Demon_Texture, demon, "ArchDemon");
            Knight Nightmare = new Knight(Nightmare_Texture, demon, "Nightmare");
            Mage DemonLord = new Mage(Demon_Lord_Texture, demon, "DemonLord");
            Archer SkeletonArcher1 = new Archer(Skeleton_Archer_Texture, demon, "SkeletonArcher");
            Archer SkeletonArcher2 = new Archer(Skeleton_Archer_Texture, demon, "SkeletonArcher");
            Peon Imp1 = new Peon(Imp_Texture, demon, "Imp");
            Peon Imp2 = new Peon(Imp_Texture, demon, "Imp");
            Peon Imp3 = new Peon(Imp_Texture, demon, "Imp");
            Guard BloodGuard1 = new Guard(Blood_Guard_Texture, demon, "BloodGuard");
            Guard BloodGuard2 = new Guard(Blood_Guard_Texture, demon, "BloodGuard");

            // Place Demon army on grid
            grid[0][4].Unit = ArchDemon;
            grid[0][5].Unit = Nightmare;
            grid[0][3].Unit = DemonLord;
            grid[0][2].Unit = SkeletonArcher1;
            grid[0][6].Unit = SkeletonArcher2;
            grid[1][2].Unit = Imp1;
            grid[1][4].Unit = Imp2;
            grid[1][6].Unit = Imp3;
            grid[1][3].Unit = BloodGuard1;
            grid[1][5].Unit = BloodGuard2;

            // Initialize Angel Army
            Faction angel = Faction.ANGEL;

            Champion ArchAngel = new Champion(Arch_Angel_Texture, angel, "ArchAngel");
            Knight Pegasus = new Knight(Pegasus_Texture, angel, "Pegasus");
            Mage HighAngel = new Mage(High_Angel_Texture, angel, "HighAngel");
            Archer ChosenOne1 = new Archer(Chosen_One_Texture, angel, "Archer");
            Archer ChosenOne2 = new Archer(Chosen_One_Texture, angel, "Archer");
            Peon Soldier1 = new Peon(Soldier_Texture, angel, "Soldier");
            Peon Soldier2 = new Peon(Soldier_Texture, angel, "Soldier");
            Peon Soldier3 = new Peon(Soldier_Texture, angel, "Soldier");
            Guard AngelicGuard1 = new Guard(Angelic_Guard_Texture, angel, "AngelicGuard");
            Guard AngelicGuard2 = new Guard(Angelic_Guard_Texture, angel, "AngelicGuard");

            // Place Angel army on grid

            grid[9][4].Unit = ArchAngel;
            grid[9][3].Unit = Pegasus;
            grid[9][5].Unit = HighAngel;
            grid[9][2].Unit = ChosenOne1;
            grid[9][6].Unit = ChosenOne2;
            grid[8][2].Unit = Soldier1;
            grid[8][4].Unit = Soldier2;
            grid[8][6].Unit = Soldier3;
            grid[8][3].Unit = AngelicGuard1;
            grid[8][5].Unit = AngelicGuard2;

            // Initiate first turn
            controllingFaction = Faction.ANGEL;
            for (int i = 0; i < grid.Length; i++)
            {
                foreach (Tile tile in grid[i])
                {
                    if (tile.Unit != null)
                    {
                        if (tile.Unit.FactionType == Faction.ANGEL)
                        {
                            tile.IsUsable = true;
                        }
                        else
                        {
                            tile.IsUsable = false;
                        }
                    }
                    else
                    {
                        tile.IsUsable = false;
                    }
                }
            }
            /*
            grid[9][4].IsUsable = true;
            grid[9][5].IsUsable = true;
            grid[9][3].IsUsable = true;
            grid[9][2].IsUsable = true;
            grid[9][6].IsUsable = true;
            grid[8][2].IsUsable = true;
            grid[8][4].IsUsable = true;
            grid[8][6].IsUsable = true;
            grid[8][3].IsUsable = true;
            grid[8][5].IsUsable = true;

            grid[0][4].IsUsable = false;
            grid[0][5].IsUsable = false;
            grid[0][3].IsUsable = false;
            grid[0][2].IsUsable = false;
            grid[0][6].IsUsable = false;
            grid[1][2].IsUsable = false;
            grid[1][4].IsUsable = false;
            grid[1][6].IsUsable = false;
            grid[1][3].IsUsable = false;
            grid[1][5].IsUsable = false;
            */
        }

        /// <summary>
        /// Gets the current state of the board
        /// </summary>
        /// <returns>current state of the board</returns>
        public Tile[][] GetBoard()
        {
            return grid;
        }


        /// <summary>
        ///  Gets a tile specified by the x and y values
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        /// <returns>tile specified by the x and y values</returns>
        public Tile GetTile(int x, int y)
        {
            return grid[x][y];
        }


        /// <summary>
        ///  Gets the tile that the cursor is on
        /// </summary>
        /// <returns>the tile that the cursor is on.</returns>
        public Tile GetCurrentTile()
        {
            return grid[(int)Cursor.position.X][(int)Cursor.position.Y];
        }

        
        /// <summary>
        ///  Moves the cursor by the amount input, resets current tile
        /// </summary>
        /// <param name="x">x amount</param>
        /// <param name="y">y amount</param>
        public void moveCursor(int x, int y)
        {
            if (Cursor.position.X + x < x_size && Cursor.position.X + x >= 0 &&
                Cursor.position.Y + y < y_size && Cursor.position.Y + y >= 0)
            {
                grid[(int)Cursor.position.X][(int)Cursor.position.Y].IsCurrentTile = false;
                Cursor.position.X += x;
                Cursor.position.Y += y;
                grid[(int)Cursor.position.X][(int)Cursor.position.Y].IsCurrentTile = true;
            }
        }

        /// <summary>
        /// Sets the cursor to the position specitfied.
        /// </summary>
        /// <param name="x">The x coordinate of the new position.</param>
        /// <param name="y">The y coordinate of the new position.</param>
        public void setCursor(int x, int y)
        {
            if (x < x_size && x >= 0 && y < y_size && y >= 0)
            {
                grid[(int)Cursor.position.X][(int)Cursor.position.Y].IsCurrentTile = false;
                Cursor.position.X = x;
                Cursor.position.Y = y;
                grid[(int)Cursor.position.X][(int)Cursor.position.Y].IsCurrentTile = true;
            }
        }

        /// <summary>
        /// Gets the x size of the grid.
        /// </summary>
        /// <returns>the x size of the grid</returns>
        public int Get_xSize()
        {
            return x_size;
        }

        /// <summary>
        /// Gets the y size of the grid.
        /// </summary>
        /// <returns>the y size of the grid</returns>
        public int Get_ySize()
        {
            return y_size;
        }

        /// <summary>
        /// Paints the grid.
        /// </summary>
        /// <param name="SPRITEBATCH">the spritebatch to draw with</param>
        public void PaintGrid(SpriteBatch SPRITEBATCH)
        {
            SpriteBatch spriteBatch = SPRITEBATCH;

            spriteBatch.Begin();

            // Draw the grid on the screen

            for (int i = 0; i < x_size; i++)
            {
                for (int j = 0; j < y_size; j++)
                {
                    if (grid[i][j].IsMovable)
                    {
                        spriteBatch.Draw(grid[i][j].sprite, grid[i][j].rect, Color.Blue);
                    }
                    else if (grid[i][j].IsAttackable)
                    {
                        spriteBatch.Draw(grid[i][j].sprite, grid[i][j].rect, Color.Red);
                    }
                    else
                    {
                        spriteBatch.Draw(grid[i][j].sprite, grid[i][j].rect, Color.White);
                    }
                }
            }

            spriteBatch.End();
        }

        /// <summary>
        /// Paints the units
        /// </summary>
        /// <param name="SPRITEBATCH">the spritebatch</param>
        public void PaintUnits(SpriteBatch SPRITEBATCH)
        {

            SpriteBatch spriteBatch = SPRITEBATCH;

            spriteBatch.Begin();

            for (int i = 0; i < x_size; i++)
            {
                for (int j = 0; j < y_size; j++)
                {
                    if (grid[i][j].IsOccupied)
                    {
                        Unit Tempunit = grid[i][j].Unit;
                        Texture2D TempTexture = Tempunit.sprite;
                        spriteBatch.Draw(TempTexture, grid[i][j].rect, Color.White);
                    }
                    if (grid[i][j].IsCurrentTile)
                    {
                        spriteBatch.Draw(Cursor_Texture, grid[i][j].rect, Color.Silver);
                    }

                }
            }

            spriteBatch.End();

        }

        public Move getLastMovement()
        {
            return recentMove;
        }
        public void makeAction()
        {
            if (movePhase)
            {
                Tile currentTile = GetCurrentTile();
                if (currentTile.IsOccupied)
                {
                    //check that there is a tile selected
                    if (selectedTile != null)
                    {
                        //if we've selected the same tile again
                        if (currentTile.position == selectedTile.position)
                        {
                            markAllTilesAsNotMovable();
                            selectedTile = null;
                        }
                        else
                        {
                            makeMove(currentTile);
                        }
                    }
                    else
                    {
                        makeMove(currentTile);
                    }
                }
                else
                {
                    //tile is not occupied, check to see if we can move to it
                    if (currentTile.IsMovable)
                    {
                        //move to this tile
                        swapTiles(currentTile);
                        //movePhase = false;
                        //attackPhase = true;
                    }
                }
            }
            if (attackPhase)
            {

            }
        }

        /// <summary>
        /// Makes moves from a given tile
        /// </summary>
        /// <param name="startTile">starting tile.</param>
        private void makeMove(Tile startTile)
        {
            selectedTile = startTile;
            markAllTilesAsNotMovable();
            makePaths(startTile.Unit.Movement, startTile);
        }
        private void makeAttack()
        {
        }
        public Tile getMoves(int distance, Tile startingTile, Boolean initiateTiles)
        {
            Tile pathOrigin = startingTile;
            markAllTilesAsNotMovable();
            makePaths(distance, pathOrigin);
            return pathOrigin;
        }

        /// <summary>
        /// Marks all tiles within a Units move range that are not occupied as movable.
        /// </summary>
        /// <param name="distance">The move range of a unit</param>
        /// <param name="currentTile">The current tile we are checking</param>
        public void makePaths(int distance, Tile currentTile)
        {
            distance--;
            grid[(int)currentTile.position.X][(int)currentTile.position.Y].IsMovable = true;
            if (distance >= 0)
            {
                // are there tiles left and the tile is not occupied, go left
                if (currentTile.position.X - 1 >= 0 && 
                    grid[(int)currentTile.position.X - 1][(int)currentTile.position.Y].IsOccupied == false)
                {
                    currentTile.PathLeft = grid[(int)currentTile.position.X - 1][(int)currentTile.position.Y];
                    makePaths(distance, currentTile.PathLeft);
                }
                // are there tiles right and the tile is not occupied, go right
                if (currentTile.position.X + 1 < x_size &&
                    grid[(int)currentTile.position.X + 1][(int)currentTile.position.Y].IsOccupied == false)
                {
                    currentTile.PathRight = grid[(int)currentTile.position.X + 1][(int)currentTile.position.Y];
                    makePaths(distance, currentTile.PathRight);

                }
                // are there tiles above and the tile is not occupied, go up
                if (currentTile.position.Y - 1 >= 0 &&
                    grid[(int)currentTile.position.X][(int)currentTile.position.Y - 1].IsOccupied == false)
                {
                    currentTile.PathTop = grid[(int)currentTile.position.X][(int)currentTile.position.Y - 1];
                    makePaths(distance, currentTile.PathTop);
                }
                // are there tiles below and the tile is not occupied, go down
                if (currentTile.position.Y + 1 < y_size &&
                    grid[(int)currentTile.position.X][(int)currentTile.position.Y + 1].IsOccupied == false)
                {
                    currentTile.PathBottom = grid[(int)currentTile.position.X][(int)currentTile.position.Y + 1];
                    makePaths(distance, currentTile.PathBottom);
                }
            }
        }

        /// <summary>
        /// Iterates through the grid and marks all tiles as not moveable
        /// </summary>
        private void markAllTilesAsNotMovable()
        {
            for (int i = 0; i < grid.Length; i++)
            {
                foreach (Tile tile in grid[i])
                {
                    tile.IsMovable = false;
                }
            }
        }

        /// <summary>
        /// Swaps the current tile with the selected tile
        /// </summary>
        /// <param name="currentTile">The tile that we are going to move to.</param>
        private void swapTiles(Tile currentTile)
        {
            currentTile.Unit = selectedTile.Unit;
            currentTile.IsOccupied = true;
            selectedTile.Unit = null;
            selectedTile.IsSelected = false;
            selectedTile.IsOccupied = false;
            markAllTilesAsNotMovable();
        }

        public object clone()
        {
            throw new NotImplementedException();
        }

        public List getValidMoves()
        {
            
            throw new NotImplementedException();
        }

        public List getValidAttacks()
        {

            throw new NotImplementedException();
        }

        public List getValidTurns()
        {

            throw new NotImplementedException();
        }


        public void move(Move move)
        {
            throw new NotImplementedException();
        }
    }
}
