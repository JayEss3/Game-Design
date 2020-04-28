using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class ChatWindow : MonoBehaviour
{
    public TMP_InputField chatMessage = null;
    public TextMeshProUGUI chatHistory = null;
    public Scrollbar scrollbar = null;

    public void Awake()
    {
        PlayerUIHandler.OnChatMessage += OnPlayerMessage;
        chatMessage.text = " has joined the game";
        OnSend();
    }

    private void OnPlayerMessage(PlayerUIHandler player, string message)
    {
        string prettyMessage = player.isLocalPlayer ?
            $"<color=green>{player.GetPlayerName()}: </color> {message}" :
            $"<color=red>{player.GetPlayerName()}: </color> {message}";
        AppendMessage(prettyMessage);

        Debug.Log(message);
    }

    public void OnSend()
    {
        if (chatMessage.text.Trim() == "")
            return;

        // get our player
        var player = NetworkClient.connection.identity.GetComponent<PlayerUIHandler>();

        // send a message
        player.CmdSendChat(chatMessage.text.Trim());

        chatMessage.text = "";
    }

    internal void AppendMessage(string message)
    {
        StartCoroutine(AppendAndScroll(message));
    }

    IEnumerator AppendAndScroll(string message)
    {
        chatHistory.text += message + "\n";

        // it takes 2 frames for the UI to update ?!?!
        yield return null;
        yield return null;

        // slam the scrollbar down
        scrollbar.value = 0;
    }
}
