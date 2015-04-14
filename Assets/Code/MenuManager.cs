using System.Net.Mime;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject ServerButton;

    private GameObject _mainMenu;
    private GameObject _findGameMenu;
    private GameObject _logInPanel;
    private GameObject _loggedInPanel;
    private GameObject _contentPanel;
    private NetworkManager _networkManager;
    private Text _loggedInCoins;

    void Awake()
    {
        _mainMenu = gameObject.transform.FindChild("MainMenu").gameObject;
        _findGameMenu = gameObject.transform.FindChild("FindGameMenu").gameObject;
        _networkManager = GameObject.Find("GameManager").GetComponent<NetworkManager>();
        _contentPanel = GameObject.Find("ContentPanel");
        _logInPanel = _mainMenu.transform.FindChild("LogInPanel").gameObject;
        _loggedInPanel = _mainMenu.transform.FindChild("LoggedInPanel").gameObject;
        _loggedInCoins = GameObject.Find("LoggedInNumberOfCoins").GetComponent<Text>();
    }

	// Use this for initialization
	void Start ()
	{
	    _findGameMenu.SetActive(false);
	    _loggedInPanel.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoggedIn(string userName)
    {
        _logInPanel.SetActive(false);
        _loggedInPanel.SetActive(true);
        _networkManager.CurrentLoggedInUser = userName;
    }

    public void LoggedOut()
    {
        _logInPanel.SetActive(true);
        _loggedInPanel.SetActive(false);
    }

    public void MainMenuToFindGameMenu()
    {
        _mainMenu.SetActive(false);
        _findGameMenu.SetActive(true);
        RefreshServerList();
    }

    public void FindGameMenuToMainMenu()
    {
        _mainMenu.SetActive(true);
        _findGameMenu.SetActive(false);
    }

    public void InGameToMenu(int CoinBonus=0)
    {
        _mainMenu.SetActive(true);
        if (_networkManager.CurrentLoggedInUser != "Player")
            _loggedInCoins.text = (int.Parse(_loggedInCoins.text) + CoinBonus).ToString();
    }

    public void CreateGame()
    {
        _mainMenu.SetActive(false);
        _networkManager.StartServer();
    }

    public void JoinGame(HostData data)
    {
        _mainMenu.SetActive(false);
        _findGameMenu.SetActive(false);
        _networkManager.JoinServer(data);
    }

    public void RefreshServerList()
    {
        foreach (Transform button in _contentPanel.transform)
        {
            Destroy(button.gameObject);
        }
        _networkManager.RefreshHostList();
    }

    public void SetupServerButtonList(HostData[] hostList)
    {
        foreach (var item in hostList)
        {
            var serverButton = Instantiate(ServerButton);
            serverButton.transform.SetParent(_contentPanel.transform);
            serverButton.GetComponent<ConnectToServerButton>().Initialise(item);
            var data = item;
            serverButton.GetComponent<Button>().onClick.AddListener(() => JoinGame(data));
        }
    }
}
