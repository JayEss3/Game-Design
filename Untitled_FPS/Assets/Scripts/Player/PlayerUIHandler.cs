using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using TMPro;
using UnityEngine.UI;

public class PlayerUIHandler : NetworkBehaviour
{
    [SerializeField] private GameObject _playerCanvas = null;
    [SerializeField] private GameObject _chatWindow = null;
    [SerializeField] private GameObject _notificationDisplayParent = null;
    [SerializeField] private GameObject _optionsMenu = null;
    [Header("Instance")]
    [SerializeField] private GameObject _instanceDisplayParent = null;
    [SerializeField] private TextMeshProUGUI instanceName = null;
    [SerializeField] private Image instanceImage = null;
    [SerializeField] private TMP_InputField instanceID = null;
    private GameObject instanceMaster = null;

    private PlayerInstanceManager _playerInstanceManager;

    #region Events/Actions
    public static event Action<PlayerUIHandler, string> OnChatMessage;
    #endregion Events/Actions

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        _playerCanvas.SetActive(true);
        _playerCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
        _playerCanvas.GetComponent<Canvas>().planeDistance = 0.1f;
        _playerInstanceManager = GetComponent<PlayerInstanceManager>();
    }

    void Update()
    {
        if (!base.hasAuthority)
            return;

        if (Input.GetKeyDown(KeyCode.Return)) // Chat using enter key
        {
            switch (Cursor.lockState)
            {
                case CursorLockMode.None:
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    _chatWindow.SetActive(false);
                    _optionsMenu.SetActive(false);
                    break;
                case CursorLockMode.Locked:
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    _chatWindow.SetActive(true);
                    _optionsMenu.SetActive(false);
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape)) // Menus using escape key
        {
            switch (Cursor.lockState)
            {
                case CursorLockMode.None:
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    _chatWindow.SetActive(false);
                    _optionsMenu.SetActive(false);
                    break;
                case CursorLockMode.Locked:
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    _chatWindow.SetActive(false);
                    _optionsMenu.SetActive(true);
                    break;
            }
        }
    }

    public string GetPlayerName()
    {
        return GetComponent<Player>().playerName;
    }

    public void DisplayNotification(GameObject notification)
    {
        notification.transform.SetParent(_notificationDisplayParent.transform);
        notification.transform.localPosition = Vector3.zero;
    }
    public void DisposeNotification()
    {
        foreach (Transform child in _notificationDisplayParent.transform)
            Destroy(child.gameObject);
    }

    #region Chat
    [Command]
    public void CmdSendChat(string message)
    {
        if (message.Trim() != "")
            RpcReceiveChat(message.Trim());
    }

    [ClientRpc]
    public void RpcReceiveChat(string message)
    {
        OnChatMessage?.Invoke(this, message);
    }
    #endregion

    #region Instance
    public void FillInstaceUI(GameObject im, string name, Sprite sprite)
    {
        instanceMaster = im;
        instanceName.text = name;
        instanceImage.sprite = sprite;
        _instanceDisplayParent.SetActive(true);
    }
    public void ClearInstanceUI()
    {
        _instanceDisplayParent.SetActive(false);
        instanceMaster = null;
        instanceName.text = "";
        instanceImage.sprite = null;
    }
    public void OnInstance()
    {
        if (instanceID.text != "")
        {
            var identity = NetworkClient.connection.identity;
            _playerInstanceManager.CmdJoinInstance(Int32.Parse(instanceID.text), instanceMaster, identity);
        }
    }
    #endregion Instance
}
