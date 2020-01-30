using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
using Mirror.Discovery;

public class ServerButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private ServerResponse info;
    [SerializeField] private GameObject panelReference;
    public void SetContent(string n, ServerResponse i, GameObject go)
    {
        buttonText.text = n;
        info = i;
        panelReference = go;
    }
    public ServerResponse GetServer() { return info; }
    public void Connect()
    {
        NetworkManager.singleton.StartClient(info.uri);
        panelReference.SetActive(false);
    }
}
