using Assets.Code;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Networking
    private float _lastSynchronizationTime = 0f;
    private float _syncDelay = 0f;
    private float _syncTime = 0f;
    private Vector3 _syncStartPosition = Vector3.zero;
    private Vector3 _syncEndPosition = Vector3.zero;

    void Update()
    {
        if (!GetComponent<NetworkView>().isMine)
            SyncedMovement();
    }

    public void Collect()
    {
        CoinManager.ActiveCoins.Remove(gameObject);
        gameObject.SetActive(false);
        CoinManager.UnusedCoins.Add(gameObject);
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

            _syncTime = 0f;
            _syncDelay = Time.time - _lastSynchronizationTime;
            _lastSynchronizationTime = Time.time;

            _syncStartPosition = GetComponent<Rigidbody>().position;
            _syncEndPosition = syncPosition;
        }
    }

    void SyncedMovement()
    {
        _syncTime += Time.deltaTime;
        GetComponent<Rigidbody>().position = Vector3.Lerp(_syncStartPosition, _syncEndPosition, _syncTime / _syncDelay);
    }
}
