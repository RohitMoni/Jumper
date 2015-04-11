using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class CoinManager : MonoBehaviour {

    /* Properties */
    public static readonly List<GameObject> UnusedCoins = new List<GameObject>();
    public static readonly List<GameObject> ActiveCoins = new List<GameObject>();

    /* Constants */
    private const string CoinPrefabName = "Prefabs/Coin";
    private const int NumberOfCoinsInBurst = 10;

    //
    void Start()
    {
        name = "CoinManager";
        InitialisePool(50);
    }

    private static void InitialisePool(int numberOfCoins)
    {
        for (var i = 0; i < numberOfCoins; i++)
        {
            var coin = Network.Instantiate(Resources.Load(CoinPrefabName), Vector3.zero, Quaternion.identity, 0) as GameObject;
            if (coin)
                coin.GetComponent<Coin>().DeActivate();
        }
    }

    /* Functions */
    public static void CreateCoinAt(Vector3 position)
    {
        CreateRecycledCoin(position);
    }

    public static void CreateCoinBurstAt(Vector3 position, int numberOfCoins = NumberOfCoinsInBurst)
    {
        for (var i = 0; i < numberOfCoins; i++)
        {
            var x = UnityEngine.Random.Range(-0.5f, 0.5f);
            var y = UnityEngine.Random.Range(-1f, 1f);

            var velocity = new Vector3(x, y, 0);

            var coin = CreateRecycledCoin(position + velocity);

            coin.GetComponent<Rigidbody>().AddForce(velocity * 10 * Time.smoothDeltaTime);
        }
    }

    private static GameObject CreateRecycledCoin(Vector3 position)
    {
        GameObject coin;

        // Use an unused coin first
        if (UnusedCoins.Count != 0)
        {
            coin = UnusedCoins[0];
            coin.GetComponent<Coin>().RePosition(position);
        }
        // If no unused coins exist, create a new one
        else
        {
            coin = Network.Instantiate(Resources.Load(CoinPrefabName), position, Quaternion.identity, 0) as GameObject;
        }

        // add the coin to the active coins list
        coin.GetComponent<Coin>().Activate();

        return coin;
    }

    public static int CountCoins()
    {
        return UnusedCoins.Count + ActiveCoins.Count;
    }
}
