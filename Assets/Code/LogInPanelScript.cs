using Assets.Code.Database;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LogInPanelScript : MonoBehaviour
{
    private MenuManager _menuManager;
    private InputField _username;
    private InputField _password;
    private Text _loggedInUserName;
    private Text _loggedInCoins;

    void Awake()
    {
        _loggedInUserName = GameObject.Find("LoggedInUserName").GetComponent<Text>();
        _loggedInCoins = GameObject.Find("LoggedInNumberOfCoins").GetComponent<Text>();
    }

	// Use this for initialization
	void Start ()
	{
	    _menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
        _username = transform.FindChild("UserName").GetComponent<InputField>();
	    _password = transform.FindChild("Password").GetComponent<InputField>();
	}

    public void LogIn()
    {
        var username = _username.textComponent.text;
        var password = _password.text;

        if (username.Length != 0 && password.Length != 0)
        {
            var result = PlayerProfileManager.Instance.AttemptLogin(username, password);

            if (result == LoginAttemptResult.Success)
            {
                var coinsNo = PlayerProfileManager.Instance.GetCoinsForLoggedInUser();
                _loggedInCoins.text = coinsNo.ToString();
                _menuManager.LoggedIn(username);
                _loggedInUserName.text = username;
            }
            else
            {
                
            }

            _password.text = "";
        }
    }

    public void Register()
    {
        var username = _username.textComponent.text;
        var password = _password.text;

        if (username.Length != 0 && password.Length != 0)
        {
            PlayerProfileManager.Instance.DeleteAccount(username);

            PlayerProfileManager.Instance.CreateAccount(username, password, 0);
        }
    }

    public void LogOut()
    {
        PlayerProfileManager.Instance.SetCoinsForLoggedInUser(int.Parse(_loggedInCoins.text));
        _menuManager.LoggedOut();
    }
}
