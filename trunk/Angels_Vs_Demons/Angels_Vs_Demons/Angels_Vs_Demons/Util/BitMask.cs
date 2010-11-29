﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Angels_Vs_Demons.Util
{
    static class BitMask
    {
        public static int[] bits = { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384, 32768, 65536, 131072, 262144, 524288 };

        public static int[] angelBits = { 1024, 2048, 4096, 8192, 16384, 32768, 65536, 131072, 262144, 524288 };

        public static int[] demonBits = { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512 };

        /// <summary>
        /// Enumerates the bitmask values for the spells.
        /// </summary>
        public enum spells : int
        {
            BOLT = 1,
            BUFF = 2,
            HEAL = 4,
            REST = 8,
            STUN = 16,
            TELE = 32
        }
        
        public static int bitsTotal = initializeBitsTotal();

        public static int angelTotal = initializeAngelTotal();

        public static int demonTotal = initializeDemonTotal();

        private static int initializeBitsTotal()
        {
            int total = 0;
            for (int i = 0; i < bits.Length; i++)
            {
                total += bits[i];
            }
            return total;
        }

        private static int initializeAngelTotal() {
            int total = 0;
            for(int i = 0; i < angelBits.Length; i++)
            {
                total += angelBits[i];
            }
            return total;
        }

        private static int initializeDemonTotal()
        {
            int total = 0;
            for (int i = 0; i < demonBits.Length; i++)
            {
                total += demonBits[i];
            }
            return total;
        }
    }
}