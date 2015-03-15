using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code
{
    static class CoinManager
    {
        /* Properties */
        private static List<GameObject> _unusedCoins;
        private static List<GameObject> _activeCoins; 

        /* References */


        /* Functions */
        public static void CreateCoinAt(Vector3 position)
        {
            // Use an unused coin first

            // If no unused coins exist, create a new one

            // add the coin to the active coins list
        }

        public static int CountCoins()
        {
            return _unusedCoins.Count + _activeCoins.Count;
        }

        public static void CollectCoin(GameObject coin)
        {
            _activeCoins.Remove(coin);
        }
    }
}
