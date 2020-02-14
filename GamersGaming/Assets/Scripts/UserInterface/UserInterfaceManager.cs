using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Mirror;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using Mirror.Discovery;

public class UserInterfaceManager : MonoBehaviour
{
    //[SerializeField] private TMP_InputField _username;
    //[SerializeField] private TMP_InputField _password;
    [Header("Panel References")]
    [SerializeField] private GameObject LoginPanel;
    [SerializeField] private GameObject CreateServerPanel;
    [SerializeField] private GameObject ConnectPanel;
    [SerializeField] private GameObject LANPanel;
    [SerializeField] private GameObject MenuPanel;
    [Header("Login References")]
    [SerializeField] private TMP_InputField _displayName;
    [Header("Create Server References")]
    [SerializeField] private TMP_InputField networkAddress;
    [SerializeField] private TMP_InputField tickRate;
    [SerializeField] private TMP_InputField maxPlayers;
    [Header("Connect References")]
    [SerializeField] private TMP_InputField connNetworkAddress;
    [Header("LAN References")]
    readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();
    public NetworkDiscovery networkDiscovery;
    [SerializeField] private GameObject serverButtonPrefab;
    [SerializeField] private Transform serverScrollViewContent;

    private LoginData loginData;
    private void Awake()
    {
        loginData = Resources.Load<LoginData>("Objects/LoginData");
        if (loginData.displayName != null)
            _displayName.text = loginData.displayName;
    }
    public void ToggleLoginServer()
    {
        LoginPanel.SetActive(!LoginPanel.activeSelf);
        CreateServerPanel.SetActive(!CreateServerPanel.activeSelf);
    }
    public void MenuToggle()
    {
        if (Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            MenuPanel.SetActive(false);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            MenuPanel.SetActive(true);
        }
    }
    public void MenutoLogin()
    {
        MenuPanel.SetActive(false);
        LoginPanel.SetActive(true);
    }
    public void QuitClick()
    {
        Application.Quit();
    }
    public void ConnectToLogin()
    {
        LoginPanel.SetActive(true);
        ConnectPanel.SetActive(false);
    }
    public void LanToConnect()
    {
        ConnectPanel.SetActive(true);
        LANPanel.SetActive(false);
    }
    public void HostServerClick()
    {
        SetManagerSettings();
        discoveredServers.Clear();
        NetworkManager.singleton.StartHost();
        if (NetworkManager.singleton.networkAddress == "localhost")
            networkDiscovery.AdvertiseServer();
        CreateServerPanel.SetActive(false);
    }
    public void StartServerClick()
    {
        SetManagerSettings();
        discoveredServers.Clear();
        NetworkManager.singleton.StartServer();
        if(NetworkManager.singleton.networkAddress == "localhost")
            networkDiscovery.AdvertiseServer();
        CreateServerPanel.SetActive(false);
    }
    public void LoginCLick()
    {
        loginData.displayName = _displayName.text;
        var playerConfig = Resources.Load<PlayerConfig>("Objects/PlayerConfig");
        playerConfig.playerName = _displayName.text;
        LoginPanel.SetActive(false);
        ConnectPanel.SetActive(true);
        /* This doesn't work rn and i really dont know why and its making me sad
        var cs = $"database=gravitag;server=gamedesign.cdrigqqp3tnk.us-east-1.rds.amazonaws.com;port=3306;user=admin;pwd=password;CharSet=utf8;";
        MySqlConnection conn = new MySqlConnection(cs);
        conn.Open();
        MySqlCommand cmd = new MySqlCommand("SELECT player_name FROM gravitag.players;", conn);
        MySqlDataReader reader = cmd.ExecuteReader();
        while(reader.Read())
            Debug.Log(reader[0].ToString());
        /*try
        {
            MySqlCommand cmd = new MySqlCommand("SELECT player_name FROM gravitag.players;", conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
                Debug.Log(reader[0].ToString());
        }
        catch (Exception e) { Debug.Log(e); }*/
    }
    public void ConnectClick()
    {
        if (connNetworkAddress.text != "")
        {
            ParseNetAddress(connNetworkAddress.text);
            NetworkManager.singleton.StartClient();
            ConnectPanel.SetActive(false);
        } else
        {
            LANPanel.SetActive(true);
            ConnectPanel.SetActive(false);
            FindServers();
        }
    }
    private void SetManagerSettings()
    {
        if (networkAddress.text != "")
            NetworkManager.singleton.networkAddress = networkAddress.text;
        if (tickRate.text != "")
            NetworkManager.singleton.serverTickRate = Int32.Parse(tickRate.text);
        if (tickRate.text != "")
            NetworkManager.singleton.maxConnections = Int32.Parse(maxPlayers.text);
    }
    public void FindServers()
    {
        discoveredServers.Clear();
        networkDiscovery.StartDiscovery();
    }
    public void OnDiscoveredServer(ServerResponse info)
    {
        discoveredServers[info.serverId] = info;

        var servers = new List<string>();
        foreach (Transform child in serverScrollViewContent)
            servers.Add(child.name);
        if (servers.Contains(info.EndPoint.Address.ToString())) return;

        var newButton = Instantiate(serverButtonPrefab);
        newButton.name = info.EndPoint.Address.ToString();
        newButton.GetComponent<ServerButton>().SetContent($"Server {newButton.name}", info, gameObject);
        newButton.GetComponent<Button>().onClick.AddListener(newButton.GetComponent<ServerButton>().Connect);
        newButton.transform.SetParent(serverScrollViewContent);
    }
    private void ParseNetAddress(string networkAddress)
    {
        switch (networkAddress)
        {
            case "dev":
                NetworkManager.singleton.networkAddress = "18.234.252.187";
                break;
            case "classic":
                NetworkManager.singleton.networkAddress = "18.234.252.187";
                break;
            default:
                NetworkManager.singleton.networkAddress = networkAddress;
                break;
        }
    }
}
