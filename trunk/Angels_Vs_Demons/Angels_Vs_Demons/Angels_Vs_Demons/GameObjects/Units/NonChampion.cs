#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Angels_Vs_Demons.Players;
#endregion

namespace Angels_Vs_Demons.GameObjects.Units
{
    #region Enumeration Classes


    /// <summary>
    /// Types of attacks. All non-champion NonChampion have an attack type. Attack type interacts with Unit.armorType.
    /// </summary>
    public enum attackType
    {
        MELEE,
        PROJECTILE,
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

        protected NonChampion(Texture2D loadedTexture, Faction factionType, string name)
            : base(loadedTexture, factionType, name)
        {
        }

        #region Public Methods


        public abstract void attack(Unit victim);


        #endregion

    }
}
