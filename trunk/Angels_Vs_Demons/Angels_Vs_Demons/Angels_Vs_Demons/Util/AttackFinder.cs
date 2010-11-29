#region Using Statements
using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Angels_Vs_Demons.BoardObjects;
using Angels_Vs_Demons.GameObjects;
using Angels_Vs_Demons.GameObjects.Units;
using Angels_Vs_Demons.Players;
using Angels_Vs_Demons.Screens.ScreenManagers;
using Angels_Vs_Demons.Screens;
#endregion

namespace Angels_Vs_Demons.Util
{
    class AttackFinder
    {
        private Board board;

        public Faction ControllingFaction
        {
            get { return controllingFaction; }
            set { controllingFaction = value; }
        }
        private Faction controllingFaction;

        /// <summary>
        /// Creates an attackfinder.
        /// </summary>
        /// <param name="newBoard">the board.</param>
        /// <param name="newControllingFaction">the controlling faction</param>
        public AttackFinder(Board newBoard, Faction newControllingFaction)
        {
            board = newBoard;
            controllingFaction = newControllingFaction;
        }

#if DEBUG
        /// <summary>
        /// Debugging method that writes the attack bitmask data of all tiles to the debug console.
        /// </summary>
        public void showAttackBitMasks()
        {
            Debug.WriteLine("\nDEBUG: entering showAttackBitMasks()");
            for (int i = 0; i < board.Grid.Length; i++)
            {
                foreach (Tile tile in board.Grid[i])
                {
                    showTileAttackBitMasks(tile);
                }
            }
            Debug.WriteLine("DEBUG: leaving showAttackBitMasks()");
        }

        /// <summary>
        /// Debugging method that writes the attack bitmask data of a tile to the debug console.
        /// </summary>
        /// <param name="currentTile"> the current tile to get the bitmask of.</param>
        private void showTileAttackBitMasks(Tile currentTile)
        {
            Debug.Write("DEBUG: x = ");
            Debug.Write(currentTile.position.X);
            Debug.Write(", y = ");
            Debug.Write(currentTile.position.Y);
            Debug.Write(": attackID = ");
            Debug.WriteLine(currentTile.AttackID);
        }
#endif

        /// <summary>
        /// Uses bit masking to mark the tiles as attackable.
        /// </summary>
        /// <returns>Whether an attack can be made this turn</returns>
        public bool findAttacks()
        {
#if DEBUG
            Debug.WriteLine("DEBUG: bit masking attacks");
#endif
            bool canAttack = false;
            int attackTotal = 0;
            for (int i = 0; i < board.Grid.Length; i++)
            {
                foreach (Tile tile in board.Grid[i])
                {
                    if (tile.IsOccupied)
                    {
                        //if we're checking one of the controlling units and it is usable
                        if (tile.Unit.FactionType == controllingFaction && tile.IsUsable)
                        {
                            if (tile.Unit is NonChampion)
                            {
                                bool isProjectile = false;
                                for (int j = 0; j < tile.Unit.Special.Length; j++)
                                {
                                    if (tile.Unit.Special[j] == specialType.PROJECTILE)
                                    {
                                        isProjectile = true;
                                    }
                                }
#if DEBUG
                                Debug.WriteLine("DEBUG: checking NonChampion");
#endif
                                NonChampion nc = tile.Unit as NonChampion;
                                int unitAttacks = 0;
                                unitAttacks = bitMaskAttacks(unitAttacks, nc.Range, isProjectile, tile.position, tile, tile.Unit.ID);
                                attackTotal += unitAttacks;
                            }
                            if (tile.Unit is Champion)
                            {
                                //do all the fancy magic stuff!?
                                Champion c = tile.Unit as Champion;
                                attackTotal = bitMaskSpells(c.position, c.CurrMP);
                            }
                        }
                    }
                }
            }
            Debug.WriteLine("DEBUG: attackTotal: " + attackTotal);
            if (attackTotal > 0)
            {
                canAttack = true;
            }
            return canAttack;
        }

        /// <summary>
        /// Gets the attacks for a given tile.
        /// </summary>
        /// <param name="tile">the tile to get the attacks of</param>
        /// <returns>true if the unit can attack</returns>
        public bool findAttacksForTile(Tile tile)
        {
            bool canAttack = false;
            int attackTotal = 0;
            if (tile.IsOccupied)
            {
                //if we're checking one of the controlling units and it is usable
                if (tile.Unit.FactionType == controllingFaction && tile.IsUsable)
                {
                    if (tile.Unit is NonChampion)
                    {
                        bool isProjectile = false;
                        for (int j = 0; j < tile.Unit.Special.Length; j++)
                        {
                            if (tile.Unit.Special[j] == specialType.PROJECTILE)
                            {
                                isProjectile = true;
                            }
                        }
#if DEBUG
                    Debug.WriteLine("DEBUG: checking NonChampion");
#endif
                        NonChampion nc = tile.Unit as NonChampion;
                        attackTotal = bitMaskAttacks(attackTotal, nc.Range, isProjectile, tile.position, tile, tile.Unit.ID);
                    }
                    if (tile.Unit is Champion)
                    {
                        //do the fancy magic stuff
                        Champion c = tile.Unit as Champion;
                        attackTotal = bitMaskSpells(c.position, c.CurrMP);
                    }
                }
            }
#if DEBUG
            Debug.WriteLine("DEBUG: attackTotal: " + attackTotal);
#endif
            if (attackTotal > 0)
            {
                canAttack = true;
            }
            return canAttack;
        }

        /// <summary>
        /// Uses bit masking to mark all tiles within a NonChampion's attack range containing enemy units as attackble.
        /// </summary>
        /// <param name="unitAttacks">the number of attacks this unit can make</param>
        /// <param name="range">The attack range of a NonChampion</param>
        /// <param name="isProjectile">true if the unit has a projectile attack</param>
        /// <param name="startPosition">The position of the unit that started the recursive call</param>
        /// <param name="currentTile">The current tile we are checking</param>
        /// <param name="attackID">the bitmask ID of the attacking unit</param>
        /// <returns>how many attacks the unit can make</returns>
        private int bitMaskAttacks(int unitAttacks, int range, bool isProjectile, Vector2 startPosition, Tile currentTile, int attackID)
        {
            range--;
            //as long as we're not checking the unit against itself
            if (currentTile.position != startPosition)
            {
                //if there's a unit on the new tile
                if (currentTile.IsOccupied)
                {
                    //if it is an opponent unit
                    if (currentTile.Unit.FactionType != controllingFaction)
                    {
                        //mark that we can attack it
                        currentTile.AttackID |= attackID;//OR EQUALS
                        unitAttacks++;
                    }
                    //don't continue looking past this unit, unless we have a projectile attack
                    if (!isProjectile)
                    {
                        return unitAttacks;
                    }
                }
            }
            if (range >= 0)
            {
                // are there tiles left, go left
                if (currentTile.position.X - 1 >= 0)
                {
                    currentTile.PathLeft = board.Grid[(int)currentTile.position.X - 1][(int)currentTile.position.Y];
                    unitAttacks = bitMaskAttacksLeft(unitAttacks, range, isProjectile, startPosition, currentTile.PathLeft, attackID);
                }
                // are there tiles right, go right
                if (currentTile.position.X + 1 < board.X_size)
                {
                    currentTile.PathRight = board.Grid[(int)currentTile.position.X + 1][(int)currentTile.position.Y];
                    unitAttacks = bitMaskAttacksRight(unitAttacks, range, isProjectile, startPosition, currentTile.PathRight, attackID);
                }
                // are there tiles above, go up
                if (currentTile.position.Y - 1 >= 0)
                {
                    currentTile.PathTop = board.Grid[(int)currentTile.position.X][(int)currentTile.position.Y - 1];
                    unitAttacks = bitMaskAttacksUp(unitAttacks, range, isProjectile, startPosition, currentTile.PathTop, attackID);
                }
                // are there tiles below, go down
                if (currentTile.position.Y + 1 < board.Y_size)
                {
                    currentTile.PathBottom = board.Grid[(int)currentTile.position.X][(int)currentTile.position.Y + 1];
                    unitAttacks = bitMaskAttacksDown(unitAttacks, range, isProjectile, startPosition, currentTile.PathBottom, attackID);
                }
            }
            return unitAttacks;
        }

        /// <summary>
        /// Uses bit masking to mark all tiles to the left of a NonChampion within the attack range containing enemy units as attackble.
        /// </summary>
        /// <param name="unitAttacks">the number of attacks this unit can make</param>
        /// <param name="range">The attack range of a NonChampion</param>
        /// <param name="isProjectile">true if the unit has a projectile attack</param>
        /// <param name="startPosition">The position of the unit that started the recursive call</param>
        /// <param name="currentTile">The current tile we are checking</param>
        /// <param name="attackID">the bitmask ID of the attacking unit</param>
        /// <returns>how many attacks the unit can make</returns>
        private int bitMaskAttacksLeft(int unitAttacks, int range, bool isProjectile, Vector2 startPosition, Tile currentTile, int attackID)
        {
            range--;
            //as long as we're not checking the unit against itself
            if (currentTile.position != startPosition)
            {
                //if there's a unit on the new tile
                if (currentTile.IsOccupied)
                {
                    //if it is an opponent unit
                    if (currentTile.Unit.FactionType != controllingFaction)
                    {
                        //mark that we can attack it
                        currentTile.AttackID |= attackID;//OR EQUALS
                        unitAttacks++;
                    }
                    //don't continue looking past this unit, unless we have a projectile attack
                    if (!isProjectile)
                    {
                        return unitAttacks;
                    }
                }
            }
            if (range >= 0)
            {
                // are there tiles left, go left
                if (currentTile.position.X - 1 >= 0)
                {
                    currentTile.PathLeft = board.Grid[(int)currentTile.position.X - 1][(int)currentTile.position.Y];
                    unitAttacks = bitMaskAttacksLeft(unitAttacks, range, isProjectile, startPosition, currentTile.PathLeft, attackID);
                }
            }
            return unitAttacks;
        }

        /// <summary>
        /// Uses bit masking to mark all tiles to the right of a NonChampion within the attack range containing enemy units as attackble.
        /// </summary>
        /// <param name="unitAttacks">the number of attacks this unit can make</param>
        /// <param name="range">The attack range of a NonChampion</param>
        /// <param name="isProjectile">true if the unit has a projectile attack</param>
        /// <param name="startPosition">The position of the unit that started the recursive call</param>
        /// <param name="currentTile">The current tile we are checking</param>
        /// <param name="attackID">the bitmask ID of the attacking unit</param>
        /// <returns>how many attacks the unit can make</returns>
        private int bitMaskAttacksRight(int unitAttacks, int range, bool isProjectile, Vector2 startPosition, Tile currentTile, int attackID)
        {
            range--;
            //as long as we're not checking the unit against itself
            if (currentTile.position != startPosition)
            {
                //if there's a unit on the new tile
                if (currentTile.IsOccupied)
                {
                    //if it is an opponent unit
                    if (currentTile.Unit.FactionType != controllingFaction)
                    {
                        //mark that we can attack it
                        currentTile.AttackID |= attackID;//OR EQUALS
                        unitAttacks++;
                    }
                    //don't continue looking past this unit, unless we have a projectile attack
                    if (!isProjectile)
                    {
                        return unitAttacks;
                    }
                }
            }
            if (range >= 0)
            {
                // are there tiles right, go right
                if (currentTile.position.X + 1 < board.X_size)
                {
                    currentTile.PathRight = board.Grid[(int)currentTile.position.X + 1][(int)currentTile.position.Y];
                    unitAttacks = bitMaskAttacksRight(unitAttacks, range, isProjectile, startPosition, currentTile.PathRight, attackID);
                }
            }
            return unitAttacks;
        }

        /// <summary>
        /// Uses bit masking to mark all tiles above a NonChampion within the attack range containing enemy units as attackble.
        /// </summary>
        /// <param name="unitAttacks">the number of attacks this unit can make</param>
        /// <param name="range">The attack range of a NonChampion</param>
        /// <param name="isProjectile">true if the unit has a projectile attack</param>
        /// <param name="startPosition">The position of the unit that started the recursive call</param>
        /// <param name="currentTile">The current tile we are checking</param>
        /// <param name="attackID">the bitmask ID of the attacking unit</param>
        /// <returns>how many attacks the unit can make</returns>
        private int bitMaskAttacksUp(int unitAttacks, int range, bool isProjectile, Vector2 startPosition, Tile currentTile, int attackID)
        {
            range--;
            //as long as we're not checking the unit against itself
            if (currentTile.position != startPosition)
            {
                //if there's a unit on the new tile
                if (currentTile.IsOccupied)
                {
                    //if it is an opponent unit
                    if (currentTile.Unit.FactionType != controllingFaction)
                    {
                        //mark that we can attack it
                        currentTile.AttackID |= attackID;//OR EQUALS
                        unitAttacks++;
                    }
                    //don't continue looking past this unit, unless we have a projectile attack
                    if (!isProjectile)
                    {
                        return unitAttacks;
                    }
                }
            }
            if (range >= 0)
            {
                // are there tiles above, go up
                if (currentTile.position.Y - 1 >= 0)
                {
                    currentTile.PathTop = board.Grid[(int)currentTile.position.X][(int)currentTile.position.Y - 1];
                    unitAttacks = bitMaskAttacksUp(unitAttacks, range, isProjectile, startPosition, currentTile.PathTop, attackID);
                }
            }
            return unitAttacks;
        }

        /// <summary>
        /// Uses bit masking to mark all tiles below a NonChampion within the attack range containing enemy units as attackble.
        /// </summary>
        /// <param name="unitAttacks">the number of attacks this unit can make</param>
        /// <param name="range">The attack range of a NonChampion</param>
        /// <param name="isProjectile">true if the unit has a projectile attack</param>
        /// <param name="startPosition">The position of the unit that started the recursive call</param>
        /// <param name="currentTile">The current tile we are checking</param>
        /// <param name="attackID">the bitmask ID of the attacking unit</param>
        /// <returns>how many attacks the unit can make</returns>
        private int bitMaskAttacksDown(int unitAttacks, int range, bool isProjectile, Vector2 startPosition, Tile currentTile, int attackID)
        {
            range--;
            //as long as we're not checking the unit against itself
            if (currentTile.position != startPosition)
            {
                //if there's a unit on the new tile
                if (currentTile.IsOccupied)
                {
                    //if it is an opponent unit
                    if (currentTile.Unit.FactionType != controllingFaction)
                    {
                        //mark that we can attack it
                        currentTile.AttackID |= attackID;//OR EQUALS
                        unitAttacks++;
                    }
                    //don't continue looking past this unit, unless we have a projectile attack
                    if (!isProjectile)
                    {
                        return unitAttacks;
                    }
                }
            }
            if (range >= 0)
            {
                // are there tiles below, go down
                if (currentTile.position.Y + 1 < board.Y_size)
                {
                    currentTile.PathBottom = board.Grid[(int)currentTile.position.X][(int)currentTile.position.Y + 1];
                    unitAttacks = bitMaskAttacksDown(unitAttacks, range, isProjectile, startPosition, currentTile.PathBottom, attackID);
                }
            }
            return unitAttacks;
        }

        /// <summary>
        /// Uses bit masking to mark all in a square radius defined by the <code>coverage</code> parameter from the attacked tile as attackble.
        /// </summary>
        /// <param name="coverage">the radius of the splash attack.</param>
        /// <param name="attackerPos">the position of the attacker.</param>
        /// <param name="startVictimPos">the position of the initial victim.</param>
        /// <param name="newVictimTile">the current tile we are checking.</param>
        /// <param name="attackID">the bitmask ID of the attacking unit.</param>
        /// <returns>a <code>List</code> of <code>Attack</code> objects that are part of the splash range.</returns>
        public List findSplashAttacks(int coverage, Vector2 attackerPos, Vector2 startVictimPos, Tile newVictimTile, int attackID)
        {
            coverage--;
            //as long as we're not checking the starting attack
            if (!newVictimTile.position.Equals(startVictimPos))
            {
                //if there's a unit there
                if (newVictimTile.IsOccupied)
                {
                    //if it is an enemy unit
                    if (newVictimTile.Unit.FactionType != controllingFaction)
                    {
                        //add an attack for that unit
                        newVictimTile.AttackID |= attackID;
                    }
                }
            }
            if (coverage >= 0)
            {
                // are there tiles left, go left
                if (newVictimTile.position.X - 1 >= 0)
                {
                    newVictimTile.PathLeft = board.Grid[(int)newVictimTile.position.X - 1][(int)newVictimTile.position.Y];
                    findSplashAttacks(coverage, attackerPos, startVictimPos, newVictimTile.PathLeft, attackID);
                }
                // are there tiles right, go right
                if (newVictimTile.position.X + 1 < board.X_size)
                {
                    newVictimTile.PathRight = board.Grid[(int)newVictimTile.position.X + 1][(int)newVictimTile.position.Y];
                    findSplashAttacks(coverage, attackerPos, startVictimPos, newVictimTile.PathRight, attackID);
                }
                // are there tiles above, go up
                if (newVictimTile.position.Y - 1 >= 0)
                {
                    newVictimTile.PathTop = board.Grid[(int)newVictimTile.position.X][(int)newVictimTile.position.Y - 1];
                    findSplashAttacks(coverage, attackerPos, startVictimPos, newVictimTile.PathTop, attackID);
                }
                // are there tiles below, go down
                if (newVictimTile.position.Y + 1 < board.Y_size)
                {
                    newVictimTile.PathBottom = board.Grid[(int)newVictimTile.position.X][(int)newVictimTile.position.Y + 1];
                    findSplashAttacks(coverage, attackerPos, startVictimPos, newVictimTile.PathBottom, attackID);
                }
            }
            return makeSplashAttacks(attackerPos, attackID);
        }

        /// <summary>
        /// Generates a <code>List</code> of <code>Attack</code>s that are generated on a splash attack.
        /// </summary>
        /// <param name="attackerPos">the attacker tile position</param>
        /// <param name="attackID">the <code>BitMask</code> attackID of the unit</param>
        /// <returns></returns>
        private List makeSplashAttacks(Vector2 attackerPos, int attackID)
        {
            List splashAttacks = new List();
            for (int i = 0; i < board.Grid.Length; i++)
            {
                foreach (Tile tile in board.Grid[i])
                {
                    if (tile.IsOccupied)
                    {
                        if ((tile.AttackID & attackID) != 0)
                        {
                            splashAttacks.push_back(new Attack(tile.position, attackerPos));
                        }
                    }
                }
            }
            return splashAttacks;
        }

        /// <summary>
        /// NOT IMPLEMENTED YET!!!!!!!!!!!!!!
        /// </summary>
        /// <returns>the number of spells that can be performed by a champion.</returns>
        private int bitMaskSpells(Vector2 championPos, int MP)
        {
            return 0;
        }

        private int bitMaskSpellBolt()
        {
            return 0;
        }

        private int bitMaskSpellBuff()
        {
            return 0;
        }

        private int bitMaskSpellHeal()
        {
            return 0;
        }

        private int bitMaskSpellRest()
        {
            return 0;
        }

        private int bitMaskSpellStun()
        {
            return 0;
        }

        private int bitMaskSpellTele()
        {
            return 0;
        }

        /// <summary>
        /// Iterates through the grid and masks all tiles as not attackable.
        /// </summary>
        public void bitMaskAllTilesAsNotAttackable()
        {
            for (int i = 0; i < board.Grid.Length; i++)
            {
                foreach (Tile tile in board.Grid[i])
                {
                    tile.AttackID = 0;
                    tile.SpellID = 0;
                }
            }
        }
    }
}
