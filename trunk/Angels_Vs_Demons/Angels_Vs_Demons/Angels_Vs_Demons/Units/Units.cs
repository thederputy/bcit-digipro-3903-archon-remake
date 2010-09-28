#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Angels_Vs_Demons
{
    #region Enumeration Classes


    /// <summary>
    /// Types of attacks. All non-champion units have an attack type. Attack type interacts with AllUnits.armorType.
    /// </summary>
    public enum attackType
    {
        MELEE,
        PROJECTILE,
        MAGIC
    }


    #endregion


    /// <summary>
    /// Units represents all non-champion units.
    /// </summary>
    abstract class Units : AllUnits
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

        protected Units(Texture2D loadedTexture)
            : base(loadedTexture)
        {

        }

        #region Public Methods


        public abstract void attack(AllUnits victim);


        #endregion

    }
}
