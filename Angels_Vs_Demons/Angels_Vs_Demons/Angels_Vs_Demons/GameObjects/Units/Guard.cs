#region Using Statements
using System;
using Angels_Vs_Demons.Players;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Angels_Vs_Demons.GameObjects.Units
{
    /// <summary>
    /// Guard represents either a Blood Guard or Angelic Guard
    /// </summary>
    class Guard : NonChampion
    {
        #region Initialization


        public Guard(Texture2D loadedTexture, Faction factionType, string name, int id)
            : base(loadedTexture, factionType, name, id)
        {
            CurrHP = 50;
            TotalHP = 50;
            AttackPower = 25;
            AttackTypeVal = attackType.MELEE;
            Armor = armorType.HEAVY;
            Range = 2;
            Special = new specialType[]{specialType.HULKING};
            Movement = 2;
            CurrRecharge = 0;
            totalRecharge = 3;
        }


        #endregion

        /// <summary>
        /// Performs a deep clone of the Guard.
        /// </summary>
        /// <returns>A new Guard instance populated with the same data as this Guard.</returns>
        public override Object Clone()
        {
            Guard other = base.Clone() as Guard;
            return other;
        }

    }
}
