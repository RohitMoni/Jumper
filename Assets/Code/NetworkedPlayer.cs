using System;
using UnityEngine;

namespace Assets.Code
{
    public class NetworkedPlayer : MonoBehaviour {

        /* Properties */
        private int _numberOfCoins;

        // Use this for initialization
        void Awake()
        {
            _numberOfCoins = 0;
        }

        // Update is called once per frame
        void Update () {

        }

        public void CollectCoin()
        {
            _numberOfCoins++;
            //GameManager.UpdateCoinText(_numberOfCoins);
        }
    }
}
