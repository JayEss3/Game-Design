using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Mirror.Discovery;

public class LANUserInterface : MonoBehaviour
{
    readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();
    public NetworkDiscovery networkDiscovery;
    [SerializeField] private GameObject serverButtonPrefab;
    [SerializeField] private Transform serverScrollViewContent;
    public void FindServers()
    {
        discoveredServers.Clear();
        networkDiscovery.StartDiscovery();
    }
    public void HostServer()
    {
        discoveredServers.Clear();
        NetworkManager.singleton.StartHost();
        networkDiscovery.AdvertiseServer();
        gameObject.SetActive(false);
    }
    public void StartServer()
    {
        discoveredServers.Clear();
        NetworkManager.singleton.StartServer();
        networkDiscovery.AdvertiseServer();
        gameObject.SetActive(false);
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
}
