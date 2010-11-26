﻿#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Angels_Vs_Demons.Players;
#endregion

namespace Angels_Vs_Demons.GameObjects.Units
{
    /// <summary>
    /// Archer represents either a Skeleton Archer or Chosen One.
    /// </summary>
    class Archer : NonChampion
    {
        #region Initialization


        public Archer(Texture2D loadedTexture, Faction factionType, string name, int id)
            : base(loadedTexture, factionType, name, id)
        {
            CurrHP = 30;
            TotalHP = 30;
            AttackPower = 20;
            AttackTypeVal = attackType.PROJECTILE;
            Armor = armorType.LIGHT;
            Range = 4;
            Special = new specialType[] {specialType.PROJECTILE};
            Movement = 2;
            CurrRecharge = 2;
            totalRecharge = 2;
        }

        #endregion

        /// <summary>
        /// Performs a deep clone of the Archer.
        /// </summary>
        /// <returns>A new Archer instance populated with the same data as this Archer.</returns>
        public override Object Clone()
        {
            Archer other = base.Clone() as Archer;
            return other;
        }
    }
}
