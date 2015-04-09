using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour
{

    private GameObject _mainMenu;
    private GameObject _findGameMenu;
    private NetworkManager _networkManager;

	// Use this for initialization
	void Start ()
	{
	    _mainMenu = gameObject.transform.FindChild("MainMenu").gameObject;
        _findGameMenu = gameObject.transform.FindChild("FindGameMenu").gameObject;
	    _networkManager = GameObject.Find("GameManager").GetComponent<NetworkManager>();



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
}
