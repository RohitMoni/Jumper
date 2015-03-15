using System;
using UnityEngine;

public class Player : MonoBehaviour {

    /* Properties */
    private bool _canInput;
    private float _inputHoldTime;
    private float _cooldownTimer;
    private int _numberOfCoins;

    /* References */
    private Rigidbody _rb;

    /* Constants */
    private const float MinimumInputHoldTime = 0.1f;
    private const float MaximumInputHoldTime = 0.5f;
    private const float JumpCooldown = 1f;

	// Use this for initialization
    void Awake()
    {
        _canInput = true;
        _inputHoldTime = _cooldownTimer = 0;
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = Vector3.zero;
        _numberOfCoins = 0;
    }

	// Update is called once per frame
	void Update () {
#if UNITY_EDITOR
	    HandleKeyboardInput();
#endif
        HandleTouchInput();

        if (!_canInput)
	        _cooldownTimer += Time.smoothDeltaTime;

	    if (_cooldownTimer >= JumpCooldown)
	    {
	        _canInput = true;
	        _cooldownTimer = 0;
	    }
	}

#if UNITY_EDITOR
    private void HandleKeyboardInput()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _inputHoldTime -= Time.smoothDeltaTime;

            _inputHoldTime = Math.Max(_inputHoldTime, -MaximumInputHoldTime);
            _inputHoldTime = Math.Min(_inputHoldTime, -MinimumInputHoldTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            _inputHoldTime += Time.smoothDeltaTime;

            _inputHoldTime = Math.Max(_inputHoldTime, MinimumInputHoldTime);
            _inputHoldTime = Math.Min(_inputHoldTime, MaximumInputHoldTime);
        }
        else if (_canInput && (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)))
        {
            SetRbVelocity();
        }
    }
#endif

    private void HandleTouchInput()
    {
        foreach (var touch in Input.touches)
        {
            if (touch.phase != TouchPhase.Canceled || touch.phase != TouchPhase.Ended)
            {
                if (touch.rawPosition.x <= Screen.width/2f)
                {
                    _inputHoldTime -= Time.smoothDeltaTime;

                    _inputHoldTime = Math.Max(_inputHoldTime, -MaximumInputHoldTime);
                    _inputHoldTime = Math.Min(_inputHoldTime, -MinimumInputHoldTime);
                }
                else
                {
                    _inputHoldTime += Time.smoothDeltaTime;

                    _inputHoldTime = Math.Max(_inputHoldTime, MinimumInputHoldTime);
                    _inputHoldTime = Math.Min(_inputHoldTime, MaximumInputHoldTime);
                }
            }
            else if (_canInput)
            {
                SetRbVelocity();
            }
        }
    }

    private void SetRbVelocity()
    {
        // We add to velocity here
        _rb.velocity = new Vector3(Mathf.Sign(_inputHoldTime), Math.Abs(_inputHoldTime) * 3, 0) * 5;
        _inputHoldTime = 0;
        _canInput = false;
    }

    public void CollectCoin()
    {
        _numberOfCoins++;
    }
}
