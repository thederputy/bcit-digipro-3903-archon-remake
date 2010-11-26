#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Angels_Vs_Demons.BoardObjects;
using Angels_Vs_Demons.GameObjects.Units;
using System.Collections.Generic;
#endregion

namespace Angels_Vs_Demons.GameObjects
{
    class Tile : GameObject, ICloneable
    {
        /// <summary>
        /// The bitmasked value of who can move to this tile.
        /// </summary>
        public int MoveID
        {
            get { return moveID; }
            set { moveID = value; }
        }
        private int moveID;

        /// <summary>
        /// The bitmasked value of who can attack to this tile.
        /// </summary>
        public int AttackID
        {
            get { return attackID; }
            set { attackID = value; }
        }
        private int attackID;

        /// <summary>
        /// The bitmasked value of what spells can be performed on this tile.
        /// </summary>
        public int SpellID
        {
            get { return spellID; }
            set { spellID = value; }
        }
        private int spellID;

        private Unit unit;

        /// <summary>
        /// The unit on this tile
        /// </summary>
        public Unit Unit
        {
            get { return unit; }
            set { 
                if (value != null)
                {
                    isOccupied = true;
                }
                else
                {
                    isOccupied = false;
                }
                unit = value;
            }
        }

        private bool isOccupied;

        /// <summary>
        /// Returns whether there is a unit on this tile or not
        /// </summary>
        public bool IsOccupied
        {
            get { return isOccupied; }
        }

        private bool isCurrentTile;

        /// <summary>
        /// Gets/sets whether this tile is the current tile (cursor is on it)
        /// </summary>
        public bool IsCurrentTile
        {
            get { return isCurrentTile; }
            set { isCurrentTile = value; }
        }


        private bool isUsable;

        /// <summary>
        /// Stores whether this tile is usable this turn.
        /// </summary>
        public bool IsUsable
        {
            get { return isUsable; }
            set { isUsable = value; }
        }


        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }

        private bool isMovable;

        public bool IsMovable
        {
            get { return isMovable; }
            set { isMovable = value; }
        }
        private bool isAttackable;

        public bool IsAttackable
        {
            get { return isAttackable; }
            set { isAttackable = value; }
        }
        private Color tileColor;

        public Color TileColor
        {
            get { return tileColor; }
            set { tileColor = value; }
        }
        
        public Tile pathLeft, pathRight, pathTop, pathBottom;

        internal Tile PathBottom
        {
            get { return pathBottom; }
            set { pathBottom = value; }
        }

        internal Tile PathTop
        {
            get { return pathTop; }
            set { pathTop = value; }
        }

        internal Tile PathRight
        {
            get { return pathRight; }
            set { pathRight = value; }
        }

        internal Tile PathLeft
        {
            get { return pathLeft; }
            set { pathLeft = value; }
        }

        /// <summary>
        /// Texture constructor.
        /// </summary>
        /// <param name="loadedTexture"></param>
        public Tile(Texture2D loadedTexture): base(loadedTexture)
        {
            this.pathLeft = null;
            this.pathRight = null;
            this.pathTop = null;
            this.pathBottom = null;
            this.tileColor = Color.White;
            this.unit = null;
            this.isOccupied = false;
            this.isCurrentTile = false;
            this.isUsable = false;
            this.isSelected = false;
            this.isMovable = false;
            this.isAttackable = false;
            this.direction = 0.0f;
            this.position = Vector2.Zero;
            this.sprite = loadedTexture;
            moveID = 0;
            attackID = 0;
            spellID = 0;
        }

        /// <summary>
        /// Performs a deep clone of the Tile.
        /// </summary>
        /// <returns>A new Tile instance populated with the same data as this Tile.</returns>
        public override Object Clone()
        {
            Tile other = base.Clone() as Tile;
            if (this.IsOccupied)
            {
                other.Unit = this.Unit.Clone() as Unit;
            }
            return other;
        }
    }
}
