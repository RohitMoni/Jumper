﻿using System;
using System.Net.Mime;
using Assets.Code;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /* Properties */

    /* References */
    private static Text _debugText;
    private static Slider _jumpSliderLeft;
    private static Slider _jumpSliderRight;
    private static Text _coinText;

    /* Constants */

	// Use this for initialization
	void Start () {
	    CreateCoins();
	}

    void Awake()
    {
        _debugText = GameObject.Find("DebugText").GetComponent<Text>();
        _jumpSliderLeft = GameObject.Find("JumpSliderLeft").GetComponent<Slider>();
        _jumpSliderRight = GameObject.Find("JumpSliderRight").GetComponent<Slider>();
        _coinText = GameObject.Find("CoinText").GetComponent<Text>();
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

    public static void UpdateJumpSliders(float inputHoldTime)
    {
        _jumpSliderLeft.value = _jumpSliderRight.value = 100 - (Math.Abs(inputHoldTime) / Player.MaximumInputHoldTime * 100);
    }

    public static void UpdateCoinText(int numberOfCoins)
    {
        _coinText.text = numberOfCoins.ToString();
    }
}
