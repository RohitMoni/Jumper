using System;
using UnityEngine;

public class Player : MonoBehaviour {

    /* Properties */
    private bool _canInput;
    private bool _isTouching;
    private float _distToGround;
    private float _inputHoldTime;
    private float _cooldownTimer;
    private int _numberOfCoins;

    /* References */
    private Rigidbody _rb;

    /* Constants */
    private const float MinimumInputHoldTime = 0.1f;
    private const float MaximumInputHoldTime = 0.5f;
    private const float JumpCooldown = 1f;

    private const float PlayerLeftBoundX = -15;
    private const float PlayerRightBoundX = 15;

	// Use this for initialization
    void Awake()
    {
        _canInput = true;
        _isTouching = false;
        _distToGround = GetComponent<Collider>().bounds.extents.y;
        
        _inputHoldTime = _cooldownTimer = 0;
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = Vector3.zero;
        _numberOfCoins = 0;

    }

	// Update is called once per frame
	void Update () {

        // Handle input
#if UNITY_EDITOR
	    HandleKeyboardInput();
#endif
        HandleTouchInput();

        // Updating cooldown timer
        if (!_canInput)
	        _cooldownTimer += Time.smoothDeltaTime;

	    if (_cooldownTimer >= JumpCooldown && IsGrounded())
	    {
            GameManager.Debug(_cooldownTimer.ToString());
	        _canInput = true;
            _cooldownTimer = 0;
	    }

        // Limiting player's X coord
	    var position = transform.position;
	    position.x = Math.Max(position.x, PlayerLeftBoundX);
	    position.x = Math.Min(position.x, PlayerRightBoundX);

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
        else if (_canInput && (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)))
        {
            SetRbVelocity();
        }
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

            if (_canInput)
                SetRbVelocity();
        }
    }

    private void SetRbVelocity()
    {
        // We add to velocity here
        _rb.velocity = new Vector3(Mathf.Sign(_inputHoldTime), Math.Abs(_inputHoldTime) * 3, 0) * 5;
        _inputHoldTime = 0;
        _cooldownTimer = 0;
        _canInput = false;
    }

    public void CollectCoin()
    {
        _numberOfCoins++;
    }

    private bool IsGrounded()
    {
        var _collider = GetComponent<Collider>();

        return Physics.CheckCapsule(_collider.bounds.center,new Vector3(_collider.bounds.center.x,_collider.bounds.min.y-0.1f,_collider.bounds.center.z),0.18f);
    }
}
