#region Using Statements
using System;
using Angels_Vs_Demons.Players;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Angels_Vs_Demons.GameObjects.Units
{
    /// <summary>
    /// Archer represents either a Skeleton Archer or Chosen One.
    /// </summary>
    class Archer : NonChampion
    {
        #region Initialization


        public Archer(Texture2D loadedTexture, Faction factionType, string name, int id)
            : base(loadedTexture, factionType, name, id)
        {
            CurrHP = 30;
            TotalHP = 30;
            AttackPower = 20;
            AttackTypeVal = attackType.PROJECTILE;
            Armor = armorType.LIGHT;
            CurrArmor = Armor;
            Range = 4;
            Special = new specialType[] {specialType.PROJECTILE};
            Movement = 2;
            CurrRecharge = 0;
            totalRecharge = 3;
        }

        #endregion

        /// <summary>
        /// Performs a deep clone of the Archer.
        /// </summary>
        /// <returns>A new Archer instance populated with the same data as this Archer.</returns>
        public override Object Clone()
        {
            Archer other = base.Clone() as Archer;
            return other;
        }
    }
}
