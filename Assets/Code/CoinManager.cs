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
        private static readonly GameObject CoinAnchor = GameObject.Find("CoinAnchor");

        /* Constants */
        private const string CoinPrefabName = "Prefabs/Coin";
        private const int NumberOfCoinsInBurst = 10;

        /* Functions */
        public static void CreateCoinAt(Vector3 position)
        {
            var coin = CreateRecycledCoin();
            coin.transform.position = position;
        }

        public static void CreateCoinBurstAt(Vector3 position)
        {
            for (var i = 0; i < NumberOfCoinsInBurst; i++)
            {
                var coin = CreateRecycledCoin();

                var x = UnityEngine.Random.Range(-1f, 1f);
                var y = UnityEngine.Random.Range(-1f, 1f);

                var velocity = new Vector3(x, y, 0);

                coin.transform.position = position;
                //coin.GetComponent<Coin>().SetVelocity(velocity);
            }
        }

        private static GameObject CreateRecycledCoin()
        {
            GameObject coin;

            // Use an unused coin first
            if (UnusedCoins.Count != 0)
            {
                coin = UnusedCoins[0];

                UnusedCoins.RemoveAt(0);

                coin.SetActive(true);
            }
            // If no unused coins exist, create a new one
            else
            {
                coin = Object.Instantiate(Resources.Load(CoinPrefabName) as GameObject);
            }

            // Call our init function
            //coin.GetComponent<Coin>().Initialize();

            // add the coin to the active coins list
            ActiveCoins.Add(coin);

            // Parent it to the anchor
            coin.transform.parent = CoinAnchor.transform;

            return coin;
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
