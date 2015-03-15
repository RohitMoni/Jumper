using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Code
{
    static class CoinManager
    {
        /* Properties */
        private static readonly List<GameObject> UnusedCoins = new List<GameObject>();
        private static readonly List<GameObject> ActiveCoins = new List<GameObject>(); 

        /* References */
        private const string CoinPrefabName = "Prefabs/Coin";

        /* Functions */
        public static void CreateCoinAt(Vector3 position)
        {
            GameObject coin;

            // Use an unused coin first
            if (UnusedCoins.Count != 0)
            {
                coin = UnusedCoins[0];

                UnusedCoins.RemoveAt(0);

                coin.SetActive(true);
                coin.transform.position = position;
            }
            // If no unused coins exist, create a new one
            else
            {
                coin = Object.Instantiate(Resources.Load(CoinPrefabName) as GameObject);

                coin.transform.position = position;
            }

            // add the coin to the active coins list
            ActiveCoins.Add(coin);
        }

        public static int CountCoins()
        {
            return UnusedCoins.Count + ActiveCoins.Count;
        }

        public static void CollectCoin(GameObject coin)
        {
            ActiveCoins.Remove(coin);
            coin.SetActive(false);
            UnusedCoins.Add(coin);
        }
    }
}
