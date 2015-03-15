using System.Net.Mime;
using Assets.Code;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static Text _debugText;

	// Use this for initialization
	void Start () {
	    CreateCoins();
	}

    void Awake()
    {
        _debugText = GameObject.Find("DebugText").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void CreateCoins()
    {
        CoinManager.CreateCoinAt(new Vector3(2, 4, 0));
    }

    public static void Debug(string text)
    {
        _debugText.text = text;
    }
}
