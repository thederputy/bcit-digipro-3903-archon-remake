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
    /// Knight represents either a Nightmare or Pegasus
    /// </summary>
    class Knight : Units
    {
        public Knight(Texture2D loadedTexture)
            : base(loadedTexture)
        {
            CurrHP = 60;
            totalHP = 60;
            attackPower = 25;
            attackTypeVal = attackType.MELEE;
            Armor = armorType.MAGIC;
            range = 1;
            special = new specialType[] { specialType.FLYING };
            movement = 4;
            CurrRecharge = 0;
            totalRecharge = 2;
        }

        public override void attack(AllUnits victim)
        {
            victim.CurrHP = victim.applyMitigation(attackPower, attackTypeVal);
        }
    }
}
