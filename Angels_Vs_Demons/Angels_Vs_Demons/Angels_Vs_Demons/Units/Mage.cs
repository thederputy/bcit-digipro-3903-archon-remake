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
    class Mage : NonChampion
    {
        #region Initialization


        public Mage(Texture2D loadedTexture)
            : base(loadedTexture)
        {
            CurrHP = 30;
            TotalHP = 30;
            AttackPower = 30;
            AttackTypeVal = attackType.MAGIC;
            Armor = armorType.MAGIC;
            Range = 3;
            Special = new specialType[] { specialType.SPLASH, specialType.PROJECTILE };
            Movement = 3;
            CurrRecharge = 0;
            TotalRecharge = 4;
        }


        #endregion

        public override void attack(Unit victim)
        {
            victim.CurrHP = victim.applyMitigation(AttackPower, AttackTypeVal);
        }
    }
}
