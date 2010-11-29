#region Using Statements
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Angels_Vs_Demons.BoardObjects;
using Angels_Vs_Demons.BoardObjects.Spells;
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
    [Serializable]
    class AvDGame : AbstractBoard, ICloneable
    {
        [NonSerialized]
        public ContentManager content;
        [NonSerialized]
        public SpriteFont gameFont;
        [NonSerialized]
        public SpriteFont debugFont;
        [NonSerialized]
        public GameObject Cursor;

        /// <summary>
        /// 
        /// </summary>
        public Board Board
        {
            get { return board; }
            set { board = value; }
        }
        private Board board;

        [NonSerialized]
        public Dictionary<int, Unit> Angels = new Dictionary<int, Unit>();
        [NonSerialized]
        public Dictionary<int, Unit> Demons = new Dictionary<int, Unit>();

        private int x_size;

        public int X_size
        {
            get { return x_size; }
            set { x_size = value; }
        }
        private int y_size;

        public int Y_size
        {
            get { return y_size; }
            set { y_size = value; }
        }

        private int tile_size;

        private Move lastMove;
        private int lastCurrRecharge;

        public MoveFinder moveFinder;
        public AttackFinder attackFinder;

        /// <summary>
        /// Gets/sets the controlling faction.
        /// </summary>
        public Faction ControllingFaction
        {
            get { return controllingFaction; }
            set { controllingFaction = value; }
        }
        private Faction controllingFaction;

        /// <summary>
        /// Gets/sets the move phase.
        /// </summary>
        public bool MovePhase
        {
            get { return movePhase; }
            set { movePhase = value; }
        }
        private bool movePhase;

        public bool IsChampionAttack
        {
            get { return isChampionAttack; }
            set { isChampionAttack = value; }
        }
        private bool isChampionAttack;

        public SpellValues.spellTypes SpellType
        {
            get { return spellType; }
            set { spellType = value; }
        }
        private SpellValues.spellTypes spellType;

        /// <summary>
        /// Gets/sets the attack phase.
        /// </summary>
        public bool AttackPhase
        {
            get { return attackPhase; }
            set { attackPhase = value; }
        }
        private bool attackPhase;

        public Tile selectedTile;

        #region Textures (not needed for AI)
        [NonSerialized]
        private Texture2D AttackableTile_Texture;
        [NonSerialized]
        private Texture2D Cursor_Texture;
        [NonSerialized]
        private Texture2D SelectedTile_Texture;
        [NonSerialized]
        private Texture2D TileTexture;

        [NonSerialized]
        private Texture2D Arch_Demon_Texture;
        [NonSerialized]
        private Texture2D Nightmare_Texture;
        [NonSerialized]
        private Texture2D Demon_Lord_Texture;
        [NonSerialized]
        private Texture2D Skeleton_Archer_Texture;
        [NonSerialized]
        private Texture2D Imp_Texture;
        [NonSerialized]
        private Texture2D Blood_Guard_Texture;

        [NonSerialized]
        private Texture2D Arch_Angel_Texture;
        [NonSerialized]
        private Texture2D Pegasus_Texture;
        [NonSerialized]
        private Texture2D High_Angel_Texture;
        [NonSerialized]
        private Texture2D Chosen_One_Texture;
        [NonSerialized]
        private Texture2D Soldier_Texture;
        [NonSerialized]
        private Texture2D Angelic_Guard_Texture;
        #endregion

        #region Texture constructor
        /// <summary>
        /// Constructor for the board.
        /// </summary>
        /// <param name="CONTENT">the content manager</param>
        public AvDGame(ContentManager CONTENT)
        {
            content = CONTENT;
            selectedTile = null;

            // Loads the textures
            gameFont = content.Load<SpriteFont>("MenuFont");
            debugFont = content.Load<SpriteFont>("debugFont");
            AttackableTile_Texture = content.Load<Texture2D>("AttackableTile");
            Cursor_Texture = content.Load<Texture2D>("Cursor");
            SelectedTile_Texture = content.Load<Texture2D>("SelectedTile");
            TileTexture = content.Load<Texture2D>("gridNormal");

            Arch_Angel_Texture = content.Load<Texture2D>("Arch_Angel");
            Pegasus_Texture = content.Load<Texture2D>("Pegasus");
            High_Angel_Texture = content.Load<Texture2D>("High_Angel");
            Chosen_One_Texture = content.Load<Texture2D>("Chosen_One");
            Soldier_Texture = content.Load<Texture2D>("Soldier");
            Angelic_Guard_Texture = content.Load<Texture2D>("Angelic_Guard");

            Arch_Demon_Texture = content.Load<Texture2D>("Arch_Demon");
            Nightmare_Texture = content.Load<Texture2D>("Nightmare");
            Demon_Lord_Texture = content.Load<Texture2D>("Demon_Lord");
            Skeleton_Archer_Texture = content.Load<Texture2D>("Skeleton_Archer");
            Imp_Texture = content.Load<Texture2D>("Imp");
            Blood_Guard_Texture = content.Load<Texture2D>("Blood_Guard");


            // Initializes the screen with an empty grid of tiles
            x_size = 10;
            y_size = 9;
            tile_size = 40;
            int grid_totalwidth = tile_size * x_size;
            int grid_x_center = grid_totalwidth / 2;
            int screen_x_center = (ScreenManager.screenWidth / 2);

            board = new Board(tile_size, TileTexture, screen_x_center, grid_x_center);

            //grid = new Tile[x_size][];
            //for (int i = 0; i < x_size; i++)
            //{
            //    grid[i] = new Tile[y_size];
            //    for (int j = 0; j < y_size; j++)
            //    {
            //        grid[i][j] = new Tile(TileTexture);
            //        grid[i][j].rect.X = (i * tile_size) + (screen_x_center - grid_x_center);
            //        grid[i][j].rect.Y = j * tile_size;
            //        grid[i][j].rect.Width = tile_size;
            //        grid[i][j].rect.Height = tile_size;
            //        grid[i][j].position.X = i;
            //        grid[i][j].position.Y = j;
            //    }
            //}

            // Initializes the cursor
            Cursor = new GameObject(Cursor_Texture);

            //Cursor.rect.X = grid[0][0].rect.X;
            //Cursor.rect.Y = grid[0][0].rect.Y;
            Cursor.rect.X = board.Grid[0][0].rect.X;
            Cursor.rect.X = board.Grid[0][0].rect.X;

            Cursor.rect.Width = tile_size;
            Cursor.rect.Height = tile_size;

            //grid[(int)Cursor.position.X][(int)Cursor.position.Y].IsCurrentTile = true;
            board.Grid[(int)Cursor.position.X][(int)Cursor.position.Y].IsCurrentTile = true;

            // Initialize Angel Army
            Faction angel = Faction.ANGEL;

            Champion ArchAngel = new Champion(Arch_Angel_Texture, angel, "ArchAngel", BitMask.angelBits[0]);
            Knight Pegasus = new Knight(Pegasus_Texture, angel, "Pegasus", BitMask.angelBits[1]);
            Mage HighAngel = new Mage(High_Angel_Texture, angel, "HighAngel", BitMask.angelBits[2]);
            Archer ChosenOne1 = new Archer(Chosen_One_Texture, angel, "Archer", BitMask.angelBits[3]);
            Archer ChosenOne2 = new Archer(Chosen_One_Texture, angel, "Archer", BitMask.angelBits[4]);
            Peon Soldier1 = new Peon(Soldier_Texture, angel, "Soldier", BitMask.angelBits[5]);
            Peon Soldier2 = new Peon(Soldier_Texture, angel, "Soldier", BitMask.angelBits[6]);
            Peon Soldier3 = new Peon(Soldier_Texture, angel, "Soldier", BitMask.angelBits[7]);
            Guard AngelicGuard1 = new Guard(Angelic_Guard_Texture, angel, "AngelicGuard", BitMask.angelBits[8]);
            Guard AngelicGuard2 = new Guard(Angelic_Guard_Texture, angel, "AngelicGuard", BitMask.angelBits[9]);

            // Add Angel army to the Angels Dictionary
            Angels.Add(ArchAngel.ID, ArchAngel);
            Angels.Add(Pegasus.ID, Pegasus);
            Angels.Add(HighAngel.ID, HighAngel);
            Angels.Add(ChosenOne1.ID, ChosenOne1);
            Angels.Add(ChosenOne2.ID, ChosenOne2);
            Angels.Add(Soldier1.ID, Soldier1);
            Angels.Add(Soldier2.ID, Soldier2);
            Angels.Add(Soldier3.ID, Soldier3);
            Angels.Add(AngelicGuard1.ID, AngelicGuard1);
            Angels.Add(AngelicGuard2.ID, AngelicGuard2);

            //// Place Angel army on grid
            //grid[0][4].Unit = ArchAngel;
            //grid[0][3].Unit = Pegasus;
            //grid[0][5].Unit = HighAngel;
            //grid[0][2].Unit = ChosenOne1;
            //grid[0][6].Unit = ChosenOne2;
            //grid[1][2].Unit = Soldier1;
            //grid[1][4].Unit = Soldier2;
            //grid[1][6].Unit = Soldier3;
            //grid[1][3].Unit = AngelicGuard1;
            //grid[1][5].Unit = AngelicGuard2;

            // Place Angel army on the board
            board.Grid[0][4].Unit = ArchAngel;
            board.Grid[0][3].Unit = Pegasus;
            board.Grid[0][5].Unit = HighAngel;
            board.Grid[0][2].Unit = ChosenOne1;
            board.Grid[0][6].Unit = ChosenOne2;
            board.Grid[1][2].Unit = Soldier1;
            board.Grid[1][4].Unit = Soldier2;
            board.Grid[1][6].Unit = Soldier3;
            board.Grid[1][3].Unit = AngelicGuard1;
            board.Grid[1][5].Unit = AngelicGuard2;

            // Initialize Demon Army
            Faction demon = Faction.DEMON;

            Champion ArchDemon = new Champion(Arch_Demon_Texture, demon, "ArchDemon", BitMask.demonBits[0]);
            Knight Nightmare = new Knight(Nightmare_Texture, demon, "Nightmare", BitMask.demonBits[1]);
            Mage DemonLord = new Mage(Demon_Lord_Texture, demon, "DemonLord", BitMask.demonBits[2]);
            Archer SkeletonArcher1 = new Archer(Skeleton_Archer_Texture, demon, "SkeletonArcher", BitMask.demonBits[3]);
            Archer SkeletonArcher2 = new Archer(Skeleton_Archer_Texture, demon, "SkeletonArcher", BitMask.demonBits[4]);
            Peon Imp1 = new Peon(Imp_Texture, demon, "Imp", BitMask.demonBits[5]);
            Peon Imp2 = new Peon(Imp_Texture, demon, "Imp", BitMask.demonBits[6]);
            Peon Imp3 = new Peon(Imp_Texture, demon, "Imp", BitMask.demonBits[7]);
            Guard BloodGuard1 = new Guard(Blood_Guard_Texture, demon, "BloodGuard", BitMask.demonBits[8]);
            Guard BloodGuard2 = new Guard(Blood_Guard_Texture, demon, "BloodGuard", BitMask.demonBits[9]);

            // Add Demon army to the Demons Dictionary
            Demons.Add(ArchDemon.ID, ArchDemon);
            Demons.Add(Nightmare.ID, Nightmare);
            Demons.Add(DemonLord.ID, DemonLord);
            Demons.Add(SkeletonArcher1.ID, SkeletonArcher1);
            Demons.Add(SkeletonArcher2.ID, SkeletonArcher2);
            Demons.Add(Imp1.ID, Imp1);
            Demons.Add(Imp2.ID, Imp2);
            Demons.Add(Imp3.ID, Imp3);
            Demons.Add(BloodGuard1.ID, BloodGuard1);
            Demons.Add(BloodGuard2.ID, BloodGuard2);

            //// Place Demon army on grid
            //grid[9][4].Unit = ArchDemon;
            //grid[9][5].Unit = Nightmare;
            //grid[9][3].Unit = DemonLord;
            //grid[9][2].Unit = SkeletonArcher1;
            //grid[9][6].Unit = SkeletonArcher2;
            //grid[8][2].Unit = Imp1;
            //grid[8][4].Unit = Imp2;
            //grid[8][6].Unit = Imp3;
            //grid[8][3].Unit = BloodGuard1;
            //grid[8][5].Unit = BloodGuard2;

            // Place Demon army on the board
            board.Grid[9][4].Unit = ArchDemon;
            board.Grid[9][5].Unit = Nightmare;
            board.Grid[9][3].Unit = DemonLord;
            board.Grid[9][2].Unit = SkeletonArcher1;
            board.Grid[9][6].Unit = SkeletonArcher2;
            board.Grid[8][2].Unit = Imp1;
            board.Grid[8][4].Unit = Imp2;
            board.Grid[8][6].Unit = Imp3;
            board.Grid[8][3].Unit = BloodGuard1;
            board.Grid[8][5].Unit = BloodGuard2;

            // Initiate first turn
            controllingFaction = Faction.ANGEL;
            moveFinder = new MoveFinder(board, controllingFaction);
            attackFinder = new AttackFinder(board, controllingFaction);
            beginTurn();
        }
        #endregion

        //#region Copy-ish constructor
        ///// <summary>
        ///// Constructs a board that the AI can use
        ///// </summary>
        //public Board(Board board)
        //{
        //    selectedTile = null;

        //    /// Initializes the screen with an empty grid of tiles
        //    x_size = 10;
        //    y_size = 9;
        //    tile_size = 40;
        //    int grid_totalwidth = tile_size * x_size;
        //    int grid_x_center = grid_totalwidth / 2;
        //    int screen_x_center = (ScreenManager.screenWidth / 2);

        //    grid = new Tile[x_size][];
        //    for (int i = 0; i < x_size; i++)
        //    {
        //        grid[i] = new Tile[y_size];
        //        for (int j = 0; j < y_size; j++)
        //        {
        //            grid[i][j] = new Tile();
        //            grid[i][j].rect.X = (i * tile_size) + (screen_x_center - grid_x_center);
        //            grid[i][j].rect.Y = j * tile_size;
        //            grid[i][j].rect.Width = tile_size;
        //            grid[i][j].rect.Height = tile_size;
        //            grid[i][j].position.X = i;
        //            grid[i][j].position.Y = j;
        //        }
        //    }

        //    // Initializes the cursor
        //    Cursor = new GameObject();
        //    Cursor.rect.X = grid[0][0].rect.X;
        //    Cursor.rect.Y = grid[0][0].rect.Y;
        //    Cursor.rect.Width = tile_size;
        //    Cursor.rect.Height = tile_size;
        //    grid[(int)Cursor.position.X][(int)Cursor.position.Y].IsCurrentTile = true;

        //    // Initialize Angel Army
        //    Faction angel = Faction.ANGEL;

        //    //List angels = new List();

        //    //angels.push_back(new Champion(angel, "ArchAngel", BitMask.angelBits[0]));
        //    //angels.push_back(new Knight(angel, "Pegasus", BitMask.angelBits[1]));
        //    //angels.push_back(new Mage(angel, "HighAngel", BitMask.angelBits[2]));
        //    //angels.push_back(new Archer(angel, "Archer", BitMask.angelBits[3]));
        //    //angels.push_back(new Archer(angel, "Archer", BitMask.angelBits[4]));
        //    //angels.push_back(new Peon(angel, "Soldier", BitMask.angelBits[5]));
        //    //angels.push_back(new Peon(angel, "Soldier", BitMask.angelBits[6]));
        //    //angels.push_back(new Peon(angel, "Soldier", BitMask.angelBits[7]));
        //    //angels.push_back(new Guard(angel, "AngelicGuard", BitMask.angelBits[8]));
        //    //angels.push_back(new Guard(angel, "AngelicGuard", BitMask.angelBits[9]));

        //    Unit[] angels = new Unit[10];

        //    angels[0] = (new Champion(angel, "ArchAngel", BitMask.angelBits[0]));
        //    angels[1] = (new Knight(angel, "Pegasus", BitMask.angelBits[1]));
        //    angels[2] = (new Mage(angel, "HighAngel", BitMask.angelBits[2]));
        //    angels[3] = (new Archer(angel, "Archer", BitMask.angelBits[3]));
        //    angels[4] = (new Archer(angel, "Archer", BitMask.angelBits[4]));
        //    angels[5] = (new Peon(angel, "Soldier", BitMask.angelBits[5]));
        //    angels[6] = (new Peon(angel, "Soldier", BitMask.angelBits[6]));
        //    angels[7] = (new Peon(angel, "Soldier", BitMask.angelBits[7]));
        //    angels[8] = (new Guard(angel, "AngelicGuard", BitMask.angelBits[8]));
        //    angels[9] = (new Guard(angel, "AngelicGuard", BitMask.angelBits[9]));

        //    // Initialize Demon Army
        //    Faction demon = Faction.DEMON;

        //    Unit[] demons = new Unit[10];

        //    demons[0] = (new Champion(demon, "ArchDemon", BitMask.demonBits[0]));
        //    demons[1] = (new Knight(demon, "Nightmare", BitMask.demonBits[1]));
        //    demons[2] = (new Mage(demon, "DemonLord", BitMask.demonBits[2]));
        //    demons[3] = (new Archer(demon, "SkeletonArcher", BitMask.demonBits[3]));
        //    demons[4] = (new Archer(demon, "SkeletonArcher", BitMask.demonBits[4]));
        //    demons[5] = (new Peon(demon, "Imp", BitMask.demonBits[5]));
        //    demons[6] = (new Peon(demon, "Imp", BitMask.demonBits[6]));
        //    demons[7] = (new Peon(demon, "Imp", BitMask.demonBits[7]));
        //    demons[8] = (new Guard(demon, "BloodGuard", BitMask.demonBits[8]));
        //    demons[9] = (new Guard(demon, "BloodGuard", BitMask.demonBits[9]));
        //    //demons.push_back(new Knight(demon, "Nightmare", BitMask.demonBits[1]));
        //    //demons.push_back(new Mage(demon, "DemonLord", BitMask.demonBits[2]));
        //    //demons.push_back(new Archer(demon, "SkeletonArcher", BitMask.demonBits[3]));
        //    //demons.push_back(new Archer(demon, "SkeletonArcher", BitMask.demonBits[4]));
        //    //demons.push_back(new Peon(demon, "Imp", BitMask.demonBits[5]));
        //    //demons.push_back(new Peon(demon, "Imp", BitMask.demonBits[6]));
        //    //demons.push_back(new Peon(demon, "Imp", BitMask.demonBits[7]));
        //    //demons.push_back(new Guard(demon, "BloodGuard", BitMask.demonBits[8]));
        //    //demons.push_back(new Guard(demon, "BloodGuard", BitMask.demonBits[9]));

        //    // Place the armies on grid
        //    Tile tmpTile, copyTile;

        //    //place angel army on the grid
        //    for (int i = 0; i < BitMask.angelBits.Length; i++)
        //    {
        //        tmpTile = board.GetTileFromUnitID(BitMask.angelBits[i]);
        //        copyTile = GetTileFromUnitID(BitMask.angelBits[i]);
        //        if (tmpTile != null)
        //        {
        //            //this unit is still alive
        //            grid[(int)tmpTile.position.X][(int)tmpTile.position.Y].Unit = angels[i];
        //        }
        //        else
        //        {
        //            copyTile.Unit = null;
        //        }
        //    }

        //    //place demon army on the grid
        //    for (int i = 0; i < BitMask.demonBits.Length; i++)
        //    {
        //        tmpTile = board.GetTileFromUnitID(BitMask.demonBits[i]);
        //        copyTile = GetTileFromUnitID(BitMask.demonBits[i]);
        //        if (tmpTile != null)
        //        {
        //            //this unit is still alive
        //            grid[(int)tmpTile.position.X][(int)tmpTile.position.Y].Unit = demons[i];
        //        }
        //        else
        //        {
        //            copyTile.Unit = null;
        //        }
        //    }
        //    ControllingFaction = board.ControllingFaction;
        //    beginTurn();
        //}
        //#endregion

        #region Tile getter methods

        /// <summary>
        ///  Gets a tile specified by two integer values: x and y.
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        /// <returns>tile specified by the x and y values</returns>
        public Tile GetTile(int x, int y)
        {
            return board.Grid[x][y];
        }

        /// <summary>
        ///  Gets a tile specified by two float values: x and y.
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        /// <returns>tile specified by the x and y values</returns>
        public Tile GetTile(float x, float y)
        {
            return board.Grid[(int)x][(int)y];
        }

        /// <summary>
        /// Gets a tile specified by a Vector2.
        /// </summary>
        /// <param name="position">the position vector</param>
        /// <returns>tile specified by Vector2</returns>
        public Tile GetTile(Vector2 position)
        {
            return board.Grid[(int)position.X][(int)position.Y];
        }

        /// <summary>
        /// Gets a tile specified by a unit's ID.
        /// </summary>
        /// <param name="ID">the unit ID to search for.</param>
        /// <returns>tile specified by the unitID</returns>
        public Tile GetTileFromUnitID(int ID)
        {
            Tile returnTile = null;
            for (int i = 0; i < board.Grid.Length; i++)
            {
                foreach (Tile tile in board.Grid[i])
                {
                    if (tile.IsOccupied)
                    {
                        if (tile.Unit.ID == ID)
                        {
                            return tile;
                        }
                    }
                }
            }
            return returnTile;
        }

        /// <summary>
        ///  Gets the tile that the cursor is on
        /// </summary>
        /// <returns>the tile that the cursor is on.</returns>
        public Tile GetCurrentTile()
        {
            return board.Grid[(int)Cursor.position.X][(int)Cursor.position.Y];
        }
        #endregion

        #region Cursor methods
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
                board.Grid[(int)Cursor.position.X][(int)Cursor.position.Y].IsCurrentTile = false;
                Cursor.position.X += x;
                Cursor.position.Y += y;
                board.Grid[(int)Cursor.position.X][(int)Cursor.position.Y].IsCurrentTile = true;
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
                board.Grid[(int)Cursor.position.X][(int)Cursor.position.Y].IsCurrentTile = false;
                Cursor.position.X = x;
                Cursor.position.Y = y;
                board.Grid[(int)Cursor.position.X][(int)Cursor.position.Y].IsCurrentTile = true;
            }
        }
        #endregion

        /// <summary>
        /// Called at the beginning of a turn, decrements the recharge of the units.
        /// </summary>
        private void decrementRecharge()
        {
            //for (int i = 0; i < grid.Length; i++)
            for (int i = 0; i < board.Grid.Length; i++)
            {
                foreach (Tile tile in board.Grid[i])
                {
                    if (tile.IsOccupied)
                    {
                        if (tile.Unit.FactionType == controllingFaction)
                        {
                            if (tile.Unit.CurrRecharge > 0)
                            {
                                tile.Unit.CurrRecharge--;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sets the tiles as usable based on the current controlling faction.
        /// </summary>
        private void setTilesUsableByControllingFaction()
        {
#if DEBUG
            Debug.WriteLine("Setting tiles to usable with factionType: " + controllingFaction);
#endif
            for (int i = 0; i < board.Grid.Length; i++)
            {
                foreach (Tile tile in board.Grid[i])
                {
                    if (tile.IsOccupied)
                    {
                        if (tile.Unit.FactionType == controllingFaction && tile.Unit.CurrRecharge == 0)
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
        }

        /// <summary>
        /// Returns the last movement
        /// </summary>
        /// <returns>the most recent move</returns>
        public Move getLastMovement()
        {
            return lastMove;
        }

        #region Turn methods

        /// <summary>
        /// Gets called once each turn.
        /// - decrements the recharge.
        /// - Sets the tiles to movable based on the controlling faction.
        /// - Begins the move phase.
        /// </summary>
        public void beginTurn()
        {
#if DEBUG
            Debug.WriteLine("\nDEBUG: entering beginTurn()");
            Debug.WriteLine("Controlling Faction: " + ControllingFaction);
#endif
            isChampionAttack = false;
            decrementRecharge();
            setTilesUsableByControllingFaction();
            beginMovePhase();

#if DEBUG
            Debug.WriteLine("DEBUG: leaving beginTurn()");
#endif
        }

        /// <summary>
        /// Begins the move phase.
        /// </summary>
        private void beginMovePhase()
        {
            movePhase = true;
            attackPhase = false;
            //get all the valid moves
            bool thereAreMoves = moveFinder.findMoves();

            //if there are no moves, end the move phase
            if (!thereAreMoves)
            {
                endMovePhaseNoMove();
            }
        }

        /// <summary>
        /// Ends the move phase after no move was performed.
        /// </summary>
        public void endMovePhaseNoMove()
        {
            moveFinder.bitMaskAllTilesAsNotMovable();
            movePhase = false;
            beginAttackPhaseNoMove();
        }

        /// <summary>
        /// Ends the move phase after a move was performed.
        /// </summary>
        /// <param name="movedTile">the tile that was moved.</param>
        public void endMovePhaseAfterMove(Tile movedTile)
        {
            moveFinder.bitMaskAllTilesAsNotMovable();
            movePhase = false;
            beginAttackPhaseAfterMove(movedTile);
        }

        /// <summary>
        /// Begin the attack phase after no moves have been made.
        /// </summary>
        private void beginAttackPhaseNoMove()
        {
            //get all the valid attacks
            attackFinder.findAttacks();
            attackPhase = true;
            Unit currentUnit = GetCurrentTile().Unit;
            if (currentUnit != null)
            {
                if (currentUnit is Champion)
                {
                    attackFinder.bitMaskAllTilesForChampionAsNotAttackable(currentUnit.ID);
                }
            }
            //bool thereAreAttacks = attackFinder.findAttacks();
            //if (thereAreAttacks)
            //{
            //    attackPhase = true;
            //}
            //else
            //{
            //    endAttackPhase();
            //}
        }

        /// <summary>
        /// Begins the attack phase after a move has been performed.
        /// </summary>
        /// <param name="movedTile"></param>
        private void beginAttackPhaseAfterMove(Tile movedTile)
        {
            //get all the valid attacks, if it is not a champion
            attackFinder.findAttacksForTile(movedTile);
            attackPhase = true;
            Unit currentUnit = GetCurrentTile().Unit;
            if (currentUnit != null)
            {
                if (currentUnit is Champion)
                {
                    attackFinder.bitMaskAllTilesForChampionAsNotAttackable(currentUnit.ID);
                }
            }
            //bool thereAreAttacks = attackFinder.findAttacksForTile(movedTile);
            //if (thereAreAttacks)
            //{
            //    attackPhase = true;
            //}
            //else
            //{
            //    endAttackPhase();
            //}
        }

        /// <summary>
        /// Ends the attack phase of the game.
        /// </summary>
        public void endAttackPhase()
        {
            moveFinder.bitMaskAllTilesAsNotMovable();
            attackFinder.bitMaskAllTilesAsNotAttackable();
            attackFinder.bitMaskAllTilesAsNonCastable();
            isChampionAttack = false;
            movePhase = false;
            attackPhase = false;
        }

        /// <summary>
        /// Ends the turn by switching the controlling faction.
        /// </summary>
        public void endTurn()
        {
#if DEBUG
            Debug.WriteLine("\nDEBUG: entering endTurn()");
#endif
            //switch the controlling faction
            if (ControllingFaction == Faction.ANGEL)
            {
                ControllingFaction = Faction.DEMON;
            }
            else
            {
                ControllingFaction = Faction.ANGEL;
            }
            moveFinder.ControllingFaction = ControllingFaction;
            attackFinder.ControllingFaction = ControllingFaction;
            if (selectedTile != null)
            {
                selectedTile.IsSelected = false;
                selectedTile = null;
            }
            lastMove = null;
            lastCurrRecharge = 0;
            //endAttackPhase();
#if DEBUG
            Debug.WriteLine("DEBUG: leaving endTurn()");
#endif
        }

        /// <summary>
        /// Applies a turn to the board, which consists of a move and an attack.
        /// </summary>
        /// <param name="turn">the turn to apply to the board</param>
        public void applyTurn(Turn turn)
        {
#if DEBUG
            Debug.WriteLine("\nDEBUG: entering applyTurn()");
#endif
            if (turn != null)
            {
                applyMove(turn.Move);
                applyAttack(turn.Attack);
            }
#if DEBUG
            Debug.WriteLine("DEBUG: turn applied");
            Debug.WriteLine("DEBUG: leaving applyTurn()");
#endif
        }

        /// <summary>
        /// Applies a move to the board. If the move is not executable,
        /// that means we want to just change to the attack phase without moving.
        /// </summary>
        /// <param name="move">the move to apply, containing a start tile and end tile</param>
        public void applyMove(Move move)
        {
#if DEBUG
            Debug.WriteLine("\nDEBUG: entering applyMove()");
#endif
            lastMove = new Move(move.PreviousTile, move.NewTile);

            if (lastMove.Equals(move))
            {
                Console.WriteLine("SHIT!!!!!!");
            }
            if (move.IsExecutable)
            {
                if ((GetTile(move.PreviousTile).Unit != null))
                {

                    //set the recharge on the unit that is going to move
                    lastCurrRecharge = GetTile(move.PreviousTile).Unit.CurrRecharge;
                    GetTile(move.PreviousTile).Unit.CurrRecharge = GetTile(move.PreviousTile).Unit.TotalRecharge;
                    bitMaskSwapTiles(GetTile(move.NewTile), GetTile(move.PreviousTile));
#if DEBUG
                    Debug.WriteLine("DEBUG: move applied");
#endif
                }
                else
                {
                    Debug.WriteLine("Found a Tile with null unit");
                }
            }
            selectedTile = null;
            endMovePhaseAfterMove(GetTile(move.NewTile));
#if DEBUG
            Debug.WriteLine("DEBUG: leaving applyMove()");
#endif
        }

        /// <summary>
        /// Undoes the last move. Sets the board back to the state that it was before the move was applied.
        /// </summary>
        public void undoLastMove()
        {
#if DEBUG
            Debug.WriteLine("\nDEBUG: entering undoLastMove()");
#endif
            //if there is a lastMove
            if (lastMove != null)
            {
                if (lastMove.IsExecutable)
                {
                    //undo the recharge reset
                    GetTile(lastMove.PreviousTile).Unit.CurrRecharge = lastCurrRecharge;
                    bitMaskSwapTiles(GetTile(lastMove.NewTile), GetTile(lastMove.PreviousTile));
#if DEBUG
                    Debug.WriteLine("DEBUG: move UNapplied");
#endif
                }
                else
                {
                    Debug.WriteLine("MORE SHIT HAPPENED");
                }
            }
            lastMove = null;
            lastCurrRecharge = 0;
            selectedTile = null;
            attackFinder.bitMaskAllTilesAsNotAttackable();
            beginMovePhase();
#if DEBUG
            Debug.WriteLine("DEBUG: leaving undoLastMove()");
#endif
        }

        /// <summary>
        /// Applies an attack to the board.
        /// </summary>
        /// <param name="attack">the attack to apply, containing a start tile and end tile</param>
        public void applyAttack(Attack attack)
        {
#if DEBUG
            Debug.WriteLine("\nDEBUG: entering applyAttack()");
#endif
            if (attack.IsExecutable)
            {
                Tile victimTile = GetTile(attack.VictimPos);
                Tile attackerTile = GetTile(attack.AttackerPos);
                if (victimTile.IsOccupied && attackerTile.IsOccupied)
                {
                    if (attack is Spell)
                    {
                        Spell spell = attack as Spell;
                        applySpell(spell);
                    }
                    else
                    {
                        #region NonChampion
                        if (attackerTile.Unit is NonChampion)
                        {
                            bool isSplash = false;
                            for (int j = 0; j < attackerTile.Unit.Special.Length; j++)
                            {
                                if (attackerTile.Unit.Special[j] == specialType.SPLASH)
                                {
                                    isSplash = true;
                                }
                            }
                            NonChampion attacker = attackerTile.Unit as NonChampion;
#if DEBUG
                        Debug.Write("DEBUG: victim's HP before attack: ");
                        Debug.WriteLine(victimTile.Unit.CurrHP);
#endif
                            attacker.attack(victimTile.Unit);
                            //check to see if we killed the unit
                            if (victimTile.Unit.CurrHP == 0)
                            {
                                victimTile.Unit = null;
#if DEBUG
                            Debug.WriteLine("DEBUG: victim is dead");
#endif
                            }
                            //set the recharge on the unit that just attacked
                            attackerTile.Unit.CurrRecharge = attackerTile.Unit.TotalRecharge;

                            if (isSplash)
                            {
                                attackFinder.bitMaskAllTilesAsNotAttackable();
                                List splashAttacks = attackFinder.findSplashAttacks(2, attack.AttackerPos, attack.VictimPos, GetTile(attack.VictimPos), attackerTile.Unit.ID);

                                while (!splashAttacks.isEmpty())
                                {
                                    Attack splash = (Attack)splashAttacks.pop_front();
                                    victimTile = GetTile(splash.VictimPos);
                                    attacker.attack(victimTile.Unit);
                                    //check to see if we killed the unit
                                    if (victimTile.Unit.CurrHP == 0)
                                    {
                                        victimTile.Unit = null;
#if DEBUG
                                    Debug.WriteLine("DEBUG: victim is dead");
#endif
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
            }
            endAttackPhase();
#if DEBUG
            Debug.WriteLine("DEBUG: leaving applyAttack()");
#endif
        }

		/// <summary>
		/// Applys a spell to the board.
		/// </summary>
        private void applySpell(Spell spell)
        {
            if (spell is Teleport)
            {
                bitMaskSwapTiles(GetTile(spell.VictimPos), GetTile(spell.AttackerPos));
                Champion attackerUnit = GetTile(spell.AttackerPos).Unit as Champion;
                attackerUnit.CurrMP -= (int)SpellValues.spellCost.TELE;
            }
            else
            {
                Unit victimUnit = GetTile(spell.VictimPos).Unit;
                Champion attackerUnit = GetTile(spell.AttackerPos).Unit as Champion;
                spell.Cast(victimUnit, attackerUnit);
                if (victimUnit.CurrHP == 0)
                {
                    GetTile(spell.VictimPos).Unit = null;
                }
            }
        }

        #endregion

        #region AI interface methods

        /// <summary>
        /// Performs a deep clone of the AvDGame.
        /// </summary>
        /// <returns>A new AvDGame instance populated with the same data as this AvDGame.</returns>
        public Object Clone()
        {
            AvDGame other = this.MemberwiseClone() as AvDGame;

            other.board = new Board();
            other.board = (Board)this.board.Clone();

            //other.Grid = new Tile[this.x_size][];
            //for (int i = 0; i < this.x_size; i++)
            //{
            //    other.Grid[i] = new Tile[this.y_size];
            //}

            //for (int i = 0; i < other.Grid.Length; i++)
            //{
            //    for (int j = 0; j < other.Grid[i].Length; j++)
            //    {
            //        other.Grid[i][j] = this.Grid[i][j].Clone() as Tile;
            //        if (this.Grid[i][j].IsOccupied)
            //        {
            //            other.Grid[i][j].Unit = this.Grid[i][j].Unit.Clone() as Unit;
            //        }
            //    }
            //}
            other.ControllingFaction = this.ControllingFaction;
            other.moveFinder = new MoveFinder(other.board, other.controllingFaction);
            other.attackFinder = new AttackFinder(other.board, other.controllingFaction);
            return other;
        }

        /// <summary>
        /// Helper method for getValidTurns.
        /// Gets all the valid moves.
        /// </summary>
        /// <returns>a list of valid moves</returns>
        private List getValidMoves()
        {
            List moves = new List();

            //create the move where we don't actually move
            //moves.push_back(new Move(null, null));

            for (int i = 0; i < board.Grid.Length; i++)
            {
                foreach (Tile iTile in board.Grid[i])
                {
                    if (ControllingFaction == Faction.ANGEL)
                    {
                        //check the angels
                        for (int j = 0; j < BitMask.angelBits.Length; j++)
                        {
                            if ((iTile.MoveID & BitMask.angelBits[j]) != 0)
                            {
                                Tile unitTile = GetTileFromUnitID(BitMask.angelBits[j]);
                                moves.push_back(new Move(iTile.position, unitTile.position));
                            }
                        }
                    }
                    else
                    {
                        //check the demons
                        for (int j = 0; j < BitMask.demonBits.Length; j++)
                        {
                            if ((iTile.MoveID & BitMask.demonBits[j]) != 0)
                            {
                                Tile unitTile = GetTileFromUnitID(BitMask.demonBits[j]);
                                moves.push_back(new Move(iTile.position, unitTile.position));
                            }
                        }
                    }
                }
            }
            return moves;
        }

        /// <summary>
        /// Helper method for getValidTurns.
        /// Gets all the attacks based on a move.
        /// </summary>
        /// <param name="move">the move to base the attack on</param>
        /// <returns>a list of valid attacks</returns>
        /// <remarks>The reason for simply not adding a default Attack (<code>new Attack(Position.nil, Position.nil)</code>)
        /// is that we don't want to /// getValidAttacks</remarks>
        private List getValidAttacks(Move move)
        {
            List attacks = new List();

            //create the attack where we don't actually attack
            attacks.push_back(new Attack(Position.nil, Position.nil));

            if (move.IsExecutable)
            {
                //apply the move to the board and get the attacks we can do
                applyMove(move);
                Tile attackerTile = GetTile(move.NewTile);
                if (attackFinder.findAttacksForTile(attackerTile))
                {
                    for (int i = 0; i < board.Grid.Length; i++)
                    {
                        foreach (Tile victimTile in board.Grid[i])
                        {
                            if ((victimTile.AttackID & attackerTile.Unit.ID) != 0)
                            {
                                attacks.push_back(new Attack(victimTile.position, attackerTile.position));
                            }
                        }
                    }
                }
                //un-apply the move
                undoLastMove();
            }
            return attacks;
        }

        /// <summary>
        /// Gets a list of valid turns for the AI to use.
        /// </summary>
        /// <returns>a list of valid turns</returns>
        public List getValidTurns()
        {
            List moves = getValidMoves();
            List attacks = new List();
            List turns = new List();
            int moveCount = 0;
            int attackCount = 0;
            int turnCount = 0;

            while (!moves.isEmpty())
            {
                Move currentMove = (Move)moves.pop_front();
                if (currentMove.IsExecutable)
                {
                    moveCount++;
                }
                attacks = getValidAttacks(currentMove);
                Attack currentAttack;

                while (!attacks.isEmpty())
                {
                    currentAttack = (Attack)attacks.pop_front();
                    if (currentAttack.IsExecutable)
                    {
                        attackCount++;
                    }

                    if (currentMove.IsExecutable || currentAttack.IsExecutable)
                    {
                        turns.push_back(new Turn(currentMove, currentAttack));
                        turnCount++;
                    }
                }
            }
            Debug.WriteLine("There are " + turnCount + " valid turns");
            return turns;
        }

        #endregion

        #region Swapping tiles

        /// <summary>
        /// Swaps the current tile with the board's selected tile.
        /// </summary>
        /// <param name="currentTile">tile that we are going to move to</param>
        public void bitMaskSwapTile(Tile currentTile)
        {
            currentTile.Unit = selectedTile.Unit;
            selectedTile.Unit = null;
            selectedTile.IsSelected = false;
            selectedTile = null;
        }

        /// <summary>
        /// Swaps the units on two tiles.
        /// </summary>
        /// <param name="destTile">tile that we are going to move to</param>
        /// <param name="srcTile">tile that we are moving from</param>
        public void bitMaskSwapTiles(Tile destTile, Tile srcTile)
        {
            /* CANNOT USE CLONING HERE!!!!!!!
             * Otherwise bad shit happens
             */ 
            if (destTile.IsOccupied)
            {
                //we're swapping two units
                Unit tempUnit = destTile.Unit;

                destTile.Unit = srcTile.Unit;
                destTile.IsUsable = true;

                srcTile.Unit = tempUnit;
                srcTile.IsUsable = false;
                srcTile.IsSelected = false;
            }
            else
            {
                //we're moving a unit to an empty space
                destTile.Unit = srcTile.Unit;
                destTile.IsUsable = true;

                srcTile.Unit = null;
                srcTile.IsUsable = false;
                srcTile.IsSelected = false;
            }
        }

        #endregion

        #region Painting

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
                    //use the selectedTile version for bitmasking
                    if (selectedTile != null && selectedTile.IsOccupied && (board.Grid[i][j].MoveID & selectedTile.Unit.ID) != 0)
                    {
                        spriteBatch.Draw(board.Grid[i][j].sprite, board.Grid[i][j].rect, Color.Blue);
                    }
                    else if (selectedTile != null && selectedTile.IsOccupied && (board.Grid[i][j].AttackID & selectedTile.Unit.ID) != 0)
                    {
                        spriteBatch.Draw(board.Grid[i][j].sprite, board.Grid[i][j].rect, Color.Red);
                    }
                    else
                    {
                        spriteBatch.Draw(board.Grid[i][j].sprite, board.Grid[i][j].rect, Color.White);
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
                    if(board.Grid[i][j].IsOccupied)
                    {
                        //Unit Tempunit = grid[i][j].Unit;
                        //Texture2D TempTexture = Tempunit.sprite;
                        //spriteBatch.Draw(TempTexture, grid[i][j].rect, Color.White);
                        Unit Tempunit = board.Grid[i][j].Unit;
                        Texture2D TempTexture = Tempunit.sprite;
                        spriteBatch.Draw(TempTexture, board.Grid[i][j].rect, Color.White);
                    }
                    if (selectedTile != null && board.Grid[i][j].position == selectedTile.position)
                    {
                        spriteBatch.Draw(SelectedTile_Texture, board.Grid[i][j].rect, Color.Yellow);
                    }
                    if (selectedTile != null && selectedTile.IsOccupied && (board.Grid[i][j].AttackID & selectedTile.Unit.ID) != 0)
                    {
                        spriteBatch.Draw(AttackableTile_Texture, board.Grid[i][j].rect, Color.Red);
                    }
                    if (board.Grid[i][j].IsCurrentTile)
                    {
                        spriteBatch.Draw(Cursor_Texture, board.Grid[i][j].rect, Color.Silver);
                    }
                }
            }

            spriteBatch.End();

        }

        #endregion
    }
}
