﻿using System.Collections.Generic;
using System.Runtime.InteropServices;
using Assets.Code;
using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

    /* Editor Set */
    public GameObject PlayerPrefab;

    /* Properties */
    private HostData[] _hostList;

    /* References */

    /* Constants */
    private const string GameTypeName = "RM_Jumper";
    private const string GameName = "Jumper_1";
    private const int NumberOfPlayers = 4;
    private const int PortNumber = 25000;

    // TEST NETWORK STUFF

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void SpawnPlayer()
    {
        Network.Instantiate(PlayerPrefab, new Vector3(0, 5f, 0), Quaternion.identity, 0);
    }

    void StartServer()
    {
        Network.InitializeServer(NumberOfPlayers, PortNumber, !Network.HavePublicAddress());
        MasterServer.RegisterHost(GameTypeName, GameName);
    }

    void OnServerInitialized()
    {
        Debug.Log("Server Initialized");
        SpawnPlayer();
    }

    void RefreshHostList()
    {
        MasterServer.RequestHostList(GameTypeName);
    }

    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (msEvent == MasterServerEvent.HostListReceived)
            _hostList = MasterServer.PollHostList();
    }

    void JoinServer(HostData hostData)
    {
        Network.Connect(hostData);
    }

    void OnConnectedToServer()
    {
        Debug.Log("Server Joined");
        SpawnPlayer();
    }

}