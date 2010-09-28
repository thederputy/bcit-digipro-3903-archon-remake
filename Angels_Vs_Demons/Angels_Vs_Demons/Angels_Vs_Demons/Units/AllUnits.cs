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
    /// Types of armor units can have. All units have 1 armor type.
    /// </summary>
    public enum armorType
    {
        HEAVY,
        MEDIUM,
        LIGHT,
        MAGIC,
        IMBUED
    }

    /// <summary>
    /// Types of special properties units can have. All units have one or more special properties. 
    /// NONE indicates no special properties on that unit. 
    /// </summary>
    public enum specialType
    {
        NONE,
        FLYING,
        SPLASH,
        PROJECTILE,
        HULKING,
        CHAMPION
    }


    #endregion

    /// <summary>
    /// AllUnits represents all unit objects.
    /// </summary>
    abstract class AllUnits : GameObject
    {
        #region Properties


        /// <summary>
        /// Total Health Points of this unit.
        /// </summary>
        public int TotalHP
        {
            get { return totalHP; }
            set { totalHP = value; }
        }

        private int totalHP;

        /// <summary>
        /// Current Health Points of this unit.
        /// </summary>
        public int CurrHP
        {
            get { return currHP; }
            set { currHP = value; }
        }

        private int currHP;

        /// <summary>
        /// The amount of spaces this unit may move.
        /// </summary>
        public int Movement
        {
            get { return movement; }
            set { movement = value; }
        }

        private int movement;

        /// <summary>
        /// Armor Type of this unit. Determines damage mitigation.
        /// </summary>
        public armorType Armor
        {
            get { return armor; }
            set { armor = value; }
        }

        private armorType armor;

        /// <summary>
        /// Special Type(s), if any, of this unit.
        /// </summary>
        protected specialType[] Special
        {
            get { return special; }
            set { special = value; }
        }

        private specialType[] special;

        /// <summary>
        /// Total recharge time (in turns) of this unit. This value is constant and represents the value current 
        /// recharge is set to when an action is taken.   
        /// </summary>
        public int TotalRecharge
        {
            get { return totalRecharge; }
            set { totalRecharge = value; }
        }

        private int totalRecharge;

        /// <summary>
        /// Current recharge time (in turns) of this unit. When this value is greater than 0 this unit cannot act.
        /// </summary>
        public int CurrRecharge
        {
            get { return currRecharge; }
            set { currRecharge = value; }
        }

        private int currRecharge;


        #endregion

        protected AllUnits(Texture2D loadedTexture)
            : base(loadedTexture)
        {

        }

        #region Public Methods


        public int applyMitigation(int attackerAP, attackType attackerType)
        {
            int effectiveAP = attackerAP;

            switch (armor)
            {
                case armorType.HEAVY:
                    switch (attackerType)
                    {
                        case attackType.MAGIC:
                            effectiveAP += 5;
                            break;

                        case attackType.MELEE:
                            effectiveAP -= 5;
                            break;

                        case attackType.PROJECTILE:
                            effectiveAP -= 5;
                            break;
                    }
                    break;

                case armorType.MEDIUM:
                    break;

                case armorType.LIGHT:
                    switch (attackerType)
                    {
                        case attackType.MAGIC:
                            break;

                        case attackType.MELEE:
                            effectiveAP += 5;
                            break;

                        case attackType.PROJECTILE:
                            break;
                    }
                    break;

                case armorType.MAGIC:
                    switch (attackerType)
                    {
                        case attackType.MAGIC:
                            effectiveAP -= 10;
                            break;

                        case attackType.MELEE:
                            break;

                        case attackType.PROJECTILE:
                            break;
                    }
                    break;

                case armorType.IMBUED:
                    effectiveAP -= 15;
                    if (effectiveAP < 5)
                    {
                        effectiveAP = 5;
                    }
                    break;
            }

            return effectiveAP;
        }


        #endregion
    }
}
