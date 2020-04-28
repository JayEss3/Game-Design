using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class UntitledNetworkManager : NetworkManager
{
    public string playerName = null;
    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<CreatePlayerMessage>(OnCreatePlayer);
    }
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        // tell the server to create a player with this name
        conn.Send(new CreatePlayerMessage { playerName = playerName });
    }

    private void OnCreatePlayer(NetworkConnection connection, CreatePlayerMessage createPlayerMessage)
    {
        // create a gameobject using the name supplied by client
        GameObject playergo = Instantiate(playerPrefab);
        playergo.GetComponent<Player>().playerName = createPlayerMessage.playerName;

        // set it as the player
        NetworkServer.AddPlayerForConnection(connection, playergo);
    }
}
public class CreatePlayerMessage : MessageBase
{
    public string playerName;
}