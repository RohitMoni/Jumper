using System.Net.Mime;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject ServerButton;

    private GameObject _mainMenu;
    private GameObject _findGameMenu;
    private NetworkManager _networkManager;
    private GameObject _contentPanel;

	// Use this for initialization
	void Start ()
	{
	    _mainMenu = gameObject.transform.FindChild("MainMenu").gameObject;
        _findGameMenu = gameObject.transform.FindChild("FindGameMenu").gameObject;
	    _networkManager = GameObject.Find("GameManager").GetComponent<NetworkManager>();
	    _contentPanel = GameObject.Find("ContentPanel");

	    _findGameMenu.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void MainMenuToFindGameMenu()
    {
        _mainMenu.SetActive(false);
        _findGameMenu.SetActive(true);
    }

    public void FindGameMenuToMainMenu()
    {
        _mainMenu.SetActive(true);
        _findGameMenu.SetActive(false);
    }

    public void CreateGame()
    {
        _mainMenu.SetActive(false);
        _networkManager.StartServer();
    }

    public void JoinGame(HostData data)
    {
        _mainMenu.SetActive(false);
        _networkManager.JoinServer(data);
    }

    public void RefreshServerList()
    {
        _networkManager.RefreshHostList();
        foreach (Transform button in _contentPanel.transform)
        {
            Destroy(button.gameObject);
        }
    }

    public void SetupServerButtonList(HostData[] hostList)
    {
        foreach (var item in hostList)
        {
            var serverButton = Instantiate(ServerButton);
            serverButton.transform.SetParent(_contentPanel.transform);
            serverButton.GetComponent<ConnectToServerButton>().Initialise(item);
            //serverButton.GetComponent<Button>().onClick.AddListener();
        }
    }
}
