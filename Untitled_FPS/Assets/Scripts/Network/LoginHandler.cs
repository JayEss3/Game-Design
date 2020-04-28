using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class LoginHandler : MonoBehaviour
{
    [Header("Key References")]
    [SerializeField] private UntitledNetworkManager _networkManager = null;
    [Header("Login")]
    [SerializeField] private TMP_InputField _username = null;
    [SerializeField] private TMP_InputField _password = null;

    public void Login()
    {
        if (Database.CorrectCredentials(_username.text, _password.text))
        {
            _networkManager.playerName = _username.text;
            _networkManager.StartClient();
            gameObject.SetActive(false);
        }
        else if (Database.AccountExists(_username.text))
        {
            Debug.Log("Username Exists");
        }
    }
    public void LoginHost()
    {
        if (Database.CorrectCredentials(_username.text, _password.text))
        {
            _networkManager.playerName = _username.text;
            _networkManager.StartHost();
            gameObject.SetActive(false);
        }
        else if (Database.AccountExists(_username.text))
        {
            Debug.Log("Username Exists");
        }
    }
}
