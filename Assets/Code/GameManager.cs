using System;
using System.Collections.Generic;
using System.Net.Mime;
using Assets.Code;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /* Editor Set */
    public GameObject PlayerPrefab;
    public GameObject CoinManagerPrefab;

    /* Properties */
    private bool _gameStarted;
    private float _timer;
    private const float CoinBurstTimer = 5f;

    /* References */
    private static Text _debugText;
    private static Slider _jumpSliderLeft;
    private static Slider _jumpSliderRight;
    private static Text _coinText;

    /* Constants */
    public const float LeftBound = -15;
    public const float RightBound = 15;
    public const float BottomBound = 0;
    public const float TopBound = 15;

	// Use this for initialization
	void Start ()
	{
	    _gameStarted = false;
	}

    void Awake()
    {
        _debugText = GameObject.Find("DebugText").GetComponent<Text>();
        _jumpSliderLeft = GameObject.Find("JumpSliderLeft").GetComponent<Slider>();
        _jumpSliderRight = GameObject.Find("JumpSliderRight").GetComponent<Slider>();
        _coinText = GameObject.Find("CoinText").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update ()
	{
	    if (Input.GetKeyDown(KeyCode.Tab))
	        CoinManager.CountCoins();

	    if (!_gameStarted)
	        return;

        _timer += Time.smoothDeltaTime;

        if (Network.isServer)
        {
            if (_timer % CoinBurstTimer < 0.02f && _timer > 5f && _timer < CoinBurstTimer*11)
                CreateCoins();
        }
        else
        {

        }
	}

    public void StartGame()
    {
        _gameStarted = true;

        if (Network.isServer)
        {
            CreateCoinManager();
        }
    }

    public void SpawnPlayer(string playerName)
    {
        var player = Network.Instantiate(PlayerPrefab, new Vector3(0, 5f, 0), Quaternion.identity, 0) as GameObject;
        player.GetComponent<Player>().SetName(playerName);
    }

    void CreateCoinManager()
    {
        Network.Instantiate(CoinManagerPrefab, Vector3.zero, Quaternion.identity, 0);
    }

    private void CreateCoins()
    {
       CoinManager.CreateCoinBurstAt(new Vector3(0, 7, 0));
    }

    public static void Debug(string text)
    {
        _debugText.text = text;
    }

    public static void UpdateJumpSliders(float inputHoldTime)
    {
        _jumpSliderLeft.value = _jumpSliderRight.value = 100 - (Math.Abs(inputHoldTime) / Player.MaximumInputHoldTime * 100);
    }

    public static void UpdateCoinText(int numberOfCoins)
    {
        _coinText.text = numberOfCoins.ToString();
    }
}
