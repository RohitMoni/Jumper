﻿using Assets.Code;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Networking
    private float _lastSynchronizationTime = 0f;
    private float _syncDelay = 0f;
    private float _syncTime = 0f;
    private Vector3 _syncStartPosition = Vector3.zero;
    private Vector3 _syncEndPosition = Vector3.zero;

    void Start()
    {
        transform.SetParent(GameObject.Find("CoinAnchor").transform);
    }

    void Update()
    {
        //if (!GetComponent<NetworkView>().isMine)
            //SyncedMovement();
    }

    [RPC]
    public void Collect()
    {
        CoinManager.ActiveCoins.Remove(gameObject);
        CoinManager.UnusedCoins.Add(gameObject);

        if (GetComponent<NetworkView>().isMine)
            GetComponent<NetworkView>().RPC("Collect", RPCMode.OthersBuffered);

        gameObject.SetActive(false);
    }

    [RPC]
    public void Activate()
    {
        CoinManager.UnusedCoins.Remove(gameObject);
        CoinManager.ActiveCoins.Add(gameObject);
        gameObject.SetActive(true);

        if (GetComponent<NetworkView>().isMine)
            GetComponent<NetworkView>().RPC("Activate", RPCMode.OthersBuffered);
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
