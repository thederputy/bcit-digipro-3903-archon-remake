#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Angels_Vs_Demons.GameObjects.Units;
#endregion

namespace Angels_Vs_Demons.GameObjects
{
    class Tile : GameObject
    {
        private Unit occupiedUnit;
        private bool isOccupied;

        public bool IsOccupied
        {
            get { return isOccupied; }
            set { isOccupied = value; }
        }
        private bool isCurrentTile;

        public bool IsCurrentTile
        {
            get { return isCurrentTile; }
            set { isCurrentTile = value; }
        }
        private bool isUsable;

        public bool IsUsable
        {
            get { return isUsable; }
            set { isUsable = value; }
        }
        private bool isAngel;

        public bool IsAngel
        {
            get { return isAngel; }
            set { isAngel = value; }
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

        public Tile(Texture2D loadedTexture): base(loadedTexture)
        {
            this.pathLeft = null;
            this.pathRight = null;
            this.pathTop = null;
            this.pathBottom = null;
            this.tileColor = Color.White;
            this.occupiedUnit = null;
            this.isOccupied = false;
            this.isCurrentTile = false;
            this.isUsable = false;
            this.isAngel = false;
            this.isSelected = false;
            this.isMovable = false;
            this.isAttackable = false;
            this.direction = 0.0f;
            this.position = Vector2.Zero;
            this.sprite = loadedTexture;
        }
        public Unit getUnit()
        {
            return this.occupiedUnit;
        }
        public void setUnit(Unit newUnit)
        {
            if (newUnit != null)
            {
                this.isOccupied = true;
            }
            else
            {
                this.isOccupied = false;
            }
            this.occupiedUnit = newUnit;
        }
    }
}
