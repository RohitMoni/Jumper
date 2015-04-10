using System;
using UnityEngine;

namespace Assets.Code
{
    public class Player : MonoBehaviour {

        /* Properties */
        private bool _canInput;
        private bool _isTouching;
        private float _inputHoldTime;
        private float _cooldownTimer;
        private int _numberOfCoins;

        // Networking
        private float lastSynchronizationTime = 0f;
        private float syncDelay = 0f;
        private float syncTime = 0f;
        private Vector3 syncStartPosition = Vector3.zero;
        private Vector3 syncEndPosition = Vector3.zero;

        /* References */
        private Rigidbody _rb;

        /* Constants */
        private const float MinimumInputHoldTime = 0.1f;
        public const float MaximumInputHoldTime = 0.75f;
        private const float JumpCooldown = 0f;
        private const float JumpMaxTime = 5f;
        private const float JumpMinTime = 0.2f;
        private const float JumpCoefficient = 4.00f;

        void Start()
        {
            name = "Player";
            transform.SetParent(GetComponent<NetworkView>().isMine
                ? GameObject.Find("GameAnchor").transform
                : GameObject.Find("NetworkedPlayerAnchor").transform);
        }

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

            if (GetComponent<NetworkView>().isMine)
                HandleInput();
            else
                SyncedMovement();

            // Limiting player's X and Y coord
            var position = transform.position;
            position.x = Math.Max(position.x, GameManager.LeftBound);
            position.x = Math.Min(position.x, GameManager.RightBound);
            position.y = Math.Min(position.y, GameManager.TopBound);

            transform.position = position;
        }

        private void HandleInput()
        {
            if (_canInput)
            {
                HandleKeyboardInput();
                HandleTouchInput();
            }
            else
            {
                // Updating cooldown timer
                _cooldownTimer += Time.smoothDeltaTime;

                if ((_cooldownTimer > JumpMinTime && IsGrounded()) || _cooldownTimer > JumpMaxTime)
                {
                    _canInput = true;
                    _cooldownTimer = 0;
                }
            }
        }

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
            var size = transform.localScale.x;

            var leftPoint = transform.position;
            var rightpoint = leftPoint;
            leftPoint.x -= size;
            rightpoint.x += size;

            var leftGrounded = Physics.Raycast(leftPoint, -Vector3.up, size);
            var rightGrounded = Physics.Raycast(rightpoint, -Vector3.up, size);

            return leftGrounded || rightGrounded;
        }

        // Networking stuff
        void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
        {
            Vector3 syncPosition = Vector3.zero;
            if (stream.isWriting)
            {
                syncPosition = GetComponent<Rigidbody>().position;
                stream.Serialize(ref syncPosition);
            }
            else
            {
                stream.Serialize(ref syncPosition);

                syncTime = 0f;
                syncDelay = Time.time - lastSynchronizationTime;
                lastSynchronizationTime = Time.time;

                syncStartPosition = GetComponent<Rigidbody>().position;
                syncEndPosition = syncPosition;
            }
        }

        void SyncedMovement()
        {
            syncTime += Time.deltaTime;
            GetComponent<Rigidbody>().position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
        }
    }
}
