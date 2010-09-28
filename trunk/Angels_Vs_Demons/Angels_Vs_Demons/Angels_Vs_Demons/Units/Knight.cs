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
        #region Initialization


        public Knight(Texture2D loadedTexture)
            : base(loadedTexture)
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
            TotalRecharge = 2;
        }


        #endregion

        public override void attack(AllUnits victim)
        {
            victim.CurrHP = victim.applyMitigation(AttackPower, AttackTypeVal);
        }
    }
}
