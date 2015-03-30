using System;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    /* Properties */
    private bool _canInput;
    private bool _isTouching;
    private float _inputHoldTime;
    private float _cooldownTimer;
    private int _numberOfCoins;

    /* References */
    private Rigidbody _rb;

    /* Constants */
    private const float MinimumInputHoldTime = 0.1f;
    public const float MaximumInputHoldTime = 0.75f;
    private const float JumpCooldown = 0f;
    private const float JumpCoefficient = 4.00f;
    private const float DistanceToGround = 0.5f;

	// Use this for initialization
    void Awake()
    {
        _canInput = true;
        _isTouching = false;
        
        _inputHoldTime = _cooldownTimer = 0;
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = Vector3.zero;
        _numberOfCoins = 0;
    }

	// Update is called once per frame
	void Update () {

        // Handle input
        if (_canInput)
        {
#if UNITY_EDITOR
            HandleKeyboardInput();
#endif
            HandleTouchInput();
        }

        // Updating cooldown timer
        if (!_canInput)
	        _cooldownTimer += Time.smoothDeltaTime;

	    if (_cooldownTimer >= JumpCooldown && IsGrounded())
	    {
	        _canInput = true;
            _cooldownTimer = 0;
	    }

        // Limiting player's X coord
	    var position = transform.position;
	    position.x = Math.Max(position.x, GameManager.LeftBound);
	    position.x = Math.Min(position.x, GameManager.RightBound);
	    position.y = Math.Min(position.y, GameManager.TopBound);

	    transform.position = position;
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
        else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            SetRbVelocity();

            _inputHoldTime = 0;
        }

        GameManager.UpdateJumpSliders(_inputHoldTime);
    }
#endif

    private void HandleTouchInput()
    {
        if (Input.touchCount >= 1)
        {
            _isTouching = true;
            var touch = Input.touches[0];

            if (touch.rawPosition.x <= Screen.width / 2f)
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
        else if (_isTouching)
        {
            // Touch release
            _isTouching = false;

            SetRbVelocity();

            _inputHoldTime = 0;
        }

        GameManager.UpdateJumpSliders(_inputHoldTime);
    }

    private void SetRbVelocity()
    {
        // We add to velocity here
        _rb.velocity = new Vector3(Mathf.Sign(_inputHoldTime), Math.Abs(_inputHoldTime) * JumpCoefficient , 0) * JumpCoefficient;
        _inputHoldTime = 0;
        _cooldownTimer = 0;
        _canInput = false;
    }

    public void CollectCoin()
    {
        _numberOfCoins++;
        GameManager.UpdateCoinText(_numberOfCoins);
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, DistanceToGround + 0.1f);
    }
}
