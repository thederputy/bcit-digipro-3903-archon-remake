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
    /// Knight represents either a Nightmare or Pegasus
    /// </summary>
    class Knight : NonChampion
    {
        #region Initialization


        public Knight(Texture2D loadedTexture, Faction factionType, string name, int id)
            : base(loadedTexture, factionType, name, id)
        {
            CurrHP = 60;
            TotalHP = 60;
            AttackPower = 25;
            AttackTypeVal = attackType.MELEE;
            Armor = armorType.MAGIC;
            Range = 1;
            Special = new specialType[] { specialType.FLYING };
            Movement = 4;
            CurrRecharge = 0;
            totalRecharge = 2;
        }


        #endregion

    }
}