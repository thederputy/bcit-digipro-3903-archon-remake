#region Using Statements
using System;
using System.Diagnostics;
#endregion

namespace Angels_Vs_Demons.BoardObjects
{
    class Turn : ICloneable
    {
        #region Fields

        private bool isExecutable;

        public bool IsExecutable
        {
            get {
                updateIsExecutable();
                return isExecutable;
            }
        }

        /// <summary>
        /// The move associated with this turn.
        /// </summary>
        public Move Move
        {
            get { return move; }
            set { move = value;
            updateIsExecutable();
            }
        }
        private Move move;

        /// <summary>
        /// The attack associated with this turn.
        /// </summary>
        public Attack Attack
        {
            get { return attack; }
            set { attack = value;
            updateIsExecutable();
            }
        }
        private Attack attack;

        #endregion

        #region Initialization

        /// <summary>
        /// Constructs a new turn object. If nothing is being done this turn, create new
        /// </summary>
        /// <param name="newMove">the move for this turn</param>
        /// <param name="newAttack">the attack for this turn</param>
        public Turn(Move newMove, Attack newAttack)
        {
            Move   = newMove;
            Attack = newAttack;
        }
        #endregion

        /// <summary>
        /// Checks and if the move is executable and returns the result.
        /// </summary>
        public void updateIsExecutable()
        {
#if DEBUG
            Debug.WriteLine("DEBUG: updating isExecutable");
#endif
            if (move == null)
            {
                isExecutable = false;
            }
            else if (attack == null)
            {
                isExecutable = false;
            }
            else if (!move.IsExecutable && !attack.IsExecutable)
            {
                isExecutable = false;
            }
            else
            {
                isExecutable = true;
            }
#if DEBUG
            Debug.WriteLine("DEBUG: isExecutable is now: " + isExecutable);
#endif
        }

        /// <summary>
        /// Performs a deep clone of the Turn.
        /// </summary>
        /// <returns>A new Turn instance populated with the same data as this Turn.</returns>
        public Object Clone()
        {
            Turn other = this.MemberwiseClone() as Turn;
            other.Move = this.Move.Clone() as Move;
            other.Attack = this.Attack.Clone() as Attack;
            return other;
        }
    }
}
