#region Using Statements
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
    /// Represents a Imp or Soldier
    /// </summary>
    class Peon : NonChampion
    {
        #region Initialization


        public Peon(Texture2D loadedTexture, Faction factionType, string name)
            : base(loadedTexture, factionType, name)
        {
            CurrHP = 40;
            TotalHP = 40;
            AttackTypeVal = attackType.MELEE;
            AttackPower = 10;
            Armor = armorType.MEDIUM;
            Special = new specialType[] { specialType.NONE };
            Range = 1;
            Movement = 3;
            CurrRecharge = 0;
            TotalRecharge = 1;
        }


        #endregion

        public override void attack(Unit victim)
        {
            victim.CurrHP = victim.applyMitigation(AttackPower, AttackTypeVal);
        }
    }
}
