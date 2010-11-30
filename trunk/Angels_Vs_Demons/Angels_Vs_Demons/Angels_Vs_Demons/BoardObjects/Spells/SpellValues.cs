#region Using statements

#endregion

namespace Angels_Vs_Demons.BoardObjects.Spells
{
    static class SpellValues
    {
        /// <summary>
        /// Enumerates the range for the spells
        /// </summary>
        public enum spellTypes
        {
            BOLT,
            BUFF,
            HEAL,
            REST,
            STUN,
            TELE,
            NONE
        }

        /// <summary>
        /// Enumerates the range for the spells
        /// </summary>
        public enum spellRange : int
        {
            BOLT = 3,
            BUFF = 3,
            HEAL = 6,
            REST = 1,
            STUN = 4,
            TELE = 5
        }

        /// <summary>
        /// Enumerates the mana cost for spells
        /// </summary>
        public enum spellCost : int
        {
            BOLT = 20,
            BUFF = 30,
            HEAL = 50,
            REST = 0,
            STUN = 20,
            TELE = 30
        }
    }
}
