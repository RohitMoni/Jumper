using Assets.Code;
using UnityEngine;
using System.Collections;

public class CoinBox : MonoBehaviour
{
    private float _timer = 0;
    private const float TimeForCoinCollect = 1f;
    private bool _isEnabled;
    private GameObject _player;

    // Use this for initialization
    void Awake()
    {
        _timer = 0;
        _isEnabled = false;
        _player = null;
    }

    void Update()
    {
        if (!_isEnabled)
            return;

        _timer += Time.smoothDeltaTime;
        if (_timer > TimeForCoinCollect)
        {
            if (_player)
                _player.gameObject.GetComponent<Player>().CollectCoin();

            _timer = 0;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "Player")
            return;

        foreach (var contact in collision.contacts)
        {
            var relativePosition = transform.InverseTransformPoint(contact.point);

            if (relativePosition.y < 0)
            {
                _isEnabled = true;
                _player = collision.gameObject;
                _timer = 0;
                break;
            }
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == _player)
        {
            _isEnabled = false;
            _player = null;
            _timer = 0;
        }
    }
}


