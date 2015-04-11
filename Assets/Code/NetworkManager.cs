using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Assets.Code;
using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

    /* Properties */
    private HostData[] _hostList;

    /* References */
    private MenuManager _menuManager;
    private GameManager _gameManager;

    /* Constants */
    private const string GameTypeName = "RM_Jumper";
    private const string GameName = "Jumper_1";
    private const int NumberOfPlayers = 4;
    private const int PortNumber = 25000;

	void Start ()
	{
	    _menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
	    _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void StartServer()
    {
        Network.InitializeServer(NumberOfPlayers, PortNumber, !Network.HavePublicAddress());
        MasterServer.RegisterHost(GameTypeName, GameName);
    }

    void OnServerInitialized()
    {
        Debug.Log("Server Initialized");
    }

    void OnPlayerConnected()
    {
        _gameManager.StartGame();
    }

    public void RefreshHostList()
    {
        MasterServer.RequestHostList(GameTypeName);
    }

    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (msEvent == MasterServerEvent.HostListReceived)
        {
            _hostList = MasterServer.PollHostList();
            _menuManager.SetupServerButtonList(_hostList);
        }
    }

    public void JoinServer(HostData hostData)
    {
        Network.Connect(hostData);
    }

    void OnConnectedToServer()
    {
        Debug.Log("Server Joined");
        _gameManager.StartGame();
    }

    void OnPlayerDisconnected(NetworkPlayer player)
    {
        Debug.Log("Clean up after player " + player);
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
    }
}
