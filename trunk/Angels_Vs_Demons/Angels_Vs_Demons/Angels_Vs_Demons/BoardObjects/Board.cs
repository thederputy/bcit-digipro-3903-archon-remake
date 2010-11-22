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
        private int x_size;
        private int y_size;
        private int tile_size;

        public Move lastMove;

        private Faction controllingFaction;

        public Faction ControllingFaction
        {
            get { return controllingFaction; }
            set { controllingFaction = value; }
        }

        public bool movePhase;
        public bool attackPhase;

        public Tile selectedTile;

        #region Textures (not needed for AI)
        private Texture2D Cursor_Texture;
        private Texture2D SelectedTile_Texture;
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
        #endregion

        /// <summary>
        /// Constructor for the board.
        /// </summary>
        /// <param name="CONTENT">the content manager</param>
        public Board(ContentManager CONTENT)
        {
            content = CONTENT;
            selectedTile = null;

            //creates a blank recent move for the class
            //lastMove = new Move();

            // Loads the textures
            gameFont = content.Load<SpriteFont>("MenuFont");
            debugFont = content.Load<SpriteFont>("debugFont");
            Cursor_Texture = content.Load<Texture2D>("Cursor");
            //SelectedTile_Texture = content.Load<Texture2D>("SelectedTile");
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

            // Place Angel army on grid
            grid[0][4].Unit = ArchAngel;
            grid[0][3].Unit = Pegasus;
            grid[0][5].Unit = HighAngel;
            grid[0][2].Unit = ChosenOne1;
            grid[0][6].Unit = ChosenOne2;
            grid[1][2].Unit = Soldier1;
            grid[1][4].Unit = Soldier2;
            grid[1][6].Unit = Soldier3;
            grid[1][3].Unit = AngelicGuard1;
            grid[1][5].Unit = AngelicGuard2;

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

            // Place Demon army on grid
            grid[9][4].Unit = ArchDemon;
            grid[9][5].Unit = Nightmare;
            grid[9][3].Unit = DemonLord;
            grid[9][2].Unit = SkeletonArcher1;
            grid[9][6].Unit = SkeletonArcher2;
            grid[8][2].Unit = Imp1;
            grid[8][4].Unit = Imp2;
            grid[8][6].Unit = Imp3;
            grid[8][3].Unit = BloodGuard1;
            grid[8][5].Unit = BloodGuard2;

            // Initiate first turn
            controllingFaction = Faction.ANGEL;
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
        ///  Gets a tile specified by two integer values: x and y.
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        /// <returns>tile specified by the x and y values</returns>
        public Tile GetTile(int x, int y)
        {
            return grid[x][y];
        }

        /// <summary>
        ///  Gets a tile specified by two float values: x and y.
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        /// <returns>tile specified by the x and y values</returns>
        public Tile GetTile(float x, float y)
        {
            return grid[(int)x][(int)y];
        }

        /// <summary>
        /// Gets a tile specified by a Vector2.
        /// </summary>
        /// <param name="position">the position vector</param>
        /// <returns>tile specified by Vector2</returns>
        public Tile GetTile(Vector2 position)
        {
            return grid[(int)position.X][(int)position.Y];
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
        /// Sets the tiles as usable based on the current controlling faction.
        /// </summary>
        /// <param name="factionType">the controlling faction</param>
        private void setTilesUsableByFaction(Faction factionType)
        {
#if DEBUG
            Console.WriteLine("Setting tiles to usable with factionType: " + factionType);
#endif
            for (int i = 0; i < grid.Length; i++)
            {
                foreach (Tile tile in grid[i])
                {
                    if (tile.Unit != null)
                    {
                        if (tile.Unit.FactionType == factionType)
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
        /// Checks to see if a Unit is a Champion.
        /// </summary>
        /// <param name="unit">the unit to check the type of</param>
        /// <returns>true if this Unit is a Champion, false if not</returns>
        private bool isChampion(Unit unit)
        {
            bool isChampion = false;
            if (unit is Champion)
            {
                isChampion = true;
            }
            return isChampion;
        }

        /// <summary>
        /// Checks to see if a Unit is a NonChampion.
        /// </summary>
        /// <param name="unit">the unit to check the type of</param>
        /// <returns>true if this Unit is a NonChampion, false if not</returns>
        private bool isNonChampion(Unit unit)
        {
            bool isNonChampion = false;
            if (unit is NonChampion)
            {
                isNonChampion = true;
            }
            return isNonChampion;
        }
        /// <summary>
        /// Returns the last movement
        /// </summary>
        /// <returns>the most recent move</returns>
        public Move getLastMovement()
        {
            return lastMove;
        }

        /// <summary>
        /// DEPRECATED
        /// </summary>
        /// <param name="move"></param>
        public void move(Move move)
        {
        }

        #region Turn methods

        /// <summary>
        /// Gets called once each turn.
        /// - Sets the tiles to movable based on the controlling faction.
        /// - Performs bitmask pathing for the turn.
        /// </summary>
        public void beginTurn()
        {
#if DEBUG
            Console.WriteLine("BEGINNING TURN");
            Console.WriteLine("Controlling Faction: " + ControllingFaction);
#endif
            movePhase = true;
            attackPhase = false;
            setTilesUsableByFaction(ControllingFaction);
            if (!bitMaskGetMoves())
            {
                movePhase = false;
                attackPhase = true;
            }
        }

        /// <summary>
        /// Ends the turn by switching the controlling faction and marking all tiles as not movable.
        /// </summary>
        public void endTurn()
        {
#if DEBUG
            Console.WriteLine("ENDING TURN");
            Console.WriteLine("Controlling Faction: " + ControllingFaction);
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
            movePhase = false;
            attackPhase = false;
            bitMaskAllTilesAsNotMovable();
            bitMaskAllTilesAsNotAttackable();
#if DEBUG
            Console.WriteLine("TURN ENDED");
            Console.WriteLine("Controlling Faction is now: " + ControllingFaction);
#endif
        }


        /// <summary>
        /// Applies a turn to the board.
        /// </summary>
        /// <param name="turn">the turn to apply to the board</param>
        public void applyTurn(Turn turn)
        {
            if (turn.Move.IsExecutable)
            {
                applyMove(turn.Move);
            }
            else
            {
                movePhase = false;
                attackPhase = true;
            }
            if (turn.Attack.IsExecutable)
            {
                applyAttack(turn.Attack);
            }
            else
            {
                attackPhase = false;
            }
#if DEBUG
            Console.WriteLine("DEBUG: turn applied, now ending turn");
#endif
            endTurn();
        }

        /// <summary>
        /// Applies a move to the board. If the move is null,
        /// that means we want to just change to the attack phase without moving.
        /// </summary>
        /// <param name="move">the move to apply, containing a start tile and end tile</param>
        public void applyMove(Move move)
        {
            if (move != null)
            {
                bitMaskSwapTiles(move.NewTile, move.PreviousTile);
            }
            selectedTile = null;
            movePhase = false;
            attackPhase = true;
            bitMaskAllTilesAsNotMovable();
#if DEBUG
            Console.WriteLine("DEBUG: move applied");
#endif
        }

        /// <summary>
        /// Applies an attack to the board.
        /// </summary>
        /// <param name="attack">the attack to apply, containing a start tile and end tile</param>
        public void applyAttack(Attack attack)
        {
            if (attack != null)
            {
                if (isNonChampion(attack.AttackerTile.Unit))
                {
#if DEBUG
                    Console.Write("DEBUG: victim's HP before attack: ");
                    Console.WriteLine(attack.VictimTile.Unit.CurrHP);
#endif
                    NonChampion nc = attack.AttackerTile.Unit as NonChampion;
                    nc.attack(attack.VictimTile.Unit);
#if DEBUG
                    Console.WriteLine("DEBUG: attack applied");
                    Console.Write("DEBUG: victim's HP after attack: ");
                    Console.WriteLine(attack.VictimTile.Unit.CurrHP);
#endif
                }
                if (isChampion(attack.AttackerTile.Unit))
                {

                }
            }
            attackPhase = false;
            bitMaskAllTilesAsNotAttackable();
#if DEBUG
            Console.WriteLine("DEBUG: attack applied");
#endif
        }


        /// <summary>
        /// NOT SURE WHO WROTE THIS, PLEASE COMMENT!!!
        /// </summary>
        /// <param name="attack"></param>
        private void makeAttack(Attack attack)
        {
        }

        #endregion

        public Object clone()
        {
            Board copy = new Board(content);

            copy.grid = this.grid;
            copy.controllingFaction = this.controllingFaction;
            copy.movePhase = this.movePhase;
            copy.attackPhase = this.attackPhase;

            return (Object)copy;
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
            getValidMoves();
            getValidAttacks();
            throw new NotImplementedException();
        }

        #region Move methods

#if DEBUG
        /// <summary>
        /// Debugging method that writes the move bitmask data of all tiles to the console.
        /// </summary>
        public void showMoveBitMasks()
        {
            Console.WriteLine("DEBUG: Move bitmasks");
            for (int i = 0; i < grid.Length; i++)
            {
                foreach (Tile tile in grid[i])
                {
                    showTileMoveBitMasks(tile);
                }
            }
        }

        /// <summary>
        /// Debugging method that writes the move bitmask data of a tile to the console.
        /// </summary>
        /// <param name="currentTile"> the current tile to get the bitmask of.</param>
        private void showTileMoveBitMasks(Tile currentTile)
        {
            Console.Write("x = ");
            Console.Write(currentTile.position.X);
            Console.Write(", y = ");
            Console.Write(currentTile.position.Y);
            Console.Write(": id = ");
            Console.WriteLine(GetTile(currentTile.position).MoveID);
        }
#endif

        /// <summary>
        /// Uses bit masking to mark the tiles as movable.
        /// </summary>
        public bool bitMaskGetMoves()
        {
            bool canMove = false;
            int moveTotal = 0;
            for (int i = 0; i < grid.Length; i++)
            {
                foreach (Tile tile in grid[i])
                {
                    if (tile.Unit != null)
                    {
                        //if we're checking one of the controlling units
                        if (tile.Unit.FactionType == controllingFaction)
                        {
                            int unitMoves = 0;
                            unitMoves = bitMaskMoves(unitMoves, tile.Unit.Movement, tile.position, tile, tile.Unit.ID);
                            moveTotal += unitMoves;
                        }
                    }
                }
            }
            Console.WriteLine("moveTotal: " + moveTotal);
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
        /// <param name="startPosition">The starting position of this recursive call</param>
        /// <param name="currentTile">The current tile we are checking</param>
        /// <param name="id">the ID of the unit we're checking</param>
        /// <returns>how many moves the unit can make</returns>
        private int bitMaskMoves(int unitMoves, int distance, Vector2 startPosition, Tile currentTile, int id)
        {
            distance--;
            //as long as we're not checking the unit against itself
            if (currentTile.position != startPosition)
            {
                //if we haven't moved to this tile before
                if ((GetTile(currentTile.position).MoveID & id) == 0)
                {
                    unitMoves++;
                    GetTile(currentTile.position).MoveID |= id;//OR EQUALS
                }
            }
            if (distance >= 0)
            {
                // are there tiles left and the tile is not occupied, go left
                if (currentTile.position.X - 1 >= 0 &&
                    grid[(int)currentTile.position.X - 1][(int)currentTile.position.Y].IsOccupied == false)
                {
                    currentTile.PathLeft = grid[(int)currentTile.position.X - 1][(int)currentTile.position.Y];
                    unitMoves = bitMaskMoves(unitMoves, distance, startPosition, currentTile.PathLeft, id);
                }
                // are there tiles right and the tile is not occupied, go right
                if (currentTile.position.X + 1 < x_size &&
                    grid[(int)currentTile.position.X + 1][(int)currentTile.position.Y].IsOccupied == false)
                {
                    currentTile.PathRight = grid[(int)currentTile.position.X + 1][(int)currentTile.position.Y];
                    unitMoves = bitMaskMoves(unitMoves, distance, startPosition, currentTile.PathRight, id);
                }
                // are there tiles above and the tile is not occupied, go up
                if (currentTile.position.Y - 1 >= 0 &&
                    grid[(int)currentTile.position.X][(int)currentTile.position.Y - 1].IsOccupied == false)
                {
                    currentTile.PathTop = grid[(int)currentTile.position.X][(int)currentTile.position.Y - 1];
                    unitMoves = bitMaskMoves(unitMoves, distance, startPosition, currentTile.PathTop, id);
                }
                // are there tiles below and the tile is not occupied, go down
                if (currentTile.position.Y + 1 < y_size &&
                    grid[(int)currentTile.position.X][(int)currentTile.position.Y + 1].IsOccupied == false)
                {
                    currentTile.PathBottom = grid[(int)currentTile.position.X][(int)currentTile.position.Y + 1];
                    unitMoves = bitMaskMoves(unitMoves, distance, startPosition, currentTile.PathBottom, id);
                }
            }
            return unitMoves;
        }

        /// <summary>
        /// Iterates through the grid and masks all tiles as not moveable.
        /// </summary>
        private void bitMaskAllTilesAsNotMovable()
        {
            for (int i = 0; i < grid.Length; i++)
            {
                foreach (Tile tile in grid[i])
                {
                    tile.MoveID = 0;
                }
            }
        }

        #endregion

        #region Attack methods
#if DEBUG
        /// <summary>
        /// Debugging method that writes the attack bitmask data of all tiles to the console.
        /// </summary>
        public void showAttackBitMasks()
        {
            Console.WriteLine("DEBUG: Attack bitmasks");
            for (int i = 0; i < grid.Length; i++)
            {
                foreach (Tile tile in grid[i])
                {
                    showTileAttackBitMasks(tile);
                }
            }
        }

        /// <summary>
        /// Debugging method that writes the attack bitmask data of a tile to the console.
        /// </summary>
        /// <param name="currentTile"> the current tile to get the bitmask of.</param>
        private void showTileAttackBitMasks(Tile currentTile)
        {
            Console.Write("x = ");
            Console.Write(currentTile.position.X);
            Console.Write(", y = ");
            Console.Write(currentTile.position.Y);
            Console.Write(": id = ");
            Console.WriteLine(GetTile(currentTile.position).AttackID);
        }
#endif

        /// <summary>
        /// Uses bit masking to mark the tiles as attackable.
        /// </summary>
        /// <returns>Whether an attack can be made this turn</returns>
        public bool bitMaskGetAttacks()
        {
#if DEBUG
            Console.WriteLine("DEBUG: bit masking attacks");
#endif
            bool canAttack = false;
            int attackTotal = 0;
            for (int i = 0; i < grid.Length; i++)
            {
                foreach (Tile tile in grid[i])
                {
                    if (tile.Unit != null)
                    {
                        //if we're checking one of the controlling units
                        if (tile.Unit.FactionType == controllingFaction)
                        {
#if DEBUG
                            Console.WriteLine("DEBUG: ckecking currently controllable unit: " + tile.Unit.Name);
#endif
                            if (isNonChampion(tile.Unit))
                            {
#if DEBUG
                                Console.WriteLine("DEBUG: checking NonChampion");
#endif
                                NonChampion nc = tile.Unit as NonChampion;
                                int unitAttacks = 0;
                                unitAttacks = bitMaskAttacks(unitAttacks, nc.Range, tile.position, tile, tile.Unit.ID);
                                attackTotal += unitAttacks;
                            }
                            if (isChampion(tile.Unit))
                            {
                                //do all the fancy magic stuff!
                            }
                        }
                    }
                }
            }
            Console.WriteLine("DEBUG: attackTotal: " + attackTotal);
            if (attackTotal > 0)
            {
                canAttack = true;
            }
            return canAttack;
        }

        /// <summary>
        /// Uses bit masking to all tiles within a NonChampion's attack range that are not occupied as attackble.
        /// </summary>
        /// <param name="unitMAttacks">the number of attacks this unit can make</param>
        /// <param name="range">The attack range of a NonChampion</param>
        /// <param name="startPosition">The position of the unit that started the recursive call</param>
        /// <param name="currentTile">The current tile we are checking</param>
        /// <param name="id">the bitmask ID of the unit</param>
        /// <returns>how many attacks the unit can make</returns>
        private int bitMaskAttacks(int unitAttacks, int range, Vector2 startPosition, Tile currentTile, int id)
        {
            range--;
            //as long as we're not checking the unit against itself
            if (currentTile.position != startPosition)
            {
                //if there's a unit on the new tile
                if (GetTile(currentTile.position).IsOccupied)
                {
                    //if it is an opponent unit
                    //GetTile(currentTile.position).Unit.FactionType = Faction.ANGEL;
                    if (GetTile(currentTile.position).Unit.FactionType != controllingFaction)
                    {
                        //mark that we can attack it
                        GetTile(currentTile.position).AttackID |= id;//OR EQUALS
                        unitAttacks++;
                    }
                    //don't continue looking past this unit
                    return unitAttacks;
                }
            }
            if (range >= 0)
            {
                // are there tiles left and the tile is not occupied, go left
                if (currentTile.position.X - 1 >= 0)// &&
                    //grid[(int)currentTile.position.X - 1][(int)currentTile.position.Y].IsOccupied == false)
                {
                    currentTile.PathLeft = grid[(int)currentTile.position.X - 1][(int)currentTile.position.Y];
                    unitAttacks = bitMaskAttacks(unitAttacks, range, startPosition, currentTile.PathLeft, id);
                }
                // are there tiles right and the tile is not occupied, go right
                if (currentTile.position.X + 1 < x_size)// &&
                    //grid[(int)currentTile.position.X + 1][(int)currentTile.position.Y].IsOccupied == false)
                {
                    currentTile.PathRight = grid[(int)currentTile.position.X + 1][(int)currentTile.position.Y];
                    unitAttacks = bitMaskAttacks(unitAttacks, range, startPosition, currentTile.PathRight, id);
                }
                // are there tiles above and the tile is not occupied, go up
                if (currentTile.position.Y - 1 >= 0)// &&
                    //grid[(int)currentTile.position.X][(int)currentTile.position.Y - 1].IsOccupied == false)
                {
                    currentTile.PathTop = grid[(int)currentTile.position.X][(int)currentTile.position.Y - 1];
                    unitAttacks = bitMaskAttacks(unitAttacks, range, startPosition, currentTile.PathTop, id);
                }
                // are there tiles below and the tile is not occupied, go down
                if (currentTile.position.Y + 1 < y_size)// &&
                    //grid[(int)currentTile.position.X][(int)currentTile.position.Y + 1].IsOccupied == false)
                {
                    currentTile.PathBottom = grid[(int)currentTile.position.X][(int)currentTile.position.Y + 1];
                    unitAttacks = bitMaskAttacks(unitAttacks, range, startPosition, currentTile.PathBottom, id);
                }
            }
            return unitAttacks;
        }

        /// <summary>
        /// Iterates through the grid and masks all tiles as not attackable.
        /// </summary>
        private void bitMaskAllTilesAsNotAttackable()
        {
            for (int i = 0; i < grid.Length; i++)
            {
                foreach (Tile tile in grid[i])
                {
                    tile.AttackID = 0;
                }
            }
        }

        #endregion

        #region BitMasking (swapping tiles)

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
        /// Swaps the values of two tiles.
        /// </summary>
        /// <param name="destTile">tile that we are going to move to</param>
        /// <param name="srcTile">tile that we are moving from</param>
        public void bitMaskSwapTiles(Tile destTile, Tile srcTile)
        {
            destTile.Unit = srcTile.Unit;
            destTile.IsUsable = true;
            srcTile.IsUsable = false;
            srcTile.Unit = null;
            srcTile.IsSelected = false;
            selectedTile.Unit = null;
            selectedTile.IsSelected = false;
            selectedTile = null;
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
                    if (selectedTile != null && selectedTile.Unit != null && (grid[i][j].MoveID & selectedTile.Unit.ID) != 0)
                    //if (grid[i][j].IsMovable)
                    {
                        spriteBatch.Draw(grid[i][j].sprite, grid[i][j].rect, Color.Blue);
                    }
                    else if (selectedTile != null && selectedTile.Unit != null && (grid[i][j].AttackID & selectedTile.Unit.ID) != 0)
                    //else if (grid[i][j].IsAttackable)
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
                    if (grid[i][j].IsSelected)
                    {

                    }

                }
            }

            spriteBatch.End();

        }

        #endregion
    }
}
