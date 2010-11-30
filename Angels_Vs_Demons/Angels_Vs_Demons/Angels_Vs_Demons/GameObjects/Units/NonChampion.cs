#region Using Statements
using System;
using Angels_Vs_Demons.Players;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Angels_Vs_Demons.GameObjects.Units
{
    #region Enumeration Classes


    /// <summary>
    /// Types of attacks. All non-champion NonChampion have an attack type. Attack type interacts with Unit.armorType.
    /// </summary>
    public enum attackType
    {
        /// <summary>
        /// Melee type
        /// </summary>
        MELEE,
        /// <summary>
        /// Projectile type
        /// </summary>
        PROJECTILE,
        /// <summary>
        /// Magic type
        /// </summary>
        MAGIC
    }


    #endregion


    /// <summary>
    /// NonChampion represents all non-champion NonChampion.
    /// </summary>
    abstract class NonChampion : Unit
    {
        #region Properties


        /// <summary>
        /// Atack Power represents the amount of damage this unit inflicts before mitigation.
        /// </summary>
        public int AttackPower
        {
            get { return attackPower; }
            set { attackPower = value; }
        }

        private int attackPower;

        /// <summary>
        /// The attack type property of this unit (MELEE, PROJECTILE, ETC).
        /// </summary>
        public attackType AttackTypeVal
        {
            get { return attackTypeVal; }
            set { attackTypeVal = value; }
        }

        private attackType attackTypeVal;
        

        public int Range
        {
            get { return range; }
            set { range = value; }
        }

        private int range;


        #endregion

        protected NonChampion(Texture2D loadedTexture, Faction factionType, string name, int id)
            : base(loadedTexture, factionType, name, id)
        {
        }

        #region Public Methods


        /// <summary>
        /// Default attack behaviour for Non-Champions.
        /// </summary>
        /// <param name="victim">the victim we are attacking</param>
        public virtual void attack(Unit victim)
        {
            victim.CurrHP -= victim.applyMitigation(AttackPower, AttackTypeVal);
            if (victim.CurrHP < 0)
            {
                victim.CurrHP = 0;
            }
        }

        /// <summary>
        /// Performs a deep clone of the NonChampion.
        /// </summary>
        /// <returns>A new NonChampion instance populated with the same data as this NonChampion.</returns>
        public override Object Clone()
        {
            NonChampion other = base.Clone() as NonChampion;
            return other;
        }


        #endregion

    }
}
