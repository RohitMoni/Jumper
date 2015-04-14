using Assets.Code.Database;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LogInPanelScript : MonoBehaviour
{
    private MenuManager _menuManager;
    private InputField _username;
    private InputField _password;

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
        var password = _password.textComponent.text;

        if (username.Length != 0 && password.Length != 0)
        {
            var result = PlayerProfileManager.Instance.AttemptLogin(username, password);

            if (result == LoginAttemptResult.Success)
            {
            }
            else
            {
                
            }

            _menuManager.LoggedIn(username, 100);
            _password.text = "";
        }
    }

    public void Register()
    {
        var username = _username.textComponent.text;
        var password = _password.textComponent.text;

        if (username.Length != 0 && password.Length != 0)
        {
            
        }
    }

    public void LogOut()
    {
        _menuManager.LoggedOut();
    }
}
