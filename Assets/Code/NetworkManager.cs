using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

    /* Properties */
    public GameObject NetworkedPlayerPrefab;
    private List<GameObject> _networkedPlayers;

    /* References */
    private Transform _networkedPlayerAnchor;

    /* Constants */
    private readonly Vector3 _startPlayerPosition;

    // TEST NETWORK STUFF
    public string gameName = "Jumper";

    public bool refreshing = false;
    public HostData[] hostData;

    public bool create = false;
    public bool joining = false;

    public string serverName = "";
    public string serverInfo = "";
    public string serverPass = "";

    public string playerName = "";

    public string clientPass = "";

    public Vector2 scrollPosition = Vector2.zero;   
    

    NetworkManager()
    {
        _startPlayerPosition = new Vector3(0, 3, 0);
    }

	// Use this for initialization
	void Start ()
	{
	    _networkedPlayerAnchor = GameObject.Find("NetworkedPlayerAnchor").transform;
        _networkedPlayers = new List<GameObject>();

        // NETWORK STUFF
	    playerName = "Player Name";
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void CreateNewPlayer()
    {
        var obj = Instantiate(NetworkedPlayerPrefab);
        obj.transform.SetParent(_networkedPlayerAnchor);
        obj.transform.position = _startPlayerPosition;

        _networkedPlayers.Add(obj);
    }

    void UpdateAllPlayers()
    {
        foreach (var player in _networkedPlayers)
        {
            
        }
    }
}
