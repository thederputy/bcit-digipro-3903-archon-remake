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
    class Tile : GameObject
    {
        private Unit occupiedUnit;
        public Boolean isOccupied;
        public Boolean isCurrentTile;
        public Boolean isUsable;
        public Boolean isAngel;
        public Boolean isSelected;
        public Boolean isMovable;
        public Boolean isAttackable;
        public Color tileColor;
        public Tile pathLeft, pathRight, pathTop, pathBottom;
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
