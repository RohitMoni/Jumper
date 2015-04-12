using Assets.Code;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Networking
    private NetworkView _networkView;
    private float _lastSynchronizationTime = 0f;
    private float _syncDelay = 0f;
    private float _syncTime = 0f;
    private Vector3 _syncStartPosition = Vector3.zero;
    private Vector3 _syncEndPosition = Vector3.zero;

    public void Initialise()
    {
        name = "Coin";
        transform.SetParent(GameObject.Find("CoinAnchor").transform);
        _networkView = GetComponent<NetworkView>();
    }

    void Update()
    {
        if (!_networkView.isMine)
            SyncedMovement();
    }

    [RPC]
    public void RePosition(Vector3 newPosition)
    {
        transform.position = newPosition;

        if (_networkView.isMine)
            _networkView.RPC("RePosition", RPCMode.OthersBuffered, newPosition);
    }

    [RPC]
    public void DeActivate()
    {
        SetActive(false);

        if (_networkView.isMine)
            _networkView.RPC("DeActivate", RPCMode.OthersBuffered);
    }

    [RPC]
    public void Activate()
    {
        SetActive(true);

        if (_networkView.isMine)
            _networkView.RPC("Activate", RPCMode.OthersBuffered);
    }

    private void SetActive(bool active)
    {
        if (active)
        {
            CoinManager.UnusedCoins.Remove(gameObject);
            CoinManager.ActiveCoins.Add(gameObject);
        }
        else
        {
            CoinManager.ActiveCoins.Remove(gameObject);
            CoinManager.UnusedCoins.Add(gameObject);
        }

        GetComponent<Renderer>().enabled = active;
        GetComponent<Collider>().enabled = active;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(active);
        }
    }

    // Networking stuff
    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        Vector3 syncPosition = Vector3.zero;
        if (stream.isWriting)
        {
            syncPosition = transform.position;
            stream.Serialize(ref syncPosition);
        }
        else
        {
            stream.Serialize(ref syncPosition);

            _syncTime = 0f;
            _syncDelay = Time.time - _lastSynchronizationTime;
            _lastSynchronizationTime = Time.time;

            _syncStartPosition = transform.position;
            _syncEndPosition = syncPosition;
        }
    }

    void SyncedMovement()
    {
        _syncTime += Time.deltaTime;
        transform.position = Vector3.Lerp(_syncStartPosition, _syncEndPosition, _syncTime / _syncDelay);
    }
}
