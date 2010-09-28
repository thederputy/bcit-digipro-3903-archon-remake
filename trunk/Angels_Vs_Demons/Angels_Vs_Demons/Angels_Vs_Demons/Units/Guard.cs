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
        #region Initialization


        public Guard(Texture2D loadedTexture)
            : base(loadedTexture)
        {
            CurrHP = 50;
            TotalHP = 50;
            AttackPower = 25;
            AttackTypeVal = attackType.MELEE;
            Armor = armorType.HEAVY;
            Range = 2;
            Special = new specialType[]{specialType.HULKING};
            Movement = 3;
            CurrRecharge = 0;
            TotalRecharge = 3;
        }


        #endregion

        public override void attack(AllUnits victim)
        {
            victim.CurrHP = victim.applyMitigation(AttackPower, AttackTypeVal);
        }
    }
}
