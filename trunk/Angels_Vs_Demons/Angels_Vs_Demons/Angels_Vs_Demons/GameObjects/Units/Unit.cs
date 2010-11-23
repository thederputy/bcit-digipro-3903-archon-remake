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
    /// Types of armor NonChampion can have. All NonChampion have 1 armor type.
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
    /// Types of special properties NonChampion can have. All NonChampion have one or more special properties. 
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
    /// Unit represents all unit objects.
    /// </summary>
    abstract class Unit : GameObject
    {
        #region Properties

        /// <summary>
        /// String representation of this unit.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string name;

        /// <summary>
        /// ID used for bitmasking the units; used for pathfinding.
        /// </summary>
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private int id;

        /// <summary>
        /// Angel/Demon association of the unit
        /// </summary>
        public Faction FactionType
        {
            get { return factionType; }
            set { factionType = value; }
        }

        private Faction factionType;

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
        public specialType[] Special
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
        }

        protected int totalRecharge;

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

        protected Unit(Texture2D loadedTexture, Faction factionType, string name, int id)
            : base(loadedTexture)
        {
            FactionType = factionType;
            Name        = name;
            this.id     = id;
        }

        #region Public Methods

        public override string ToString()
        {
            return name;
        }

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
