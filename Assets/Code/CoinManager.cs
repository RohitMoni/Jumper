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
    private const int NumberOfCoinsInPool = 50;

    //
    void Start()
    {
        name = "CoinManager";
        InitialisePool(NumberOfCoinsInPool);
    }

    void Update()
    {
        SeparateCoins();
    }

    private static void InitialisePool(int numberOfCoins)
    {
        for (var i = 0; i < numberOfCoins; i++)
        {
            var coin = Network.Instantiate(Resources.Load(CoinPrefabName), Vector3.zero, Quaternion.identity, 0) as GameObject;
            coin.GetComponent<Coin>().Initialise();
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

            CreateRecycledCoin(position + velocity);
        }
    }

    private static void CreateRecycledCoin(Vector3 position)
    {
        // Use an unused coin first
        if (UnusedCoins.Count == 0) return;

        var coin = UnusedCoins[0];
        coin.GetComponent<Coin>().RePosition(position);

        coin.GetComponent<Coin>().Activate();
    }

    private static void SeparateCoins()
    {
        foreach (var coin in ActiveCoins)
        {
            var movement = Vector3.zero;

            foreach (var coin2 in ActiveCoins)
            {
                if (coin != coin2)
                {
                    var difference = coin.transform.position - coin2.transform.position;
                    var magnitude = difference.magnitude;
                    if (magnitude <= 0.6)
                    {
                        movement += 1f * difference;
                    }
                }
            }

            coin.GetComponent<Coin>().RePosition(coin.transform.position + movement);
        }
    }

    public static int CountCoins()
    {
        Debug.Log("Unused Coins: " + UnusedCoins.Count);
        Debug.Log("Active Coins: " + ActiveCoins.Count);
        return UnusedCoins.Count + ActiveCoins.Count;
    }
}
