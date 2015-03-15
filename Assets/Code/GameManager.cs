using Assets.Code;
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    CreateCoins();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void CreateCoins()
    {
        CoinManager.CreateCoinAt(new Vector3(2, 4, 0));
    }
}
