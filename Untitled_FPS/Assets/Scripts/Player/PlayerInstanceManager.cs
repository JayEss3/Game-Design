using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInstanceManager : NetworkBehaviour
{
    [SyncVar] public bool inInstance = false;
    [SerializeField] public readonly SyncDictionaryPlayerInstances instances = new SyncDictionaryPlayerInstances();
    public GameObject currentPlayerInstance = null;

    [Command]
    public void CmdJoinInstance(int joinningID, GameObject instanceMaster, NetworkIdentity identity)
    {
        var im = instanceMaster.GetComponent<InstanceMaster>();
        var instanceMap = im.instanceMap;
        var spawnOffset = im.spawnOffset;
        if (im.spawnedInstances.Contains(joinningID))
        {
            var hits = Physics.OverlapSphere(new Vector3(0, spawnOffset * (1 + im.spawnedInstances.IndexOf(joinningID)), 0), 50f);
            GameObject instance = null;
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Instance"))
                    instance = hit.gameObject;
            }
            if (instance != null)
            {
                var instanceManager = instance.GetComponent<InstanceManager>();
                if (instanceManager.currentPlayerCount < instanceManager.maxPlayerCount)
                {
                    currentPlayerInstance = instance;
                    inInstance = true;
                    var instanceData = instance.GetComponent<InstanceData>();
                    var player = identity.GetComponent<Player>();
                    instances.Add(im.instanceName, joinningID);
                    player.playerType = "Taggie";
                    RpcMovePlayer(instanceData.connectionSpawnLocation.position, identity.gameObject);
                }
            }
        }
        else
        {
            var instance = Instantiate(instanceMap);
            im.spawnedInstances.Add(joinningID);
            instance.transform.position = new Vector3(0, spawnOffset * (1 + im.spawnedInstances.IndexOf(joinningID)), 0);
            NetworkServer.Spawn(instance);
            currentPlayerInstance = instance;
            inInstance = true;

            var instanceManager = instance.GetComponent<InstanceManager>();
            instanceManager.SpawnWeapons();
            instanceManager.currentPlayerCount++;

            var instanceData = instance.GetComponent<InstanceData>();
            instanceData.instanceID = joinningID;

            var player = identity.GetComponent<Player>();
            instances.Add(im.instanceName, joinningID);
            player.playerType = "Tagger";
            player.health = 2500f;

            RpcMovePlayer(instanceData.connectionSpawnLocation.position, identity.gameObject);
        }
    }
    [ClientRpc]
    public void RpcMovePlayer(Vector3 pos, GameObject player)
    {
        player.transform.position = pos;
    }
}

[System.Serializable]
public class SyncDictionaryPlayerInstances : SyncDictionary<string, int> { }