#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Angels_Vs_Demons.BoardObjects;
using Angels_Vs_Demons.GameObjects;
#endregion

namespace Angels_Vs_Demons.Screens.GameplayScreens
{
    class HvHGameplayScreen : GameplayScreen
    {
        /// <summary>
        /// Executes the move phase for a hotseat game
        /// </summary>
        protected override void executeMovePhase(Tile currentTile, Tile boardSelectedTile)
        {
            board.applyMove(new Move(currentTile, boardSelectedTile));
        }

        /// <summary>
        /// Processes the attack phase for a hotseat game.
        /// </summary>
        protected override void processAttackPhase()
        {
            ///this attack is just default for now, until we have attacking working
            Attack attack = new Attack(null, null);
            board.applyAttack(attack);
            board.endTurn();
        }

        protected override void executeAttackPhase(Tile currentTile, Tile boardSelectedTile)
        {

        }
    }
}
