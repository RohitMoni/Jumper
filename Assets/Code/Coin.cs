using Assets.Code;
using UnityEngine;

public class Coin : MonoBehaviour {

    private Vector3 _velocity;
    private bool _isMoving;

    private const float FrictionConst = 0.1f;

    public void Initialize()
    {
        _velocity = Vector3.zero;
        _isMoving = false;
    }

    void Update()
    {
        if (!_isMoving)
            return;

        transform.position += _velocity;

        var frictionVec = -_velocity.normalized * FrictionConst;
        if (_velocity.magnitude < frictionVec.magnitude)
        {
            _velocity = Vector3.zero;
            _isMoving = false;
        }
        else
            _velocity += frictionVec;
    }

    public void SetVelocity (Vector3 velocity)
    {
        _velocity = velocity;
        _isMoving = true;
    }

    /* References */
    void OnColliderEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<Player>().CollectCoin();
            CoinManager.CollectCoin(gameObject);
            Debug.Log("Coin collected!");
        }
        else
        {
            
        }
    }
}
