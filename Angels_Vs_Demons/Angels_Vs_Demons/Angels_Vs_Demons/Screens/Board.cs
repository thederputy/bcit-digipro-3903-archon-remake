#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace Angels_Vs_Demons
{
    class Board : AbstractBoard
    {
        public ContentManager content;
        public SpriteFont gameFont;
        public SpriteFont debugFont;
        public GameObject Cursor;
        public Tile[][] grid;

        public Boolean isAngelTurn;
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

        // contains the recent move performed and is to be passed between classes and for undoing a move
        //public struct Move 
        //{
        //   public Tile originTile;
        //   public Tile newTile;
        //   public Unit unitMoved;
        //   public Move(Tile ORIGINTILE, Tile NEWTILE, Unit UNITMOVED) 
        //   {
        //      originTile = ORIGINTILE;
        //      newTile = NEWTILE;
        //      unitMoved = UNITMOVED;
        //   }
        //}

        public Move recentMove;

        public Board(ContentManager CONTENT)
        {
            content = CONTENT;
            isAngelTurn = true;
            movePhase = true;
            attackPhase = false;
            selectedTile = null;

            //creates a blank recent move for the class

            recentMove = new Move(null, null, null);

            // Loads the textures

            gameFont = content.Load<SpriteFont>("MenuFont");
            debugFont = content.Load<SpriteFont>("MenuFont");

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
                }
            }

            // Initializes the cursor

            Cursor = new GameObject(Cursor_Texture);
            Cursor.rect.X = grid[0][0].rect.X;
            Cursor.rect.Y = grid[0][0].rect.Y;
            Cursor.rect.Width = tile_size;
            Cursor.rect.Height = tile_size;
            grid[(int)Cursor.position.X][(int)Cursor.position.Y].isCurrentTile = true;

            // Initialize Demon Army

            Champion ArchDemon = new Champion(Arch_Demon_Texture);
            Knight Nightmare = new Knight(Nightmare_Texture);
            Mage DemonLord = new Mage(Demon_Lord_Texture);
            Archer SkeletonArcher1 = new Archer(Skeleton_Archer_Texture);
            Archer SkeletonArcher2 = new Archer(Skeleton_Archer_Texture);
            Peon Imp1 = new Peon(Imp_Texture);
            Peon Imp2 = new Peon(Imp_Texture);
            Peon Imp3 = new Peon(Imp_Texture);
            Guard BloodGuard1 = new Guard(Blood_Guard_Texture);
            Guard BloodGuard2 = new Guard(Blood_Guard_Texture);

            // Place Demon army on grid

            grid[0][4].setUnit(ArchDemon);
            grid[0][5].setUnit(Nightmare);
            grid[0][3].setUnit(DemonLord);
            grid[0][2].setUnit(SkeletonArcher1);
            grid[0][6].setUnit(SkeletonArcher2);
            grid[1][2].setUnit(Imp1);
            grid[1][4].setUnit(Imp2);
            grid[1][6].setUnit(Imp3);
            grid[1][3].setUnit(BloodGuard1);
            grid[1][5].setUnit(BloodGuard2);

            // Register Demon army as demons

            grid[0][4].isAngel = false;
            grid[0][5].isAngel = false;
            grid[0][3].isAngel = false;
            grid[0][2].isAngel = false;
            grid[0][6].isAngel = false;
            grid[1][2].isAngel = false;
            grid[1][4].isAngel = false;
            grid[1][6].isAngel = false;
            grid[1][3].isAngel = false;
            grid[1][5].isAngel = false;

            // Initialize Angel Army

            Champion ArchAngel = new Champion(Arch_Angel_Texture);
            Knight Pegasus = new Knight(Pegasus_Texture);
            Mage HighAngel = new Mage(High_Angel_Texture);
            Archer ChosenOne1 = new Archer(Chosen_One_Texture);
            Archer ChosenOne2 = new Archer(Chosen_One_Texture);
            Peon Soldier1 = new Peon(Soldier_Texture);
            Peon Soldier2 = new Peon(Soldier_Texture);
            Peon Soldier3 = new Peon(Soldier_Texture);
            Guard AngelicGuard1 = new Guard(Angelic_Guard_Texture);
            Guard AngelicGuard2 = new Guard(Angelic_Guard_Texture);

            // Place Angel army on grid

            grid[9][4].setUnit(ArchAngel);
            grid[9][3].setUnit(Pegasus);
            grid[9][5].setUnit(HighAngel);
            grid[9][2].setUnit(ChosenOne1);
            grid[9][6].setUnit(ChosenOne2);
            grid[8][2].setUnit(Soldier1);
            grid[8][4].setUnit(Soldier2);
            grid[8][6].setUnit(Soldier3);
            grid[8][3].setUnit(AngelicGuard1);
            grid[8][5].setUnit(AngelicGuard2);

            // Register Angel army as angels

            grid[9][4].isAngel = true;
            grid[9][3].isAngel = true;
            grid[9][5].isAngel = true;
            grid[9][2].isAngel = true;
            grid[9][6].isAngel = true;
            grid[8][2].isAngel = true;
            grid[8][4].isAngel = true;
            grid[8][6].isAngel = true;
            grid[8][3].isAngel = true;
            grid[8][5].isAngel = true;

            // Initiate first turn

            grid[9][4].isUsable = true;
            grid[9][5].isUsable = true;
            grid[9][3].isUsable = true;
            grid[9][2].isUsable = true;
            grid[9][6].isUsable = true;
            grid[8][2].isUsable = true;
            grid[8][4].isUsable = true;
            grid[8][6].isUsable = true;
            grid[8][3].isUsable = true;
            grid[8][5].isUsable = true;

            grid[0][4].isUsable = false;
            grid[0][5].isUsable = false;
            grid[0][3].isUsable = false;
            grid[0][2].isUsable = false;
            grid[0][6].isUsable = false;
            grid[1][2].isUsable = false;
            grid[1][4].isUsable = false;
            grid[1][6].isUsable = false;
            grid[1][3].isUsable = false;
            grid[1][5].isUsable = false;
        }

        //Gets the current state of the board
        public Tile[][] GetBoard()
        {
            return grid;
        }

        //Gets a specific tile
        public Tile GetTile(int x, int y)
        {
            return grid[x][y];
        }

        //Gets the current tile
        public Tile GetCurrentTile()
        {
            return grid[(int)Cursor.position.X][(int)Cursor.position.Y];
        }

        //Moves the cursor by the amount inputed, resets current tile
        public void moveCursor(int x, int y)
        {
            if (Cursor.position.X + x < x_size && Cursor.position.X + x >= 0 &&
                Cursor.position.Y + y < y_size && Cursor.position.Y + y >= 0)
            {
                grid[(int)Cursor.position.X][(int)Cursor.position.Y].isCurrentTile = false;
                Cursor.position.X += x;
                Cursor.position.Y += y;
                grid[(int)Cursor.position.X][(int)Cursor.position.Y].isCurrentTile = true;
            }
        }

        public int Get_xSize()
        {
            return x_size;
        }
        public int Get_ySize()
        {
            return y_size;
        }
        public void PaintGrid(SpriteBatch SPRITEBATCH)
        {
            SpriteBatch spriteBatch = SPRITEBATCH;

            spriteBatch.Begin();

            // Draw the grid on the screen

            for (int i = 0; i < x_size; i++)
            {
                for (int j = 0; j < y_size; j++)
                {
                    if (grid[i][j].isMovable)
                    {
                        spriteBatch.Draw(grid[i][j].sprite, grid[i][j].rect, Color.Blue);
                    }
                    else if (grid[i][j].isAttackable)
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
        public void PaintUnits(SpriteBatch SPRITEBATCH)
        {

            SpriteBatch spriteBatch = SPRITEBATCH;

            spriteBatch.Begin();

            for (int i = 0; i < x_size; i++)
            {
                for (int j = 0; j < y_size; j++)
                {
                    if (grid[i][j].isOccupied)
                    {
                        Unit Tempunit = grid[i][j].getUnit();
                        Texture2D TempTexture = Tempunit.sprite;
                        spriteBatch.Draw(TempTexture, grid[i][j].rect, Color.White);
                    }
                    if (grid[i][j].isCurrentTile)
                    {
                        spriteBatch.Draw(Cursor_Texture, grid[i][j].rect, Color.Silver);
                    }

                }
            }

            spriteBatch.End();

        }

        public object clone()
        {
            throw new NotImplementedException();
        }

        public System.Collections.Generic.List<Move> getValidMoves()
        {
            throw new NotImplementedException();
        }
    }
}
