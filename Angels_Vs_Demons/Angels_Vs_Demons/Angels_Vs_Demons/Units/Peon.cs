#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Angels_Vs_Demons
{
    /// <summary>
    /// Represents a Imp or Soldier
    /// </summary>
    class Peon : Units
    {
        public Peon(Texture2D loadedTexture)
            : base(loadedTexture)
        {
            CurrHP = 40;
            totalHP = 40;
            attackTypeVal = attackType.MELEE;
            attackPower = 10;
            Armor = armorType.MEDIUM;
            special = new specialType[] { specialType.NONE };
            range = 1;
            movement = 3;
            CurrRecharge = 0;
            totalRecharge = 1;
        }

        public override void attack(AllUnits victim)
        {
            victim.CurrHP = victim.applyMitigation(attackPower, attackTypeVal);
        }
    }
}
