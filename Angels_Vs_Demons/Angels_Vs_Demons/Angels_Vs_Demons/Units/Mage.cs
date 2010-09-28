using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Angels_Vs_Demons
{
    /// <summary>
    /// Mage represents either a Demon Lord or High Angel
    /// </summary>
    class Mage : Units
    {
        public Mage(Texture2D loadedTexture)
            : base(loadedTexture)
        {
            CurrHP = 30;
            totalHP = 30;
            attackPower = 30;
            attackTypeVal = attackType.MAGIC;
            Armor = armorType.MAGIC;
            range = 3;
            special = new specialType[] { specialType.SPLASH, specialType.PROJECTILE };
            movement = 3;
            CurrRecharge = 0;
            totalRecharge = 4;
        }

        public override void attack(AllUnits victim)
        {
            victim.CurrHP = victim.applyMitigation(attackPower, attackTypeVal);
        }
    }
}
