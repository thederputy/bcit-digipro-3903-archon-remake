﻿#region Using Statements
using System;
using Angels_Vs_Demons.Players;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Angels_Vs_Demons.GameObjects.Units
{
    /// <summary>
    /// Represents a Imp or Soldier
    /// </summary>
    class Peon : NonChampion
    {
        #region Initialization


        public Peon(Texture2D loadedTexture, Faction factionType, string name, int id)
            : base(loadedTexture, factionType, name, id)
        {
            CurrHP = 40;
            TotalHP = 40;
            AttackTypeVal = attackType.MELEE;
            AttackPower = 10;
            Armor = armorType.MEDIUM;
            CurrArmor = Armor;
            Special = new specialType[] { specialType.NONE };
            Range = 1;
            Movement = 3;
            CurrRecharge = 0;
            totalRecharge = 3;
        }

        #endregion

        /// <summary>
        /// Performs a deep clone of the Peon.
        /// </summary>
        /// <returns>A new Peon instance populated with the same data as this Peon.</returns>
        public override Object Clone()
        {
            Peon other = base.Clone() as Peon;
            return other;
        }
    }
}
