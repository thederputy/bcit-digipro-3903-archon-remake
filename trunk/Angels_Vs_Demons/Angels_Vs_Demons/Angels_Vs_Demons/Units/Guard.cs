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
    /// Guard represents either a Blood Guard or Angelic Guard
    /// </summary>
    class Guard : Units
    {
        public Guard(Texture2D loadedTexture)
            : base(loadedTexture)
        {
            CurrHP = 50;
            totalHP = 50;
            attackPower = 25;
            attackTypeVal = attackType.MELEE;
            Armor = armorType.HEAVY;
            range = 2;
            special = new specialType[]{specialType.HULKING};
            movement = 3;
            CurrRecharge = 0;
            totalRecharge = 3;
        }

        public override void attack(AllUnits victim)
        {
            victim.CurrHP = victim.applyMitigation(attackPower, attackTypeVal);
        }
    }
}
